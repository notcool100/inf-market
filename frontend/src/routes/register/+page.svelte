<script lang="ts">
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';
  import { authStore } from '../../stores/authStore';
  import { authApi } from '../../lib/api';
  import { toastStore } from '../../lib/stores/toastStore';
  import { validateRegister } from '../../lib/utils/validation';

  let email = '';
  let password = '';
  let confirmPassword = '';
  let firstName = '';
  let lastName = '';
  let phoneNumber = '';
  let userType: 'Brand' | 'Influencer' = 'Brand';
  let errors: Record<string, string> = {};
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
    errors = {};
    const validation = validateRegister({
      email,
      password,
      confirmPassword,
      firstName,
      lastName,
      phoneNumber,
    });

    if (!validation.isValid) {
      validation.errors.forEach((err) => {
        errors[err.field] = err.message;
      });
      return;
    }

    try {
      isLoading = true;
      const response = await authApi.register({
        email,
        password,
        confirmPassword,
        firstName,
        lastName,
        phoneNumber,
        userType,
      });

      authStore.login(response);
      toastStore.success('Registration successful!');

      // Redirect based on user type
      if (userType === 'Brand') {
        goto('/brand/dashboard');
      } else if (userType === 'Influencer') {
        goto('/influencer/profile/create');
      }
    } catch (error: any) {
      console.error('Registration error:', error);
      toastStore.error(error.message || 'An error occurred during registration');
    } finally {
      isLoading = false;
    }
  }
</script>

<div class="flex min-h-full flex-col justify-center py-12 sm:px-6 lg:px-8">
  <div class="sm:mx-auto sm:w-full sm:max-w-md">
    <h2 class="mt-6 text-center text-3xl font-bold tracking-tight text-gray-900">Create your account</h2>
    <p class="mt-2 text-center text-sm text-gray-600">
      Or
      <a href="/login" class="font-medium text-indigo-600 hover:text-indigo-500">sign in to your existing account</a>
    </p>
  </div>

  <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
    <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
      <form class="space-y-6" on:submit|preventDefault={handleSubmit}>
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email address</label>
          <div class="mt-1">
            <input
              id="email"
              name="email"
              type="email"
              autocomplete="email"
              required
              bind:value={email}
              class="block w-full appearance-none rounded-md border {errors.email ? 'border-red-500' : 'border-gray-300'} px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"
            />
            {#if errors.email}
              <p class="mt-1 text-sm text-red-600">{errors.email}</p>
            {/if}
          </div>
        </div>

        <div class="grid grid-cols-1 gap-y-6 gap-x-4 sm:grid-cols-2">
          <div>
            <label for="first-name" class="block text-sm font-medium text-gray-700">First name</label>
            <div class="mt-1">
              <input
                id="first-name"
                name="first-name"
                type="text"
                autocomplete="given-name"
                required
                bind:value={firstName}
                class="block w-full appearance-none rounded-md border {errors.firstName ? 'border-red-500' : 'border-gray-300'} px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"
              />
              {#if errors.firstName}
                <p class="mt-1 text-sm text-red-600">{errors.firstName}</p>
              {/if}
            </div>
          </div>

          <div>
            <label for="last-name" class="block text-sm font-medium text-gray-700">Last name</label>
            <div class="mt-1">
              <input
                id="last-name"
                name="last-name"
                type="text"
                autocomplete="family-name"
                required
                bind:value={lastName}
                class="block w-full appearance-none rounded-md border {errors.lastName ? 'border-red-500' : 'border-gray-300'} px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"
              />
              {#if errors.lastName}
                <p class="mt-1 text-sm text-red-600">{errors.lastName}</p>
              {/if}
            </div>
          </div>
        </div>

        <div>
          <label for="phone-number" class="block text-sm font-medium text-gray-700">Phone number</label>
          <div class="mt-1">
            <input
              id="phone-number"
              name="phone-number"
              type="tel"
              autocomplete="tel"
              bind:value={phoneNumber}
              class="block w-full appearance-none rounded-md border {errors.phoneNumber ? 'border-red-500' : 'border-gray-300'} px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"
            />
            {#if errors.phoneNumber}
              <p class="mt-1 text-sm text-red-600">{errors.phoneNumber}</p>
            {/if}
          </div>
        </div>

        <div>
          <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
          <div class="mt-1">
            <input
              id="password"
              name="password"
              type="password"
              autocomplete="new-password"
              required
              bind:value={password}
              class="block w-full appearance-none rounded-md border {errors.password ? 'border-red-500' : 'border-gray-300'} px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"
            />
            {#if errors.password}
              <p class="mt-1 text-sm text-red-600">{errors.password}</p>
            {/if}
          </div>
        </div>

        <div>
          <label for="confirm-password" class="block text-sm font-medium text-gray-700">Confirm password</label>
          <div class="mt-1">
            <input
              id="confirm-password"
              name="confirm-password"
              type="password"
              autocomplete="new-password"
              required
              bind:value={confirmPassword}
              class="block w-full appearance-none rounded-md border {errors.confirmPassword ? 'border-red-500' : 'border-gray-300'} px-3 py-2 placeholder-gray-400 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm"
            />
            {#if errors.confirmPassword}
              <p class="mt-1 text-sm text-red-600">{errors.confirmPassword}</p>
            {/if}
          </div>
        </div>

        <div>
          <label for="user-type" class="block text-sm font-medium text-gray-700">I am a</label>
          <div class="mt-1">
            <select id="user-type" name="user-type" required bind:value={userType} class="block w-full appearance-none rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-indigo-500 sm:text-sm">
              <option value="Brand">Brand/Business</option>
              <option value="Influencer">Influencer/Creator</option>
            </select>
          </div>
        </div>

        <div>
          <button type="submit" disabled={isLoading} class="flex w-full justify-center rounded-md border border-transparent bg-indigo-600 py-2 px-4 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed">
            {#if isLoading}
              <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
              Creating account...
            {:else}
              Create account
            {/if}
          </button>
        </div>
      </form>
    </div>
  </div>
</div>