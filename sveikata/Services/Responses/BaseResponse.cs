using giveatry.DTOs;
using giveatry.DTOs.Errors;

namespace giveatry.Services.Responses
{
    public abstract class BaseResponse
    {
        public ErrorDTO Messages { get; set; }
        public bool Success { get; set; }
        public bool Autorise { get; set; }

        public BaseResponse(string message, bool success)
        {
            Messages = new ErrorDTO();
            Messages.Errors.Add(new Error() { Message = message });
            Success = success;
        }
        public BaseResponse(string message, bool success, bool autorise)
        {
            Messages = new ErrorDTO();
            Messages.Errors.Add(new Error() { Message = message });
            Success = success;
            Autorise = autorise;
        }
    }
}
