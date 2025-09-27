<script lang="ts">
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';
  import { authStore } from '../../stores/authStore';
  import { apiClient } from '../../lib/api';

  let email = '';
  let password = '';
  let errorMessage = '';
  let isLoading = false;
  let isLoggedIn = false;
  let userRoles: string[] = [];

  onMount(() => {
    const unsubscribe = authStore.subscribe(auth => {
      isLoggedIn = !!auth.token;
      userRoles = auth.roles || [];

      // Redirect to appropriate dashboard if already logged in
      if (isLoggedIn) {
        if (userRoles.includes('Brand')) {
          goto('/brand/dashboard');
        } else if (userRoles.includes('Influencer')) {
          goto('/influencer/dashboard');
        } else if (userRoles.includes('Admin')) {
          goto('/admin/dashboard');
        }
      }
    });

    return unsubscribe;
  });

  async function handleSubmit() {
    if (!email || !password) {
      errorMessage = 'Please enter both email and password';
      return;
    }

    try {
      isLoading = true;
      errorMessage = '';

      const response = await apiClient.post('/api/auth/login', {
        email,
        password
      });

      if (response.data.success) {
        authStore.login(response.data);

        // Redirect based on user role
        if (response.data.roles.includes('Brand')) {
          goto('/brand/dashboard');
        } else if (response.data.roles.includes('Influencer')) {
          goto('/influencer/dashboard');
        } else if (response.data.roles.includes('Admin')) {
          goto('/admin/dashboard');
        }
      } else {
        errorMessage = response.data.message || 'Login failed';
      }
    } catch (error: any) {
      console.error('Login error:', error);
      errorMessage = error.response?.data?.message || 'An error occurred during login';
    } finally {
      isLoading = false;
    }
  }
</script>

<div class="flex min-h-full flex-col justify-center py-12 sm:px-6 lg:px-8">
  <div class="sm:mx-auto sm:w-full sm:max-w-md">
    <h2 class="mt-6 text-center text-3xl font-bold tracking-tight text-gray-900">Sign in to your account</h2>
    <p class="mt-2 text-center text-sm text-gray-600">
      Or
      <a href="/register" class="font-medium text-indigo-600 hover:text-indigo-500">create a new account</a>
    </p>
  </div>

  <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
    <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
      <form class="space-y-6" on:submit|preventDefault={handleSubmit}>
        {#if errorMessage}
          <div class="rounded-md bg-red-50 p-4">
            <div class="flex">
              <div class="ml-3">
                <h3 class="text-sm font-medium text-red-800">{errorMessage}</h3>
              </div>
            </div>
          </div>
        {/if}

        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email address</label>
          <div class="mt-1">
            <input id="email" name="email" type="email" autocomplete="email" required bind:value={email} class="block w-full appearance-none rounded-md border border-gray-300 px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm" />
          </div>
        </div>

        <div>
          <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
          <div class="mt-1">
            <input id="password" name="password" type="password" autocomplete="current-password" required bind:value={password} class="block w-full appearance-none rounded-md border border-gray-300 px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm" />
          </div>
        </div>

        <div class="flex items-center justify-between">
          <div class="flex items-center">
            <input id="remember-me" name="remember-me" type="checkbox" class="h-4 w-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-500" />
            <label for="remember-me" class="ml-2 block text-sm text-gray-900">Remember me</label>
          </div>

          <div class="text-sm">
            <a href="#" class="font-medium text-indigo-600 hover:text-indigo-500">Forgot your password?</a>
          </div>
        </div>

        <div>
          <button type="submit" disabled={isLoading} class="flex w-full justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed">
            {#if isLoading}
              <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Signing in...
            {:else}
              Sign in
            {/if}
          </button>
        </div>
      </form>
    </div>
  </div>
</div>