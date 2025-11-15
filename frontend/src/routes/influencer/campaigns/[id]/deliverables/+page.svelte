<script lang="ts">
  import { onMount } from 'svelte';
  import { page } from '$app/stores';
  import { goto } from '$app/navigation';
  import { campaignsApi, uploadApi } from '../../../../../lib/api';
  import { toastStore } from '../../../../../lib/stores/toastStore';
  import { formatDate } from '../../../../../lib/utils/format';
  import type { CampaignDto, CampaignDeliverableDto } from '../../../../../lib/api/types';
  import LoadingSpinner from '../../../../../lib/components/LoadingSpinner.svelte';
  import StatusBadge from '../../../../../lib/components/StatusBadge.svelte';
  import Modal from '../../../../../lib/components/Modal.svelte';
  import { Upload, X } from 'lucide-svelte';

  const campaignId = $page.params.id;
  let isLoading = true;
  let campaign: CampaignDto | null = null;
  let deliverables: CampaignDeliverableDto[] = [];
  let error = '';
  let showSubmitModal = false;
  let selectedDeliverable: CampaignDeliverableDto | null = null;
  let proofUrl = '';
  let screenshotUrl = '';
  let uploadingProof = false;
  let uploadingScreenshot = false;
  let proofFile: File | null = null;
  let screenshotFile: File | null = null;

  onMount(async () => {
    await loadData();
  });

  async function loadData() {
    try {
      isLoading = true;
      error = '';
      [campaign, deliverables] = await Promise.all([
        campaignsApi.getCampaign(campaignId),
        campaignsApi.getCampaignDeliverables(campaignId),
      ]);
    } catch (err: any) {
      console.error('Error loading data:', err);
      error = err.message || 'Failed to load data';
      toastStore.error(error);
    } finally {
      isLoading = false;
    }
  }

  function openSubmitModal(deliverable: CampaignDeliverableDto) {
    selectedDeliverable = deliverable;
    proofUrl = '';
    screenshotUrl = '';
    proofFile = null;
    screenshotFile = null;
    showSubmitModal = true;
  }

  async function handleFileUpload(file: File, type: 'proof' | 'screenshot') {
    try {
      if (type === 'proof') {
        uploadingProof = true;
      } else {
        uploadingScreenshot = true;
      }

      const result = await uploadApi.uploadImage(file, (progress) => {
        // Progress callback if needed
      });

      if (result.success && result.fileUrl) {
        if (type === 'proof') {
          proofUrl = result.fileUrl;
        } else {
          screenshotUrl = result.fileUrl;
        }
        toastStore.success('File uploaded successfully');
      } else {
        toastStore.error(result.errorMessage || 'File upload failed');
      }
    } catch (err: any) {
      toastStore.error(err.message || 'File upload failed');
    } finally {
      if (type === 'proof') {
        uploadingProof = false;
      } else {
        uploadingScreenshot = false;
      }
    }
  }

  async function handleSubmit() {
    if (!selectedDeliverable) return;

    if (!proofUrl) {
      toastStore.error('Please upload proof or provide proof URL');
      return;
    }

    try {
      await campaignsApi.submitDeliverable(selectedDeliverable.id, {
        proofUrl,
        screenshotUrl: screenshotUrl || undefined,
      });
      toastStore.success('Deliverable submitted successfully');
      showSubmitModal = false;
      await loadData();
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to submit deliverable');
    }
  }

  function handleProofFileChange(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files[0]) {
      proofFile = target.files[0];
      handleFileUpload(proofFile, 'proof');
    }
  }

  function handleScreenshotFileChange(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files[0]) {
      screenshotFile = target.files[0];
      handleFileUpload(screenshotFile, 'screenshot');
    }
  }
</script>

<svelte:head>
  <title>Campaign Deliverables - Influencer Marketplace</title>
</svelte:head>

