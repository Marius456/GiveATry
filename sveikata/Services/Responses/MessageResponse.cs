using giveatry.DTOs;

namespace giveatry.Services.Responses
{
    public class MessageResponse : BaseResponse
    {
        public MessageDTO Message { get; set; }

        public MessageResponse(MessageDTO item) : base(string.Empty, true)
        {
            this.Message = item;
        }

        public MessageResponse() : base(string.Empty, true)
        {
        }

        public MessageResponse(string message) : base(message, false)
        {
        }
    }
}
