namespace DevBlog.Application.Response
{
	public class ApiResponseError
	{
		public ApiResponseError(ApiResponseErrorType errorType, string errorMessage)
		{
			ErrorType = errorType;
			ErrorMessage = errorMessage;
		}

		public ApiResponseError() { }

		public ApiResponseErrorType ErrorType { get; set; }
		public string ErrorMessage { get; set; }
	}
}