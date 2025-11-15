# Phase 1: Foundation & Core Repositories

**Status:** 🔴 NOT STARTED  
**Priority:** CRITICAL - BLOCKER  
**Estimated Time:** 3-5 days  
**Dependencies:** None

## Overview

This phase is CRITICAL as it unblocks all other development. Currently, services reference repositories that don't exist, preventing the application from running.

## Objectives

1. Implement all missing repository classes
2. Fix Program.cs registration issues
3. Ensure database connectivity
4. Test all repository operations

## Current Issues

### Critical Issues
1. **CampaignRepository** - Interface exists, implementation missing
2. **InfluencerProfileRepository** - Interface exists, implementation missing
3. **PaymentRepository** - Interface exists, implementation missing
4. **WalletRepository** - Interface exists, implementation missing
5. **Program.cs Line 65** - Incorrectly registers `CampaignService` as `ICampaignRepository`

### Database Connection
- Connection string configured in appsettings.json
- Need to verify PostgreSQL connection works

## Tasks

### Task 1.1: Implement CampaignRepository
**File:** `backend/src/Infrastructure/Data/CampaignRepository.cs`

**Requirements:**
- Extend `BaseRepository<Campaign>`
- Implement `ICampaignRepository` interface
- Methods to implement:
  - `GetCampaignsByBrandIdAsync(Guid brandId)`
  - `GetCampaignsByInfluencerIdAsync(Guid influencerId)`
  - `AssignInfluencerAsync(Guid campaignId, Guid influencerId)`
  - `UpdateCampaignStatusAsync(Guid campaignId, CampaignStatus status)`
  - `GetAvailableCampaignsAsync()` - Campaigns with status "Open" and no influencer assigned
  - Override `CreateAsync`, `UpdateAsync` for JSONB fields

**Database Considerations:**
- `Deliverables`, `TargetAudience`, `TargetPlatforms` are JSONB - use JSON serialization
- `Status` is stored as VARCHAR - map to/from enum

**Acceptance Criteria:**
- ✅ All methods implemented
- ✅ JSONB fields properly serialized/deserialized
- ✅ Status enum properly mapped
- ✅ Unit tests pass

---

### Task 1.2: Implement InfluencerProfileRepository
**File:** `backend/src/Infrastructure/Data/InfluencerProfileRepository.cs`

**Requirements:**
- Extend `BaseRepository<InfluencerProfile>`
- Implement `IInfluencerProfileRepository` interface
- Methods to implement:
  - `GetByUserIdAsync(Guid userId)`
  - `SearchInfluencersAsync(string nicheFocus, string location, int? minFollowers, decimal? maxRate)`
  - `UpdateRatingAsync(Guid influencerProfileId, double newAverageRating)`
  - `IncrementCompletedCampaignsAsync(Guid influencerProfileId)`
  - Override `CreateAsync`, `UpdateAsync` for JSONB fields

**Database Considerations:**
- `ContentTypes`, `Demographics` are JSONB - use JSON serialization
- Search method needs dynamic SQL with WHERE conditions

**Acceptance Criteria:**
- ✅ All methods implemented
- ✅ JSONB fields properly serialized/deserialized
- ✅ Search functionality works with filters
- ✅ Unit tests pass

---

### Task 1.3: Implement PaymentRepository
**File:** `backend/src/Infrastructure/Data/PaymentRepository.cs`

**Requirements:**
- Extend `BaseRepository<Payment>`
- Implement `IPaymentRepository` interface
- Methods to implement:
  - `GetPaymentsByCampaignIdAsync(Guid campaignId)`
  - `GetPaymentsBySenderIdAsync(Guid senderId)`
  - `GetPaymentsByRecipientIdAsync(Guid recipientId)`
  - `UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status)`
  - Override `CreateAsync`, `UpdateAsync`

**Database Considerations:**
- `Status` is stored as VARCHAR - map to/from enum
- `Type` is stored as VARCHAR - map to/from enum

**Acceptance Criteria:**
- ✅ All methods implemented
- ✅ Status enum properly mapped
- ✅ Unit tests pass

---

### Task 1.4: Implement WalletRepository
**File:** `backend/src/Infrastructure/Data/WalletRepository.cs`

