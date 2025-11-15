<script lang="ts">
  import { onMount } from 'svelte';
  import { page } from '$app/stores';
  import { goto } from '$app/navigation';
  import { influencersApi } from '../../../lib/api';
  import { toastStore } from '../../../lib/stores/toastStore';
  import { formatCurrency, formatDate, formatLargeNumber } from '../../../lib/utils/format';
  import { authStore } from '../../../lib/stores/authStore';
  import type { InfluencerProfileDto, ReviewDto } from '../../../lib/api/types';
  import LoadingSpinner from '../../../lib/components/LoadingSpinner.svelte';
  import { Star, MapPin, Users, DollarSign, CheckCircle } from 'lucide-svelte';

  const influencerId = $page.params.id;
  let isLoading = true;
  let profile: InfluencerProfileDto | null = null;
  let reviews: ReviewDto[] = [];
  let error = '';
  let isOwnProfile = false;
  let currentUserId: string | null = null;

  onMount(async () => {
    authStore.subscribe((auth) => {
      currentUserId = auth.userId;
      isOwnProfile = auth.userId === profile?.userId;
    });
    await loadProfile();
    await loadReviews();
  });

  async function loadProfile() {
    try {
      isLoading = true;
      error = '';
      profile = await influencersApi.getProfile(influencerId);
      isOwnProfile = currentUserId === profile?.userId;
    } catch (err: any) {
      console.error('Error loading profile:', err);
      error = err.message || 'Failed to load influencer profile';
      toastStore.error(error);
    } finally {
      isLoading = false;
    }
  }

  async function loadReviews() {
    try {
      reviews = await influencersApi.getReviews(influencerId);
    } catch (err: any) {
      console.error('Error loading reviews:', err);
    }
  }

  function renderStars(rating: number) {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    const emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);

    return {
      full: fullStars,
      half: hasHalfStar,
      empty: emptyStars,
    };
  }
</script>

<svelte:head>
  <title>{profile?.fullName || 'Influencer Profile'} - Influencer Marketplace</title>
</svelte:head>

