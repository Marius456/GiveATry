using giveatry.DTOs;

namespace giveatry.Services.Responses
{
    public class ExerciseResponse : BaseResponse
    {
        public ExerciseDTO Exercise { get; set; }

        public ExerciseResponse(ExerciseDTO item) : base(string.Empty, true)
        {
            this.Exercise = item;
        }

        public ExerciseResponse() : base(string.Empty, true, true)
        {
        }

        public ExerciseResponse(string message) : base(message, false)
        {
        }
        public ExerciseResponse(string message, bool authorise) : base(message, false, authorise)
        {
        }
    }
}
