using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<Wallet> GetWalletByUserIdAsync(Guid userId);
        Task<bool> UpdateBalanceAsync(Guid walletId, decimal newBalance);
        Task<Guid> CreateTransactionAsync(WalletTransaction transaction);
        Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid walletId);
    }
}