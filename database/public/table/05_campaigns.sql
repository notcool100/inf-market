-- Create Campaigns table
CREATE TABLE IF NOT EXISTS Campaigns (
    Id UUID PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    BrandId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    InfluencerId UUID REFERENCES Users(Id),
    Budget DECIMAL(10, 2) NOT NULL,
    StartDate TIMESTAMP NOT NULL,
    EndDate TIMESTAMP NOT NULL,
    Requirements TEXT,
    Deliverables JSONB, -- Stored as JSON array
    TargetAudience JSONB, -- Stored as JSON object
    TargetPlatforms JSONB, -- Stored as JSON array
    Status VARCHAR(20) NOT NULL DEFAULT 'Draft',
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

