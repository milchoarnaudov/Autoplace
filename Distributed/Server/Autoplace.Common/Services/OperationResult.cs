namespace Autoplace.Common.Services
{
    public class OperationResult
    {
        public OperationResult(bool isSuccessful, IEnumerable<string> errorMessages)
        {
            IsSuccessful = isSuccessful;
            ErrorMessages = errorMessages;
        }

        public bool IsSuccessful { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }

        public static OperationResult Success()
            => new OperationResult(true, new List<string>());

        public static OperationResult Failure(string errorMessage)
           => new OperationResult(false, new List<string> { errorMessage });

        public static OperationResult Failure(IEnumerable<string> errorMessages)
            => new OperationResult(false, errorMessages);
    }

    public class OperationResult<TModel> : OperationResult
    {
        public OperationResult(bool isSuccessful, TModel model, IEnumerable<string> errors)
            : base(isSuccessful, errors)
        {
            Model = model;
        }

        public TModel Model { get; }

        public static OperationResult<TModel> Success(TModel data)
           => new(true, data, new List<string>());

        public static OperationResult<TModel> Failure(IEnumerable<string> errors)
          => new(false, default, errors.ToList());

        public static OperationResult<TModel> Failure(string error)
          => new(false, default, new List<string> { error });
    }
}
