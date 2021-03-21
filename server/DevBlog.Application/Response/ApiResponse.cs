namespace DevBlog.Application.Response
{
	public class ApiResponse
	{
		public ApiResponseError Error { get; set; }
		public bool IsSuccess { get; set; }
		public bool IsBadRequest => !IsSuccess && Error.ErrorType == ApiResponseErrorType.BadRequest;
		public bool IsNotFound => !IsSuccess && Error.ErrorType == ApiResponseErrorType.NotFound;
		public bool IsConflict => !IsSuccess && Error.ErrorType == ApiResponseErrorType.Conflict;
		public bool IsUnauthorized => !IsSuccess && Error.ErrorType == ApiResponseErrorType.Unauthorized;

		public static ApiResponse Ok()
		{
			return new ApiResponse
			{
				IsSuccess = true
			};
		}

		public static ApiResponse ErrorFor(string errorMsg, ApiResponseErrorType errorType)
		{
			return new ApiResponse
			{
				IsSuccess = false,
				Error = new ApiResponseError(errorType, errorMsg)
			};
		}

		public static ApiResponse NotFound(string errorMsg = "")
		{
			return new ApiResponse
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.NotFound, errorMsg)
			};
		}

		public static ApiResponse BadRequest(string errorMsg)
		{
			return new ApiResponse
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.BadRequest, errorMsg)
			};
		}

		public static ApiResponse Conflict(string errorMsg)
		{
			return new ApiResponse
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.Conflict, errorMsg)
			};
		}

		public static ApiResponse Unauthorized(string errorMsg)
		{
			return new ApiResponse
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.Unauthorized, errorMsg)
			};
		}
	}

	public class ApiResponse<T> : ApiResponse
	{
		public T Value { get; set; }

		public new static ApiResponse<T> ErrorFor(string errorMsg, ApiResponseErrorType errorType)
		{
			return new ApiResponse<T>
			{
				IsSuccess = false,
				Error = new ApiResponseError(errorType, errorMsg)
			};
		}

		public static ApiResponse<T> Ok(T value)
		{
			return new ApiResponse<T>
			{
				IsSuccess = true,
				Value = value
			};
		}

		public new static ApiResponse<T> NotFound(string errorMsg = "")
		{
			return new ApiResponse<T>
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.NotFound, errorMsg)
			};
		}

		public new static ApiResponse<T> BadRequest(string errorMsg)
		{
			return new ApiResponse<T>
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.BadRequest, errorMsg)
			};
		}

		public new static ApiResponse<T> Conflict(string errorMsg)
		{
			return new ApiResponse<T>
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.Conflict, errorMsg)
			};
		}

		public new static ApiResponse<T> Unauthorized(string errorMsg)
		{
			return new ApiResponse<T>
			{
				IsSuccess = false,
				Error = new ApiResponseError(ApiResponseErrorType.Unauthorized, errorMsg)
			};
		}
	}
}