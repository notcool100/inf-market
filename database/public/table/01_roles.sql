-- Create Roles table
CREATE TABLE IF NOT EXISTS Roles (
    Id UUID PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255)
);

