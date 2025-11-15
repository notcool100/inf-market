-- Create Notifications table
CREATE TABLE IF NOT EXISTS Notifications (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Title VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    Type VARCHAR(50) NOT NULL,
    RelatedEntityType VARCHAR(50),
    RelatedEntityId UUID,
    IsRead BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    ReadAt TIMESTAMP
);

