using System;
namespace Web_api_pizza.Storage
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        
        public OperationResult(bool isSuccess, string message = "Новый, пустой экземпляр")
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
