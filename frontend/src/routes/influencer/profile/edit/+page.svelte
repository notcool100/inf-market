<script lang="ts">
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';
  import { influencersApi } from '../../../../lib/api';
  import { toastStore } from '../../../../lib/stores/toastStore';
  import { validateInfluencerProfile } from '../../../../lib/utils/validation';
  import type { InfluencerProfileDto } from '../../../../lib/api/types';
  import LoadingSpinner from '../../../../lib/components/LoadingSpinner.svelte';
  import { NICHE_OPTIONS, CONTENT_TYPES } from '../../../../lib/constants';

  let isLoading = true;
  let isSaving = false;
  let profile: InfluencerProfileDto | null = null;
  let formData = {
    bio: '',
    nicheFocus: '',
    followersCount: 0,
    instagramHandle: '',
    tiktokHandle: '',
    youtubeChannel: '',
    facebookPage: '',
    linkedInProfile: '',
    websiteUrl: '',
    minCampaignRate: 0,
    contentTypes: [] as string[],
    demographics: {} as Record<string, any>,
    location: '',
  };
  let selectedContentType = '';
  let errors: Record<string, string> = {};

  onMount(async () => {
    await loadProfile();
  });

  async function loadProfile() {
    try {
      isLoading = true;
      profile = await influencersApi.getMyProfile();
      if (profile) {
        formData = {
          bio: profile.bio || '',
          nicheFocus: profile.nicheFocus || '',
          followersCount: profile.followersCount || 0,
          instagramHandle: profile.instagramHandle || '',
          tiktokHandle: profile.tiktokHandle || '',
          youtubeChannel: profile.youtubeChannel || '',
          facebookPage: profile.facebookPage || '',
          linkedInProfile: profile.linkedInProfile || '',
          websiteUrl: profile.websiteUrl || '',
          minCampaignRate: profile.minCampaignRate || 0,
          contentTypes: profile.contentTypes || [],
          demographics: profile.demographics || {},
          location: profile.location || '',
        };
      }
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to load profile');
      goto('/influencer/dashboard');
    } finally {
      isLoading = false;
    }
  }

  function addContentType() {
    if (selectedContentType && !formData.contentTypes.includes(selectedContentType)) {
      formData.contentTypes = [...formData.contentTypes, selectedContentType];
      selectedContentType = '';
    }
  }

  function removeContentType(type: string) {
    formData.contentTypes = formData.contentTypes.filter((t) => t !== type);
  }

  async function handleSubmit() {
    errors = {};
    const validation = validateInfluencerProfile(formData);

    if (!validation.isValid) {
      validation.errors.forEach((err) => {
        errors[err.field] = err.message;
      });
      toastStore.error('Please fix the errors in the form');
      return;
    }

    try {
      isSaving = true;
      if (!profile) return;
      await influencersApi.updateProfile(profile.id, formData);
      toastStore.success('Profile updated successfully');
      goto('/influencer/profile');
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to update profile');
    } finally {
      isSaving = false;
    }
  }
</script>

<svelte:head>
  <title>Edit Profile - Influencer Marketplace</title>
</svelte:head>

