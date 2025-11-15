<script lang="ts">
  import { toastStore } from '../stores/toastStore';
  import { X, CheckCircle, XCircle, Info, AlertTriangle } from 'lucide-svelte';
  import type { Toast, ToastType } from '../stores/toastStore';

  let toasts: Toast[] = [];
  toastStore.subscribe((value) => (toasts = value));

  const icons = {
    success: CheckCircle,
    error: XCircle,
    info: Info,
    warning: AlertTriangle,
  };

  const colors = {
    success: 'bg-green-50 border-green-200 text-green-800',
    error: 'bg-red-50 border-red-200 text-red-800',
    info: 'bg-blue-50 border-blue-200 text-blue-800',
    warning: 'bg-yellow-50 border-yellow-200 text-yellow-800',
  };

  function handleRemove(id: string) {
    toastStore.remove(id);
  }
</script>

<div class="fixed top-4 right-4 z-50 flex flex-col gap-2 max-w-md w-full">
  {#each toasts as toast (toast.id)}
    <div
      class="flex items-start gap-3 p-4 rounded-lg shadow-lg border animate-slide-in {colors[toast.type]}"
      role="alert"
    >
      <svelte:component this={icons[toast.type]} class="w-5 h-5 mt-0.5 flex-shrink-0" />
      <div class="flex-1 min-w-0">
        <p class="text-sm font-medium">{toast.message}</p>
      </div>
      <button
        type="button"
        class="flex-shrink-0 text-current opacity-50 hover:opacity-100 transition-opacity"
        on:click={() => handleRemove(toast.id)}
        aria-label="Close notification"
      >
        <X class="w-4 h-4" />
      </button>
    </div>
  {/each}
</div>

<style>
  @keyframes slide-in {
    from {
      transform: translateX(100%);
      opacity: 0;
    }
    to {
      transform: translateX(0);
      opacity: 1;
    }
  }

  .animate-slide-in {
    animation: slide-in 0.3s ease-out;
  }
</style>

