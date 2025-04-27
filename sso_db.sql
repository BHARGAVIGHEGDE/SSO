create database  SSOApplication;
use SSOApplication
-- Create the Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    HashPassword NVARCHAR(255) NOT NULL
);
--create userTokens
CREATE TABLE UserTokens (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    Token NVARCHAR(1000),
    ExpirationDate DATETIME,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
--create LoginRequest
CREATE TABLE LoginRequest (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Timestamp DATETIME DEFAULT GETDATE(),
    IsSuccessful BIT
);
--initially add values for the login purpose
INSERT INTO Users (Username, HashPassword)
VALUES ('pinks', 'iii');
