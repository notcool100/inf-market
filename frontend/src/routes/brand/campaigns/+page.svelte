<script lang="ts">
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';
  import { campaignsApi } from '../../../lib/api';
  import { toastStore } from '../../../lib/stores/toastStore';
  import { formatCurrency, formatDate } from '../../../lib/utils/format';
  import type { CampaignDto } from '../../../lib/api/types';
  import LoadingSpinner from '../../../lib/components/LoadingSpinner.svelte';
  import StatusBadge from '../../../lib/components/StatusBadge.svelte';
  
  let isLoading = true;
  let campaigns: CampaignDto[] = [];
  let filteredCampaigns: CampaignDto[] = [];
  let error = '';
  let searchTerm = '';
  let statusFilter = 'all';
  
  const statusOptions = [
    { value: 'all', label: 'All Campaigns' },
    { value: 'Draft', label: 'Draft' },
    { value: 'Open', label: 'Open' },
    { value: 'InProgress', label: 'In Progress' },
    { value: 'PendingReview', label: 'Pending Review' },
    { value: 'Completed', label: 'Completed' },
    { value: 'Cancelled', label: 'Cancelled' },
  ];
  
  onMount(async () => {
    await loadCampaigns();
  });
  
  async function loadCampaigns() {
    try {
      isLoading = true;
      error = '';
      campaigns = await campaignsApi.getBrandCampaigns();
      filterCampaigns();
    } catch (err: any) {
      console.error('Error loading campaigns:', err);
      error = err.message || 'Failed to load campaigns';
      toastStore.error(error);
    } finally {
      isLoading = false;
    }
  }

  async function handleDelete(id: string) {
    if (!confirm('Are you sure you want to delete this campaign?')) {
      return;
    }

    try {
      await campaignsApi.deleteCampaign(id);
      toastStore.success('Campaign deleted successfully');
      await loadCampaigns();
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to delete campaign');
    }
  }
  
  function filterCampaigns() {
    filteredCampaigns = campaigns.filter(campaign => {
      const matchesSearch = campaign.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
                           campaign.description.toLowerCase().includes(searchTerm.toLowerCase());
      const matchesStatus = statusFilter === 'all' || campaign.status === statusFilter;
      return matchesSearch && matchesStatus;
    });
  }
  
  $: {
    searchTerm;
    statusFilter;
    filterCampaigns();
  }
</script>

<svelte:head>
  <title>My Campaigns - Influencer Marketplace</title>
</svelte:head>

<div class="space-y-6">
  <div class="flex items-center justify-between">
    <div>
      <h1 class="text-2xl font-bold text-gray-900">My Campaigns</h1>
      <p class="mt-1 text-sm text-gray-500">
        Manage and track your influencer marketing campaigns
      </p>
    </div>
    <a href="/brand/campaigns/create" class="btn btn-primary">
      Create Campaign
    </a>
  </div>

  <!-- Filters -->
  <div class="bg-white shadow rounded-lg">
    <div class="px-4 py-5 sm:p-6">
      <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
        <div>
          <label for="search" class="form-label">Search campaigns</label>
          <input
            type="text"
            id="search"
            bind:value={searchTerm}
            placeholder="Search by title or description..."
            class="form-input"
          />
        </div>
        <div>
          <label for="status" class="form-label">Filter by status</label>
          <select id="status" bind:value={statusFilter} class="form-input">
            {#each statusOptions as option}
              <option value={option.value}>{option.label}</option>
            {/each}
          </select>
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
  {:else if filteredCampaigns.length === 0}
    <div class="text-center py-12">
      <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
      </svg>
      <h3 class="mt-2 text-sm font-medium text-gray-900">No campaigns found</h3>
      <p class="mt-1 text-sm text-gray-500">
        {campaigns.length === 0 ? 'Get started by creating your first campaign.' : 'Try adjusting your search or filter criteria.'}
      </p>
      {#if campaigns.length === 0}
        <div class="mt-6">
          <a href="/brand/campaigns/create" class="btn btn-primary">
            Create Campaign
          </a>
        </div>
      {/if}
    </div>
  {:else}
    <!-- Campaigns Grid -->
    <div class="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
      {#each filteredCampaigns as campaign}
        <div class="card">
          <div class="card-body">
            <div class="flex items-center justify-between">
              <StatusBadge status={campaign.status} />
              <span class="text-sm text-gray-500">{formatDate(campaign.createdAt)}</span>
            </div>
            
            <div class="mt-4">
              <h3 class="text-lg font-medium text-gray-900">
                <a href="/brand/campaigns/{campaign.id}" class="hover:text-indigo-600">
                  {campaign.title}
                </a>
              </h3>
              <p class="mt-1 text-sm text-gray-500 line-clamp-2">
                {campaign.description}
              </p>
            </div>
            
            <div class="mt-4 flex items-center justify-between">
              <div class="text-sm text-gray-500">
                Budget: <span class="font-medium text-gray-900">{formatCurrency(campaign.budget)}</span>
              </div>
              {#if campaign.influencerName}
                <div class="text-sm text-gray-500">
                  Influencer: <span class="font-medium text-gray-900">{campaign.influencerName}</span>
                </div>
              {/if}
            </div>
            
            <div class="mt-4 flex space-x-2">
              <a href="/brand/campaigns/{campaign.id}" class="flex-1 btn btn-secondary text-center">
                View Details
              </a>
              {#if campaign.status === 'Draft' || campaign.status === 'Open'}
                <a href="/brand/campaigns/{campaign.id}/edit" class="flex-1 btn btn-primary text-center">
                  Edit
                </a>
              {/if}
              <button
                on:click={() => handleDelete(campaign.id)}
                class="px-3 py-2 text-sm text-red-600 hover:text-red-800"
                title="Delete campaign"
              >
                Delete
              </button>
            </div>
          </div>
        </div>
      {/each}
    </div>
  {/if}
</div>

<style>
  .line-clamp-2 {
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
  }
</style>