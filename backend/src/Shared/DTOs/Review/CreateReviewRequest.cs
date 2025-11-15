using System.ComponentModel.DataAnnotations;

namespace InfluencerMarketplace.Shared.DTOs.Review
{
    /// <summary>
    /// Request model for creating a review
    /// </summary>
    public class CreateReviewRequest
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public bool IsPublic { get; set; } = true;
    }
}

