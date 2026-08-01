namespace API_ADO_EmpDept_CRUD.Models.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> Ok(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data,
                StatusCode = 200
            };
        }

        public static ServiceResult<T> Created(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data,
                StatusCode = 201
            };
        }

        public static ServiceResult<T> BadRequest(string errorMessage)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = 400
            };
        }

        public static ServiceResult<T> NotFound(string errorMessage)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = 404
            };
        }

        public static ServiceResult<T> Conflict(string errorMessage)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                StatusCode = 409
            };
        }
    }
}
