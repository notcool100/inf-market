-- Create CampaignDeliverables table
CREATE TABLE IF NOT EXISTS CampaignDeliverables (
    Id UUID PRIMARY KEY,
    CampaignId UUID NOT NULL REFERENCES Campaigns(Id) ON DELETE CASCADE,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    DeliverableType VARCHAR(50) NOT NULL,
    ProofUrl TEXT,
    ScreenshotUrl TEXT,
    FeedbackNotes TEXT,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    DueDate TIMESTAMP NOT NULL,
    SubmittedAt TIMESTAMP,
    ReviewedAt TIMESTAMP,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

