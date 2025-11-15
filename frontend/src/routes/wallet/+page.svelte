<script lang="ts">
  import { onMount } from 'svelte';
  import { walletApi } from '../../lib/api';
  import { toastStore } from '../../lib/stores/toastStore';
  import { formatCurrency, formatDateTime } from '../../lib/utils/format';
  import type { WalletDto, WalletTransactionDto } from '../../lib/api/types';
  import LoadingSpinner from '../../lib/components/LoadingSpinner.svelte';
  import Modal from '../../lib/components/Modal.svelte';
  
  let isLoading = true;
  let wallet: WalletDto | null = null;
  let transactions: WalletTransactionDto[] = [];
  let error = '';
  
  // Modals
  let showDepositModal = false;
  let showWithdrawModal = false;
  let showTransferModal = false;
  
  // Form data
  let depositAmount = 0;
  let withdrawAmount = 0;
  let transferAmount = 0;
  let transferRecipientId = '';
  let paymentMethod = 'eSewa';
  let isProcessing = false;
  
  const paymentMethods = [
    { value: 'eSewa', label: 'eSewa' },
    { value: 'Khalti', label: 'Khalti' },
    { value: 'Bank Transfer', label: 'Bank Transfer' }
  ];
  
  onMount(async () => {
    await loadWalletData();
  });
  
  async function loadWalletData() {
    try {
      isLoading = true;
      error = '';
      [wallet, transactions] = await Promise.all([
        walletApi.getWallet(),
        walletApi.getTransactions()
      ]);
    } catch (err: any) {
      console.error('Error loading wallet data:', err);
      error = err.message || 'Failed to load wallet data';
      toastStore.error(error);
    } finally {
      isLoading = false;
    }
  }
  
  async function handleDeposit() {
    if (!depositAmount || depositAmount <= 0) {
      toastStore.error('Please enter a valid deposit amount');
      return;
    }
    
    try {
      isProcessing = true;
      error = '';
      wallet = await walletApi.deposit(depositAmount, paymentMethod);
      toastStore.success('Deposit successful!');
      showDepositModal = false;
      depositAmount = 0;
      await loadWalletData();
    } catch (err: any) {
      console.error('Error processing deposit:', err);
      toastStore.error(err.message || 'Failed to process deposit');
    } finally {
      isProcessing = false;
    }
  }
  
  async function handleWithdraw() {
    if (!withdrawAmount || withdrawAmount <= 0) {
      toastStore.error('Please enter a valid withdrawal amount');
      return;
    }
    
    if (!wallet || withdrawAmount > wallet.balance) {
      toastStore.error('Insufficient balance');
      return;
    }
    
    try {
      isProcessing = true;
      error = '';
      wallet = await walletApi.withdraw(withdrawAmount, paymentMethod);
      toastStore.success('Withdrawal request submitted!');
      showWithdrawModal = false;
      withdrawAmount = 0;
      await loadWalletData();
    } catch (err: any) {
      console.error('Error processing withdrawal:', err);
      toastStore.error(err.message || 'Failed to process withdrawal');
    } finally {
      isProcessing = false;
    }
  }

  async function handleTransfer() {
    if (!transferAmount || transferAmount <= 0) {
      toastStore.error('Please enter a valid transfer amount');
      return;
    }

    if (!transferRecipientId) {
      toastStore.error('Please enter recipient ID');
      return;
    }

    if (!wallet || transferAmount > wallet.balance) {
      toastStore.error('Insufficient balance');
      return;
    }

    try {
      isProcessing = true;
      wallet = await walletApi.transfer(transferRecipientId, transferAmount);
      toastStore.success('Transfer successful!');
      showTransferModal = false;
      transferAmount = 0;
      transferRecipientId = '';
      await loadWalletData();
    } catch (err: any) {
      console.error('Error processing transfer:', err);
      toastStore.error(err.message || 'Failed to process transfer');
    } finally {
      isProcessing = false;
    }
  }
  
  function getTransactionIcon(type: string) {
    switch (type) {
      case 'Deposit':
        return 'M12 6v6m0 0v6m0-6h6m-6 0H6';
      case 'Withdrawal':
        return 'M18 12H6';
      case 'CampaignEarning':
        return 'M13 7h8m0 0v8m0-8l-8 8-4-4-6 6';
      case 'CampaignPayment':
        return 'M17 9V7a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2m2 4h10a2 2 0 002-2v-6a2 2 0 00-2-2H9a2 2 0 00-2 2v6a2 2 0 002 2zm7-5a2 2 0 11-4 0 2 2 0 014 0z';
      case 'CommissionFee':
        return 'M9 7h6m0 10v-3m-3 3h.01M9 17h.01M9 14h.01M12 14h.01M15 11h.01M12 11h.01M9 11h.01M7 21h10a2 2 0 002-2V5a2 2 0 00-2-2H7a2 2 0 00-2 2v14a2 2 0 002 2z';
      default:
        return 'M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1';
    }
  }
  
  function getTransactionColor(type: string) {
    switch (type) {
      case 'Deposit':
      case 'CampaignEarning':
        return 'text-green-600';
      case 'Withdrawal':
      case 'CampaignPayment':
      case 'CommissionFee':
        return 'text-red-600';
      default:
        return 'text-gray-600';
    }
  }
