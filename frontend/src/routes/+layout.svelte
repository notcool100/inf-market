<script lang="ts">
  import '../app.css';
  import { onMount } from 'svelte';
  import { page } from '$app/stores';
  import { goto } from '$app/navigation';
  import { authStore } from '../stores/authStore';

  let isLoggedIn = false;
  let userRoles: string[] = [];
  let userName = '';

  onMount(() => {
    const unsubscribe = authStore.subscribe(auth => {
      isLoggedIn = !!auth.token;
      userRoles = auth.roles || [];
      userName = auth.firstName ? `${auth.firstName} ${auth.lastName}` : '';
    });

    return unsubscribe;
  });

  function logout() {
    authStore.logout();
    goto('/login');
  }
</script>

<div class="min-h-screen flex flex-col bg-gray-50">
  <header class="bg-white shadow-sm">
    <nav class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex justify-between h-16">
        <div class="flex">
          <div class="flex-shrink-0 flex items-center">
            <a href="/" class="text-xl font-bold text-indigo-600">Influencer Marketplace</a>
          </div>
          <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
            <a href="/" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
              Home
            </a>
            {#if isLoggedIn}
              {#if userRoles.includes('Brand')}
                <a href="/brand/dashboard" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Dashboard
                </a>
                <a href="/brand/campaigns" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Campaigns
                </a>
                <a href="/influencers" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Find Influencers
                </a>
              {:else if userRoles.includes('Influencer')}
                <a href="/influencer/dashboard" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Dashboard
                </a>
                <a href="/influencer/campaigns" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Campaigns
                </a>
                <a href="/influencer/profile" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  My Profile
                </a>
              {:else if userRoles.includes('Admin')}
                <a href="/admin/dashboard" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Admin Dashboard
                </a>
                <a href="/admin/users" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Users
                </a>
                <a href="/admin/campaigns" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                  Campaigns
                </a>
              {/if}
              <a href="/wallet" class="border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700 inline-flex items-center px-1 pt-1 border-b-2 text-sm font-medium">
                Wallet
              </a>
            {/if}
          </div>
        </div>
        <div class="hidden sm:ml-6 sm:flex sm:items-center">
          {#if isLoggedIn}
            <div class="ml-3 relative">
              <div>
                <span class="mr-4 text-sm text-gray-700">Welcome, {userName}</span>
                <button on:click={logout} class="bg-indigo-600 text-white px-4 py-2 rounded-md text-sm font-medium hover:bg-indigo-700">
                  Logout
                </button>
              </div>
            </div>
          {:else}
            <a href="/login" class="text-gray-500 hover:text-gray-700 px-3 py-2 rounded-md text-sm font-medium">
              Login
            </a>
            <a href="/register" class="bg-indigo-600 text-white px-4 py-2 rounded-md text-sm font-medium hover:bg-indigo-700">
              Register
            </a>
          {/if}
        </div>
      </div>
    </nav>
  </header>

  <main class="flex-grow">
    <div class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
      <slot />
    </div>
  </main>

  <footer class="bg-white">
    <div class="max-w-7xl mx-auto py-6 px-4 sm:px-6 lg:px-8">
      <p class="text-center text-sm text-gray-500">
        &copy; 2023 Influencer Marketplace. All rights reserved.
      </p>
    </div>
  </footer>
</div>