<script lang="ts">
  import { goto } from '$app/navigation';
  import { campaignsApi } from '../../../../lib/api';
  import { toastStore } from '../../../../lib/stores/toastStore';
  import { validateCampaign } from '../../../../lib/utils/validation';
  import LoadingSpinner from '../../../../lib/components/LoadingSpinner.svelte';
  import { PLATFORMS } from '../../../../lib/constants';
  
  let isLoading = false;
  let errors: Record<string, string> = {};
  
  // Form data
  let title = '';
  let description = '';
  let budget = 0;
  let startDate = '';
  let endDate = '';
  let requirements = '';
  let deliverables: string[] = [];
  let newDeliverable = '';
  let targetPlatforms: string[] = [];
  let targetAudience: Record<string, any> = {
    ageRange: '',
    gender: '',
    location: '',
    interests: ''
  };
  
  function addDeliverable() {
    if (newDeliverable.trim()) {
      deliverables = [...deliverables, newDeliverable.trim()];
      newDeliverable = '';
    }
  }
  
  function removeDeliverable(index: number) {
    deliverables = deliverables.filter((_, i) => i !== index);
  }
  
  function togglePlatform(platform: string) {
    if (targetPlatforms.includes(platform)) {
      targetPlatforms = targetPlatforms.filter(p => p !== platform);
    } else {
      targetPlatforms = [...targetPlatforms, platform];
    }
  }
  
  async function handleSubmit() {
    errors = {};
    const validation = validateCampaign({
      title,
      description,
      budget,
      startDate,
      endDate,
      requirements,
      deliverables,
      targetPlatforms,
      targetAudience: targetAudience || {},
    });

    if (!validation.isValid) {
      validation.errors.forEach((err) => {
        errors[err.field] = err.message;
      });
      toastStore.error('Please fix the errors in the form');
      return;
    }
    
    try {
      isLoading = true;
      const campaign = await campaignsApi.createCampaign({
        title,
        description,
        budget,
        startDate,
        endDate,
        requirements,
        deliverables,
        targetPlatforms,
        targetAudience: Object.keys(targetAudience).length > 0 ? targetAudience : undefined,
      });
      
      toastStore.success('Campaign created successfully!');
      goto(`/brand/campaigns/${campaign.id}`);
    } catch (err: any) {
      console.error('Error creating campaign:', err);
      toastStore.error(err.message || 'Failed to create campaign');
    } finally {
      isLoading = false;
    }
  }
</script>

<svelte:head>
  <title>Create Campaign - Influencer Marketplace</title>
</svelte:head>

