<script lang="ts">
  import { onMount } from 'svelte';
  import { apiClient } from '../../lib/api';
  import { formatCurrency, debounce } from '../../lib/utils';
  import LoadingSpinner from '../../lib/components/LoadingSpinner.svelte';
  
  let isLoading = true;
  let influencers: any[] = [];
  let filteredInfluencers: any[] = [];
  let error = '';
  
  // Search and filter state
  let searchTerm = '';
  let nicheFilter = '';
  let locationFilter = '';
  let minFollowers = '';
  let maxRate = '';
  
  const nicheOptions = [
    'Fashion & Beauty',
    'Lifestyle',
    'Travel',
    'Food & Cooking',
    'Fitness & Health',
    'Technology',
    'Gaming',
    'Business & Finance',
    'Education',
    'Entertainment',
    'Art & Design',
    'Music',
    'Sports',
    'Parenting',
    'Home & Garden',
    'Automotive',
    'Photography',
    'Other'
  ];
  
  onMount(async () => {
    await loadInfluencers();
  });
  
  async function loadInfluencers() {
    try {
      isLoading = true;
      const response = await apiClient.get('/api/influencers');
      influencers = response.data;
      filteredInfluencers = influencers;
    } catch (err: any) {
      console.error('Error loading influencers:', err);
      error = 'Failed to load influencers';
    } finally {
      isLoading = false;
    }
  }
  
  const debouncedFilter = debounce(() => {
    filterInfluencers();
  }, 300);
  
  function filterInfluencers() {
    filteredInfluencers = influencers.filter(influencer => {
      const matchesSearch = !searchTerm || 
        influencer.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        influencer.bio.toLowerCase().includes(searchTerm.toLowerCase());
      
      const matchesNiche = !nicheFilter || influencer.nicheFocus === nicheFilter;
      const matchesLocation = !locationFilter || 
        influencer.location.toLowerCase().includes(locationFilter.toLowerCase());
      
      const matchesFollowers = !minFollowers || 
        influencer.followersCount >= parseInt(minFollowers);
      
      const matchesRate = !maxRate || 
        influencer.minCampaignRate <= parseFloat(maxRate);
      
      return matchesSearch && matchesNiche && matchesLocation && matchesFollowers && matchesRate;
    });
  }
  
  $: {
    searchTerm;
    nicheFilter;
    locationFilter;
    minFollowers;
    maxRate;
    debouncedFilter();
  }
</script>

<svelte:head>
  <title>Find Influencers - Influencer Marketplace</title>
</svelte:head>

<div class="space-y-6">
  <div>
    <h1 class="text-2xl font-bold text-gray-900">Find Influencers</h1>
    <p class="mt-1 text-sm text-gray-500">
      Discover and connect with influencers for your marketing campaigns
    </p>
  </div>

  <!-- Search and Filters -->
  <div class="bg-white shadow rounded-lg">
    <div class="px-4 py-5 sm:p-6">
      <div class="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-5">
        <div>
          <label for="search" class="form-label">Search</label>
          <input
            type="text"
            id="search"
            bind:value={searchTerm}
            placeholder="Search by name or bio..."
            class="form-input"
          />
        </div>
        
        <div>
          <label for="niche" class="form-label">Niche</label>
          <select id="niche" bind:value={nicheFilter} class="form-input">
            <option value="">All Niches</option>
            {#each nicheOptions as niche}
              <option value={niche}>{niche}</option>
            {/each}
          </select>
        </div>
        
        <div>
          <label for="location" class="form-label">Location</label>
          <input
            type="text"
            id="location"
            bind:value={locationFilter}
            placeholder="Enter location..."
            class="form-input"
          />
        </div>
        
        <div>
          <label for="followers" class="form-label">Min Followers</label>
          <input
            type="number"
            id="followers"
            bind:value={minFollowers}
            placeholder="0"
            min="0"
            class="form-input"
          />
        </div>
        
        <div>
          <label for="rate" class="form-label">Max Rate (NPR)</label>
          <input
            type="number"
            id="rate"
            bind:value={maxRate}
            placeholder="Any"
            min="0"
            class="form-input"
          />
        </div>
      </div>
    </div>
  </div>

  {#if isLoading}
    <div class="flex justify-center py-12">
      <LoadingSpinner size="lg" />
    </div>
  {:else if error}
    <div class="rounded-md bg-red-50 p-4">
      <div class="text-sm text-red-700">{error}</div>
    </div>
  {:else if filteredInfluencers.length === 0}
    <div class="text-center py-12">
      <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
      </svg>
      <h3 class="mt-2 text-sm font-medium text-gray-900">No influencers found</h3>
      <p class="mt-1 text-sm text-gray-500">
        Try adjusting your search criteria to find more influencers.
      </p>
    </div>
  {:else}
    <!-- Results -->
    <div class="bg-white shadow rounded-lg">
      <div class="px-4 py-5 sm:px-6">
        <h3 class="text-lg leading-6 font-medium text-gray-900">
          {filteredInfluencers.length} Influencer{filteredInfluencers.length !== 1 ? 's' : ''} Found
        </h3>
      </div>
      <div class="border-t border-gray-200">
        <div class="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3 p-6">
          {#each filteredInfluencers as influencer}
            <div class="card">
              <div class="card-body">
                <div class="flex items-center space-x-4">
                  <div class="flex-shrink-0">
                    <div class="h-12 w-12 rounded-full bg-gray-300 flex items-center justify-center">
                      <svg class="h-6 w-6 text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                      </svg>
                    </div>
                  </div>
                  <div class="flex-1 min-w-0">
                    <p class="text-sm font-medium text-gray-900 truncate">
                      {influencer.fullName}
                    </p>
                    <p class="text-sm text-gray-500 truncate">
                      {influencer.nicheFocus}
                    </p>
                    {#if influencer.isVerified}
                      <span class="inline-flex items-center px-2 py-0.5 rounded text-xs font-medium bg-green-100 text-green-800">
                        Verified
                      </span>
                    {/if}
                  </div>
                </div>
                
                <div class="mt-4">
                  <p class="text-sm text-gray-600 line-clamp-3">
                    {influencer.bio}
                  </p>
                </div>
                
                <div class="mt-4 grid grid-cols-2 gap-4 text-sm">
                  <div>
                    <span class="text-gray-500">Followers:</span>
                    <span class="font-medium text-gray-900">
                      {influencer.followersCount.toLocaleString()}
                    </span>
                  </div>
                  <div>
                    <span class="text-gray-500">Min Rate:</span>
                    <span class="font-medium text-gray-900">
                      {formatCurrency(influencer.minCampaignRate)}
                    </span>
                  </div>
                  <div>
                    <span class="text-gray-500">Rating:</span>
                    <span class="font-medium text-gray-900">
                      {influencer.averageRating.toFixed(1)} ⭐
                    </span>
                  </div>
                  <div>
                    <span class="text-gray-500">Campaigns:</span>
                    <span class="font-medium text-gray-900">
                      {influencer.completedCampaigns}
                    </span>
                  </div>
                </div>
                
                <div class="mt-4 flex space-x-2">
                  <a href="/influencers/{influencer.id}" class="flex-1 btn btn-secondary text-center">
                    View Profile
                  </a>
                  <button class="flex-1 btn btn-primary">
                    Contact
                  </button>
                </div>
              </div>
            </div>
          {/each}
        </div>
      </div>
    </div>
  {/if}
</div>

<style>
  .line-clamp-3 {
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
</style>