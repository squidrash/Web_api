using System;
namespace Web_api_pizza.Storage
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
