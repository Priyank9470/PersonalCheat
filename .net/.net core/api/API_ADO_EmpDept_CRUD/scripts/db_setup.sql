-- ====================================================================
-- Database Setup Script for API_ADO_EmpDept_CRUD
-- Server: PRIYANK_WORLD\SQLEXPRESS
-- Database: EmpDeptDB
-- ====================================================================

-- Create Database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'EmpDeptDB')
BEGIN
    CREATE DATABASE EmpDeptDB;
END
GO

USE EmpDeptDB;
GO

-- Create Departments Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Departments] (
        [DepartmentId]   INT IDENTITY(1,1) NOT NULL,
        [DepartmentName] NVARCHAR(100) NOT NULL,
        [Location]       NVARCHAR(150) NULL,
        [CreatedDate]    DATETIME NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([DepartmentId] ASC),
        CONSTRAINT [UQ_Departments_DepartmentName] UNIQUE ([DepartmentName])
    );
END
GO

-- Create Employees Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employees]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Employees] (
        [EmployeeId]   INT IDENTITY(1,1) NOT NULL,
        [FirstName]    NVARCHAR(50) NOT NULL,
        [LastName]     NVARCHAR(50) NOT NULL,
        [Email]        NVARCHAR(150) NOT NULL,
        [Salary]       DECIMAL(18,2) NOT NULL,
        [DepartmentId] INT NOT NULL,
        [HireDate]     DATETIME NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([EmployeeId] ASC),
        CONSTRAINT [UQ_Employees_Email] UNIQUE ([Email]),
        CONSTRAINT [FK_Employees_Departments] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([DepartmentId]),
        CONSTRAINT [CK_Employees_Salary] CHECK ([Salary] > 0)
    );
END
GO

-- Seed Data (Departments)
IF NOT EXISTS (SELECT * FROM [dbo].[Departments])
BEGIN
    INSERT INTO [dbo].[Departments] ([DepartmentName], [Location], [CreatedDate])
    VALUES 
        (N'Human Resources', N'Building A - Floor 2', GETDATE()),
        (N'Software Development', N'Building B - Floor 4', GETDATE()),
        (N'Finance & Accounting', N'Building A - Floor 3', GETDATE()),
        (N'Marketing & Sales', N'Building C - Floor 1', GETDATE());
END
GO

-- Seed Data (Employees)
IF NOT EXISTS (SELECT * FROM [dbo].[Employees])
BEGIN
    INSERT INTO [dbo].[Employees] ([FirstName], [LastName], [Email], [Salary], [DepartmentId], [HireDate])
    VALUES 
        (N'Priyank', N'Patel', N'priyank.patel@example.com', 85000.00, 2, GETDATE()),
        (N'Rahul', N'Sharma', N'rahul.sharma@example.com', 65000.00, 1, GETDATE()),
        (N'Anita', N'Desai', N'anita.desai@example.com', 72000.00, 3, GETDATE()),
        (N'Vikram', N'Singh', N'vikram.singh@example.com', 90000.00, 2, GETDATE());
END
GO
