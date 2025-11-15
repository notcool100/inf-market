-- Triggers for updating timestamps

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

CREATE TRIGGER update_permissions_timestamp
BEFORE UPDATE ON permissions
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_navigation_groups_timestamp
BEFORE UPDATE ON navigation_groups
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

CREATE TRIGGER update_navigation_items_timestamp
BEFORE UPDATE ON navigation_items
FOR EACH ROW EXECUTE FUNCTION update_timestamp();

