using System;
namespace Web_api_pizza.Storage
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        
        public ValidationResult(bool isValid, string message = "Новый экземпляр")
        {
            IsValid = isValid;
            Message = message;
        }
    }
}
