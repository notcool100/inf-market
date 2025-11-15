-- Create InfluencerProfiles table
CREATE TABLE IF NOT EXISTS InfluencerProfiles (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL UNIQUE REFERENCES Users(Id) ON DELETE CASCADE,
    Bio TEXT,
    NicheFocus VARCHAR(100),
    FollowersCount INT NOT NULL DEFAULT 0,
    InstagramHandle VARCHAR(100),
    TikTokHandle VARCHAR(100),
    YouTubeChannel VARCHAR(100),
    FacebookPage VARCHAR(100),
    LinkedInProfile VARCHAR(100),
    WebsiteUrl TEXT,
    MinCampaignRate DECIMAL(10, 2) NOT NULL DEFAULT 0,
    ContentTypes JSONB, -- Stored as JSON array
    Demographics JSONB, -- Stored as JSON object
    Location VARCHAR(100),
    IsVerified BOOLEAN NOT NULL DEFAULT FALSE,
    AverageRating FLOAT NOT NULL DEFAULT 0,
    CompletedCampaigns INT NOT NULL DEFAULT 0,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

