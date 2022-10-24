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
);

CREATE TABLE Branch (
	Id INT PRIMARY KEY IDENTITY (1,1),
	BankId INT  CONSTRAINT Branch_BankId_FK REFERENCES Bank(Id),
	CityId INT CONSTRAINT Branch_CityId_FK REFERENCES City(Id),
);

CREATE TABLE AccountStatus (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) not null unique
);

CREATE TABLE Account (
	Id INT PRIMARY KEY IDENTITY(1,1),
	BankId INT CONSTRAINT Account_BankId_FK REFERENCES Bank(Id),
	StatusId INT CONSTRAINT Account_StatusId_FK REFERENCES AccountStatus(Id),
	Balance MONEY DEFAULT 0
);

CREATE TABLE Client (
	AccountId INT CONSTRAINT Client_AccountId_FK REFERENCES Account(Id),
	Name varchar(100) NOT NULL,
);

CREATE TABLE AccountCard (
	Id INT PRIMARY KEY IDENTITY(1,1),
	AccountId INT CONSTRAINT Account_CardId_FK REFERENCES Account(Id),
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

INSERT INTO Bank(Name)
VALUES
	('Prior'),
	('Alfa'),
	('Belarus'),
	('BSB'),
	('BTA'),
	('Belinvest');

INSERT INTO Branch(BankId, CityId)
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
	
INSERT INTO Account(StatusId, BankId)
VALUES
	(1, 1),
	(2, 2),
	(2, 2),
	(3, 3),
	(4, 4),
	(4, 5),
	(5, 5);

INSERT INTO Client(Name, AccountId)
VALUES
	('Vlad', 1),
	('Alex', 2),
	('Bob', 3),
	('Gena', 4),
	('Gabriel', 5),
	('Anton', 6),
	('Lier', 7);

INSERT INTO AccountCard(AccountId)
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
-- ������ ������ � ������� ���� ������� � ������ X

SELECT DISTINCT b.Name
FROM Bank b
JOIN Branch br
	ON b.Id = br.BankId
	AND br.CityId = 3;

-- ���

SELECT DISTINCT b.Name
FROM Bank AS b
JOIN Branch br
	ON br.BankId = b.Id
JOIN City c 
	ON c.Id = br.CityId
	AND c.Name = 'Minsk';

-- (2) 
-- �������� ������ �������� � ��������� ����� ���������, ������� � �������� �����

SELECT ac.Id AS [Card ID],
	c.Name AS [Owner],
	ac.Balance AS [Card Balance],
	b.Name AS [Bank name]
FROM AccountCard AS ac
JOIN Account AS a
	ON a.Id = ac.AccountId
JOIN Client AS c
	ON ac.AccountId = c.AccountId
JOIN Bank AS b
	ON a.BankId = b.Id;

-- (3) 
-- �������� ������ ���������� ���������
-- � ������� ������ �� ��������� � ������ ������� �� ���������.
-- � ��������� ������� ������� �������

UPDATE Account SET Balance = 100 WHERE Id = 1;
UPDATE AccountCard SET Balance = 20 WHERE AccountId = 1;

SELECT a.Id AS [Account Id],
	Sum(a.Balance)-Sum(ac.Balance) AS [Difference]
FROM Account a
JOIN AccountCard ac
	ON a.Id = ac.AccountId
GROUP BY a.Id, a.Balance, ac.Balance
HAVING a.Balance != Sum(ac.Balance);


-- (4) 
-- ������� ���-�� ���������� �������� ��� ������� ��� �������
-- (2 ����������, GROUP BY � �����������)

--		(1)

SELECT acs.Name, 
	Count(ac.Id) AS [Number of cards]
FROM AccountStatus acs
JOIN Account a 
	ON a.StatusId = acs.Id
JOIN AccountCard ac
	ON ac.AccountId = a.Id
GROUP BY acs.Name	

--		(2)

SELECT acs.Name,
	(SELECT Count(*)
	FROM Account a
	JOIN AccountCard ac 
		ON ac.AccountId = a.Id
	WHERE a.StatusId = acs.Id )
FROM AccountStatus acs

-- (5) 
-- �������� stored procedure 
-- ������� ����� ��������� �� 10$ �� ������ ���������� ������� 
-- ��� ������������� ��� �������
-- ������� �������� ��������� - Id ����������� �������
-- ���������� �������������� ��������

CREATE OR ALTER PROCEDURE uspAddTenToGroup
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
			WHERE a.StatusId = @statusId
			) = 0
				RAISERROR ('There is no account in this status group', 11, 1);
		ELSE
			UPDATE Account 
			SET Balance += 10
			WHERE StatusId = @statusId
	END TRY
	BEGIN CATCH
		DECLARE 
			@Message varchar(MAX) = ERROR_MESSAGE(),
			@Severity int = ERROR_SEVERITY(),
			@State smallint = ERROR_STATE()
 
		RAISERROR (@Message, @Severity, @State)
	END CATCH;
