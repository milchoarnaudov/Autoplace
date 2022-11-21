namespace Autoplace.Common.Models
{
    public class ApiResponse
    {
        private ApiResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; }

        public static ApiResponse Failure(IEnumerable<string> errorMessages) => new ApiResponse(errorMessages);

        public static ApiResponse Failure(params string[] errorMessages) => new ApiResponse(errorMessages);
    }
}
