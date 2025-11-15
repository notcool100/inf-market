-- Database: influencer_marketplace
-- Main schema file that orchestrates all database objects

-- Create database (run this separately if needed)
-- CREATE DATABASE influencer_marketplace
--     WITH
--     OWNER = postgres
--     ENCODING = 'UTF8'
--     LC_COLLATE = 'en_US.utf8'
--     LC_CTYPE = 'en_US.utf8'
--     TABLESPACE = pg_default
--     CONNECTION LIMIT = -1;

-- Connect to database
\c influencer_marketplace;

-- Create tables
\i public/table/01_roles.sql
\i public/table/02_users.sql
\i public/table/03_user_roles.sql
\i public/table/04_influencer_profiles.sql
\i public/table/05_campaigns.sql
\i public/table/06_campaign_deliverables.sql
\i public/table/07_wallets.sql
\i public/table/08_payments.sql
\i public/table/09_wallet_transactions.sql
\i public/table/10_reviews.sql
\i public/table/11_notifications.sql
\i public/table/12_permissions.sql
\i public/table/13_role_permissions.sql
\i public/table/14_navigation_groups.sql
\i public/table/15_navigation_items.sql
\i public/table/16_role_navigation.sql

-- Create indexes
\i public/table/indexes.sql

-- Create functions
\i public/function/update_timestamp.sql
\i public/function/get_user_navigation.sql
\i public/function/get_user_permissions.sql

-- Create triggers
\i public/trigger/01_update_timestamps.sql

-- Seed data
\i public/table/seed_data.sql