</script>

<svelte:head>
  <title>Wallet - Influencer Marketplace</title>
</svelte:head>

<div class="space-y-6">
  <div>
    <h1 class="text-2xl font-bold text-gray-900">Wallet</h1>
    <p class="mt-1 text-sm text-gray-500">
      Manage your funds and view transaction history
    </p>
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
    <!-- Wallet Balance -->
    <div class="bg-gradient-to-r from-indigo-500 to-purple-600 rounded-lg shadow">
      <div class="px-4 py-5 sm:p-6">
        <div class="flex items-center">
          <div class="flex-shrink-0">
            <svg class="h-8 w-8 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" />
            </svg>
          </div>
          <div class="ml-5 w-0 flex-1">
            <dl>
              <dt class="text-sm font-medium text-indigo-100 truncate">Current Balance</dt>
              <dd class="text-3xl font-semibold text-white">{formatCurrency(wallet?.balance || 0)}</dd>
            </dl>
          </div>
        </div>
        <div class="mt-5 flex space-x-3">
          <button
            on:click={() => showDepositModal = true}
            class="bg-white bg-opacity-20 backdrop-blur-sm text-white px-4 py-2 rounded-md text-sm font-medium hover:bg-opacity-30 transition-colors"
          >
            Add Funds
          </button>
          <button
            on:click={() => showWithdrawModal = true}
            disabled={!wallet || wallet.balance <= 0}
            class="bg-white bg-opacity-20 backdrop-blur-sm text-white px-4 py-2 rounded-md text-sm font-medium hover:bg-opacity-30 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Withdraw
          </button>
          <button
            on:click={() => showTransferModal = true}
            disabled={!wallet || wallet.balance <= 0}
            class="bg-white bg-opacity-20 backdrop-blur-sm text-white px-4 py-2 rounded-md text-sm font-medium hover:bg-opacity-30 transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Transfer
          </button>
        </div>
      </div>
    </div>

    <!-- Transaction History -->
    <div class="bg-white shadow rounded-lg">
      <div class="px-4 py-5 sm:px-6">
        <h3 class="text-lg leading-6 font-medium text-gray-900">Transaction History</h3>
      </div>
      <div class="border-t border-gray-200">
        {#if transactions.length === 0}
          <div class="px-4 py-5 sm:p-6 text-center">
            <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg>
            <h3 class="mt-2 text-sm font-medium text-gray-900">No transactions</h3>
            <p class="mt-1 text-sm text-gray-500">Your transaction history will appear here.</p>
          </div>
        {:else}
          <ul class="divide-y divide-gray-200">
            {#each transactions as transaction}
              <li class="px-4 py-4 sm:px-6">
                <div class="flex items-center justify-between">
                  <div class="flex items-center">
                    <div class="flex-shrink-0">
                      <svg class="h-6 w-6 {getTransactionColor(transaction.type)}" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d={getTransactionIcon(transaction.type)} />
                      </svg>
                    </div>
                    <div class="ml-4">
                      <p class="text-sm font-medium text-gray-900">{transaction.description}</p>
                      <p class="text-sm text-gray-500">{formatDateTime(transaction.createdAt)}</p>
                    </div>
                  </div>
                  <div class="flex items-center">
                    <span class="text-sm font-medium {getTransactionColor(transaction.type)}">
                      {transaction.amount > 0 ? '+' : ''}{formatCurrency(transaction.amount)}
                    </span>
                  </div>
                </div>
              </li>
            {/each}
          </ul>
        {/if}
      </div>
    </div>
  {/if}
</div>

<!-- Deposit Modal -->
<Modal bind:isOpen={showDepositModal} title="Add Funds" on:close={() => showDepositModal = false}>
  <form on:submit|preventDefault={handleDeposit} class="space-y-4">
    <div>
      <label for="deposit-amount" class="form-label">Amount (NPR)</label>
      <input
        type="number"
        id="deposit-amount"
        bind:value={depositAmount}
        required
        min="100"
        step="100"
        class="form-input"
        placeholder="Enter amount"
      />
    </div>
    
    <div>
      <label for="deposit-method" class="form-label">Payment Method</label>
      <select id="deposit-method" bind:value={paymentMethod} class="form-input">
        {#each paymentMethods as method}
          <option value={method.value}>{method.label}</option>
        {/each}
      </select>
    </div>
    
    <div class="bg-blue-50 p-4 rounded-md">
      <div class="flex">
        <div class="flex-shrink-0">
          <svg class="h-5 w-5 text-blue-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
          </svg>
        </div>
        <div class="ml-3">
          <p class="text-sm text-blue-700">
            You will be redirected to {paymentMethod} to complete the payment.
          </p>
        </div>
      </div>
    </div>
  </form>
  
  <div slot="footer" class="space-x-3">
    <button
      type="button"
      on:click={() => showDepositModal = false}
      class="btn btn-secondary"
    >
      Cancel
    </button>
    <button
      type="submit"
      on:click={handleDeposit}
      disabled={isProcessing}
      class="btn btn-primary"
    >
      {#if isProcessing}
        <LoadingSpinner size="sm" color="text-white" />
        Processing...
      {:else}
        Add Funds
      {/if}
    </button>
  </div>
</Modal>

<!-- Withdraw Modal -->
<Modal bind:isOpen={showWithdrawModal} title="Withdraw Funds" on:close={() => showWithdrawModal = false}>
  <form on:submit|preventDefault={handleWithdraw} class="space-y-4">
    <div>
      <label for="withdraw-amount" class="form-label">Amount (NPR)</label>
      <input
        type="number"
        id="withdraw-amount"
        bind:value={withdrawAmount}
        required
        min="100"
        max={wallet?.balance || 0}
        step="100"
        class="form-input"
        placeholder="Enter amount"
      />
      <p class="mt-1 text-sm text-gray-500">
        Available balance: {formatCurrency(wallet?.balance || 0)}
      </p>
    </div>
    
    <div>
      <label for="withdraw-method" class="form-label">Withdrawal Method</label>
      <select id="withdraw-method" bind:value={paymentMethod} class="form-input">
        {#each paymentMethods as method}
          <option value={method.value}>{method.label}</option>
        {/each}
      </select>
    </div>
    
    <div class="bg-yellow-50 p-4 rounded-md">
      <div class="flex">
        <div class="flex-shrink-0">
          <svg class="h-5 w-5 text-yellow-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z" />
          </svg>
        </div>
        <div class="ml-3">
          <p class="text-sm text-yellow-700">
            Withdrawals may take 1-3 business days to process.
          </p>
        </div>
      </div>
    </div>
  </form>
  
  <div slot="footer" class="space-x-3">
    <button
      type="button"
      on:click={() => showWithdrawModal = false}
      class="btn btn-secondary"
    >
      Cancel
    </button>
    <button
      type="submit"
      on:click={handleWithdraw}
      disabled={isProcessing}
      class="btn btn-primary"
    >
      {#if isProcessing}
        <LoadingSpinner size="sm" color="text-white" />
        Processing...
      {:else}
        Withdraw
      {/if}
    </button>
  </div>
</Modal>