namespace Autoplace.Common.Models
{
    public class Result
    {
        public Result(bool isSuccessful, IEnumerable<string> errorMessages)
        {
            IsSuccessful = isSuccessful;
            ErrorMessages = errorMessages;
        }

        public bool IsSuccessful { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }

        public static Result Success()
            => new Result(true, new List<string>());

        public static Result Failure(string errorMessage)
           => new Result(false, new List<string> { errorMessage });

        public static Result Failure(IEnumerable<string> errorMessages)
            => new Result(false, errorMessages);
    }

    public class Result<TModel> : Result
    {
        public Result(bool isSuccessful, TModel model, IEnumerable<string> errors)
            : base(isSuccessful, errors)
        {
            Model = model;
        }

        public TModel Model { get; }

        public static Result<TModel> Success(TModel data)
           => new(true, data, new List<string>());

        public static Result<TModel> Failure(IEnumerable<string> errors)
          => new(false, default, errors.ToList());

        public static Result<TModel> Failure(string error)
          => new(false, default, new List<string> { error });
    }
}
