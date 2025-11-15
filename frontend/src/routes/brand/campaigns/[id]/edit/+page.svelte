<script lang="ts">
  import { onMount } from 'svelte';
  import { page } from '$app/stores';
  import { goto } from '$app/navigation';
  import { campaignsApi } from '../../../../../lib/api';
  import { toastStore } from '../../../../../lib/stores/toastStore';
  import { validateCampaign } from '../../../../../lib/utils/validation';
  import type { CampaignDto, CreateCampaignRequest } from '../../../../../lib/api/types';
  import LoadingSpinner from '../../../../../lib/components/LoadingSpinner.svelte';
  import { PLATFORMS } from '../../../../../lib/constants';

  const campaignId = $page.params.id;
  let isLoading = true;
  let isSaving = false;
  let campaign: CampaignDto | null = null;
  let formData: CreateCampaignRequest = {
    title: '',
    description: '',
    budget: 0,
    startDate: '',
    endDate: '',
    requirements: '',
    deliverables: [],
    targetAudience: {},
    targetPlatforms: [],
  };
  let newDeliverable = '';
  let newPlatform = '';
  let errors: Record<string, string> = {};

  onMount(async () => {
    await loadCampaign();
  });

  async function loadCampaign() {
    try {
      isLoading = true;
      campaign = await campaignsApi.getCampaign(campaignId);
      if (campaign) {
        formData = {
          title: campaign.title,
          description: campaign.description,
          budget: campaign.budget,
          startDate: campaign.startDate.split('T')[0],
          endDate: campaign.endDate.split('T')[0],
          requirements: campaign.requirements,
          deliverables: campaign.deliverables || [],
          targetAudience: campaign.targetAudience || {},
          targetPlatforms: campaign.targetPlatforms || [],
        };
      }
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to load campaign');
      goto('/brand/campaigns');
    } finally {
      isLoading = false;
    }
  }

  function addDeliverable() {
    if (newDeliverable.trim()) {
      formData.deliverables = [...formData.deliverables, newDeliverable.trim()];
      newDeliverable = '';
    }
  }

  function removeDeliverable(index: number) {
    formData.deliverables = formData.deliverables.filter((_, i) => i !== index);
  }

  function addPlatform() {
    if (newPlatform && !formData.targetPlatforms.includes(newPlatform)) {
      formData.targetPlatforms = [...formData.targetPlatforms, newPlatform];
      newPlatform = '';
    }
  }

  function removePlatform(platform: string) {
    formData.targetPlatforms = formData.targetPlatforms.filter((p) => p !== platform);
  }

  async function handleSubmit() {
    errors = {};
    const validation = validateCampaign(formData);

    if (!validation.isValid) {
      validation.errors.forEach((err) => {
        errors[err.field] = err.message;
      });
      toastStore.error('Please fix the errors in the form');
      return;
    }

    try {
      isSaving = true;
      await campaignsApi.updateCampaign(campaignId, formData);
      toastStore.success('Campaign updated successfully');
      goto(`/brand/campaigns/${campaignId}`);
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to update campaign');
    } finally {
      isSaving = false;
    }
  }
</script>

<svelte:head>
  <title>Edit Campaign - Influencer Marketplace</title>
</svelte:head>

