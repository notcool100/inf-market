-- Create Payments table
CREATE TABLE IF NOT EXISTS Payments (
    Id UUID PRIMARY KEY,
    CampaignId UUID REFERENCES Campaigns(Id) ON DELETE SET NULL,
    SenderId UUID NOT NULL REFERENCES Users(Id),
    RecipientId UUID NOT NULL REFERENCES Users(Id),
    Amount DECIMAL(10, 2) NOT NULL,
    CommissionAmount DECIMAL(10, 2) NOT NULL DEFAULT 0,
    NetAmount DECIMAL(10, 2) NOT NULL,
    Currency VARCHAR(3) NOT NULL DEFAULT 'NPR',
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    Type VARCHAR(20) NOT NULL,
    TransactionReference VARCHAR(100),
    PaymentMethod VARCHAR(50) NOT NULL,
    Notes TEXT,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    CompletedAt TIMESTAMP,
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

