using giveatry.DTOs;

namespace giveatry.Services.Responses
{
    public class UserResponse : BaseResponse
    {
        public UserDTO User { get; set; }
        public PlanDTO Plan { get; set; }

        public UserResponse(UserDTO item) : base(string.Empty, true)
        {
            this.User = item;
        }
        public UserResponse(PlanDTO item) : base(string.Empty, true)
        {
            this.Plan = item;
        }

        public UserResponse() : base(string.Empty, true)
        {
        }

        public UserResponse(string message) : base(message, false)
        {
        }
    }
}
