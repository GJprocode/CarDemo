CREATE DATABASE AngularCarDemo; --Schema
GO

USE AngularCarDemo;
GO

-- Create Users Table
CREATE TABLE Users (
	Id INT PRIMARY KEY IDENTITY (1,1),
	Email NVARCHAR(255) NOT NULL UNIQUE,
	Password NVARCHAR(255) NOT NULL --Bcrypt hash/salty
	);
	GO

-- Added Username field to table Users 
ALTER TABLE Users
	ADD Username NVARCHAR (255);
	GO


-- Create Cars Table

CREATE TABLE Cars (
	Id INT PRIMARY KEY IDENTITY(1,1),
	CarBrand NVARCHAR(255) NOT NULL,
	CarColour NVARCHAR(255) NOT NULL,
	CarPrice DECIMAL(18,2) NOT NULL,
	ModelDate DATE NOT NULL,
	InStock NVARCHAR(50) NOT NULL, -- Yes/No
	);
	GO


-- Create log table for Serilog 

CREATE TABLE Serilogs (
	Id INT PRIMARY KEY IDENTITY (1,1),
	MESSAGE NVARCHAR (255),
	Level NVARCHAR (255),
	TimeStamp DATETIME  NOT NULL,
	Exception NVARCHAR (255),
	CorrelationId NVARCHAR(36)
	);
	GO

-- Generated random data on https://generatedata.com/generator

INSERT INTO Cars (CarBrand,CarColour,CarPrice,ModelDate,InStock)
	VALUES 
	('Peugeot', 'Blue', 8482.61, '2024-05-26', 'No'),
    ('Audi', 'Blue', 3861.77, '2024-05-05', 'Yes'),
    ('JLR', 'Red', 1806.14, '2024-03-17', 'Yes'),
    ('Chevrolet', 'Green', 5455.99, '2024-07-10', 'No'),
    ('Kia Motors', 'Green', 9732.88, '2024-06-02', 'Yes'),
    ('Daihatsu', 'Blue', 8482.61, '2024-05-26', 'No'),
    ('Chrysler', 'Blue', 3861.77, '2024-05-05', 'Yes'),
    ('Lexus', 'Red', 1806.14, '2024-03-17', 'Yes'),
    ('Toyota', 'Green', 5455.99, '2024-07-10', 'No'),
    ('Lexus', 'Green', 9732.88, '2024-06-02', 'Yes');


	

