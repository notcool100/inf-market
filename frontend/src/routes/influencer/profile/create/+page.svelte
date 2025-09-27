<script lang="ts">
  import { goto } from '$app/navigation';
  import { apiClient } from '../../../../lib/api';
  import LoadingSpinner from '../../../../lib/components/LoadingSpinner.svelte';
  
  let isLoading = false;
  let error = '';
  
  // Form data
  let bio = '';
  let nicheFocus = '';
  let followersCount = 0;
  let instagramHandle = '';
  let tiktokHandle = '';
  let youtubeChannel = '';
  let facebookPage = '';
  let linkedinProfile = '';
  let websiteUrl = '';
  let minCampaignRate = 0;
  let contentTypes = [''];
  let location = '';
  let demographics = {
    primaryAgeGroup: '',
    primaryGender: '',
    topLocations: '',
    topInterests: ''
  };
  
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
  
  function addContentType() {
    contentTypes = [...contentTypes, ''];
  }
  
  function removeContentType(index: number) {
    contentTypes = contentTypes.filter((_, i) => i !== index);
  }
  
  async function handleSubmit() {
    if (!bio || !nicheFocus || !followersCount || !minCampaignRate || !location) {
      error = 'Please fill in all required fields';
      return;
    }
    
    if (contentTypes.filter(ct => ct.trim()).length === 0) {
      error = 'Please add at least one content type';
      return;
    }
    
    try {
      isLoading = true;
      error = '';
      
      const profileData = {
        bio,
        nicheFocus,
        followersCount,
        instagramHandle,
        tiktokHandle,
        youtubeChannel,
        facebookPage,
        linkedinProfile,
        websiteUrl,
        minCampaignRate,
        contentTypes: contentTypes.filter(ct => ct.trim()),
        location,
        demographics
      };
      
      const response = await apiClient.post('/api/influencer/profile', profileData);
      
      if (response.data) {
        goto('/influencer/dashboard');
      }
    } catch (err: any) {
      console.error('Error creating profile:', err);
      error = err.response?.data?.message || 'Failed to create profile';
    } finally {
      isLoading = false;
    }
  }
</script>

<svelte:head>
  <title>Create Influencer Profile - Influencer Marketplace</title>
</svelte:head>

