using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using InfluencerMarketplace.Shared.DTOs.CampaignDeliverable;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class CampaignDeliverableService : ICampaignDeliverableService
    {
        private readonly ICampaignDeliverableRepository _deliverableRepository;
        private readonly ICampaignRepository _campaignRepository;

        public CampaignDeliverableService(
            ICampaignDeliverableRepository deliverableRepository,
            ICampaignRepository campaignRepository)
        {
            _deliverableRepository = deliverableRepository;
            _campaignRepository = campaignRepository;
        }

        public async Task<CampaignDeliverableDto> GetDeliverableAsync(Guid id)
        {
            var deliverable = await _deliverableRepository.GetByIdAsync(id);
            if (deliverable == null)
                return null;

            return MapToDto(deliverable);
        }

        public async Task<IEnumerable<CampaignDeliverableDto>> GetDeliverablesByCampaignIdAsync(Guid campaignId)
        {
            var deliverables = await _deliverableRepository.GetDeliverablesByCampaignIdAsync(campaignId);
            return deliverables.Select(MapToDto);
        }

        public async Task<CampaignDeliverableDto> CreateDeliverableAsync(Guid campaignId, CreateDeliverableRequest request)
        {
            // Verify campaign exists
            var campaign = await _campaignRepository.GetByIdAsync(campaignId);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            var deliverable = new CampaignDeliverable
            {
                CampaignId = campaignId,
                Title = request.Title,
                Description = request.Description,
                DeliverableType = request.DeliverableType,
                DueDate = request.DueDate,
                Status = DeliverableStatus.Pending
            };

            var id = await _deliverableRepository.CreateAsync(deliverable);
            return await GetDeliverableAsync(id);
        }

        public async Task<CampaignDeliverableDto> UpdateDeliverableAsync(Guid id, CreateDeliverableRequest request)
        {
            var deliverable = await _deliverableRepository.GetByIdAsync(id);
            if (deliverable == null)
                throw new ArgumentException("Deliverable not found");

            deliverable.Title = request.Title;
            deliverable.Description = request.Description;
            deliverable.DeliverableType = request.DeliverableType;
            deliverable.DueDate = request.DueDate;

            await _deliverableRepository.UpdateAsync(deliverable);
            return await GetDeliverableAsync(id);
        }

        public async Task<bool> SubmitDeliverableAsync(Guid id, SubmitDeliverableRequest request)
        {
            var deliverable = await _deliverableRepository.GetByIdAsync(id);
            if (deliverable == null)
                throw new ArgumentException("Deliverable not found");

            if (deliverable.Status != DeliverableStatus.Pending)
                throw new InvalidOperationException("Only pending deliverables can be submitted");

            deliverable.ProofUrl = request.ProofUrl;
            deliverable.ScreenshotUrl = request.ScreenshotUrl;
            deliverable.Status = DeliverableStatus.Submitted;
            deliverable.SubmittedAt = DateTime.UtcNow;

            return await _deliverableRepository.UpdateAsync(deliverable);
        }

        public async Task<bool> ApproveDeliverableAsync(Guid id, ReviewDeliverableRequest request)
        {
            var deliverable = await _deliverableRepository.GetByIdAsync(id);
            if (deliverable == null)
                throw new ArgumentException("Deliverable not found");

            if (deliverable.Status != DeliverableStatus.Submitted)
                throw new InvalidOperationException("Only submitted deliverables can be approved");

            return await _deliverableRepository.UpdateStatusAsync(
                id, 
                DeliverableStatus.Approved, 
                request.FeedbackNotes);
        }

        public async Task<bool> RejectDeliverableAsync(Guid id, ReviewDeliverableRequest request)
        {
            var deliverable = await _deliverableRepository.GetByIdAsync(id);
            if (deliverable == null)
                throw new ArgumentException("Deliverable not found");

            if (deliverable.Status != DeliverableStatus.Submitted)
                throw new InvalidOperationException("Only submitted deliverables can be rejected");

            return await _deliverableRepository.UpdateStatusAsync(
                id, 
                DeliverableStatus.Rejected, 
                request.FeedbackNotes);
        }

        private CampaignDeliverableDto MapToDto(CampaignDeliverable deliverable)
        {
            return new CampaignDeliverableDto
            {
                Id = deliverable.Id,
                CampaignId = deliverable.CampaignId,
                Title = deliverable.Title,
                Description = deliverable.Description,
                DeliverableType = deliverable.DeliverableType,
                ProofUrl = deliverable.ProofUrl,
                ScreenshotUrl = deliverable.ScreenshotUrl,
                FeedbackNotes = deliverable.FeedbackNotes,
                Status = deliverable.Status.ToString(),
                DueDate = deliverable.DueDate,
                SubmittedAt = deliverable.SubmittedAt,
                ReviewedAt = deliverable.ReviewedAt,
                CreatedAt = deliverable.CreatedAt,
                UpdatedAt = deliverable.UpdatedAt
            };
        }
    }
}

