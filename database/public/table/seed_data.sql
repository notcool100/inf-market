-- Seed default roles
INSERT INTO Roles (Id, Name, Description) VALUES 
    ('11111111-1111-1111-1111-111111111111', 'Admin', 'System administrator'),
    ('22222222-2222-2222-2222-222222222222', 'Brand', 'Brand/Vendor user'),
    ('33333333-3333-3333-3333-333333333333', 'Influencer', 'Influencer/Creator user')
ON CONFLICT (Id) DO NOTHING;

-- Note: Navigation, Permissions, and Role mappings are seeded in seed_navigation_permissions.sql

