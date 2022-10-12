CREATE DATABASE BankSystem;
GO

USE BankSystem;

-- Creating tables

CREATE TABLE City (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Bank (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL UNIQUE,
	City int CONSTRAINT Bank_City_FK REFERENCES City(Id)
);

CREATE TABLE Branch (
	Id INT PRIMARY KEY IDENTITY (1,1),
	Bank INT  CONSTRAINT Branch_Bank_FK REFERENCES Bank(Id),
	City INT CONSTRAINT Branch_City_FK REFERENCES City(Id),
);

CREATE TABLE AccountStatus (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) not null unique
);

CREATE TABLE Account (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	Bank INT CONSTRAINT Account_Bank_FK REFERENCES Bank(Id),
	Status INT CONSTRAINT Account_Status_FK REFERENCES AccountStatus(Id),
	Balance MONEY DEFAULT 0
);

CREATE TABLE AccountCard (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Account INT CONSTRAINT Account_Card_FK REFERENCES Account(Id),
	Balance MONEY DEFAULT 0
);

GO

-- Inserting data into tables

INSERT INTO City(Name)
VALUES
	('Minsk'),
	('Gomel'),
	('Brest'),
	('Vitebsk'),
	('Mogilev');

INSERT INTO Bank(Name,City)
VALUES
	('Prior', 1),
	('Alfa', 3),
	('Belarus', 5),
	('BSB', 2),
	('BTA', 4),
	('Belinvest', 1);

INSERT INTO Branch(Bank, City)
VALUES
	(1,3),
	(1,4),
	(1,5),
	(1,2),
	(2,1),
	(2,2),
	(2,3),
	(2,4),
	(2,5),
	(3,1),
	(3,1),
	(3,2),
	(3,1),
	(4,1),
	(4,4),
	(4,5),
	(4,2),
	(5,3),
	(5,3),
	(5,3),
	(5,4),
	(6,1),
	(6,2),
	(6,3),
	(6,5);

INSERT INTO AccountStatus(Name)
values
	('Standart'),
	('Pension'),
	('Gold'),
	('Platinum'),
	('Student');
	
INSERT INTO Account(Status, Name, Bank)
VALUES
	(1, 'Alex', 1),
	(2, 'Vlad', 2),
	(2, 'Lia', 2),
	(3, 'Owen', 3),
	(4, 'Emma', 4),
	(4, 'Dima', 5),
	(5, 'Scarlet', 5);

INSERT INTO AccountCard(Account)
VALUES
	(1),
	(2),
	(2),
	(3),
	(3),
	(4),
	(4),
	(4),
	(5),
	(5),
	(5),
	(5),
	(6),
	(7),
	(7);

	
GO

--

SELECT * FROM City;
SELECT * FROM Bank;
SELECT * FROM Branch;
SELECT * FROM Account;


DROP TABLE City;
DROP TABLE Bank;
DROP TABLE Branch;
DROP TABLE AccountStatus;
DROP TABLE Account;
DROP TABLE AccountCard;

GO
-- Tasks 

-- (1) 
-- Список банков у которых есть филиалы в городе X

SELECT b.Name
FROM Bank AS b
WHERE b.City = 1;

-- Или

SELECT b.Name
FROM Bank AS b
INNER JOIN City AS c
	ON b.City = c.Id
WHERE c.Name = 'Minsk';

-- (2) 
-- Получить список карточек с указанием имени владельца, баланса и названия банка

SELECT ac.Id AS [Card ID],
	a.Name AS [Owner],
	ac.Balance AS [Card Balance],
	b.Name AS [Bank name]
FROM AccountCard AS ac
JOIN Account AS a
	ON ac.Account = a.Id
JOIN Bank AS b
	ON a.Bank = b.Id;

-- (3) 
-- Получить список банковских аккаунтов
-- у которых баланс не совпадает с суммой баланса по карточкам.
-- В отдельной колонке вывести разницу

UPDATE Account SET Balance = 100 WHERE Id = 1;
UPDATE AccountCard SET Balance = 20 WHERE Account = 1;

SELECT a.Id, a.Name,
	Sum(a.Balance)-Sum(ac.Balance) AS [Difference]
FROM Account a
JOIN AccountCard ac
	ON a.Id = ac.Account
GROUP BY a.Id, a.Name, a.Balance, ac.Balance
HAVING a.Balance != Sum(ac.Balance);


-- (4) 
-- Вывести кол-во банковских карточек для каждого соц статуса
-- (2 реализации, GROUP BY и подзапросом)

--		(1)

SELECT acs.Name, 
	Count(ac.Id) AS [Number of cards]
FROM AccountStatus acs
JOIN Account a 
	ON a.Status = acs.Id
JOIN AccountCard ac
	ON ac.Account = a.Id
GROUP BY acs.Name	

--		(2)

SELECT acs.Name,
	(SELECT Count(*)
	FROM Account a
	JOIN AccountCard ac 
		ON ac.Account = a.Id
	WHERE a.Status = acs.Id )
FROM AccountStatus acs

-- (5) 
-- Написать stored procedure 
-- которая будет добавлять по 10$ на каждый банковский аккаунт 
-- для определенного соц статуса
-- Входной параметр процедуры - Id социального статуса
-- Обработать исключительные ситуации

CREATE PROCEDURE uspAddTenToGroup
	@statusId int
