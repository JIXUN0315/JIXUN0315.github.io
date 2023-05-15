namespace AdminManagement.Models.Dtos
{
    public class ReviewerDto
    {
        public int ReviewerId { get; set; }
    }
    public class RejectReviewDto
    {
        public int ReviewerId { get; set; }
        public string Reason { get; set; }
    }
}
