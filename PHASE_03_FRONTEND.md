# Phase 3: Frontend Core Features

**Status:** 🔴 NOT STARTED  
**Priority:** HIGH  
**Estimated Time:** 7-10 days  
**Dependencies:** Phase 2 (Backend API Completion)

## Overview

Complete all essential frontend pages, implement core user workflows, and add form validation and error handling.

## Objectives

1. Complete all essential frontend pages
2. Implement core user workflows
3. Add form validation and error handling
4. Ensure good user experience

## Tasks

### Task 3.1: Campaign Listing Page
**File:** `frontend/src/routes/brand/campaigns/+page.svelte`

**Features:**
- List all campaigns for the brand
- Filter by status
- Search functionality
- Pagination
- Create campaign button
- Status badges
- Quick actions (view, edit, delete)

**API Integration:**
- `GET /api/campaign/brand` - Get brand campaigns
- `DELETE /api/campaign/{id}` - Delete campaign

**Acceptance Criteria:**
- ✅ Campaigns displayed correctly
- ✅ Filtering works
- ✅ Pagination functional
- ✅ Actions work properly

---

### Task 3.2: Campaign Detail Page
**File:** `frontend/src/routes/brand/campaigns/[id]/+page.svelte` (new)

**Features:**
- Display campaign details
- Show deliverables list
- Show assigned influencer (if any)
- Show payment status
- Actions: Edit, Delete, Update Status
- For brands: Approve/reject deliverables
- For influencers: Submit deliverables

**API Integration:**
- `GET /api/campaign/{id}` - Get campaign
- `GET /api/campaigns/{id}/deliverables` - Get deliverables
- `PUT /api/campaign/{id}` - Update campaign
- `PUT /api/campaign/{id}/status` - Update status

**Acceptance Criteria:**
- ✅ All campaign details displayed
- ✅ Deliverables shown
- ✅ Actions functional
- ✅ Role-based UI

---

### Task 3.3: Campaign Edit Page
**File:** `frontend/src/routes/brand/campaigns/[id]/edit/+page.svelte` (new)

**Features:**
- Pre-populate form with existing data
- Same form structure as create page
- Update campaign on submit
- Cancel button returns to detail page

**API Integration:**
- `GET /api/campaign/{id}` - Load campaign data
- `PUT /api/campaign/{id}` - Update campaign

**Acceptance Criteria:**
- ✅ Form pre-populated
- ✅ Update works correctly
- ✅ Validation works

---

### Task 3.4: Influencer Search/Browse Page
**File:** `frontend/src/routes/influencers/+page.svelte` (enhance existing)

**Features:**
- Search influencers by niche, location, followers
- Filter by minimum rate, followers count
- Display influencer cards with:
  - Profile picture
  - Name and bio
  - Niche
  - Followers count
  - Average rating
  - Minimum rate
- Click to view profile
- Pagination

**API Integration:**
- `GET /api/influencerprofile/search` - Search influencers

**Acceptance Criteria:**
- ✅ Search works correctly
- ✅ Filters applied
- ✅ Cards display properly
- ✅ Navigation to profile works

---

### Task 3.5: Influencer Profile Detail Page
**File:** `frontend/src/routes/influencers/[id]/+page.svelte` (new)

**Features:**
- Display full influencer profile
- Social media links
- Reviews/ratings
- Completed campaigns count
- Contact/apply button (for brands)
- Edit button (for own profile)

**API Integration:**
- `GET /api/influencerprofile/{id}` - Get profile
- `GET /api/influencers/{id}/reviews` - Get reviews
- `POST /api/campaign/{id}/apply` - Apply to campaign (if applicable)

**Acceptance Criteria:**
- ✅ Profile displayed correctly
- ✅ Reviews shown
- ✅ Actions work

---

### Task 3.6: Influencer Profile Edit Page
**File:** `frontend/src/routes/influencer/profile/edit/+page.svelte` (new)

**Features:**
- Pre-populate form with existing data
- Same form structure as create page
- Update profile on submit
- Cancel button returns to profile

**API Integration:**
- `GET /api/influencerprofile/me` - Load profile
- `PUT /api/influencerprofile/{id}` - Update profile

**Acceptance Criteria:**
- ✅ Form pre-populated
- ✅ Update works correctly
- ✅ Validation works

---

### Task 3.7: Wallet Page
**File:** `frontend/src/routes/wallet/+page.svelte` (enhance existing)

**Features:**
- Display current balance
- Transaction history table
- Deposit button (opens modal)
- Withdraw button (opens modal)
- Transfer button (opens modal)
- Filter transactions by type/date
- Export transactions (optional)

**API Integration:**
- `GET /api/wallet` - Get wallet
- `GET /api/wallet/transactions` - Get transactions
- `POST /api/wallet/deposit` - Deposit
- `POST /api/wallet/withdraw` - Withdraw
- `POST /api/wallet/transfer` - Transfer

