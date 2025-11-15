<script lang="ts">
  import { onMount } from 'svelte';
  import { paymentsApi } from '../../lib/api';
  import { toastStore } from '../../lib/stores/toastStore';
  import { formatCurrency, formatDateTime } from '../../lib/utils/format';
  import type { PaymentDto } from '../../lib/api/types';
  import LoadingSpinner from '../../lib/components/LoadingSpinner.svelte';
  import StatusBadge from '../../lib/components/StatusBadge.svelte';

  let isLoading = true;
  let sentPayments: PaymentDto[] = [];
  let receivedPayments: PaymentDto[] = [];
  let error = '';
  let activeTab: 'sent' | 'received' = 'sent';
  let statusFilter = 'all';
  let typeFilter = 'all';

  const statusOptions = [
    { value: 'all', label: 'All Statuses' },
    { value: 'Pending', label: 'Pending' },
    { value: 'InEscrow', label: 'In Escrow' },
    { value: 'Released', label: 'Released' },
    { value: 'Refunded', label: 'Refunded' },
    { value: 'Failed', label: 'Failed' },
  ];

  const typeOptions = [
    { value: 'all', label: 'All Types' },
    { value: 'CampaignDeposit', label: 'Campaign Deposit' },
    { value: 'CampaignPayout', label: 'Campaign Payout' },
    { value: 'WalletDeposit', label: 'Wallet Deposit' },
    { value: 'WalletWithdrawal', label: 'Wallet Withdrawal' },
    { value: 'CommissionFee', label: 'Commission Fee' },
  ];

  onMount(async () => {
    await loadPayments();
  });

  async function loadPayments() {
    try {
      isLoading = true;
      error = '';
      [sentPayments, receivedPayments] = await Promise.all([
        paymentsApi.getSentPayments(),
        paymentsApi.getReceivedPayments(),
      ]);
    } catch (err: any) {
      console.error('Error loading payments:', err);
      error = err.message || 'Failed to load payments';
      toastStore.error(error);
    } finally {
      isLoading = false;
    }
  }

  function getFilteredPayments() {
    const payments = activeTab === 'sent' ? sentPayments : receivedPayments;
    return payments.filter((payment) => {
      const matchesStatus = statusFilter === 'all' || payment.status === statusFilter;
      const matchesType = typeFilter === 'all' || payment.type === typeFilter;
      return matchesStatus && matchesType;
    });
  }

  async function handleReleasePayment(id: string) {
    if (!confirm('Are you sure you want to release this payment from escrow?')) {
      return;
    }

    try {
      await paymentsApi.releasePayment(id);
      toastStore.success('Payment released successfully');
      await loadPayments();
    } catch (err: any) {
      toastStore.error(err.message || 'Failed to release payment');
    }
  }

  $: filteredPayments = getFilteredPayments();
</script>

<svelte:head>
  <title>Payment History - Influencer Marketplace</title>
</svelte:head>

<div class="space-y-6">
  <div>
    <h1 class="text-2xl font-bold text-gray-900">Payment History</h1>
    <p class="mt-1 text-sm text-gray-500">View all your sent and received payments</p>
  </div>

  {#if isLoading}
    <div class="flex justify-center py-12">
      <LoadingSpinner size="lg" />
    </div>
  {:else if error}
    <div class="rounded-md bg-red-50 p-4">
      <div class="text-sm text-red-700">{error}</div>
    </div>
  {:else}
    <!-- Tabs -->
    <div class="bg-white shadow rounded-lg">
      <div class="border-b border-gray-200">
        <nav class="-mb-px flex" aria-label="Tabs">
          <button
            on:click={() => activeTab = 'sent'}
            class="flex-1 py-4 px-1 text-center border-b-2 font-medium text-sm {activeTab === 'sent'
              ? 'border-indigo-500 text-indigo-600'
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}"
          >
            Sent Payments ({sentPayments.length})
          </button>
          <button
            on:click={() => activeTab = 'received'}
            class="flex-1 py-4 px-1 text-center border-b-2 font-medium text-sm {activeTab === 'received'
              ? 'border-indigo-500 text-indigo-600'
              : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'}"
          >
            Received Payments ({receivedPayments.length})
          </button>
        </nav>
      </div>

      <!-- Filters -->
      <div class="px-4 py-5 sm:p-6 border-b border-gray-200">
        <div class="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <div>
            <label for="status-filter" class="form-label">Filter by Status</label>
            <select id="status-filter" bind:value={statusFilter} class="form-input">
              {#each statusOptions as option}
                <option value={option.value}>{option.label}</option>
              {/each}
            </select>
          </div>
          <div>
            <label for="type-filter" class="form-label">Filter by Type</label>
            <select id="type-filter" bind:value={typeFilter} class="form-input">
              {#each typeOptions as option}
                <option value={option.value}>{option.label}</option>
              {/each}
            </select>
          </div>
        </div>
      </div>

      <!-- Payments List -->
      <div class="divide-y divide-gray-200">
        {#if filteredPayments.length === 0}
          <div class="px-4 py-12 text-center">
            <svg
              class="mx-auto h-12 w-12 text-gray-400"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"
              />
            </svg>
            <h3 class="mt-2 text-sm font-medium text-gray-900">No payments found</h3>
            <p class="mt-1 text-sm text-gray-500">
              {activeTab === 'sent'
                ? 'You haven\'t sent any payments yet.'
                : 'You haven\'t received any payments yet.'}
            </p>
          </div>
        {:else}
          {#each filteredPayments as payment}
            <div class="px-4 py-4 sm:px-6 hover:bg-gray-50">
              <div class="flex items-center justify-between">
                <div class="flex-1">
                  <div class="flex items-center gap-3">
                    <StatusBadge status={payment.status} />
                    <span class="text-sm font-medium text-gray-900">{payment.type}</span>
                  </div>
                  <div class="mt-2 text-sm text-gray-500">
                    {#if payment.campaignId}
                      <a
                        href="/brand/campaigns/{payment.campaignId}"
                        class="text-indigo-600 hover:text-indigo-800"
                      >
                        Campaign: {payment.campaignId.substring(0, 8)}...
                      </a>
                      <span class="mx-2">•</span>
                    {/if}
                    {formatDateTime(payment.createdAt)}
                    {#if payment.paymentMethod}
                      <span class="mx-2">•</span>
                      {payment.paymentMethod}
                    {/if}
                  </div>
                  {#if payment.notes}
                    <p class="mt-1 text-sm text-gray-600">{payment.notes}</p>
                  {/if}
                </div>
                <div class="ml-4 text-right">
                  <div
                    class="text-lg font-semibold {activeTab === 'sent'
                      ? 'text-red-600'
                      : 'text-green-600'}"
                  >
                    {activeTab === 'sent' ? '-' : '+'}
                    {formatCurrency(activeTab === 'sent' ? payment.amount : payment.netAmount)}
                  </div>
                  {#if payment.commissionAmount > 0}
                    <div class="text-sm text-gray-500">
                      Commission: {formatCurrency(payment.commissionAmount)}
                    </div>
                  {/if}
                  {#if payment.status === 'InEscrow' && activeTab === 'sent'}
                    <button
                      on:click={() => handleReleasePayment(payment.id)}
                      class="mt-2 btn btn-sm btn-primary"
                    >
                      Release
                    </button>
                  {/if}
                </div>
              </div>
            </div>
          {/each}
        {/if}
      </div>
    </div>
  {/if}
</div>

