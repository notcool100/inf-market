using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;

        public WalletService(IWalletRepository walletRepository, IUserRepository userRepository)
        {
            _walletRepository = walletRepository;
            _userRepository = userRepository;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(Guid userId)
        {
            var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
            
            // If wallet doesn't exist, create one
            if (wallet == null)
            {
                // Verify user exists
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new ArgumentException("User not found");

                wallet = new Wallet
                {
                    UserId = userId,
                    Balance = 0,
                    Currency = "NPR"
                };

                var walletId = await _walletRepository.CreateAsync(wallet);
                wallet.Id = walletId;
            }

            return wallet;
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid walletId)
        {
            return await _walletRepository.GetTransactionsAsync(walletId);
        }

        public async Task<bool> DepositToWalletAsync(Guid userId, decimal amount, string description, string reference = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero");

            var wallet = await GetWalletByUserIdAsync(userId);

            // Update wallet balance
            wallet.Balance += amount;
            await _walletRepository.UpdateAsync(wallet);

            // Create transaction record
            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                BalanceAfter = wallet.Balance,
                Type = TransactionType.Deposit,
                Description = description,
                Reference = reference
            };

            await _walletRepository.CreateTransactionAsync(transaction);

            return true;
        }

        public async Task<bool> WithdrawFromWalletAsync(Guid userId, decimal amount, string description, string reference = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be greater than zero");

            var wallet = await GetWalletByUserIdAsync(userId);

            // Check if wallet has sufficient balance
            if (wallet.Balance < amount)
                throw new InvalidOperationException("Insufficient wallet balance");

            // Update wallet balance
            wallet.Balance -= amount;
            await _walletRepository.UpdateAsync(wallet);

            // Create transaction record
            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = -amount, // Negative amount for withdrawal
                BalanceAfter = wallet.Balance,
                Type = TransactionType.Withdrawal,
                Description = description,
                Reference = reference
            };

            await _walletRepository.CreateTransactionAsync(transaction);

            return true;
        }

        public async Task<bool> TransferBetweenWalletsAsync(Guid fromUserId, Guid toUserId, decimal amount, string description, string reference = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be greater than zero");

            if (fromUserId == toUserId)
                throw new ArgumentException("Cannot transfer to the same wallet");

            // Get source wallet
            var sourceWallet = await GetWalletByUserIdAsync(fromUserId);

            // Check if source wallet has sufficient balance
            if (sourceWallet.Balance < amount)
                throw new InvalidOperationException("Insufficient wallet balance");

            // Get destination wallet
            var destinationWallet = await GetWalletByUserIdAsync(toUserId);

            // Begin transaction (ideally this would be wrapped in a database transaction)
            
            // Update source wallet balance
            sourceWallet.Balance -= amount;
            await _walletRepository.UpdateAsync(sourceWallet);

            // Create source transaction record
            var sourceTransaction = new WalletTransaction
            {
                WalletId = sourceWallet.Id,
                Amount = -amount, // Negative amount for outgoing transfer
                BalanceAfter = sourceWallet.Balance,
                Type = TransactionType.CampaignPayment,
                Description = description,
                Reference = reference
            };

            await _walletRepository.CreateTransactionAsync(sourceTransaction);

            // Update destination wallet balance
            destinationWallet.Balance += amount;
            await _walletRepository.UpdateAsync(destinationWallet);

            // Create destination transaction record
            var destinationTransaction = new WalletTransaction
            {
                WalletId = destinationWallet.Id,
                Amount = amount,
                BalanceAfter = destinationWallet.Balance,
                Type = TransactionType.CampaignEarning,
                Description = description,
                Reference = reference
            };

            await _walletRepository.CreateTransactionAsync(destinationTransaction);

            return true;
        }
    }
}