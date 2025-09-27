-- Database: influencer_marketplace

CREATE DATABASE influencer_marketplace
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'en_US.utf8'
    LC_CTYPE = 'en_US.utf8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

\c influencer_marketplace;

-- Create Roles table
CREATE TABLE Roles (
    Id UUID PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255)
);

-- Create Users table
CREATE TABLE Users (
    Id UUID PRIMARY KEY,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(20),
    ProfilePictureUrl TEXT,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    IsActive BOOLEAN NOT NULL DEFAULT TRUE
);

-- Create UserRoles table
CREATE TABLE UserRoles (
    UserId UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    RoleId UUID NOT NULL REFERENCES Roles(Id) ON DELETE CASCADE,
    PRIMARY KEY (UserId, RoleId)
);

-- Create InfluencerProfiles table
CREATE TABLE InfluencerProfiles (
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

-- Create Campaigns table
CREATE TABLE Campaigns (
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

-- Create CampaignDeliverables table
CREATE TABLE CampaignDeliverables (
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

-- Create Wallets table
CREATE TABLE Wallets (
    Id UUID PRIMARY KEY,
    UserId UUID NOT NULL UNIQUE REFERENCES Users(Id) ON DELETE CASCADE,
    Balance DECIMAL(10, 2) NOT NULL DEFAULT 0,
    Currency VARCHAR(3) NOT NULL DEFAULT 'NPR',
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create Payments table
CREATE TABLE Payments (
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

-- Create WalletTransactions table
CREATE TABLE WalletTransactions (
    Id UUID PRIMARY KEY,
    WalletId UUID NOT NULL REFERENCES Wallets(Id) ON DELETE CASCADE,
    PaymentId UUID REFERENCES Payments(Id) ON DELETE SET NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    BalanceAfter DECIMAL(10, 2) NOT NULL,
    Type VARCHAR(20) NOT NULL,
    Description TEXT,
    Reference VARCHAR(100),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create Reviews table
CREATE TABLE Reviews (
    Id UUID PRIMARY KEY,
    CampaignId UUID NOT NULL REFERENCES Campaigns(Id) ON DELETE CASCADE,
    ReviewerId UUID NOT NULL REFERENCES Users(Id),
    InfluencerProfileId UUID NOT NULL REFERENCES InfluencerProfiles(Id) ON DELETE CASCADE,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment TEXT,
    IsPublic BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create Notifications table
CREATE TABLE Notifications (
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

-- Insert default roles
INSERT INTO Roles (Id, Name, Description) VALUES 
    ('11111111-1111-1111-1111-111111111111', 'Admin', 'System administrator'),
    ('22222222-2222-2222-2222-222222222222', 'Brand', 'Brand/Vendor user'),
    ('33333333-3333-3333-3333-333333333333', 'Influencer', 'Influencer/Creator user');

-- Create indexes
CREATE INDEX idx_users_email ON Users(Email);
CREATE INDEX idx_influencer_profiles_user_id ON InfluencerProfiles(UserId);
CREATE INDEX idx_campaigns_brand_id ON Campaigns(BrandId);
CREATE INDEX idx_campaigns_influencer_id ON Campaigns(InfluencerId);
CREATE INDEX idx_campaign_deliverables_campaign_id ON CampaignDeliverables(CampaignId);
CREATE INDEX idx_payments_campaign_id ON Payments(CampaignId);
CREATE INDEX idx_payments_sender_id ON Payments(SenderId);
CREATE INDEX idx_payments_recipient_id ON Payments(RecipientId);
CREATE INDEX idx_wallet_transactions_wallet_id ON WalletTransactions(WalletId);
CREATE INDEX idx_reviews_campaign_id ON Reviews(CampaignId);
CREATE INDEX idx_reviews_influencer_profile_id ON Reviews(InfluencerProfileId);
CREATE INDEX idx_notifications_user_id ON Notifications(UserId);

-- Create triggers for updating timestamps
CREATE OR REPLACE FUNCTION update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_users_timestamp
BEFORE UPDATE ON Users
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_influencer_profiles_timestamp
BEFORE UPDATE ON InfluencerProfiles
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_campaigns_timestamp
BEFORE UPDATE ON Campaigns
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_campaign_deliverables_timestamp
BEFORE UPDATE ON CampaignDeliverables
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_wallets_timestamp
BEFORE UPDATE ON Wallets
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_payments_timestamp
BEFORE UPDATE ON Payments
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_reviews_timestamp
BEFORE UPDATE ON Reviews
FOR EACH ROW EXECUTE FUNCTION update_timestamp();