<div class="max-w-3xl mx-auto">
  <div class="space-y-6">
    <div>
      <h1 class="text-2xl font-bold text-gray-900">Create Your Influencer Profile</h1>
      <p class="mt-1 text-sm text-gray-500">
        Complete your profile to start receiving campaign offers from brands
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
            <div class="mt-4 space-y-6">
              <div>
                <label for="bio" class="form-label">Bio *</label>
                <textarea
                  id="bio"
                  bind:value={bio}
                  required
                  rows="4"
                  class="form-input"
                  placeholder="Tell brands about yourself, your content style, and what makes you unique..."
                ></textarea>
              </div>
              
              <div class="grid grid-cols-1 gap-6 sm:grid-cols-2">
                <div>
                  <label for="niche" class="form-label">Niche Focus *</label>
                  <select id="niche" bind:value={nicheFocus} required class="form-input">
                    <option value="">Select your niche</option>
                    {#each nicheOptions as niche}
                      <option value={niche}>{niche}</option>
                    {/each}
                  </select>
                </div>
                
                <div>
                  <label for="followers" class="form-label">Total Followers *</label>
                  <input
                    type="number"
                    id="followers"
                    bind:value={followersCount}
                    required
                    min="0"
                    class="form-input"
                    placeholder="0"
                  />
                </div>
              </div>
              
              <div class="grid grid-cols-1 gap-6 sm:grid-cols-2">
                <div>
                  <label for="location" class="form-label">Location *</label>
                  <input
                    type="text"
                    id="location"
                    bind:value={location}
                    required
                    class="form-input"
                    placeholder="e.g., Kathmandu, Nepal"
                  />
                </div>
                
                <div>
                  <label for="rate" class="form-label">Minimum Campaign Rate (NPR) *</label>
                  <input
                    type="number"
                    id="rate"
                    bind:value={minCampaignRate}
                    required
                    min="0"
                    step="100"
                    class="form-input"
                    placeholder="0"
                  />
                </div>
              </div>
            </div>
          </div>

          <!-- Social Media Handles -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Social Media Profiles</h3>
            <div class="mt-4 grid grid-cols-1 gap-6 sm:grid-cols-2">
              <div>
                <label for="instagram" class="form-label">Instagram Handle</label>
                <div class="mt-1 flex rounded-md shadow-sm">
                  <span class="inline-flex items-center px-3 rounded-l-md border border-r-0 border-gray-300 bg-gray-50 text-gray-500 text-sm">
                    @
                  </span>
                  <input
                    type="text"
                    id="instagram"
                    bind:value={instagramHandle}
                    class="flex-1 min-w-0 block w-full px-3 py-2 rounded-none rounded-r-md border-gray-300 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    placeholder="username"
                  />
                </div>
              </div>
              
              <div>
                <label for="tiktok" class="form-label">TikTok Handle</label>
                <div class="mt-1 flex rounded-md shadow-sm">
                  <span class="inline-flex items-center px-3 rounded-l-md border border-r-0 border-gray-300 bg-gray-50 text-gray-500 text-sm">
                    @
                  </span>
                  <input
                    type="text"
                    id="tiktok"
                    bind:value={tiktokHandle}
                    class="flex-1 min-w-0 block w-full px-3 py-2 rounded-none rounded-r-md border-gray-300 focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
                    placeholder="username"
                  />
                </div>
              </div>
              
              <div>
                <label for="youtube" class="form-label">YouTube Channel</label>
                <input
                  type="url"
                  id="youtube"
                  bind:value={youtubeChannel}
                  class="form-input"
                  placeholder="https://youtube.com/c/yourchannel"
                />
              </div>
              
              <div>
                <label for="facebook" class="form-label">Facebook Page</label>
                <input
                  type="url"
                  id="facebook"
                  bind:value={facebookPage}
                  class="form-input"
                  placeholder="https://facebook.com/yourpage"
                />
              </div>
              
              <div>
                <label for="linkedin" class="form-label">LinkedIn Profile</label>
                <input
                  type="url"
                  id="linkedin"
                  bind:value={linkedinProfile}
                  class="form-input"
                  placeholder="https://linkedin.com/in/yourprofile"
                />
              </div>
              
              <div>
                <label for="website" class="form-label">Website</label>
                <input
                  type="url"
                  id="website"
                  bind:value={websiteUrl}
                  class="form-input"
                  placeholder="https://yourwebsite.com"
                />
              </div>
            </div>
          </div>

          <!-- Content Types -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Content Types</h3>
            <div class="mt-4 space-y-3">
              {#each contentTypes as contentType, index}
                <div class="flex items-center space-x-3">
                  <input
                    type="text"
                    bind:value={contentTypes[index]}
                    placeholder="e.g., Instagram Posts, Stories, Reels, YouTube Videos"
                    class="flex-1 form-input"
                  />
                  {#if contentTypes.length > 1}
                    <button
                      type="button"
                      on:click={() => removeContentType(index)}
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
                on:click={addContentType}
                class="text-indigo-600 hover:text-indigo-800 text-sm font-medium"
              >
                + Add Content Type
              </button>
            </div>
          </div>

          <!-- Audience Demographics -->
          <div>
            <h3 class="text-lg font-medium text-gray-900">Audience Demographics</h3>
            <p class="mt-1 text-sm text-gray-500">Help brands understand your audience</p>
            <div class="mt-4 grid grid-cols-1 gap-6 sm:grid-cols-2">
              <div>
                <label for="age-group" class="form-label">Primary Age Group</label>
                <input
                  type="text"
                  id="age-group"
                  bind:value={demographics.primaryAgeGroup}
                  class="form-input"
                  placeholder="e.g., 18-24, 25-34"
                />
              </div>
              
              <div>
                <label for="gender" class="form-label">Primary Gender</label>
                <select id="gender" bind:value={demographics.primaryGender} class="form-input">
                  <option value="">Select primary gender</option>
                  <option value="Male">Male</option>
                  <option value="Female">Female</option>
                  <option value="Mixed">Mixed</option>
                </select>
              </div>
              
              <div>
                <label for="top-locations" class="form-label">Top Locations</label>
                <input
                  type="text"
                  id="top-locations"
                  bind:value={demographics.topLocations}
                  class="form-input"
                  placeholder="e.g., Nepal 60%, India 25%, USA 15%"
                />
              </div>
              
              <div>
                <label for="interests" class="form-label">Top Interests</label>
                <input
                  type="text"
                  id="interests"
                  bind:value={demographics.topInterests}
                  class="form-input"
                  placeholder="e.g., Fashion, Travel, Technology"
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Form Actions -->
      <div class="flex justify-end space-x-3">
        <a href="/influencer/dashboard" class="btn btn-secondary">
          Cancel
        </a>
        <button type="submit" disabled={isLoading} class="btn btn-primary">
          {#if isLoading}
            <LoadingSpinner size="sm" color="text-white" />
            Creating Profile...
          {:else}
            Create Profile
          {/if}
        </button>
      </div>
    </form>
  </div>
</div>