{#if isLoading}
  <div class="flex justify-center py-12">
    <LoadingSpinner size="lg" />
  </div>
{:else}
  <div class="max-w-4xl mx-auto space-y-6">
    <div class="flex items-center justify-between">
      <div>
        <h1 class="text-3xl font-bold text-gray-900">Edit Profile</h1>
        <p class="mt-1 text-sm text-gray-500">Update your influencer profile information</p>
      </div>
      <a href="/influencer/profile" class="btn btn-secondary">Cancel</a>
    </div>

    <form on:submit|preventDefault={handleSubmit} class="bg-white shadow rounded-lg p-6 space-y-6">
      <!-- Bio -->
      <div>
        <label for="bio" class="block text-sm font-medium text-gray-700 mb-2">
          Bio <span class="text-red-500">*</span>
        </label>
        <textarea
          id="bio"
          bind:value={formData.bio}
          rows="4"
          class="form-input {errors.bio ? 'border-red-500' : ''}"
          required
          placeholder="Tell us about yourself..."
        ></textarea>
        {#if errors.bio}
          <p class="mt-1 text-sm text-red-600">{errors.bio}</p>
        {/if}
      </div>

      <!-- Niche & Location -->
      <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <div>
          <label for="nicheFocus" class="block text-sm font-medium text-gray-700 mb-2">
            Niche Focus <span class="text-red-500">*</span>
          </label>
          <select id="nicheFocus" bind:value={formData.nicheFocus} class="form-input {errors.nicheFocus ? 'border-red-500' : ''}" required>
            <option value="">Select niche...</option>
            {#each NICHE_OPTIONS as niche}
              <option value={niche}>{niche}</option>
            {/each}
          </select>
          {#if errors.nicheFocus}
            <p class="mt-1 text-sm text-red-600">{errors.nicheFocus}</p>
          {/if}
        </div>

        <div>
          <label for="location" class="block text-sm font-medium text-gray-700 mb-2">
            Location <span class="text-red-500">*</span>
          </label>
          <input
            type="text"
            id="location"
            bind:value={formData.location}
            class="form-input {errors.location ? 'border-red-500' : ''}"
            required
            placeholder="City, Country"
          />
          {#if errors.location}
            <p class="mt-1 text-sm text-red-600">{errors.location}</p>
          {/if}
        </div>
      </div>

      <!-- Followers & Rate -->
      <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <div>
          <label for="followersCount" class="block text-sm font-medium text-gray-700 mb-2">
            Followers Count <span class="text-red-500">*</span>
          </label>
          <input
            type="number"
            id="followersCount"
            bind:value={formData.followersCount}
            min="0"
            class="form-input {errors.followersCount ? 'border-red-500' : ''}"
            required
          />
          {#if errors.followersCount}
            <p class="mt-1 text-sm text-red-600">{errors.followersCount}</p>
          {/if}
        </div>

        <div>
          <label for="minCampaignRate" class="block text-sm font-medium text-gray-700 mb-2">
            Minimum Campaign Rate (NPR) <span class="text-red-500">*</span>
          </label>
          <input
            type="number"
            id="minCampaignRate"
            bind:value={formData.minCampaignRate}
            min="0"
            step="0.01"
            class="form-input {errors.minCampaignRate ? 'border-red-500' : ''}"
            required
          />
          {#if errors.minCampaignRate}
            <p class="mt-1 text-sm text-red-600">{errors.minCampaignRate}</p>
          {/if}
        </div>
      </div>

      <!-- Social Media -->
      <div>
        <h3 class="text-lg font-medium text-gray-900 mb-4">Social Media Handles</h3>
        <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div>
            <label for="instagramHandle" class="block text-sm font-medium text-gray-700 mb-2">
              Instagram Handle
            </label>
            <input
              type="text"
              id="instagramHandle"
              bind:value={formData.instagramHandle}
              class="form-input"
              placeholder="@username"
            />
          </div>
          <div>
            <label for="tiktokHandle" class="block text-sm font-medium text-gray-700 mb-2">
              TikTok Handle
            </label>
            <input
              type="text"
              id="tiktokHandle"
              bind:value={formData.tiktokHandle}
              class="form-input"
              placeholder="@username"
            />
          </div>
          <div>
            <label for="youtubeChannel" class="block text-sm font-medium text-gray-700 mb-2">
              YouTube Channel URL
            </label>
            <input
              type="url"
              id="youtubeChannel"
              bind:value={formData.youtubeChannel}
              class="form-input"
              placeholder="https://youtube.com/..."
            />
          </div>
          <div>
            <label for="facebookPage" class="block text-sm font-medium text-gray-700 mb-2">
              Facebook Page URL
            </label>
            <input
              type="url"
              id="facebookPage"
              bind:value={formData.facebookPage}
              class="form-input"
              placeholder="https://facebook.com/..."
            />
          </div>
          <div>
            <label for="linkedInProfile" class="block text-sm font-medium text-gray-700 mb-2">
              LinkedIn Profile URL
            </label>
            <input
              type="url"
              id="linkedInProfile"
              bind:value={formData.linkedInProfile}
              class="form-input"
              placeholder="https://linkedin.com/in/..."
            />
          </div>
          <div>
            <label for="websiteUrl" class="block text-sm font-medium text-gray-700 mb-2">
              Website URL
            </label>
            <input
              type="url"
              id="websiteUrl"
              bind:value={formData.websiteUrl}
              class="form-input"
              placeholder="https://..."
            />
          </div>
        </div>
      </div>

      <!-- Content Types -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Content Types
        </label>
        <div class="flex gap-2 mb-2">
          <select bind:value={selectedContentType} class="form-input flex-1">
            <option value="">Select content type...</option>
            {#each CONTENT_TYPES as type}
              <option value={type}>{type}</option>
            {/each}
          </select>
          <button type="button" on:click={addContentType} class="btn btn-secondary">
            Add
          </button>
        </div>
        <div class="flex flex-wrap gap-2">
          {#each formData.contentTypes as type}
            <span class="inline-flex items-center gap-1 px-3 py-1 bg-indigo-100 text-indigo-800 rounded-full text-sm">
              {type}
              <button
                type="button"
                on:click={() => removeContentType(type)}
                class="text-indigo-600 hover:text-indigo-800"
              >
                ×
              </button>
            </span>
          {/each}
        </div>
      </div>

      <!-- Actions -->
      <div class="flex justify-end gap-2 pt-4 border-t">
        <a href="/influencer/profile" class="btn btn-secondary">Cancel</a>
        <button type="submit" class="btn btn-primary" disabled={isSaving}>
          {isSaving ? 'Saving...' : 'Update Profile'}
        </button>
      </div>
    </form>
  </div>
{/if}

