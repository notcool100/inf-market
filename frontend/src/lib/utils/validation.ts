/**
 * Form Validation Utilities
 * Centralized validation functions
 */

export interface ValidationError {
  field: string;
  message: string;
}

export interface ValidationResult {
  isValid: boolean;
  errors: ValidationError[];
}

/**
 * Email validation
 */
export function isValidEmail(email: string): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

/**
 * Phone number validation (basic)
 */
export function isValidPhone(phone: string): boolean {
  const phoneRegex = /^[\d\s\-\+\(\)]+$/;
  return phoneRegex.test(phone) && phone.replace(/\D/g, '').length >= 10;
}

/**
 * URL validation
 */
export function isValidUrl(url: string): boolean {
  try {
    new URL(url);
    return true;
  } catch {
    return false;
  }
}

/**
 * Required field validation
 */
export function isRequired(value: any): boolean {
  if (typeof value === 'string') {
    return value.trim().length > 0;
  }
  if (Array.isArray(value)) {
    return value.length > 0;
  }
  return value !== null && value !== undefined;
}

/**
 * Minimum length validation
 */
export function minLength(value: string, min: number): boolean {
  return value.length >= min;
}

/**
 * Maximum length validation
 */
export function maxLength(value: string, max: number): boolean {
  return value.length <= max;
}

/**
 * Number range validation
 */
export function isInRange(value: number, min: number, max: number): boolean {
  return value >= min && value <= max;
}

/**
 * Password strength validation
 */
export function isStrongPassword(password: string): boolean {
  // At least 8 characters, 1 uppercase, 1 lowercase, 1 number
  const strongPasswordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;
  return strongPasswordRegex.test(password);
}

/**
 * Validate campaign form
 */
export function validateCampaign(data: {
  title: string;
  description: string;
  budget: number;
  startDate: string;
  endDate: string;
  requirements: string;
  deliverables: string[];
  targetPlatforms: string[];
}): ValidationResult {
  const errors: ValidationError[] = [];

  if (!isRequired(data.title)) {
    errors.push({ field: 'title', message: 'Title is required' });
  } else if (!minLength(data.title, 3)) {
    errors.push({ field: 'title', message: 'Title must be at least 3 characters' });
  }

  if (!isRequired(data.description)) {
    errors.push({ field: 'description', message: 'Description is required' });
  } else if (!minLength(data.description, 10)) {
    errors.push({ field: 'description', message: 'Description must be at least 10 characters' });
  }

  if (!data.budget || data.budget <= 0) {
    errors.push({ field: 'budget', message: 'Budget must be greater than 0' });
  }

  if (!isRequired(data.startDate)) {
    errors.push({ field: 'startDate', message: 'Start date is required' });
  }

  if (!isRequired(data.endDate)) {
    errors.push({ field: 'endDate', message: 'End date is required' });
  } else if (data.startDate && new Date(data.endDate) <= new Date(data.startDate)) {
    errors.push({ field: 'endDate', message: 'End date must be after start date' });
  }

  if (!isRequired(data.requirements)) {
    errors.push({ field: 'requirements', message: 'Requirements are required' });
  }

  if (!data.deliverables || data.deliverables.length === 0) {
    errors.push({ field: 'deliverables', message: 'At least one deliverable is required' });
  }

  if (!data.targetPlatforms || data.targetPlatforms.length === 0) {
    errors.push({ field: 'targetPlatforms', message: 'At least one target platform is required' });
  }

  return {
    isValid: errors.length === 0,
    errors,
  };
}

/**
 * Validate influencer profile form
 */
export function validateInfluencerProfile(data: {
  bio: string;
  nicheFocus: string;
  followersCount: number;
  minCampaignRate: number;
  location: string;
}): ValidationResult {
  const errors: ValidationError[] = [];

  if (!isRequired(data.bio)) {
    errors.push({ field: 'bio', message: 'Bio is required' });
  } else if (!minLength(data.bio, 20)) {
    errors.push({ field: 'bio', message: 'Bio must be at least 20 characters' });
  }

  if (!isRequired(data.nicheFocus)) {
    errors.push({ field: 'nicheFocus', message: 'Niche focus is required' });
  }

  if (!data.followersCount || data.followersCount < 0) {
    errors.push({ field: 'followersCount', message: 'Followers count must be 0 or greater' });
  }

  if (!data.minCampaignRate || data.minCampaignRate <= 0) {
    errors.push({ field: 'minCampaignRate', message: 'Minimum campaign rate must be greater than 0' });
  }

  if (!isRequired(data.location)) {
    errors.push({ field: 'location', message: 'Location is required' });
  }

  return {
    isValid: errors.length === 0,
    errors,
  };
}

/**
 * Validate login form
 */
export function validateLogin(data: { email: string; password: string }): ValidationResult {
  const errors: ValidationError[] = [];

  if (!isRequired(data.email)) {
    errors.push({ field: 'email', message: 'Email is required' });
  } else if (!isValidEmail(data.email)) {
    errors.push({ field: 'email', message: 'Invalid email format' });
  }

  if (!isRequired(data.password)) {
    errors.push({ field: 'password', message: 'Password is required' });
  }

  return {
    isValid: errors.length === 0,
    errors,
  };
}

/**
 * Validate registration form
 */
export function validateRegister(data: {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
}): ValidationResult {
  const errors: ValidationError[] = [];

  if (!isRequired(data.email)) {
    errors.push({ field: 'email', message: 'Email is required' });
  } else if (!isValidEmail(data.email)) {
    errors.push({ field: 'email', message: 'Invalid email format' });
  }

  if (!isRequired(data.password)) {
    errors.push({ field: 'password', message: 'Password is required' });
  } else if (!minLength(data.password, 8)) {
    errors.push({ field: 'password', message: 'Password must be at least 8 characters' });
  } else if (!isStrongPassword(data.password)) {
    errors.push({
      field: 'password',
      message: 'Password must contain uppercase, lowercase, and number',
    });
  }

  if (data.password !== data.confirmPassword) {
    errors.push({ field: 'confirmPassword', message: 'Passwords do not match' });
  }

  if (!isRequired(data.firstName)) {
    errors.push({ field: 'firstName', message: 'First name is required' });
  }

  if (!isRequired(data.lastName)) {
    errors.push({ field: 'lastName', message: 'Last name is required' });
  }

  if (!isRequired(data.phoneNumber)) {
    errors.push({ field: 'phoneNumber', message: 'Phone number is required' });
  } else if (!isValidPhone(data.phoneNumber)) {
    errors.push({ field: 'phoneNumber', message: 'Invalid phone number format' });
  }

  return {
    isValid: errors.length === 0,
    errors,
  };
}