{#if isLoading}
  <div class="flex justify-center py-12">
    <LoadingSpinner size="lg" />
  </div>
{:else if error || !profile}
  <div class="rounded-md bg-red-50 p-4">
    <div class="text-sm text-red-700">{error || 'Profile not found'}</div>
    <a href="/influencers" class="mt-2 inline-block text-sm text-red-600 hover:text-red-800">
      ← Back to Influencers
    </a>
  </div>
{:else}
  <div class="space-y-6">
    <!-- Header -->
    <div class="bg-white shadow rounded-lg p-6">
      <div class="flex items-start justify-between">
        <div class="flex items-start space-x-4">
          <div class="flex-shrink-0">
            <div class="h-24 w-24 rounded-full bg-indigo-100 flex items-center justify-center">
              <span class="text-3xl font-bold text-indigo-600">
                {profile.fullName.charAt(0).toUpperCase()}
              </span>
            </div>
          </div>
          <div class="flex-1">
            <div class="flex items-center gap-3">
              <h1 class="text-3xl font-bold text-gray-900">{profile.fullName}</h1>
              {#if profile.isVerified}
                <span class="inline-flex items-center gap-1 px-2 py-1 rounded-full text-xs font-medium bg-green-100 text-green-800">
                  <CheckCircle class="w-4 h-4" />
                  Verified
                </span>
              {/if}
            </div>
            <p class="mt-2 text-lg text-gray-600">{profile.nicheFocus}</p>
            <div class="mt-3 flex items-center gap-4 text-sm text-gray-500">
              <div class="flex items-center gap-1">
                <MapPin class="w-4 h-4" />
                {profile.location}
              </div>
              <div class="flex items-center gap-1">
                <Users class="w-4 h-4" />
                {formatLargeNumber(profile.followersCount)} followers
              </div>
              <div class="flex items-center gap-1">
                <DollarSign class="w-4 h-4" />
                Min: {formatCurrency(profile.minCampaignRate)}
              </div>
            </div>
          </div>
        </div>
        {#if isOwnProfile}
          <a href="/influencer/profile/edit" class="btn btn-primary">Edit Profile</a>
        {/if}
      </div>
    </div>

    <div class="grid grid-cols-1 gap-6 lg:grid-cols-3">
      <!-- Main Content -->
      <div class="lg:col-span-2 space-y-6">
        <!-- Bio -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium text-gray-900 mb-4">About</h2>
          <p class="text-gray-700 whitespace-pre-wrap">{profile.bio}</p>
        </div>

        <!-- Social Media Links -->
        {#if profile.instagramHandle || profile.tiktokHandle || profile.youtubeChannel || profile.facebookPage || profile.linkedInProfile || profile.websiteUrl}
          <div class="bg-white shadow rounded-lg p-6">
            <h2 class="text-lg font-medium text-gray-900 mb-4">Social Media & Links</h2>
            <div class="grid grid-cols-2 gap-4">
              {#if profile.instagramHandle}
                <a
                  href="https://instagram.com/{profile.instagramHandle}"
                  target="_blank"
                  rel="noopener noreferrer"
                  class="text-indigo-600 hover:text-indigo-800"
                >
                  Instagram: @{profile.instagramHandle}
                </a>
              {/if}
              {#if profile.tiktokHandle}
                <a
                  href="https://tiktok.com/@{profile.tiktokHandle}"
                  target="_blank"
                  rel="noopener noreferrer"
                  class="text-indigo-600 hover:text-indigo-800"
                >
                  TikTok: @{profile.tiktokHandle}
                </a>
              {/if}
              {#if profile.youtubeChannel}
                <a
                  href="{profile.youtubeChannel}"
                  target="_blank"
                  rel="noopener noreferrer"
                  class="text-indigo-600 hover:text-indigo-800"
                >
                  YouTube Channel
                </a>
              {/if}
              {#if profile.facebookPage}
                <a
                  href="{profile.facebookPage}"
                  target="_blank"
                  rel="noopener noreferrer"
                  class="text-indigo-600 hover:text-indigo-800"
                >
                  Facebook Page
                </a>
              {/if}
              {#if profile.linkedInProfile}
                <a
                  href="{profile.linkedInProfile}"
                  target="_blank"
                  rel="noopener noreferrer"
                  class="text-indigo-600 hover:text-indigo-800"
                >
                  LinkedIn Profile
                </a>
              {/if}
              {#if profile.websiteUrl}
                <a
                  href={profile.websiteUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  class="text-indigo-600 hover:text-indigo-800"
                >
                  Website
                </a>
              {/if}
            </div>
          </div>
        {/if}

        <!-- Reviews -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium text-gray-900 mb-4">
            Reviews ({reviews.length})
          </h2>
          {#if reviews.length === 0}
            <p class="text-sm text-gray-500">No reviews yet.</p>
          {:else}
            <div class="space-y-4">
              {#each reviews as review}
                <div class="border-t pt-4 first:border-t-0 first:pt-0">
                  <div class="flex items-center gap-2 mb-2">
                    <div class="flex items-center">
                      {#each Array(review.rating) as _}
                        <Star class="w-4 h-4 fill-yellow-400 text-yellow-400" />
                      {/each}
                      {#each Array(5 - review.rating) as _}
                        <Star class="w-4 h-4 text-gray-300" />
                      {/each}
                    </div>
                    <span class="text-sm text-gray-500">{formatDate(review.createdAt)}</span>
                  </div>
                  {#if review.comment}
                    <p class="text-gray-700">{review.comment}</p>
                  {/if}
                </div>
              {/each}
            </div>
          {/if}
        </div>
      </div>

      <!-- Sidebar -->
      <div class="space-y-6">
        <!-- Stats Card -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium text-gray-900 mb-4">Statistics</h2>
          <dl class="space-y-4">
            <div>
              <dt class="text-sm font-medium text-gray-500">Average Rating</dt>
              <dd class="mt-1 flex items-center gap-2">
                <span class="text-2xl font-semibold text-gray-900">
                  {profile.averageRating.toFixed(1)}
                </span>
                <div class="flex items-center">
                  {#each Array(Math.floor(profile.averageRating)) as _}
                    <Star class="w-5 h-5 fill-yellow-400 text-yellow-400" />
                  {/each}
                  {#if profile.averageRating % 1 >= 0.5}
                    <Star class="w-5 h-5 fill-yellow-400 text-yellow-400 opacity-50" />
                  {/if}
                  {#each Array(5 - Math.ceil(profile.averageRating)) as _}
                    <Star class="w-5 h-5 text-gray-300" />
                  {/each}
                </div>
              </dd>
            </div>
            <div>
              <dt class="text-sm font-medium text-gray-500">Completed Campaigns</dt>
              <dd class="mt-1 text-2xl font-semibold text-gray-900">
                {profile.completedCampaigns}
              </dd>
            </div>
            <div>
              <dt class="text-sm font-medium text-gray-500">Minimum Rate</dt>
              <dd class="mt-1 text-2xl font-semibold text-gray-900">
                {formatCurrency(profile.minCampaignRate)}
              </dd>
            </div>
            <div>
              <dt class="text-sm font-medium text-gray-500">Content Types</dt>
              <dd class="mt-2 flex flex-wrap gap-2">
                {#each profile.contentTypes as type}
                  <span class="px-2 py-1 bg-indigo-100 text-indigo-800 rounded text-xs">
                    {type}
                  </span>
                {/each}
              </dd>
            </div>
          </dl>
        </div>

        <!-- Contact Card -->
        {#if !isOwnProfile}
          <div class="bg-white shadow rounded-lg p-6">
            <h2 class="text-lg font-medium text-gray-900 mb-4">Interested in working together?</h2>
            <p class="text-sm text-gray-600 mb-4">
              Browse available campaigns or create a new one to get started.
            </p>
            <div class="space-y-2">
              <a href="/brand/campaigns/create" class="w-full btn btn-primary text-center block">
                Create Campaign
              </a>
              <a href="/campaigns/available" class="w-full btn btn-secondary text-center block">
                Browse Campaigns
              </a>
            </div>
          </div>
        {/if}
      </div>
    </div>
  </div>
{/if}

