using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletByUserIdAsync(Guid userId);
        Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid walletId);
        Task<bool> DepositToWalletAsync(Guid userId, decimal amount, string description, string reference = null);
        Task<bool> WithdrawFromWalletAsync(Guid userId, decimal amount, string description, string reference = null);
        Task<bool> TransferBetweenWalletsAsync(Guid fromUserId, Guid toUserId, decimal amount, string description, string reference = null);
    }
}