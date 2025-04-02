INSERT INTO Users (FirstName, LastName, Username, Address, Email, Phone, Website, Company)
VALUES (@FirstName, @LastName, @Username, @Address, @Email, @Phone, @Website, @Company);

SELECT * FROM Users WHERE Email = @Email;