{#if isLoading}
  <div class="flex justify-center py-12">
    <LoadingSpinner size="lg" />
  </div>
{:else if error || !campaign}
  <div class="rounded-md bg-red-50 p-4">
    <div class="text-sm text-red-700">{error || 'Campaign not found'}</div>
    <a href="/influencer/campaigns" class="mt-2 inline-block text-sm text-red-600 hover:text-red-800">
      ← Back to Campaigns
    </a>
  </div>
{:else}
  <div class="space-y-6">
    <!-- Header -->
    <div>
      <h1 class="text-3xl font-bold text-gray-900">{campaign.title}</h1>
      <p class="mt-1 text-sm text-gray-500">Submit deliverables for this campaign</p>
    </div>

    <!-- Deliverables List -->
    <div class="bg-white shadow rounded-lg">
      <div class="px-4 py-5 sm:p-6">
        <h2 class="text-lg font-medium text-gray-900 mb-4">Deliverables</h2>

        {#if deliverables.length === 0}
          <p class="text-sm text-gray-500">No deliverables assigned yet.</p>
        {:else}
          <div class="space-y-4">
            {#each deliverables as deliverable}
              <div class="border rounded-lg p-4">
                <div class="flex items-start justify-between">
                  <div class="flex-1">
                    <div class="flex items-center gap-3 mb-2">
                      <h3 class="text-lg font-medium text-gray-900">{deliverable.title}</h3>
                      <StatusBadge status={deliverable.status} />
                    </div>
                    <p class="text-sm text-gray-600 mb-3">{deliverable.description}</p>
                    <div class="flex items-center gap-4 text-sm text-gray-500">
                      <span>Type: {deliverable.deliverableType}</span>
                      <span>Due: {formatDate(deliverable.dueDate)}</span>
                      {#if deliverable.submittedAt}
                        <span>Submitted: {formatDate(deliverable.submittedAt)}</span>
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
                        <p class="text-sm font-medium text-gray-700">Feedback:</p>
                        <p class="text-sm text-gray-600">{deliverable.feedbackNotes}</p>
                      </div>
                    {/if}
                  </div>
                  {#if deliverable.status === 'Pending'}
                    <button
                      on:click={() => openSubmitModal(deliverable)}
                      class="btn btn-primary"
                    >
                      Submit
                    </button>
                  {/if}
                </div>
              </div>
            {/each}
          </div>
        {/if}
      </div>
    </div>
  </div>
{/if}

<!-- Submit Modal -->
<Modal bind:isOpen={showSubmitModal} title="Submit Deliverable">
  {#if selectedDeliverable}
    <div class="space-y-4">
      <div>
        <h3 class="font-medium text-gray-900">{selectedDeliverable.title}</h3>
        <p class="text-sm text-gray-500 mt-1">{selectedDeliverable.description}</p>
      </div>

      <!-- Proof Upload -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Proof URL or Upload <span class="text-red-500">*</span>
        </label>
        <div class="flex gap-2">
          <input
            type="text"
            bind:value={proofUrl}
            class="form-input flex-1"
            placeholder="Enter proof URL or upload file"
          />
          <label class="btn btn-secondary cursor-pointer">
            <Upload class="w-4 h-4 mr-1" />
            Upload
            <input
              type="file"
              accept="image/*"
              class="hidden"
              on:change={handleProofFileChange}
              disabled={uploadingProof}
            />
          </label>
        </div>
        {#if uploadingProof}
          <p class="mt-1 text-sm text-gray-500">Uploading...</p>
        {/if}
        {#if proofUrl}
          <div class="mt-2">
            <a
              href={proofUrl}
              target="_blank"
              rel="noopener noreferrer"
              class="text-sm text-indigo-600 hover:text-indigo-800"
            >
              View uploaded file →
            </a>
          </div>
        {/if}
      </div>

      <!-- Screenshot Upload -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Screenshot (Optional)
        </label>
        <div class="flex gap-2">
          <input
            type="text"
            bind:value={screenshotUrl}
            class="form-input flex-1"
            placeholder="Enter screenshot URL or upload file"
          />
          <label class="btn btn-secondary cursor-pointer">
            <Upload class="w-4 h-4 mr-1" />
            Upload
            <input
              type="file"
              accept="image/*"
              class="hidden"
              on:change={handleScreenshotFileChange}
              disabled={uploadingScreenshot}
            />
          </label>
        </div>
        {#if uploadingScreenshot}
          <p class="mt-1 text-sm text-gray-500">Uploading...</p>
        {/if}
        {#if screenshotUrl}
          <div class="mt-2">
            <a
              href={screenshotUrl}
              target="_blank"
              rel="noopener noreferrer"
              class="text-sm text-indigo-600 hover:text-indigo-800"
            >
              View uploaded file →
            </a>
          </div>
        {/if}
      </div>

      <div class="flex justify-end gap-2">
        <button on:click={() => (showSubmitModal = false)} class="btn btn-secondary">
          Cancel
        </button>
        <button on:click={handleSubmit} class="btn btn-primary" disabled={!proofUrl}>
          Submit Deliverable
        </button>
      </div>
    </div>
  {/if}
</Modal>

