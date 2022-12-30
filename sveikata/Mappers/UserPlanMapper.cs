using giveatry.DTOs;
using giveatry.Models;

namespace giveatry.Mappers
{
    public class UserPlanMapper
    {
        public static BookmarkDTO Map(Bookmark item)
        {
            return new BookmarkDTO()
            {
                PlanId = item.PlanId,
                UserId = item.UserId
            };
        }
        public static Bookmark Map(BookmarkDTO itemDTO)
        {
            return new Bookmark()
            {
                PlanId = itemDTO.PlanId,
                UserId = itemDTO.UserId
            };
        }
    }
}
