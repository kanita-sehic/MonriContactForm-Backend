IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ContactForm')
BEGIN
	CREATE DATABASE ContactForm;
END