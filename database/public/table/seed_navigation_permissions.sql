-- Seed Navigation Groups
INSERT INTO navigation_groups (id, "name", description, "order", "isActive", "createdAt", "updatedAt") VALUES
    ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Main', 'Main navigation items', 1, true, NOW(), NOW()),
    ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Campaigns', 'Campaign management', 2, true, NOW(), NOW()),
    ('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Profile', 'User profile management', 3, true, NOW(), NOW()),
    ('dddddddd-dddd-dddd-dddd-dddddddddddd', 'Financial', 'Payments and wallet', 4, true, NOW(), NOW()),
    ('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'Admin', 'Administration panel', 5, true, NOW(), NOW())
ON CONFLICT (id) DO NOTHING;

-- Seed Navigation Items - Common/Main
INSERT INTO navigation_items (id, "label", icon, url, "order", "parentId", "groupId", "isActive", "createdAt", "updatedAt") VALUES
    -- Main Navigation (All Users)
    ('10000000-0000-0000-0000-000000000001', 'Dashboard', 'home', '/dashboard', 1, NULL, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', true, NOW(), NOW()),
    ('10000000-0000-0000-0000-000000000002', 'Influencers', 'users', '/influencers', 2, NULL, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', true, NOW(), NOW()),
    
    -- Campaign Navigation - Brand
    ('20000000-0000-0000-0000-000000000001', 'My Campaigns', 'briefcase', '/brand/campaigns', 1, NULL, 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', true, NOW(), NOW()),
    ('20000000-0000-0000-0000-000000000002', 'Create Campaign', 'plus-circle', '/brand/campaigns/create', 2, NULL, 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', true, NOW(), NOW()),
    
    -- Campaign Navigation - Influencer
    ('20000000-0000-0000-0000-000000000003', 'Available Campaigns', 'search', '/influencer/campaigns', 1, NULL, 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', true, NOW(), NOW()),
    ('20000000-0000-0000-0000-000000000004', 'My Applications', 'file-text', '/influencer/applications', 2, NULL, 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', true, NOW(), NOW()),
    
    -- Profile Navigation
    ('30000000-0000-0000-0000-000000000001', 'My Profile', 'user', '/profile', 1, NULL, 'cccccccc-cccc-cccc-cccc-cccccccccccc', true, NOW(), NOW()),
    ('30000000-0000-0000-0000-000000000002', 'Edit Profile', 'edit', '/profile/edit', 2, '30000000-0000-0000-0000-000000000001', 'cccccccc-cccc-cccc-cccc-cccccccccccc', true, NOW(), NOW()),
    ('30000000-0000-0000-0000-000000000003', 'Influencer Profile', 'star', '/influencer/profile', 1, NULL, 'cccccccc-cccc-cccc-cccc-cccccccccccc', true, NOW(), NOW()),
    ('30000000-0000-0000-0000-000000000004', 'Create Profile', 'user-plus', '/influencer/profile/create', 2, '30000000-0000-0000-0000-000000000003', 'cccccccc-cccc-cccc-cccc-cccccccccccc', true, NOW(), NOW()),
    
    -- Financial Navigation
    ('40000000-0000-0000-0000-000000000001', 'Wallet', 'wallet', '/wallet', 1, NULL, 'dddddddd-dddd-dddd-dddd-dddddddddddd', true, NOW(), NOW()),
    ('40000000-0000-0000-0000-000000000002', 'Transactions', 'list', '/wallet/transactions', 2, '40000000-0000-0000-0000-000000000001', 'dddddddd-dddd-dddd-dddd-dddddddddddd', true, NOW(), NOW()),
    ('40000000-0000-0000-0000-000000000003', 'Payments', 'credit-card', '/payments', 3, NULL, 'dddddddd-dddd-dddd-dddd-dddddddddddd', true, NOW(), NOW()),
    ('40000000-0000-0000-0000-000000000004', 'Payment History', 'history', '/payments/history', 4, '40000000-0000-0000-0000-000000000003', 'dddddddd-dddd-dddd-dddd-dddddddddddd', true, NOW(), NOW()),
    
    -- Admin Navigation
    ('50000000-0000-0000-0000-000000000001', 'Admin Dashboard', 'shield', '/admin/dashboard', 1, NULL, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', true, NOW(), NOW()),
    ('50000000-0000-0000-0000-000000000002', 'User Management', 'users', '/admin/users', 2, NULL, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', true, NOW(), NOW()),
    ('50000000-0000-0000-0000-000000000003', 'Campaign Management', 'briefcase', '/admin/campaigns', 3, NULL, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', true, NOW(), NOW()),
    ('50000000-0000-0000-0000-000000000004', 'Permissions', 'key', '/admin/permissions', 4, NULL, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', true, NOW(), NOW()),
    ('50000000-0000-0000-0000-000000000005', 'Navigation Settings', 'menu', '/admin/navigation', 5, NULL, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', true, NOW(), NOW()),
    ('50000000-0000-0000-0000-000000000006', 'System Settings', 'settings', '/admin/settings', 6, NULL, 'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', true, NOW(), NOW())
ON CONFLICT (id) DO NOTHING;

-- Seed Permissions
INSERT INTO permissions (id, code, description, "module", "action", "createdAt", "updatedAt") VALUES
    -- Campaign Permissions
    ('perm_campaign_view', 'campaign.view', 'View campaigns', 'Campaign', 'View', NOW(), NOW()),
    ('perm_campaign_create', 'campaign.create', 'Create campaigns', 'Campaign', 'Create', NOW(), NOW()),
    ('perm_campaign_edit', 'campaign.edit', 'Edit campaigns', 'Campaign', 'Edit', NOW(), NOW()),
    ('perm_campaign_delete', 'campaign.delete', 'Delete campaigns', 'Campaign', 'Delete', NOW(), NOW()),
    ('perm_campaign_assign', 'campaign.assign', 'Assign influencers to campaigns', 'Campaign', 'Assign', NOW(), NOW()),
    ('perm_campaign_apply', 'campaign.apply', 'Apply to campaigns', 'Campaign', 'Apply', NOW(), NOW()),
    ('perm_campaign_approve', 'campaign.approve', 'Approve campaign deliverables', 'Campaign', 'Approve', NOW(), NOW()),
    
    -- Influencer Profile Permissions
    ('perm_profile_view', 'profile.view', 'View influencer profiles', 'Profile', 'View', NOW(), NOW()),
    ('perm_profile_create', 'profile.create', 'Create influencer profile', 'Profile', 'Create', NOW(), NOW()),
    ('perm_profile_edit', 'profile.edit', 'Edit influencer profile', 'Profile', 'Edit', NOW(), NOW()),
    ('perm_profile_delete', 'profile.delete', 'Delete influencer profile', 'Profile', 'Delete', NOW(), NOW()),
    ('perm_profile_verify', 'profile.verify', 'Verify influencer profiles', 'Profile', 'Verify', NOW(), NOW()),
    
    -- Payment Permissions
    ('perm_payment_view', 'payment.view', 'View payments', 'Payment', 'View', NOW(), NOW()),
    ('perm_payment_create', 'payment.create', 'Create payments', 'Payment', 'Create', NOW(), NOW()),
    ('perm_payment_process', 'payment.process', 'Process payments', 'Payment', 'Process', NOW(), NOW()),
    ('perm_payment_release', 'payment.release', 'Release escrow payments', 'Payment', 'Release', NOW(), NOW()),
    ('perm_payment_refund', 'payment.refund', 'Refund payments', 'Payment', 'Refund', NOW(), NOW()),
    
    -- Wallet Permissions
    ('perm_wallet_view', 'wallet.view', 'View wallet balance', 'Wallet', 'View', NOW(), NOW()),
    ('perm_wallet_deposit', 'wallet.deposit', 'Deposit to wallet', 'Wallet', 'Deposit', NOW(), NOW()),
    ('perm_wallet_withdraw', 'wallet.withdraw', 'Withdraw from wallet', 'Wallet', 'Withdraw', NOW(), NOW()),
    ('perm_wallet_transfer', 'wallet.transfer', 'Transfer between wallets', 'Wallet', 'Transfer', NOW(), NOW()),
    
    -- User Management Permissions
    ('perm_user_view', 'user.view', 'View users', 'User', 'View', NOW(), NOW()),
    ('perm_user_create', 'user.create', 'Create users', 'User', 'Create', NOW(), NOW()),
    ('perm_user_edit', 'user.edit', 'Edit users', 'User', 'Edit', NOW(), NOW()),
    ('perm_user_delete', 'user.delete', 'Delete users', 'User', 'Delete', NOW(), NOW()),
    ('perm_user_activate', 'user.activate', 'Activate/deactivate users', 'User', 'Activate', NOW(), NOW()),
    
    -- Admin Permissions
    ('perm_admin_dashboard', 'admin.dashboard', 'Access admin dashboard', 'Admin', 'Dashboard', NOW(), NOW()),
    ('perm_admin_settings', 'admin.settings', 'Manage system settings', 'Admin', 'Settings', NOW(), NOW()),
    ('perm_admin_permissions', 'admin.permissions', 'Manage permissions', 'Admin', 'Permissions', NOW(), NOW()),
    ('perm_admin_navigation', 'admin.navigation', 'Manage navigation', 'Admin', 'Navigation', NOW(), NOW()),
    ('perm_admin_roles', 'admin.roles', 'Manage roles', 'Admin', 'Roles', NOW(), NOW()),
    
    -- Review Permissions
    ('perm_review_create', 'review.create', 'Create reviews', 'Review', 'Create', NOW(), NOW()),
    ('perm_review_view', 'review.view', 'View reviews', 'Review', 'View', NOW(), NOW()),
    ('perm_review_edit', 'review.edit', 'Edit reviews', 'Review', 'Edit', NOW(), NOW()),
    ('perm_review_delete', 'review.delete', 'Delete reviews', 'Review', 'Delete', NOW(), NOW())
ON CONFLICT (id) DO NOTHING;

-- Seed Role Navigation - Admin (has access to everything)
INSERT INTO role_navigation (id, "roleId", "navigationItemId", "createdAt") VALUES
    -- Admin gets all navigation items
    ('a1000000-0000-0000-0000-000000000001', '11111111-1111-1111-1111-111111111111', '10000000-0000-0000-0000-000000000001', NOW()), -- Dashboard
    ('a1000000-0000-0000-0000-000000000002', '11111111-1111-1111-1111-111111111111', '10000000-0000-0000-0000-000000000002', NOW()), -- Influencers
    ('a1000000-0000-0000-0000-000000000003', '11111111-1111-1111-1111-111111111111', '20000000-0000-0000-0000-000000000001', NOW()), -- My Campaigns
    ('a1000000-0000-0000-0000-000000000004', '11111111-1111-1111-1111-111111111111', '20000000-0000-0000-0000-000000000002', NOW()), -- Create Campaign
    ('a1000000-0000-0000-0000-000000000005', '11111111-1111-1111-1111-111111111111', '30000000-0000-0000-0000-000000000001', NOW()), -- My Profile
    ('a1000000-0000-0000-0000-000000000006', '11111111-1111-1111-1111-111111111111', '40000000-0000-0000-0000-000000000001', NOW()), -- Wallet
    ('a1000000-0000-0000-0000-000000000007', '11111111-1111-1111-1111-111111111111', '40000000-0000-0000-0000-000000000003', NOW()), -- Payments
    ('a1000000-0000-0000-0000-000000000008', '11111111-1111-1111-1111-111111111111', '50000000-0000-0000-0000-000000000001', NOW()), -- Admin Dashboard
    ('a1000000-0000-0000-0000-000000000009', '11111111-1111-1111-1111-111111111111', '50000000-0000-0000-0000-000000000002', NOW()), -- User Management
    ('a1000000-0000-0000-0000-000000000010', '11111111-1111-1111-1111-111111111111', '50000000-0000-0000-0000-000000000003', NOW()), -- Campaign Management
    ('a1000000-0000-0000-0000-000000000011', '11111111-1111-1111-1111-111111111111', '50000000-0000-0000-0000-000000000004', NOW()), -- Permissions
    ('a1000000-0000-0000-0000-000000000012', '11111111-1111-1111-1111-111111111111', '50000000-0000-0000-0000-000000000005', NOW()), -- Navigation Settings
    ('a1000000-0000-0000-0000-000000000013', '11111111-1111-1111-1111-111111111111', '50000000-0000-0000-0000-000000000006', NOW())  -- System Settings
ON CONFLICT (id) DO NOTHING;

-- Seed Role Navigation - Brand
INSERT INTO role_navigation (id, "roleId", "navigationItemId", "createdAt") VALUES
    ('b1000000-0000-0000-0000-000000000001', '22222222-2222-2222-2222-222222222222', '10000000-0000-0000-0000-000000000001', NOW()), -- Dashboard
    ('b1000000-0000-0000-0000-000000000002', '22222222-2222-2222-2222-222222222222', '10000000-0000-0000-0000-000000000002', NOW()), -- Influencers
    ('b1000000-0000-0000-0000-000000000003', '22222222-2222-2222-2222-222222222222', '20000000-0000-0000-0000-000000000001', NOW()), -- My Campaigns
    ('b1000000-0000-0000-0000-000000000004', '22222222-2222-2222-2222-222222222222', '20000000-0000-0000-0000-000000000002', NOW()), -- Create Campaign
    ('b1000000-0000-0000-0000-000000000005', '22222222-2222-2222-2222-222222222222', '30000000-0000-0000-0000-000000000001', NOW()), -- My Profile
    ('b1000000-0000-0000-0000-000000000006', '22222222-2222-2222-2222-222222222222', '40000000-0000-0000-0000-000000000001', NOW()), -- Wallet
    ('b1000000-0000-0000-0000-000000000007', '22222222-2222-2222-2222-222222222222', '40000000-0000-0000-0000-000000000003', NOW())  -- Payments
ON CONFLICT (id) DO NOTHING;

-- Seed Role Navigation - Influencer
INSERT INTO role_navigation (id, "roleId", "navigationItemId", "createdAt") VALUES
    ('11000000-0000-0000-0000-000000000001', '33333333-3333-3333-3333-333333333333', '10000000-0000-0000-0000-000000000001', NOW()), -- Dashboard
    ('11000000-0000-0000-0000-000000000002', '33333333-3333-3333-3333-333333333333', '20000000-0000-0000-0000-000000000003', NOW()), -- Available Campaigns
    ('11000000-0000-0000-0000-000000000003', '33333333-3333-3333-3333-333333333333', '20000000-0000-0000-0000-000000000004', NOW()), -- My Applications
    ('11000000-0000-0000-0000-000000000004', '33333333-3333-3333-3333-333333333333', '30000000-0000-0000-0000-000000000003', NOW()), -- Influencer Profile
    ('11000000-0000-0000-0000-000000000005', '33333333-3333-3333-3333-333333333333', '30000000-0000-0000-0000-000000000004', NOW()), -- Create Profile
    ('11000000-0000-0000-0000-000000000006', '33333333-3333-3333-3333-333333333333', '40000000-0000-0000-0000-000000000001', NOW()), -- Wallet
    ('11000000-0000-0000-0000-000000000007', '33333333-3333-3333-3333-333333333333', '40000000-0000-0000-0000-000000000003', NOW())  -- Payments
ON CONFLICT (id) DO NOTHING;

-- Seed Role Permissions - Admin (has all permissions)
INSERT INTO role_permissions (id, "roleId", "permissionId", "createdAt") VALUES
    ('a2000000-0000-0000-0000-000000000001', '11111111-1111-1111-1111-111111111111', 'perm_campaign_view', NOW()),
    ('a2000000-0000-0000-0000-000000000002', '11111111-1111-1111-1111-111111111111', 'perm_campaign_create', NOW()),
    ('a2000000-0000-0000-0000-000000000003', '11111111-1111-1111-1111-111111111111', 'perm_campaign_edit', NOW()),
    ('a2000000-0000-0000-0000-000000000004', '11111111-1111-1111-1111-111111111111', 'perm_campaign_delete', NOW()),
    ('a2000000-0000-0000-0000-000000000005', '11111111-1111-1111-1111-111111111111', 'perm_campaign_assign', NOW()),
    ('a2000000-0000-0000-0000-000000000006', '11111111-1111-1111-1111-111111111111', 'perm_campaign_approve', NOW()),
    ('a2000000-0000-0000-0000-000000000007', '11111111-1111-1111-1111-111111111111', 'perm_profile_view', NOW()),
    ('a2000000-0000-0000-0000-000000000008', '11111111-1111-1111-1111-111111111111', 'perm_profile_create', NOW()),
    ('a2000000-0000-0000-0000-000000000009', '11111111-1111-1111-1111-111111111111', 'perm_profile_edit', NOW()),
    ('a2000000-0000-0000-0000-000000000010', '11111111-1111-1111-1111-111111111111', 'perm_profile_delete', NOW()),
    ('a2000000-0000-0000-0000-000000000011', '11111111-1111-1111-1111-111111111111', 'perm_profile_verify', NOW()),
    ('a2000000-0000-0000-0000-000000000012', '11111111-1111-1111-1111-111111111111', 'perm_payment_view', NOW()),
    ('a2000000-0000-0000-0000-000000000013', '11111111-1111-1111-1111-111111111111', 'perm_payment_create', NOW()),
    ('a2000000-0000-0000-0000-000000000014', '11111111-1111-1111-1111-111111111111', 'perm_payment_process', NOW()),
    ('a2000000-0000-0000-0000-000000000015', '11111111-1111-1111-1111-111111111111', 'perm_payment_release', NOW()),
    ('a2000000-0000-0000-0000-000000000016', '11111111-1111-1111-1111-111111111111', 'perm_payment_refund', NOW()),
    ('a2000000-0000-0000-0000-000000000017', '11111111-1111-1111-1111-111111111111', 'perm_wallet_view', NOW()),
    ('a2000000-0000-0000-0000-000000000018', '11111111-1111-1111-1111-111111111111', 'perm_wallet_deposit', NOW()),
    ('a2000000-0000-0000-0000-000000000019', '11111111-1111-1111-1111-111111111111', 'perm_wallet_withdraw', NOW()),
    ('a2000000-0000-0000-0000-000000000020', '11111111-1111-1111-1111-111111111111', 'perm_wallet_transfer', NOW()),
    ('a2000000-0000-0000-0000-000000000021', '11111111-1111-1111-1111-111111111111', 'perm_user_view', NOW()),
    ('a2000000-0000-0000-0000-000000000022', '11111111-1111-1111-1111-111111111111', 'perm_user_create', NOW()),
    ('a2000000-0000-0000-0000-000000000023', '11111111-1111-1111-1111-111111111111', 'perm_user_edit', NOW()),
    ('a2000000-0000-0000-0000-000000000024', '11111111-1111-1111-1111-111111111111', 'perm_user_delete', NOW()),
    ('a2000000-0000-0000-0000-000000000025', '11111111-1111-1111-1111-111111111111', 'perm_user_activate', NOW()),
    ('a2000000-0000-0000-0000-000000000026', '11111111-1111-1111-1111-111111111111', 'perm_admin_dashboard', NOW()),
    ('a2000000-0000-0000-0000-000000000027', '11111111-1111-1111-1111-111111111111', 'perm_admin_settings', NOW()),
    ('a2000000-0000-0000-0000-000000000028', '11111111-1111-1111-1111-111111111111', 'perm_admin_permissions', NOW()),
    ('a2000000-0000-0000-0000-000000000029', '11111111-1111-1111-1111-111111111111', 'perm_admin_navigation', NOW()),
    ('a2000000-0000-0000-0000-000000000030', '11111111-1111-1111-1111-111111111111', 'perm_admin_roles', NOW()),
    ('a2000000-0000-0000-0000-000000000031', '11111111-1111-1111-1111-111111111111', 'perm_review_create', NOW()),
    ('a2000000-0000-0000-0000-000000000032', '11111111-1111-1111-1111-111111111111', 'perm_review_view', NOW()),
    ('a2000000-0000-0000-0000-000000000033', '11111111-1111-1111-1111-111111111111', 'perm_review_edit', NOW()),
    ('a2000000-0000-0000-0000-000000000034', '11111111-1111-1111-1111-111111111111', 'perm_review_delete', NOW())
ON CONFLICT (id) DO NOTHING;

-- Seed Role Permissions - Brand
INSERT INTO role_permissions (id, "roleId", "permissionId", "createdAt") VALUES
    ('b2000000-0000-0000-0000-000000000001', '22222222-2222-2222-2222-222222222222', 'perm_campaign_view', NOW()),
    ('b2000000-0000-0000-0000-000000000002', '22222222-2222-2222-2222-222222222222', 'perm_campaign_create', NOW()),
    ('b2000000-0000-0000-0000-000000000003', '22222222-2222-2222-2222-222222222222', 'perm_campaign_edit', NOW()),
    ('b2000000-0000-0000-0000-000000000004', '22222222-2222-2222-2222-222222222222', 'perm_campaign_delete', NOW()),
    ('b2000000-0000-0000-0000-000000000005', '22222222-2222-2222-2222-222222222222', 'perm_campaign_assign', NOW()),
    ('b2000000-0000-0000-0000-000000000006', '22222222-2222-2222-2222-222222222222', 'perm_campaign_approve', NOW()),
    ('b2000000-0000-0000-0000-000000000007', '22222222-2222-2222-2222-222222222222', 'perm_profile_view', NOW()),
    ('b2000000-0000-0000-0000-000000000008', '22222222-2222-2222-2222-222222222222', 'perm_payment_view', NOW()),
    ('b2000000-0000-0000-0000-000000000009', '22222222-2222-2222-2222-222222222222', 'perm_payment_create', NOW()),
    ('b2000000-0000-0000-0000-000000000010', '22222222-2222-2222-2222-222222222222', 'perm_payment_release', NOW()),
    ('b2000000-0000-0000-0000-000000000011', '22222222-2222-2222-2222-222222222222', 'perm_wallet_view', NOW()),
    ('b2000000-0000-0000-0000-000000000012', '22222222-2222-2222-2222-222222222222', 'perm_wallet_deposit', NOW()),
    ('b2000000-0000-0000-0000-000000000013', '22222222-2222-2222-2222-222222222222', 'perm_review_create', NOW()),
    ('b2000000-0000-0000-0000-000000000014', '22222222-2222-2222-2222-222222222222', 'perm_review_view', NOW())
ON CONFLICT (id) DO NOTHING;

-- Seed Role Permissions - Influencer
INSERT INTO role_permissions (id, "roleId", "permissionId", "createdAt") VALUES
    ('i2000000-0000-0000-0000-000000000001', '33333333-3333-3333-3333-333333333333', 'perm_campaign_view', NOW()),
    ('i2000000-0000-0000-0000-000000000002', '33333333-3333-3333-3333-333333333333', 'perm_campaign_apply', NOW()),
    ('i2000000-0000-0000-0000-000000000003', '33333333-3333-3333-3333-333333333333', 'perm_profile_view', NOW()),
    ('i2000000-0000-0000-0000-000000000004', '33333333-3333-3333-3333-333333333333', 'perm_profile_create', NOW()),
    ('i2000000-0000-0000-0000-000000000005', '33333333-3333-3333-3333-333333333333', 'perm_profile_edit', NOW()),
    ('i2000000-0000-0000-0000-000000000006', '33333333-3333-3333-3333-333333333333', 'perm_payment_view', NOW()),
    ('i2000000-0000-0000-0000-000000000007', '33333333-3333-3333-3333-333333333333', 'perm_wallet_view', NOW()),
    ('i2000000-0000-0000-0000-000000000008', '33333333-3333-3333-3333-333333333333', 'perm_wallet_withdraw', NOW()),
    ('i2000000-0000-0000-0000-000000000009', '33333333-3333-3333-3333-333333333333', 'perm_review_view', NOW())
ON CONFLICT (id) DO NOTHING;

