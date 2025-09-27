<script lang="ts">
  import { createEventDispatcher } from 'svelte';
  
  export let isOpen = false;
  export let title = '';
  export let size: 'sm' | 'md' | 'lg' | 'xl' = 'md';
  
  const dispatch = createEventDispatcher();
  
  const sizeClasses = {
    sm: 'max-w-md',
    md: 'max-w-lg',
    lg: 'max-w-2xl',
    xl: 'max-w-4xl'
  };
  
  function closeModal() {
    dispatch('close');
  }
  
  function handleKeydown(event: KeyboardEvent) {
    if (event.key === 'Escape') {
      closeModal();
    }
  }
</script>

{#if isOpen}
  <div class="fixed inset-0 z-50 overflow-y-auto" on:keydown={handleKeydown}>
    <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
      <!-- Background overlay -->
      <div class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" on:click={closeModal}></div>
      
      <!-- Modal panel -->
      <div class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle {sizeClasses[size]} sm:w-full">
        {#if title}
          <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
            <div class="flex items-center justify-between">
              <h3 class="text-lg leading-6 font-medium text-gray-900">{title}</h3>
              <button
                type="button"
                class="bg-white rounded-md text-gray-400 hover:text-gray-600 focus:outline-none focus:ring-2 focus:ring-indigo-500"
                on:click={closeModal}
              >
                <span class="sr-only">Close</span>
                <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          </div>
        {/if}
        
        <div class="bg-white px-4 pt-5 pb-4 sm:p-6">
          <slot />
        </div>
        
        {#if $$slots.footer}
          <div class="bg-gray-50 px-4 py-3 sm:px-6 sm:flex sm:flex-row-reverse">
            <slot name="footer" />
          </div>
        {/if}
      </div>
    </div>
  </div>
{/if}