<div class="max-w-3xl mx-auto">
  <div class="space-y-6">
    <div>
      <h1 class="text-2xl font-bold text-gray-900">Create New Campaign</h1>
      <p class="mt-1 text-sm text-gray-500">
        Fill out the details below to create your influencer marketing campaign
      </p>
    </div>

    <form on:submit|preventDefault={handleSubmit} class="space-y-6">

      <div class="bg-white shadow rounded-lg">
        <div class="px-4 py-5 sm:p-6 space-y-6">
          <!-- Basic Information -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Basic Information</h3>
            <div class="mt-4 grid grid-cols-1 gap-6">
              <div>
                <label for="title" class="form-label">Campaign Title *</label>
                <input
                  type="text"
                  id="title"
                  bind:value={title}
                  required
                  class="form-input {errors.title ? 'border-red-500' : ''}"
                  placeholder="Enter campaign title"
                />
                {#if errors.title}
                  <p class="mt-1 text-sm text-red-600">{errors.title}</p>
                {/if}
              </div>
              
              <div>
                <label for="description" class="form-label">Description *</label>
                <textarea
                  id="description"
                  bind:value={description}
                  required
                  rows="4"
                  class="form-input {errors.description ? 'border-red-500' : ''}"
                  placeholder="Describe your campaign goals and expectations"
                ></textarea>
                {#if errors.description}
                  <p class="mt-1 text-sm text-red-600">{errors.description}</p>
                {/if}
              </div>
              
              <div class="grid grid-cols-1 gap-6 sm:grid-cols-3">
                <div>
                  <label for="budget" class="form-label">Budget (NPR) *</label>
                  <input
                    type="number"
                    id="budget"
                    bind:value={budget}
                    required
                    min="0"
                    step="100"
                    class="form-input"
                    placeholder="0"
                  />
                </div>
                
                <div>
                  <label for="start-date" class="form-label">Start Date *</label>
                  <input
                    type="date"
                    id="start-date"
                    bind:value={startDate}
                    required
                    class="form-input"
                  />
                </div>
                
                <div>
                  <label for="end-date" class="form-label">End Date *</label>
                  <input
                    type="date"
                    id="end-date"
                    bind:value={endDate}
                    required
                    class="form-input"
                  />
                </div>
              </div>
            </div>
          </div>

          <!-- Requirements -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Campaign Requirements</h3>
            <div class="mt-4">
              <label for="requirements" class="form-label">Requirements *</label>
              <textarea
                id="requirements"
                bind:value={requirements}
                required
                rows="4"
                class="form-input"
                placeholder="Specify your requirements for influencers (e.g., minimum followers, content style, etc.)"
              ></textarea>
            </div>
          </div>

          <!-- Deliverables -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Deliverables</h3>
            <div class="mt-4 space-y-3">
              <div class="flex gap-2">
                <input
                  type="text"
                  bind:value={newDeliverable}
                  placeholder="Enter deliverable (e.g., 1 Instagram post, 3 stories)"
                  class="flex-1 form-input"
                  on:keydown={(e) => e.key === 'Enter' && (e.preventDefault(), addDeliverable())}
                />
                <button
                  type="button"
                  on:click={addDeliverable}
                  class="btn btn-secondary"
                >
                  Add
                </button>
              </div>
              {#if errors.deliverables}
                <p class="text-sm text-red-600">{errors.deliverables}</p>
              {/if}
              <div class="flex flex-wrap gap-2">
                {#each deliverables as deliverable, index}
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
          </div>

          <!-- Target Platforms -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Target Platforms</h3>
            <div class="mt-4">
              <div class="grid grid-cols-2 gap-3 sm:grid-cols-3">
                {#each PLATFORMS as platform}
                  <label class="flex items-center">
                    <input
                      type="checkbox"
                      checked={targetPlatforms.includes(platform)}
                      on:change={() => togglePlatform(platform)}
                      class="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
                    />
                    <span class="ml-2 text-sm text-gray-700">{platform}</span>
                  </label>
                {/each}
              </div>
              {#if errors.targetPlatforms}
                <p class="mt-1 text-sm text-red-600">{errors.targetPlatforms}</p>
              {/if}
            </div>
          </div>

          <!-- Target Audience -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Target Audience</h3>
            <div class="mt-4 grid grid-cols-1 gap-6 sm:grid-cols-2">
              <div>
                <label for="age-range" class="form-label">Age Range</label>
                <input
                  type="text"
                  id="age-range"
                  bind:value={targetAudience.ageRange}
                  class="form-input"
                  placeholder="e.g., 18-35"
                />
              </div>
              
              <div>
                <label for="gender" class="form-label">Gender</label>
                <select id="gender" bind:value={targetAudience.gender} class="form-input">
                  <option value="">Any</option>
                  <option value="Male">Male</option>
                  <option value="Female">Female</option>
                  <option value="Non-binary">Non-binary</option>
                </select>
              </div>
              
              <div>
                <label for="location" class="form-label">Location</label>
                <input
                  type="text"
                  id="location"
                  bind:value={targetAudience.location}
                  class="form-input"
                  placeholder="e.g., Nepal, Kathmandu"
                />
              </div>
              
              <div>
                <label for="interests" class="form-label">Interests</label>
                <input
                  type="text"
                  id="interests"
                  bind:value={targetAudience.interests}
                  class="form-input"
                  placeholder="e.g., Fashion, Technology, Travel"
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Form Actions -->
      <div class="flex justify-end space-x-3">
        <a href="/brand/campaigns" class="btn btn-secondary">
          Cancel
        </a>
        <button type="submit" disabled={isLoading} class="btn btn-primary">
          {#if isLoading}
            <LoadingSpinner size="sm" color="text-white" />
            Creating...
          {:else}
            Create Campaign
          {/if}
        </button>
      </div>
    </form>
  </div>
</div>