AS
BEGIN
	BEGIN TRY
		IF (SELECT Count(*)
			FROM AccountStatus acs
			WHERE acs.Id = @statusId 
			) = 0
				RAISERROR ('There is no status group with this ID', 11, 1);
		ELSE
		IF (SELECT Count(*) 
			FROM Account a
			WHERE a.Status = @statusId
			) = 0
				RAISERROR ('There is no account in this status group', 11, 1);
		ELSE
			UPDATE Account 
			SET Balance += 10
			WHERE Status = @statusId
	END TRY
	BEGIN CATCH
		DECLARE 
			@Message varchar(MAX) = ERROR_MESSAGE(),
			@Severity int = ERROR_SEVERITY(),
			@State smallint = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH;
END;

SELECT a.Id, a.Name, a.Balance, A.Status
FROM Account a;

EXEC uspAddTenToGroup 2;

SELECT a.Id, a.Name, a.Balance, A.Status
FROM Account a;

DROP PROCEDURE uspAddTenToGroup;

-- (6)
-- Получить список доступных средств для каждого клиента

SELECT a.Id, a.Name,
	a.Balance - Sum(ac.Balance) AS [Available balance]
FROM Account a
JOIN AccountCard ac
	ON a.Id = ac.Account
GROUP BY a.Id, a.Name, a.Balance, ac.Balance;

-- (7)
-- Написать процедуру 
-- которая будет переводить определённую сумму со счёта на карту этого аккаунта

CREATE PROCEDURE uspTransfer
	@accountId int,
	@cardId int,
	@amount int
AS
BEGIN
	BEGIN TRY
		IF (SELECT Count(*)
			FROM Account a
			WHERE a.Id = @accountId
			) = 0
				RAISERROR ('There is no Account with this ID', 11, 1);
		IF (SELECT Count(*)
			FROM AccountCard ac
			WHERE ac.Id = @cardId
			) = 0
				RAISERROR ('There is no Card with this ID', 11, 1);
		IF (SELECT Count(*)
			FROM AccountCard ac
			JOIN Account a
				ON ac.Account = @accountId
				AND ac.Id = @cardId
			) = 0
				RAISERROR ('This user doesn`t have card with this ID', 11, 1);
		IF (SELECT (a.Balance - SUM(ac.Balance) - @amount)
			FROM Account a
			JOIN AccountCard ac
				ON a.Id = ac.Account
			WHERE a.Id = @accountId
			GROUP BY a.Balance
			) <= 0
				RAISERROR ('Not enough money to transfer', 11, 1);
		BEGIN TRAN

			UPDATE AccountCard
			SET Balance += @amount
			WHERE Id = @cardId;

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
				ROLLBACK TRAN
		DECLARE 
			@Message varchar(MAX) = ERROR_MESSAGE(),
			@Severity int = ERROR_SEVERITY(),
			@State smallint = ERROR_STATE();
 
		RAISERROR (@Message, @Severity, @State);
	END CATCH;
END;

UPDATE Account 
SET Balance = 1000
WHERE Id = 5;

SELECT ac.Id, ac.Account AS [Account ID], ac.Balance AS [Card balance],
	a.Balance AS [Account balance]
FROM AccountCard ac
JOIN Account a
	ON a.Id = ac.Account
WHERE a.Id = 5;

EXEC uspTransfer 5,12,700;

SELECT ac.Id, ac.Account AS [Account ID], ac.Balance AS [Card balance],
	a.Balance AS [Account balance]
FROM AccountCard ac
JOIN Account a
	ON a.Id = ac.Account
WHERE a.Id = 5;


-- (8)
-- Написать триггер на таблицы Account/Cards 
-- чтобы нельзя была занести значения в поле баланс если это противоречит условиям

CREATE TRIGGER AccounTrigger
ON Account
AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @AccountID int, 
		@OldBalance money;

	DECLARE Curs CURSOR LOCAL
	for	SELECT i.Id, d.Balance AS OldBalance 
		FROM inserted i
		JOIN deleted d
			ON i.Id = d.Id
	OPEN Curs
	FETCH Curs INTO @AccountID, @OldBalance
	while @@FETCH_STATUS = 0
	BEGIN
		IF (SELECT (a.Balance - SUM(ac.Balance))
			FROM Account a
			JOIN AccountCard ac
				ON a.Id = ac.Account
			WHERE a.Id = @AccountID
			GROUP BY a.Balance
			) < 0
				BEGIN
					UPDATE Account
					SET Balance = @OldBalance
					WHERE Id = @AccountId;
				END;

		FETCH Curs INTO @AccountID, @OldBalance
	END;
	CLOSE Curs;
	DEALLOCATE CURS;
END;


CREATE TRIGGER CardTrigger
ON AccountCard
AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CardID int, 
		@AccountID int,
		@OldBalance money;

	DECLARE Curs CURSOR LOCAL
	for
	SELECT i.Id, i.Account, d.Balance AS OldBalance 
	FROM inserted i
	JOIN deleted d
		ON i.Id = d.Id
	OPEN Curs
	FETCH Curs INTO @CardID, @AccountID, @OldBalance
	while @@FETCH_STATUS = 0
	BEGIN
		IF (SELECT (a.Balance - SUM(ac.Balance))
		FROM Account a
		JOIN AccountCard ac
			ON a.Id = ac.Account
		WHERE a.Id = @AccountID
		GROUP BY a.Balance
		) < 0
		BEGIN
			UPDATE AccountCard
			SET Balance = @OldBalance
			WHERE Id = @CardID;
		END;

		FETCH Curs INTO @CardID, @AccountID, @OldBalance
	END;

	CLOSE CURS;
	DEALLOCATE CURS;
END;


--  test account trigger

SELECT * FROM Account;

UPDATE Account 
SET Balance = 0
WHERE Id = 5;

SELECT * FROM Account;


--  test account card trigger

SELECT * FROM AccountCard;

UPDATE AccountCard 
SET Balance = 9999
WHERE Id = 9;

SELECT * FROM AccountCard;