**Requirements:**
- Extend `BaseRepository<Wallet>`
- Implement `IWalletRepository` interface
- Methods to implement:
  - `GetByUserIdAsync(Guid userId)` - Note: Service expects `GetWalletByUserIdAsync`
  - `UpdateBalanceAsync(Guid walletId, decimal newBalance)`
  - `AddTransactionAsync(WalletTransaction transaction)` - Note: Service expects `CreateTransactionAsync`
  - `GetTransactionsByWalletIdAsync(Guid walletId)` - Note: Service expects `GetTransactionsAsync`

**Interface Mismatch Notes:**
- Service uses `GetWalletByUserIdAsync` but interface has `GetByUserIdAsync`
- Service uses `CreateTransactionAsync` but interface has `AddTransactionAsync`
- Service uses `GetTransactionsAsync` but interface has `GetTransactionsByWalletIdAsync`
- **Action Required:** Either update interface or service to match

**Database Considerations:**
- `WalletTransaction.Type` is stored as VARCHAR - map to/from enum

**Acceptance Criteria:**
- ✅ All methods implemented
- ✅ Interface/service naming resolved
- ✅ Transaction enum properly mapped
- ✅ Unit tests pass

---

### Task 1.5: Fix Program.cs Repository Registrations
**File:** `backend/src/API/Program.cs`

**Current Issues:**
- Line 65: `builder.Services.AddScoped<ICampaignRepository, CampaignService>();` - WRONG
- Should be: `builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();`

**Required Changes:**
1. Fix CampaignRepository registration
2. Verify all repository registrations are correct
3. Ensure all repositories are registered before services

**Acceptance Criteria:**
- ✅ All repositories correctly registered
- ✅ No service registered as repository
- ✅ Application starts without errors

---

### Task 1.6: Verify Database Connection
**File:** `backend/src/API/appsettings.json`

**Tasks:**
1. Verify PostgreSQL is running
2. Test database connection
3. Run schema.sql to create database
4. Verify connection string format
5. Test a simple query

**Acceptance Criteria:**
- ✅ Database connection successful
- ✅ All tables created
- ✅ Can execute queries

---

### Task 1.7: Test All Repository Operations
**Create:** `backend/tests/Infrastructure.Tests/RepositoryTests.cs`

**Test Cases:**
1. CampaignRepository CRUD operations
2. InfluencerProfileRepository CRUD operations
3. PaymentRepository CRUD operations
4. WalletRepository CRUD operations
5. JSONB serialization/deserialization
6. Enum mapping
7. Search functionality

**Acceptance Criteria:**
- ✅ All repository tests pass
- ✅ No database errors
- ✅ JSONB fields work correctly

---

## Implementation Notes

### JSONB Handling
PostgreSQL JSONB fields need special handling:
```csharp
// Serialize to JSONB
var jsonString = JsonSerializer.Serialize(object);

// Deserialize from JSONB
var object = JsonSerializer.Deserialize<T>(jsonString);
```

### Enum Mapping
Store enums as strings in database:
```csharp
// To database
var statusString = status.ToString();

// From database
var status = Enum.Parse<CampaignStatus>(statusString);
```

### Dynamic Search Query
For InfluencerProfileRepository search:
```csharp
var conditions = new List<string>();
var parameters = new DynamicParameters();

if (!string.IsNullOrEmpty(nicheFocus))
{
    conditions.Add("NicheFocus = @NicheFocus");
    parameters.Add("NicheFocus", nicheFocus);
}
// ... add other conditions

var sql = $"SELECT * FROM InfluencerProfiles WHERE {string.Join(" AND ", conditions)}";
```

## Dependencies

- ✅ PostgreSQL database
- ✅ Dapper NuGet package (already in project)
- ✅ Npgsql NuGet package (already in project)
- ✅ System.Text.Json (for JSONB)

## Deliverables

1. ✅ CampaignRepository.cs
2. ✅ InfluencerProfileRepository.cs
3. ✅ PaymentRepository.cs
4. ✅ WalletRepository.cs
5. ✅ Fixed Program.cs
6. ✅ Repository unit tests
7. ✅ Database connection verified

## Success Criteria

- ✅ All repositories implemented
- ✅ All services can execute without errors
- ✅ Database operations verified
- ✅ Application starts successfully
- ✅ No compilation errors

## Next Phase

After Phase 1 completion, proceed to **Phase 2: Backend API Completion**

---

*Status: Ready to Start*
*Blocking: All other phases*