END;

SELECT a.Id, a.Balance, a.StatusId
FROM Account a;

EXEC uspAddTenToGroup 2;

SELECT a.Id, a.Balance, A.StatusId
FROM Account a;

DROP PROCEDURE uspAddTenToGroup;

-- (6)
-- �������� ������ ��������� ������� ��� ������� �������

SELECT a.Id, c.Name,
	a.Balance - Sum(ac.Balance) AS [Available balance]
FROM Account a
JOIN AccountCard ac
	ON a.Id = ac.AccountId
JOIN Client c
	ON c.AccountId = a.Id
GROUP BY a.Id, c.Name, a.Balance, ac.Balance;

-- (7)
-- �������� ��������� 
-- ������� ����� ���������� ����������� ����� �� ����� �� ����� ����� ��������

CREATE OR ALTER PROCEDURE uspTransfer
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
				ON ac.AccountId = @accountId
				AND ac.Id = @cardId
			) = 0
				RAISERROR ('This user doesn`t have card with this ID', 11, 1);
		IF (SELECT (a.Balance - SUM(ac.Balance) - @amount)
			FROM Account a
			JOIN AccountCard ac
				ON a.Id = ac.AccountId
			WHERE a.Id = @accountId
			GROUP BY a.Balance
			) <= 0
				RAISERROR ('Not enough money to transfer', 11, 1);
		BEGIN

			UPDATE AccountCard
			SET Balance += @amount
			WHERE Id = @cardId;

		END
	END TRY
	BEGIN CATCH
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

SELECT ac.Id, ac.AccountId AS [Account ID], ac.Balance AS [Card balance],
	a.Balance AS [Account balance]
FROM AccountCard ac
JOIN Account a
	ON a.Id = ac.AccountId
WHERE a.Id = 5;

EXEC uspTransfer 5,12,700;

SELECT ac.Id, ac.AccountId AS [Account ID], ac.Balance AS [Card balance],
	a.Balance AS [Account balance]
FROM AccountCard ac
JOIN Account a
	ON a.Id = ac.AccountId
WHERE a.Id = 5;

-- (8)
-- �������� ������� �� ������� Account/Cards 
-- ����� ������ ���� ������� �������� � ���� ������ ���� ��� ������������ ��������

CREATE OR ALTER TRIGGER AccounTrigger
ON Account
AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @AccountID INT, 
		@OldBalance MONEY,
		@IsFailed BIT = 0;

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
				ON a.Id = ac.AccountId
			WHERE a.Id = @AccountID
			GROUP BY a.Balance
			) < 0
				BEGIN
					SET @IsFailed = 1;

					UPDATE Account
					SET Balance = @OldBalance
					WHERE Id = @AccountId;
				END;

		FETCH Curs INTO @AccountID, @OldBalance
	END;
	CLOSE Curs;
	DEALLOCATE CURS;

	IF @IsFailed = 1
	BEGIN
		;THROW 51000, 'Failed to update values, all changes will canceled', 1;
	END
END;



CREATE OR ALTER TRIGGER CardTrigger
ON AccountCard
AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CardID int, 
		@AccountID int,
		@OldBalance money,
		@IsFailed bit = 0;

	DECLARE Curs CURSOR LOCAL
	for
	SELECT i.Id, i.AccountId, d.Balance AS OldBalance 
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
			ON a.Id = ac.AccountId
		WHERE a.Id = @AccountID
		GROUP BY a.Balance
		) < 0
		BEGIN
			SET @IsFailed = 1;

			UPDATE AccountCard
			SET Balance = @OldBalance
			WHERE Id = @CardID;
		END;

		FETCH Curs INTO @CardID, @AccountID, @OldBalance
	END;

	CLOSE CURS;
	DEALLOCATE CURS;

	IF @IsFailed = 1
	BEGIN
		;THROW 51000, 'Failed to update values, all changes will canceled', 1;
	END

END;


--  test account trigger

SELECT * FROM Account;

UPDATE Account 
SET Balance = -1
WHERE Id = 5;

SELECT * FROM Account;


--  test account card trigger

SELECT * FROM AccountCard;

UPDATE AccountCard 
SET Balance = 9999
WHERE Id = 9;

SELECT * FROM AccountCard;
