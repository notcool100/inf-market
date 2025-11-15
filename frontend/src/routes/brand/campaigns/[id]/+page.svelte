<script lang="ts">
  import { onMount } from 'svelte';
  import { page } from '$app/stores';
  import { goto } from '$app/navigation';
  import { campaignsApi } from '../../../../lib/api';
  import { toastStore } from '../../../../lib/stores/toastStore';
  import { formatCurrency, formatDate, formatDateTime } from '../../../../lib/utils/format';
  import type { CampaignDto, CampaignDeliverableDto } from '../../../../lib/api/types';
  import LoadingSpinner from '../../../../lib/components/LoadingSpinner.svelte';
  import StatusBadge from '../../../../lib/components/StatusBadge.svelte';
  import Modal from '../../../../lib/components/Modal.svelte';

  const campaignId = $page.params.id;
  let isLoading = true;
  let campaign: CampaignDto | null = null;
  let deliverables: CampaignDeliverableDto[] = [];
  let error = '';
  let showStatusModal = false;
  let newStatus = '';
  let showDeliverableModal = false;
  let selectedDeliverable: CampaignDeliverableDto | null = null;
  let reviewFeedback = '';

  onMount(async () => {
    await loadCampaign();
    await loadDeliverables();
  });

  async function loadCampaign() {
    try {
      isLoading = true;
      error = '';
      campaign = await campaignsApi.getCampaign(campaignId);
    } catch (err: any) {
      console.error('Error loading campaign:', err);
      error = err.message || 'Failed to load campaign';
      toastStore.error(error);
    } finally {
      isLoading = false;
    }
  }

  async function loadDeliverables() {
    try {
      deliverables = await campaignsApi.getCampaignDeliverables(campaignId);
    } catch (err: any) {
      console.error('Error loading deliverables:', err);
    }
  }

  async function handleDelete() {
    if (!confirm('Are you sure you want to delete this campaign? This action cannot be undone.')) {
      return;
    }

    try {
      await campaignsApi.deleteCampaign(campaignId);
      toastStore.success('Campaign deleted successfully');
      goto('/brand/campaigns');
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to delete campaign');
    }
  }

  function openStatusModal() {
    newStatus = campaign?.status || '';
    showStatusModal = true;
  }

  async function updateStatus() {
    if (!newStatus || newStatus === campaign?.status) {
      showStatusModal = false;
      return;
    }

    try {
      await campaignsApi.updateCampaignStatus(campaignId, newStatus);
      toastStore.success('Campaign status updated successfully');
      showStatusModal = false;
      await loadCampaign();
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to update status');
    }
  }

  function openReviewModal(deliverable: CampaignDeliverableDto) {
    selectedDeliverable = deliverable;
    reviewFeedback = '';
    showDeliverableModal = true;
  }

  async function approveDeliverable() {
    if (!selectedDeliverable) return;

    try {
      await campaignsApi.approveDeliverable(selectedDeliverable.id, {
        status: 'Approved',
        feedbackNotes: reviewFeedback,
      });
      toastStore.success('Deliverable approved successfully');
      showDeliverableModal = false;
      await loadDeliverables();
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to approve deliverable');
    }
  }

  async function rejectDeliverable() {
    if (!selectedDeliverable) return;

    try {
      await campaignsApi.rejectDeliverable(selectedDeliverable.id, {
        status: 'Rejected',
        feedbackNotes: reviewFeedback,
      });
      toastStore.success('Deliverable rejected');
      showDeliverableModal = false;
      await loadDeliverables();
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to reject deliverable');
    }
  }
</script>

<svelte:head>
  <title>{campaign?.title || 'Campaign Details'} - Influencer Marketplace</title>
</svelte:head>

