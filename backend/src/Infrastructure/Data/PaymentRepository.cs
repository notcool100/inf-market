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
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration)
            : base(configuration, "Payments")
        {
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByCampaignIdAsync(Guid campaignId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Payments WHERE CampaignId = @CampaignId ORDER BY CreatedAt DESC";
            var payments = await connection.QueryAsync<Payment>(sql, new { CampaignId = campaignId });
            
            foreach (var payment in payments)
            {
                DeserializeEnums(payment);
            }
            
            return payments;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsBySenderIdAsync(Guid senderId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Payments WHERE SenderId = @SenderId ORDER BY CreatedAt DESC";
            var payments = await connection.QueryAsync<Payment>(sql, new { SenderId = senderId });
            
            foreach (var payment in payments)
            {
                DeserializeEnums(payment);
            }
            
            return payments;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByRecipientIdAsync(Guid recipientId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Payments WHERE RecipientId = @RecipientId ORDER BY CreatedAt DESC";
            var payments = await connection.QueryAsync<Payment>(sql, new { RecipientId = recipientId });
            
            foreach (var payment in payments)
            {
                DeserializeEnums(payment);
            }
            
            return payments;
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid paymentId, PaymentStatus status)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Payments 
                SET Status = @Status, 
                    UpdatedAt = NOW()
                WHERE Id = @PaymentId";
            
            var affectedRows = await connection.ExecuteAsync(sql, new 
            { 
                PaymentId = paymentId, 
                Status = status.ToString()
            });
            
            return affectedRows > 0;
        }

        public override async Task<Guid> CreateAsync(Payment entity)
        {
            using var connection = CreateConnection();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO Payments (
                    Id, CampaignId, SenderId, RecipientId, Amount, 
                    CommissionAmount, NetAmount, Currency, Status, Type,
                    TransactionReference, PaymentMethod, Notes, 
                    CreatedAt, CompletedAt, UpdatedAt
                )
                VALUES (
                    @Id, @CampaignId, @SenderId, @RecipientId, @Amount,
                    @CommissionAmount, @NetAmount, @Currency, @Status, @Type,
                    @TransactionReference, @PaymentMethod, @Notes,
                    @CreatedAt, @CompletedAt, @UpdatedAt
                )
                RETURNING Id";
            
            var parameters = new
            {
                entity.Id,
                entity.CampaignId,
                entity.SenderId,
                entity.RecipientId,
                entity.Amount,
                entity.CommissionAmount,
                entity.NetAmount,
                entity.Currency,
                Status = entity.Status.ToString(),
                Type = entity.Type.ToString(),
                entity.TransactionReference,
                entity.PaymentMethod,
                entity.Notes,
                entity.CreatedAt,
                entity.CompletedAt,
                entity.UpdatedAt
            };
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
            return id;
        }

        public override async Task<bool> UpdateAsync(Payment entity)
        {
            using var connection = CreateConnection();
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE Payments 
                SET CampaignId = @CampaignId,
                    SenderId = @SenderId,
                    RecipientId = @RecipientId,
                    Amount = @Amount,
                    CommissionAmount = @CommissionAmount,
                    NetAmount = @NetAmount,
                    Currency = @Currency,
                    Status = @Status,
                    Type = @Type,
                    TransactionReference = @TransactionReference,
                    PaymentMethod = @PaymentMethod,
                    Notes = @Notes,
                    CompletedAt = @CompletedAt,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var parameters = new
            {
                entity.Id,
                entity.CampaignId,
                entity.SenderId,
                entity.RecipientId,
                entity.Amount,
                entity.CommissionAmount,
                entity.NetAmount,
                entity.Currency,
                Status = entity.Status.ToString(),
                Type = entity.Type.ToString(),
                entity.TransactionReference,
                entity.PaymentMethod,
                entity.Notes,
                entity.CompletedAt,
                entity.UpdatedAt
            };
            
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public override async Task<Payment> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            var payment = await connection.QueryFirstOrDefaultAsync<Payment>(sql, new { Id = id });
            
            if (payment != null)
            {
                DeserializeEnums(payment);
            }
            
            return payment;
        }

        public override async Task<IEnumerable<Payment>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} ORDER BY CreatedAt DESC";
            var payments = await connection.QueryAsync<Payment>(sql);
            
            foreach (var payment in payments)
            {
                DeserializeEnums(payment);
            }
            
            return payments;
        }

        private void DeserializeEnums(Payment payment)
        {
            // Dapper automatically maps VARCHAR to enum when the property type is enum
            // But we need to handle it explicitly if the database returns string
            // Since Payment model has enum properties, Dapper should handle it automatically
        }
    }
}

