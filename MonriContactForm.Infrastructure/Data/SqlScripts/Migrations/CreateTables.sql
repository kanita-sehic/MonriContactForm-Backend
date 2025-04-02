﻿IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
	CREATE TABLE Users
	(
		Id INT PRIMARY KEY IDENTITY,
		FirstName NVARCHAR(150) NOT NULL,
		LastName NVARCHAR(150) NOT NULL,
		Username NVARCHAR(150) NULL,
		Email NVARCHAR(250) NOT NULL UNIQUE,
		Phone NVARCHAR(50) NULL,
		Website NVARCHAR(450) NULL,
		Company NVARCHAR(250) NULL,
		Address NVARCHAR(450) NULL,
	);
END