CREATE DATABASE Fridge;
GO

USE Fridge;

CREATE TABLE FridgeModel (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(100) NOT NULL,
	Year INT NOT NULL
);

CREATE TABLE Product (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	DefaultQuantity INT NOT NULL
);

CREATE TABLE Fridge (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	OwnerName NVARCHAR(100) NOT NULL,
	Model INT CONSTRAINT Fridge_Model_FK REFERENCES FridgeModel(Id)
);

CREATE TABLE FridgeProduct (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Product INT CONSTRAINT FridgeProduct_Product_FK REFERENCES Product(Id) ON DELETE CASCADE,
	Fridge INT CONSTRAINT FridgeProduct_Fridge_FK REFERENCES Fridge(Id) ON DELETE CASCADE,
	Quantity INT NOT NULL
);

CREATE TABLE Role (
	Id INT PRIMARY KEY,
	Name nvarchar(30)
);

CREATE TABLE [User] (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(50) NOT NULL,
	Role INT CONSTRAINT User_Role_FK REFERENCES Role(Id) DEFAULT 10,
	PasswordHash varbinary(256) NOT NULL,
	PasswordSalt varbinary(256) NOT NULL
);

CREATE TABLE Image(
	Id INT PRIMARY KEY CONSTRAINT Image_Product_FK REFERENCES Product(Id) ON DELETE CASCADE,
	Path nvarchar(300) not null
);


GO



CREATE PROCEDURE uspFindEmpty
AS 
SELECT fp.Product, fp.Fridge, p.DefaultQuantity
FROM FridgeProduct fp
JOIN Product p
	ON p.Id = fp.Product;


-- Scaffold-DbContext "Server=(localdb)\mssqllocaldb;Database=Fridge;Trusted_Connection=True;" -OutputDir Entities/temp Microsoft.EntityFrameworkCore.SqlServer


-- insert some info

INSERT INTO FridgeModel(Name,Year)
VALUES 
	('Atlant', 1998),
	('SMEG', 1993),
	('Ice', 2001),
	('Eletrical', 1978),
	('Beko',2005);

INSERT INTO Product(Name,DefaultQuantity)
VALUES
	('Potato', 5),
	('Milk', 2),
	('Cheese', 1),
	('Apple', 9),
	('Waffles', 3);


INSERT INTO Fridge(Name,OwnerName, Model)
VALUES 
	('MX100', 'Alex', 1),
	('QWE321', 'Vladis', 2),
	('Aksel', 'Jej', 2),
	('Berni', 'Olis', 2);


INSERT INTO FridgeProduct(Quantity, Fridge, Product)
VALUES 
	(5, 1, 3),
	(3, 1, 2),
	(1, 2, 1),
	(4, 2, 2),
	(3, 2, 5);

INSERT INTO Role(Id, Name)
VALUES
	(0, 'Admin'),
	(10, 'User');