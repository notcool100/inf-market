using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Wallet management endpoints for user wallets and transactions
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<ActionResult<Wallet>> GetWallet()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var wallet = await _walletService.GetWalletByUserIdAsync(userId);
                return Ok(wallet);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<WalletTransaction>>> GetTransactions()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var wallet = await _walletService.GetWalletByUserIdAsync(userId);
                var transactions = await _walletService.GetTransactionsAsync(wallet.Id);
                return Ok(transactions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deposit")]
        public async Task<ActionResult> DepositToWallet([FromBody] WalletTransactionRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var result = await _walletService.DepositToWalletAsync(
                    userId, 
                    request.Amount, 
                    request.Description, 
                    request.Reference);
                
                return Ok(new { message = "Deposit successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult> WithdrawFromWallet([FromBody] WalletTransactionRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var result = await _walletService.WithdrawFromWalletAsync(
                    userId, 
                    request.Amount, 
                    request.Description, 
                    request.Reference);
                
                return Ok(new { message = "Withdrawal successful" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult> TransferBetweenWallets([FromBody] WalletTransferRequest request)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var result = await _walletService.TransferBetweenWalletsAsync(
                    userId, 
                    request.RecipientUserId, 
                    request.Amount, 
                    request.Description, 
                    request.Reference);
                
                return Ok(new { message = "Transfer successful" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class WalletTransactionRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
    }

    public class WalletTransferRequest
    {
        public Guid RecipientUserId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
    }
}