{#if isLoading}
  <div class="flex justify-center py-12">
    <LoadingSpinner size="lg" />
  </div>
{:else}
  <div class="max-w-4xl mx-auto space-y-6">
    <div class="flex items-center justify-between">
      <div>
        <h1 class="text-3xl font-bold text-gray-900">Edit Campaign</h1>
        <p class="mt-1 text-sm text-gray-500">Update your campaign details</p>
      </div>
      <a href="/brand/campaigns/{campaignId}" class="btn btn-secondary">Cancel</a>
    </div>

    <form on:submit|preventDefault={handleSubmit} class="bg-white shadow rounded-lg p-6 space-y-6">
      <!-- Title -->
      <div>
        <label for="title" class="block text-sm font-medium text-gray-700 mb-2">
          Campaign Title <span class="text-red-500">*</span>
        </label>
        <input
          type="text"
          id="title"
          bind:value={formData.title}
          class="form-input {errors.title ? 'border-red-500' : ''}"
          required
        />
        {#if errors.title}
          <p class="mt-1 text-sm text-red-600">{errors.title}</p>
        {/if}
      </div>

      <!-- Description -->
      <div>
        <label for="description" class="block text-sm font-medium text-gray-700 mb-2">
          Description <span class="text-red-500">*</span>
        </label>
        <textarea
          id="description"
          bind:value={formData.description}
          rows="4"
          class="form-input {errors.description ? 'border-red-500' : ''}"
          required
        ></textarea>
        {#if errors.description}
          <p class="mt-1 text-sm text-red-600">{errors.description}</p>
        {/if}
      </div>

      <!-- Budget & Dates -->
      <div class="grid grid-cols-1 gap-4 sm:grid-cols-3">
        <div>
          <label for="budget" class="block text-sm font-medium text-gray-700 mb-2">
            Budget (NPR) <span class="text-red-500">*</span>
          </label>
          <input
            type="number"
            id="budget"
            bind:value={formData.budget}
            min="0"
            step="0.01"
            class="form-input {errors.budget ? 'border-red-500' : ''}"
            required
          />
          {#if errors.budget}
            <p class="mt-1 text-sm text-red-600">{errors.budget}</p>
          {/if}
        </div>
        <div>
          <label for="startDate" class="block text-sm font-medium text-gray-700 mb-2">
            Start Date <span class="text-red-500">*</span>
          </label>
          <input
            type="date"
            id="startDate"
            bind:value={formData.startDate}
            class="form-input {errors.startDate ? 'border-red-500' : ''}"
            required
          />
          {#if errors.startDate}
            <p class="mt-1 text-sm text-red-600">{errors.startDate}</p>
          {/if}
        </div>
        <div>
          <label for="endDate" class="block text-sm font-medium text-gray-700 mb-2">
            End Date <span class="text-red-500">*</span>
          </label>
          <input
            type="date"
            id="endDate"
            bind:value={formData.endDate}
            class="form-input {errors.endDate ? 'border-red-500' : ''}"
            required
          />
          {#if errors.endDate}
            <p class="mt-1 text-sm text-red-600">{errors.endDate}</p>
          {/if}
        </div>
      </div>

      <!-- Requirements -->
      <div>
        <label for="requirements" class="block text-sm font-medium text-gray-700 mb-2">
          Requirements <span class="text-red-500">*</span>
        </label>
        <textarea
          id="requirements"
          bind:value={formData.requirements}
          rows="4"
          class="form-input {errors.requirements ? 'border-red-500' : ''}"
          required
        ></textarea>
        {#if errors.requirements}
          <p class="mt-1 text-sm text-red-600">{errors.requirements}</p>
        {/if}
      </div>

      <!-- Deliverables -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Deliverables <span class="text-red-500">*</span>
        </label>
        <div class="flex gap-2 mb-2">
          <input
            type="text"
            bind:value={newDeliverable}
            placeholder="Add deliverable..."
            class="form-input flex-1"
            on:keydown={(e) => e.key === 'Enter' && (e.preventDefault(), addDeliverable())}
          />
          <button type="button" on:click={addDeliverable} class="btn btn-secondary">
            Add
          </button>
        </div>
        {#if errors.deliverables}
          <p class="mt-1 text-sm text-red-600">{errors.deliverables}</p>
        {/if}
        <div class="flex flex-wrap gap-2 mt-2">
          {#each formData.deliverables as deliverable, index}
            <span class="inline-flex items-center gap-1 px-3 py-1 bg-indigo-100 text-indigo-800 rounded-full text-sm">
              {deliverable}
              <button
                type="button"
                on:click={() => removeDeliverable(index)}
                class="text-indigo-600 hover:text-indigo-800"
              >
                ×
              </button>
            </span>
          {/each}
        </div>
      </div>

      <!-- Target Platforms -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-2">
          Target Platforms <span class="text-red-500">*</span>
        </label>
        <div class="flex gap-2 mb-2">
          <select bind:value={newPlatform} class="form-input flex-1">
            <option value="">Select platform...</option>
            {#each PLATFORMS as platform}
              <option value={platform}>{platform}</option>
            {/each}
          </select>
          <button type="button" on:click={addPlatform} class="btn btn-secondary">
            Add
          </button>
        </div>
        {#if errors.targetPlatforms}
          <p class="mt-1 text-sm text-red-600">{errors.targetPlatforms}</p>
        {/if}
        <div class="flex flex-wrap gap-2 mt-2">
          {#each formData.targetPlatforms as platform}
            <span class="inline-flex items-center gap-1 px-3 py-1 bg-green-100 text-green-800 rounded-full text-sm">
              {platform}
              <button
                type="button"
                on:click={() => removePlatform(platform)}
                class="text-green-600 hover:text-green-800"
              >
                ×
              </button>
            </span>
          {/each}
        </div>
      </div>

      <!-- Actions -->
      <div class="flex justify-end gap-2 pt-4 border-t">
        <a href="/brand/campaigns/{campaignId}" class="btn btn-secondary">Cancel</a>
        <button type="submit" class="btn btn-primary" disabled={isSaving}>
          {isSaving ? 'Saving...' : 'Update Campaign'}
        </button>
      </div>
    </form>
  </div>
{/if}