**Components:**
- Create `DepositModal.svelte`
- Create `WithdrawModal.svelte`
- Create `TransferModal.svelte`

**Acceptance Criteria:**
- ✅ Balance displayed
- ✅ Transactions listed
- ✅ Deposit/withdraw/transfer work
- ✅ Modals functional

---

### Task 3.8: Payment History Page
**File:** `frontend/src/routes/payments/+page.svelte` (new)

**Features:**
- List all payments (sent and received)
- Filter by type, status, date
- Show payment details
- Link to related campaign
- Export functionality

**API Integration:**
- `GET /api/payment/sent` - Get sent payments
- `GET /api/payment/received` - Get received payments

**Acceptance Criteria:**
- ✅ Payments displayed
- ✅ Filters work
- ✅ Details accessible

---

### Task 3.9: Campaign Deliverable Submission Page
**File:** `frontend/src/routes/influencer/campaigns/[id]/deliverables/+page.svelte` (new)

**Features:**
- List deliverables for campaign
- Submit proof/screenshot for each deliverable
- Upload images
- Add notes
- Submit button
- View submission status

**API Integration:**
- `GET /api/campaigns/{id}/deliverables` - Get deliverables
- `POST /api/deliverables/{id}/submit` - Submit deliverable
- `POST /api/upload/image` - Upload image

**Acceptance Criteria:**
- ✅ Deliverables listed
- ✅ File upload works
- ✅ Submission successful

---

### Task 3.10: Campaign Approval/Rejection UI
**Enhance:** `frontend/src/routes/brand/campaigns/[id]/+page.svelte`

**Features:**
- View submitted deliverables
- Approve button
- Reject button (with feedback)
- View proof images
- Status indicators

**API Integration:**
- `POST /api/deliverables/{id}/approve` - Approve
- `POST /api/deliverables/{id}/reject` - Reject

**Components:**
- Create `DeliverableReviewModal.svelte`

**Acceptance Criteria:**
- ✅ Deliverables reviewable
- ✅ Approve/reject work
- ✅ Feedback can be added

---

### Task 3.11: Form Validation Enhancements
**Files:** All form pages

**Features:**
- Client-side validation
- Real-time error messages
- Required field indicators
- Format validation (email, phone, etc.)
- Custom validation rules
- Disable submit on invalid form

**Libraries:**
- Consider using a validation library (e.g., Yup, Zod)
- Or implement custom validation

**Acceptance Criteria:**
- ✅ All forms validated
- ✅ Error messages clear
- ✅ User-friendly validation

---

### Task 3.12: Error Handling and User Feedback
**Files:** All pages

**Features:**
- Global error handler
- Toast notifications for success/error
- Loading states
- Error boundaries
- Retry mechanisms
- Offline detection

**Components:**
- Create `Toast.svelte` component
- Create `ErrorBoundary.svelte` component
- Enhance `LoadingSpinner.svelte`

**Acceptance Criteria:**
- ✅ Errors handled gracefully
- ✅ User feedback provided
- ✅ Loading states shown

---

## Implementation Notes

### Toast Notification System
```typescript
// Create toast store
import { writable } from 'svelte/store';

export const toastStore = writable([]);

export function showToast(message: string, type: 'success' | 'error' | 'info') {
  toastStore.update(toasts => [...toasts, { id: Date.now(), message, type }]);
}
```

### Form Validation Pattern
```typescript
function validateForm() {
  const errors = {};
  
  if (!email) errors.email = 'Email is required';
  else if (!isValidEmail(email)) errors.email = 'Invalid email format';
  
  return errors;
}
```

### File Upload Component
```svelte
<script>
  let file;
  let uploading = false;
  
  async function handleUpload() {
    uploading = true;
    const formData = new FormData();
    formData.append('file', file);
    
    try {
      const response = await apiClient.post('/api/upload/image', formData);
      // Handle success
    } catch (error) {
      // Handle error
    } finally {
      uploading = false;
    }
  }
</script>
```

## Dependencies

- ✅ Phase 2 completed (all API endpoints available)
- ✅ File upload API working
- ✅ Image handling library (if needed)

## Deliverables

1. ✅ Campaign listing page
2. ✅ Campaign detail page
3. ✅ Campaign edit page
4. ✅ Influencer search page
5. ✅ Influencer profile detail page
6. ✅ Influencer profile edit page
7. ✅ Wallet page
8. ✅ Payment history page
9. ✅ Deliverable submission page
10. ✅ Campaign approval/rejection UI
11. ✅ Form validation
12. ✅ Error handling and feedback

## Success Criteria

- ✅ All core pages implemented
- ✅ User workflows functional
- ✅ Good user experience
- ✅ Forms validated
- ✅ Errors handled gracefully

## Next Phase

After Phase 3 completion, proceed to **Phase 4: Advanced Features**

---

*Status: Waiting for Phase 2*
*Dependencies: Phase 2 must be completed first*

