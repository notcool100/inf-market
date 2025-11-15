-- Create indexes for performance optimization

CREATE INDEX IF NOT EXISTS idx_users_email ON Users(Email);
CREATE INDEX IF NOT EXISTS idx_influencer_profiles_user_id ON InfluencerProfiles(UserId);
CREATE INDEX IF NOT EXISTS idx_campaigns_brand_id ON Campaigns(BrandId);
CREATE INDEX IF NOT EXISTS idx_campaigns_influencer_id ON Campaigns(InfluencerId);
CREATE INDEX IF NOT EXISTS idx_campaigns_status ON Campaigns(Status);
CREATE INDEX IF NOT EXISTS idx_campaign_deliverables_campaign_id ON CampaignDeliverables(CampaignId);
CREATE INDEX IF NOT EXISTS idx_payments_campaign_id ON Payments(CampaignId);
CREATE INDEX IF NOT EXISTS idx_payments_sender_id ON Payments(SenderId);
CREATE INDEX IF NOT EXISTS idx_payments_recipient_id ON Payments(RecipientId);
CREATE INDEX IF NOT EXISTS idx_payments_status ON Payments(Status);
CREATE INDEX IF NOT EXISTS idx_wallet_transactions_wallet_id ON WalletTransactions(WalletId);
CREATE INDEX IF NOT EXISTS idx_wallet_transactions_type ON WalletTransactions(Type);
CREATE INDEX IF NOT EXISTS idx_reviews_campaign_id ON Reviews(CampaignId);
CREATE INDEX IF NOT EXISTS idx_reviews_influencer_profile_id ON Reviews(InfluencerProfileId);
CREATE INDEX IF NOT EXISTS idx_notifications_user_id ON Notifications(UserId);
CREATE INDEX IF NOT EXISTS idx_notifications_is_read ON Notifications(IsRead);
CREATE INDEX IF NOT EXISTS idx_user_roles_user_id ON UserRoles(UserId);
CREATE INDEX IF NOT EXISTS idx_user_roles_role_id ON UserRoles(RoleId);
CREATE INDEX IF NOT EXISTS idx_role_permissions_role_id ON role_permissions("roleId");
CREATE INDEX IF NOT EXISTS idx_role_navigation_role_id ON role_navigation("roleId");
CREATE INDEX IF NOT EXISTS idx_role_navigation_navigation_item_id ON role_navigation("navigationItemId");

