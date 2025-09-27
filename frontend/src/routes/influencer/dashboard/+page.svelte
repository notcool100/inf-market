<script lang="ts">
  import { onMount } from 'svelte';
  import { authStore } from '../../../stores/authStore';
  import { apiClient } from '../../../lib/api';
  import { formatCurrency } from '../../../lib/utils';
  import LoadingSpinner from '../../../lib/components/LoadingSpinner.svelte';
  import StatusBadge from '../../../lib/components/StatusBadge.svelte';
  
  let isLoading = true;
  let stats = {
    totalCampaigns: 0,
    activeCampaigns: 0,
    totalEarnings: 0,
    pendingPayments: 0,
    averageRating: 0
  };
  let recentCampaigns: any[] = [];
  let hasProfile = false;
  let error = '';
  
  onMount(async () => {
    try {
      // Check if user has a profile
      const profileResponse = await apiClient.get('/api/influencer/profile');
      hasProfile = !!profileResponse.data;
      
      if (hasProfile) {
        // Fetch dashboard stats
        const [statsResponse, campaignsResponse] = await Promise.all([
          apiClient.get('/api/influencer/dashboard/stats'),
          apiClient.get('/api/influencer/campaigns?limit=5')
        ]);
        
        stats = statsResponse.data;
        recentCampaigns = campaignsResponse.data;
      }
    } catch (err: any) {
      console.error('Error loading dashboard:', err);
      if (err.response?.status === 404) {
        hasProfile = false;
      } else {
        error = 'Failed to load dashboard data';
      }
    } finally {
      isLoading = false;
    }
  });
</script>

<svelte:head>
  <title>Influencer Dashboard - Influencer Marketplace</title>
</svelte:head>

