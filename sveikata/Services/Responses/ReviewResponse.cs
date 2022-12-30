using giveatry.DTOs;

namespace giveatry.Services.Responses
{
    public class ReviewResponse : BaseResponse
    {
        public ReviewDTO Review { get; set; }

        public ReviewResponse(ReviewDTO item) : base(string.Empty, true)
        {
            this.Review = item;
        }

        public ReviewResponse() : base(string.Empty, true, true)
        {
        }

        public ReviewResponse(string message) : base(message, false)
        {
        }
        public ReviewResponse(string message, bool authorise) : base(message, false, authorise)
        {
        }
    }
}
