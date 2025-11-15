using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Models;
using Microsoft.Extensions.Configuration;

namespace InfluencerMarketplace.Infrastructure.Data
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(IConfiguration configuration)
            : base(configuration, "Wallets")
        {
        }

        public async Task<Wallet> GetWalletByUserIdAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Wallets WHERE UserId = @UserId";
            return await connection.QueryFirstOrDefaultAsync<Wallet>(sql, new { UserId = userId });
        }

        public async Task<bool> UpdateBalanceAsync(Guid walletId, decimal newBalance)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Wallets 
                SET Balance = @Balance, 
                    UpdatedAt = NOW()
                WHERE Id = @WalletId";
            
            var affectedRows = await connection.ExecuteAsync(sql, new 
            { 
                WalletId = walletId, 
                Balance = newBalance
            });
            
            return affectedRows > 0;
        }

        public async Task<Guid> CreateTransactionAsync(WalletTransaction transaction)
        {
            using var connection = CreateConnection();
            
            if (transaction.Id == Guid.Empty)
                transaction.Id = Guid.NewGuid();
            
            transaction.CreatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO WalletTransactions (
                    Id, WalletId, PaymentId, Amount, BalanceAfter, 
                    Type, Description, Reference, CreatedAt
                )
                VALUES (
                    @Id, @WalletId, @PaymentId, @Amount, @BalanceAfter,
                    @Type, @Description, @Reference, @CreatedAt
                )
                RETURNING Id";
            
            var parameters = new
            {
                transaction.Id,
                transaction.WalletId,
                transaction.PaymentId,
                transaction.Amount,
                transaction.BalanceAfter,
                Type = transaction.Type.ToString(),
                transaction.Description,
                transaction.Reference,
                transaction.CreatedAt
            };
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
            return id;
        }

        public async Task<IEnumerable<WalletTransaction>> GetTransactionsAsync(Guid walletId)
        {
            using var connection = CreateConnection();
            var sql = @"
                SELECT * FROM WalletTransactions 
                WHERE WalletId = @WalletId 
                ORDER BY CreatedAt DESC";
            
            var transactions = await connection.QueryAsync<WalletTransaction>(sql, new { WalletId = walletId });
            
            foreach (var transaction in transactions)
            {
                DeserializeTransactionType(transaction);
            }
            
            return transactions;
        }

        public override async Task<Guid> CreateAsync(Wallet entity)
        {
            using var connection = CreateConnection();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO Wallets (
                    Id, UserId, Balance, Currency, CreatedAt, UpdatedAt
                )
                VALUES (
                    @Id, @UserId, @Balance, @Currency, @CreatedAt, @UpdatedAt
                )
                RETURNING Id";
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, entity);
            return id;
        }

        public override async Task<bool> UpdateAsync(Wallet entity)
        {
            using var connection = CreateConnection();
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE Wallets 
                SET UserId = @UserId,
                    Balance = @Balance,
                    Currency = @Currency,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, entity);
            return affectedRows > 0;
        }

        private void DeserializeTransactionType(WalletTransaction transaction)
        {
            // Dapper automatically maps VARCHAR to enum when the property type is enum
            // Since WalletTransaction model has TransactionType enum property, Dapper should handle it
        }
    }
}

