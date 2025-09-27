<script lang="ts">
  import { goto } from '$app/navigation';
  import { apiClient } from '../../../../lib/api';
  import LoadingSpinner from '../../../../lib/components/LoadingSpinner.svelte';
  
  let isLoading = false;
  let error = '';
  
  // Form data
  let title = '';
  let description = '';
  let budget = 0;
  let startDate = '';
  let endDate = '';
  let requirements = '';
  let deliverables = [''];
  let targetPlatforms = ['Instagram'];
  let targetAudience = {
    ageRange: '',
    gender: '',
    location: '',
    interests: ''
  };
  
  const platformOptions = [
    'Instagram',
    'TikTok',
    'YouTube',
    'Facebook',
    'LinkedIn',
    'Twitter'
  ];
  
  function addDeliverable() {
    deliverables = [...deliverables, ''];
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
    if (!title || !description || !budget || !startDate || !endDate || !requirements) {
      error = 'Please fill in all required fields';
      return;
    }
    
    if (new Date(startDate) >= new Date(endDate)) {
      error = 'End date must be after start date';
      return;
    }
    
    if (deliverables.filter(d => d.trim()).length === 0) {
      error = 'Please add at least one deliverable';
      return;
    }
    
    if (targetPlatforms.length === 0) {
      error = 'Please select at least one target platform';
      return;
    }
    
    try {
      isLoading = true;
      error = '';
      
      const campaignData = {
        title,
        description,
        budget,
        startDate,
        endDate,
        requirements,
        deliverables: deliverables.filter(d => d.trim()),
        targetPlatforms,
        targetAudience
      };
      
      const response = await apiClient.post('/api/brand/campaigns', campaignData);
      
      if (response.data) {
        goto(`/brand/campaigns/${response.data.id}`);
      }
    } catch (err: any) {
      console.error('Error creating campaign:', err);
      error = err.response?.data?.message || 'Failed to create campaign';
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
      {#if error}
        <div class="rounded-md bg-red-50 p-4">
          <div class="text-sm text-red-700">{error}</div>
        </div>
      {/if}

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
                  class="form-input"
                  placeholder="Enter campaign title"
                />
              </div>
              
              <div>
                <label for="description" class="form-label">Description *</label>
                <textarea
                  id="description"
                  bind:value={description}
                  required
                  rows="4"
                  class="form-input"
                  placeholder="Describe your campaign goals and expectations"
                ></textarea>
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
              {#each deliverables as deliverable, index}
                <div class="flex items-center space-x-3">
                  <input
                    type="text"
                    bind:value={deliverables[index]}
                    placeholder="Enter deliverable (e.g., 1 Instagram post, 3 stories)"
                    class="flex-1 form-input"
                  />
                  {#if deliverables.length > 1}
                    <button
                      type="button"
                      on:click={() => removeDeliverable(index)}
                      class="text-red-600 hover:text-red-800"
                    >
                      <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                      </svg>
                    </button>
                  {/if}
                </div>
              {/each}
              <button
                type="button"
                on:click={addDeliverable}
                class="text-indigo-600 hover:text-indigo-800 text-sm font-medium"
              >
                + Add Deliverable
              </button>
            </div>
          </div>

          <!-- Target Platforms -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Target Platforms</h3>
            <div class="mt-4">
              <div class="grid grid-cols-2 gap-3 sm:grid-cols-3">
                {#each platformOptions as platform}
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