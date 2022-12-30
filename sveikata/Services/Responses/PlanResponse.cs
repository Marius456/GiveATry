using giveatry.DTOs;

namespace giveatry.Services.Responses
{
    public class PlanResponse : BaseResponse
    {
        public PlanDTO Plan { get; set; }

        public PlanResponse(PlanDTO item) : base(string.Empty, true)
        {
            this.Plan = item;
        }

        public PlanResponse() : base(string.Empty, true)
        {
        }

        public PlanResponse(string message) : base(message, false)
        {
        }
    }
}