<div class="space-y-6">
  <div>
    <h1 class="text-2xl font-bold text-gray-900">Influencer Dashboard</h1>
    <p class="mt-1 text-sm text-gray-500">
      Manage your campaigns and track your performance
    </p>
  </div>

  {#if isLoading}
    <div class="flex justify-center py-12">
      <LoadingSpinner size="lg" />
    </div>
  {:else if !hasProfile}
    <!-- Profile Setup Required -->
    <div class="bg-white shadow rounded-lg">
      <div class="px-4 py-5 sm:p-6 text-center">
        <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
        </svg>
        <h3 class="mt-2 text-sm font-medium text-gray-900">Complete Your Profile</h3>
        <p class="mt-1 text-sm text-gray-500">
          You need to complete your influencer profile before you can start receiving campaign offers.
        </p>
        <div class="mt-6">
          <a href="/influencer/profile/create" class="btn btn-primary">
            Create Profile
          </a>
        </div>
      </div>
    </div>
  {:else if error}
    <div class="rounded-md bg-red-50 p-4">
      <div class="text-sm text-red-700">{error}</div>
    </div>
  {:else}
    <!-- Stats Grid -->
    <div class="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-5">
      <div class="stat-card">
        <div class="stat-card-content">
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <svg class="h-8 w-8 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
              </svg>
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="stat-card-title">Total Campaigns</dt>
                <dd class="stat-card-value">{stats.totalCampaigns}</dd>
              </dl>
            </div>
          </div>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-card-content">
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <svg class="h-8 w-8 text-green-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
              </svg>
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="stat-card-title">Active Campaigns</dt>
                <dd class="stat-card-value">{stats.activeCampaigns}</dd>
              </dl>
            </div>
          </div>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-card-content">
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <svg class="h-8 w-8 text-blue-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1" />
              </svg>
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="stat-card-title">Total Earnings</dt>
                <dd class="stat-card-value">{formatCurrency(stats.totalEarnings)}</dd>
              </dl>
            </div>
          </div>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-card-content">
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <svg class="h-8 w-8 text-yellow-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="stat-card-title">Pending Payments</dt>
                <dd class="stat-card-value">{formatCurrency(stats.pendingPayments)}</dd>
              </dl>
            </div>
          </div>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-card-content">
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <svg class="h-8 w-8 text-purple-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z" />
              </svg>
            </div>
            <div class="ml-5 w-0 flex-1">
              <dl>
                <dt class="stat-card-title">Average Rating</dt>
                <dd class="stat-card-value">{stats.averageRating.toFixed(1)}</dd>
              </dl>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Quick Actions -->
    <div class="bg-white shadow rounded-lg">
      <div class="px-4 py-5 sm:p-6">
        <h3 class="text-lg leading-6 font-medium text-gray-900">Quick Actions</h3>
        <div class="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
          <a href="/influencer/campaigns" class="relative group bg-white p-6 focus-within:ring-2 focus-within:ring-inset focus-within:ring-indigo-500 rounded-lg border border-gray-300 hover:border-gray-400">
            <div>
              <span class="rounded-lg inline-flex p-3 bg-indigo-50 text-indigo-700 ring-4 ring-white">
                <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
                </svg>
              </span>
            </div>
            <div class="mt-8">
              <h3 class="text-lg font-medium">
                <span class="absolute inset-0" aria-hidden="true"></span>
                View Campaigns
              </h3>
              <p class="mt-2 text-sm text-gray-500">
                Browse and manage your campaign offers
              </p>
            </div>
          </a>

          <a href="/influencer/profile" class="relative group bg-white p-6 focus-within:ring-2 focus-within:ring-inset focus-within:ring-indigo-500 rounded-lg border border-gray-300 hover:border-gray-400">
            <div>
              <span class="rounded-lg inline-flex p-3 bg-green-50 text-green-700 ring-4 ring-white">
                <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                </svg>
              </span>
            </div>
            <div class="mt-8">
              <h3 class="text-lg font-medium">
                <span class="absolute inset-0" aria-hidden="true"></span>
                Update Profile
              </h3>
              <p class="mt-2 text-sm text-gray-500">
                Keep your influencer profile up to date
              </p>
            </div>
          </a>

          <a href="/wallet" class="relative group bg-white p-6 focus-within:ring-2 focus-within:ring-inset focus-within:ring-indigo-500 rounded-lg border border-gray-300 hover:border-gray-400">
            <div>
              <span class="rounded-lg inline-flex p-3 bg-blue-50 text-blue-700 ring-4 ring-white">
                <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" />
                </svg>
              </span>
            </div>
            <div class="mt-8">
              <h3 class="text-lg font-medium">
                <span class="absolute inset-0" aria-hidden="true"></span>
                Manage Wallet
              </h3>
              <p class="mt-2 text-sm text-gray-500">
                View earnings and withdraw funds
              </p>
            </div>
          </a>
        </div>
      </div>
    </div>

    <!-- Recent Campaigns -->
    <div class="bg-white shadow rounded-lg">
      <div class="px-4 py-5 sm:px-6">
        <div class="flex items-center justify-between">
          <h3 class="text-lg leading-6 font-medium text-gray-900">Recent Campaigns</h3>
          <a href="/influencer/campaigns" class="text-sm font-medium text-indigo-600 hover:text-indigo-500">
            View all
          </a>
        </div>
      </div>
      <div class="border-t border-gray-200">
        {#if recentCampaigns.length === 0}
          <div class="px-4 py-5 sm:p-6 text-center">
            <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
            </svg>
            <h3 class="mt-2 text-sm font-medium text-gray-900">No campaigns yet</h3>
            <p class="mt-1 text-sm text-gray-500">Campaign offers will appear here once brands discover your profile.</p>
          </div>
        {:else}
          <ul class="divide-y divide-gray-200">
            {#each recentCampaigns as campaign}
              <li>
                <a href="/influencer/campaigns/{campaign.id}" class="block hover:bg-gray-50">
                  <div class="px-4 py-4 sm:px-6">
                    <div class="flex items-center justify-between">
                      <div class="flex items-center">
                        <div class="flex-shrink-0">
                          <StatusBadge status={campaign.status} />
                        </div>
                        <div class="ml-4">
                          <p class="text-sm font-medium text-gray-900">{campaign.title}</p>
                          <p class="text-sm text-gray-500">Budget: {formatCurrency(campaign.budget)}</p>
                        </div>
                      </div>
                      <div class="flex items-center">
                        <svg class="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                        </svg>
                      </div>
                    </div>
                  </div>
                </a>
              </li>
            {/each}
          </ul>
        {/if}
      </div>
    </div>
  {/if}
</div>