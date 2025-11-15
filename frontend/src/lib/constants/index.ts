/**
 * Application Constants
 */

export const CAMPAIGN_STATUSES = {
  DRAFT: 'Draft',
  OPEN: 'Open',
  IN_PROGRESS: 'InProgress',
  PENDING_REVIEW: 'PendingReview',
  COMPLETED: 'Completed',
  CANCELLED: 'Cancelled',
  DISPUTED: 'Disputed',
} as const;

export const DELIVERABLE_STATUSES = {
  PENDING: 'Pending',
  SUBMITTED: 'Submitted',
  APPROVED: 'Approved',
  REJECTED: 'Rejected',
} as const;

export const PAYMENT_STATUSES = {
  PENDING: 'Pending',
  IN_ESCROW: 'InEscrow',
  RELEASED: 'Released',
  REFUNDED: 'Refunded',
  FAILED: 'Failed',
} as const;

export const USER_ROLES = {
  BRAND: 'Brand',
  INFLUENCER: 'Influencer',
  ADMIN: 'Admin',
} as const;

export const NOTIFICATION_TYPES = {
  CAMPAIGN_INVITATION: 'CampaignInvitation',
  CAMPAIGN_ACCEPTED: 'CampaignAccepted',
  CAMPAIGN_REJECTED: 'CampaignRejected',
  DELIVERABLE_SUBMITTED: 'DeliverableSubmitted',
  DELIVERABLE_APPROVED: 'DeliverableApproved',
  DELIVERABLE_REJECTED: 'DeliverableRejected',
  PAYMENT_RECEIVED: 'PaymentReceived',
  PAYMENT_RELEASED: 'PaymentReleased',
  REVIEW_RECEIVED: 'ReviewReceived',
  SYSTEM_NOTIFICATION: 'SystemNotification',
} as const;

export const PLATFORMS = ['Instagram', 'TikTok', 'YouTube', 'Facebook', 'LinkedIn', 'Twitter'] as const;

export const NICHE_OPTIONS = [
  'Fashion',
  'Beauty',
  'Fitness',
  'Food',
  'Travel',
  'Technology',
  'Gaming',
  'Lifestyle',
  'Business',
  'Education',
  'Entertainment',
  'Sports',
  'Health',
  'Finance',
  'Other',
] as const;

export const CONTENT_TYPES = [
  'Posts',
  'Stories',
  'Reels',
  'Videos',
  'Blog Posts',
  'Product Reviews',
  'Tutorials',
  'Live Streams',
] as const;

