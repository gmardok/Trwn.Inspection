namespace Trwn.Inspection.WebClient.Models
{
    /// <summary>
    /// Generic API response wrapper
    /// </summary>
    /// <typeparam name="T">The type of data returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates if the request was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The response data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Error message if the request failed
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Creates a successful response
        /// </summary>
        /// <param name="data">The response data</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <returns>Successful API response</returns>
        public static ApiResponse<T> Success(T data, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates a failed response
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <returns>Failed API response</returns>
        public static ApiResponse<T> Failure(string errorMessage, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}