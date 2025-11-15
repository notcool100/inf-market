# Database Seed Data Documentation

## Overview

This document describes the seed data that populates the database with initial navigation items, permissions, and role mappings.

## Seed Files

1. **seed_data.sql** - Basic roles (Admin, Brand, Influencer)
2. **seed_navigation_permissions.sql** - Navigation groups, items, permissions, and role mappings

## Navigation Groups

The following navigation groups are created:

1. **Main** (Order: 1) - Main navigation items accessible to all users
2. **Campaigns** (Order: 2) - Campaign management navigation
3. **Profile** (Order: 3) - User profile management
4. **Financial** (Order: 4) - Payments and wallet management
5. **Admin** (Order: 5) - Administration panel (Admin only)

## Navigation Items

### Main Navigation (All Users)
- **Dashboard** (`/dashboard`) - Home dashboard
- **Influencers** (`/influencers`) - Browse influencers

### Campaign Navigation

#### Brand Role:
- **My Campaigns** (`/brand/campaigns`) - View brand's campaigns
- **Create Campaign** (`/brand/campaigns/create`) - Create new campaign

#### Influencer Role:
- **Available Campaigns** (`/influencer/campaigns`) - Browse available campaigns
- **My Applications** (`/influencer/applications`) - View campaign applications

### Profile Navigation
- **My Profile** (`/profile`) - View/edit user profile
  - **Edit Profile** (`/profile/edit`) - Edit profile (child item)
- **Influencer Profile** (`/influencer/profile`) - Influencer-specific profile
  - **Create Profile** (`/influencer/profile/create`) - Create influencer profile (child item)

### Financial Navigation
- **Wallet** (`/wallet`) - Wallet management
  - **Transactions** (`/wallet/transactions`) - View transactions (child item)
- **Payments** (`/payments`) - Payment management
  - **Payment History** (`/payments/history`) - View payment history (child item)

### Admin Navigation
- **Admin Dashboard** (`/admin/dashboard`) - Admin overview
- **User Management** (`/admin/users`) - Manage users
- **Campaign Management** (`/admin/campaigns`) - Manage all campaigns
- **Permissions** (`/admin/permissions`) - Manage permissions
- **Navigation Settings** (`/admin/navigation`) - Manage navigation items
- **System Settings** (`/admin/settings`) - System configuration

## Permissions

### Campaign Permissions
- `campaign.view` - View campaigns
- `campaign.create` - Create campaigns
- `campaign.edit` - Edit campaigns
- `campaign.delete` - Delete campaigns
- `campaign.assign` - Assign influencers to campaigns
- `campaign.apply` - Apply to campaigns
- `campaign.approve` - Approve campaign deliverables

### Profile Permissions
- `profile.view` - View influencer profiles
- `profile.create` - Create influencer profile
- `profile.edit` - Edit influencer profile
- `profile.delete` - Delete influencer profile
- `profile.verify` - Verify influencer profiles

### Payment Permissions
- `payment.view` - View payments
- `payment.create` - Create payments
- `payment.process` - Process payments
- `payment.release` - Release escrow payments
- `payment.refund` - Refund payments

### Wallet Permissions
- `wallet.view` - View wallet balance
- `wallet.deposit` - Deposit to wallet
- `wallet.withdraw` - Withdraw from wallet
- `wallet.transfer` - Transfer between wallets

### User Management Permissions
- `user.view` - View users
- `user.create` - Create users
- `user.edit` - Edit users
- `user.delete` - Delete users
- `user.activate` - Activate/deactivate users

### Admin Permissions
- `admin.dashboard` - Access admin dashboard
- `admin.settings` - Manage system settings
- `admin.permissions` - Manage permissions
- `admin.navigation` - Manage navigation
- `admin.roles` - Manage roles

### Review Permissions
- `review.create` - Create reviews
- `review.view` - View reviews
- `review.edit` - Edit reviews
- `review.delete` - Delete reviews

## Role Navigation Mappings

### Admin Role
Has access to **ALL** navigation items including:
- All main navigation
- All campaign navigation
- All profile navigation
- All financial navigation
- All admin navigation

### Brand Role
Has access to:
- Dashboard
- Influencers (browse)
- My Campaigns
- Create Campaign
- My Profile
- Wallet
- Payments

### Influencer Role
Has access to:
- Dashboard
- Available Campaigns
- My Applications
- Influencer Profile
- Create Profile
- Wallet
- Payments

## Role Permission Mappings

### Admin Role
Has **ALL** permissions (34 permissions total):
- All campaign permissions
- All profile permissions
- All payment permissions
- All wallet permissions
- All user management permissions
- All admin permissions
- All review permissions

### Brand Role
Has the following permissions (14 permissions):
- `campaign.view`, `campaign.create`, `campaign.edit`, `campaign.delete`, `campaign.assign`, `campaign.approve`
- `profile.view`
- `payment.view`, `payment.create`, `payment.release`
- `wallet.view`, `wallet.deposit`
- `review.create`, `review.view`

### Influencer Role
Has the following permissions (9 permissions):
- `campaign.view`, `campaign.apply`
- `profile.view`, `profile.create`, `profile.edit`
- `payment.view`
- `wallet.view`, `wallet.withdraw`
- `review.view`

## Usage

To apply the seed data, run:

```bash
psql -U postgres -d influencer_marketplace -f database/schema.sql
```

Or run the seed file directly:

```bash
psql -U postgres -d influencer_marketplace -f database/public/table/seed_navigation_permissions.sql
```

## Notes

- All seed data uses `ON CONFLICT DO NOTHING` to prevent errors on re-runs
- UUIDs are fixed for consistency
- Navigation items support hierarchical structure (parent-child relationships)
- Permissions follow a `module.action` naming convention
- Role mappings determine what users can see and do based on their roles

