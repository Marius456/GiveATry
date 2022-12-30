using giveatry.DTOs;
using giveatry.Models;

namespace giveatry.Mappers
{
    public class ReviewMapper
    {
        public static ReviewDTO Map(Review item)
        {
            return new ReviewDTO()
            {
                Id = item.Id,
                UserId = item.UserId,
                PlanId = item.PlanId,
                Rating = item.Rating,
                Text = item.Text
            };
        }
        public static Review Map(ReviewDTO itemDTO)
        {
            return new Review()
            {
                Id = itemDTO.Id,
                UserId = itemDTO.UserId,
                PlanId = itemDTO.PlanId,
                Rating = itemDTO.Rating,
                Text = itemDTO.Text
            };
        }
    }
}
