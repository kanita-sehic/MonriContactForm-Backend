UPDATE Users 
SET 
    FirstName = @FirstName,
    LastName = @LastName,
    Username = @Username,
    Address = @Address,
    Email = @Email,
    Phone = @Phone,
    Website = @Website,
    Company = @Company
WHERE 
    Id = @Id;

SELECT * FROM Users WHERE Id = @Id;