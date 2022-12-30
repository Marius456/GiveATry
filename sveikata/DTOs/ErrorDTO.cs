using giveatry.DTOs.Errors;
using System.Collections.Generic;

namespace giveatry.DTOs
{
    public class ErrorDTO
    {
        public List<Error> Errors { get; set; }

        public ErrorDTO()
        {
            Errors = new List<Error>();
        }
    }
}