{#if isLoading}
  <div class="flex justify-center py-12">
    <LoadingSpinner size="lg" />
  </div>
{:else if error || !campaign}
  <div class="rounded-md bg-red-50 p-4">
    <div class="text-sm text-red-700">{error || 'Campaign not found'}</div>
    <a href="/brand/campaigns" class="mt-2 inline-block text-sm text-red-600 hover:text-red-800">
      ← Back to Campaigns
    </a>
  </div>
{:else}
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex items-center justify-between">
      <div>
        <div class="flex items-center gap-3">
          <h1 class="text-3xl font-bold text-gray-900">{campaign.title}</h1>
          <StatusBadge status={campaign.status} />
        </div>
        <p class="mt-2 text-sm text-gray-500">
          Created {formatDate(campaign.createdAt)} • Last updated {formatDate(campaign.updatedAt)}
        </p>
      </div>
      <div class="flex gap-2">
        {#if campaign.status === 'Draft' || campaign.status === 'Open'}
          <a href="/brand/campaigns/{campaign.id}/edit" class="btn btn-primary">Edit Campaign</a>
        {/if}
        <button on:click={openStatusModal} class="btn btn-secondary">Update Status</button>
        <button on:click={handleDelete} class="btn btn-danger">Delete</button>
      </div>
    </div>

    <!-- Campaign Details -->
    <div class="grid grid-cols-1 gap-6 lg:grid-cols-3">
      <!-- Main Content -->
      <div class="lg:col-span-2 space-y-6">
        <!-- Description -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium text-gray-900 mb-4">Description</h2>
          <p class="text-gray-700 whitespace-pre-wrap">{campaign.description}</p>
        </div>

        <!-- Requirements -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium text-gray-900 mb-4">Requirements</h2>
          <p class="text-gray-700 whitespace-pre-wrap">{campaign.requirements}</p>
        </div>

        <!-- Deliverables -->
        <div class="bg-white shadow rounded-lg p-6">
          <div class="flex items-center justify-between mb-4">
            <h2 class="text-lg font-medium text-gray-900">Deliverables</h2>
            {#if campaign.status === 'Open' || campaign.status === 'InProgress'}
              <a href="/brand/campaigns/{campaign.id}/deliverables/create" class="btn btn-sm btn-primary">
                Add Deliverable
              </a>
            {/if}
          </div>

          {#if deliverables.length === 0}
            <p class="text-sm text-gray-500">No deliverables added yet.</p>
          {:else}
            <div class="space-y-4">
              {#each deliverables as deliverable}
                <div class="border rounded-lg p-4">
                  <div class="flex items-center justify-between">
                    <div>
                      <h3 class="font-medium text-gray-900">{deliverable.title}</h3>
                      <p class="text-sm text-gray-500 mt-1">{deliverable.description}</p>
                      <div class="mt-2 flex items-center gap-4 text-sm text-gray-500">
                        <span>Type: {deliverable.deliverableType}</span>
                        <span>Due: {formatDate(deliverable.dueDate)}</span>
                        <StatusBadge status={deliverable.status} />
                      </div>
                    </div>
                    {#if deliverable.status === 'Submitted'}
                      <button
                        on:click={() => openReviewModal(deliverable)}
                        class="btn btn-sm btn-primary"
                      >
                        Review
                      </button>
                    {/if}
                  </div>
                  {#if deliverable.proofUrl}
                    <div class="mt-3">
                      <a
                        href={deliverable.proofUrl}
                        target="_blank"
                        rel="noopener noreferrer"
                        class="text-sm text-indigo-600 hover:text-indigo-800"
                      >
                        View Proof →
                      </a>
                    </div>
                  {/if}
                  {#if deliverable.feedbackNotes}
                    <div class="mt-3 p-3 bg-gray-50 rounded">
                      <p class="text-sm text-gray-700">{deliverable.feedbackNotes}</p>
                    </div>
                  {/if}
                </div>
              {/each}
            </div>
          {/if}
        </div>
      </div>

      <!-- Sidebar -->
      <div class="space-y-6">
        <!-- Budget & Dates -->
        <div class="bg-white shadow rounded-lg p-6">
          <h2 class="text-lg font-medium text-gray-900 mb-4">Campaign Details</h2>
          <dl class="space-y-3">
            <div>
              <dt class="text-sm font-medium text-gray-500">Budget</dt>
              <dd class="text-lg font-semibold text-gray-900">{formatCurrency(campaign.budget)}</dd>
            </div>
            <div>
              <dt class="text-sm font-medium text-gray-500">Start Date</dt>
              <dd class="text-gray-900">{formatDate(campaign.startDate)}</dd>
            </div>
            <div>
              <dt class="text-sm font-medium text-gray-500">End Date</dt>
              <dd class="text-gray-900">{formatDate(campaign.endDate)}</dd>
            </div>
          </dl>
        </div>

        <!-- Target Platforms -->
        {#if campaign.targetPlatforms && campaign.targetPlatforms.length > 0}
          <div class="bg-white shadow rounded-lg p-6">
            <h2 class="text-lg font-medium text-gray-900 mb-4">Target Platforms</h2>
            <div class="flex flex-wrap gap-2">
              {#each campaign.targetPlatforms as platform}
                <span class="px-3 py-1 bg-indigo-100 text-indigo-800 rounded-full text-sm">
                  {platform}
                </span>
              {/each}
            </div>
          </div>
        {/if}
      </div>
    </div>
  </div>
{/if}

<!-- Status Update Modal -->
<Modal bind:open={showStatusModal} title="Update Campaign Status">
  <div class="space-y-4">
    <div>
      <label for="status" class="block text-sm font-medium text-gray-700 mb-2">
        New Status
      </label>
      <select id="status" bind:value={newStatus} class="form-input">
        <option value="Draft">Draft</option>
        <option value="Open">Open</option>
        <option value="InProgress">In Progress</option>
        <option value="PendingReview">Pending Review</option>
        <option value="Completed">Completed</option>
        <option value="Cancelled">Cancelled</option>
      </select>
    </div>
    <div class="flex justify-end gap-2">
      <button on:click={() => (showStatusModal = false)} class="btn btn-secondary">Cancel</button>
      <button on:click={updateStatus} class="btn btn-primary">Update</button>
    </div>
  </div>
</Modal>

<!-- Deliverable Review Modal -->
<Modal bind:open={showDeliverableModal} title="Review Deliverable">
  {#if selectedDeliverable}
    <div class="space-y-4">
      <div>
        <h3 class="font-medium text-gray-900">{selectedDeliverable.title}</h3>
        <p class="text-sm text-gray-500 mt-1">{selectedDeliverable.description}</p>
      </div>
      {#if selectedDeliverable.proofUrl}
        <div>
          <a
            href={selectedDeliverable.proofUrl}
            target="_blank"
            rel="noopener noreferrer"
            class="text-sm text-indigo-600 hover:text-indigo-800"
          >
            View Proof →
          </a>
        </div>
      {/if}
      <div>
        <label for="feedback" class="block text-sm font-medium text-gray-700 mb-2">
          Feedback Notes (Optional)
        </label>
        <textarea
          id="feedback"
          bind:value={reviewFeedback}
          rows="4"
          class="form-input"
          placeholder="Add feedback for the influencer..."
        ></textarea>
      </div>
      <div class="flex justify-end gap-2">
        <button on:click={() => (showDeliverableModal = false)} class="btn btn-secondary">
          Cancel
        </button>
        <button on:click={rejectDeliverable} class="btn btn-danger">Reject</button>
        <button on:click={approveDeliverable} class="btn btn-primary">Approve</button>
      </div>
    </div>
  {/if}
</Modal>

