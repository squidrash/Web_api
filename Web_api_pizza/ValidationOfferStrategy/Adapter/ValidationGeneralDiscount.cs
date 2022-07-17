using System;
using Web_api_pizza.Storage;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.ValidationOfferStrategy.Adapter
{
    public class ValidationGeneralDiscount
    {
        public OperationResult ValidateGeneralDiscount(SpecialOfferDTO specialOffer)
        {
            Console.WriteLine("скидка");
            var result = new OperationResult(false);
            if (specialOffer.Discount < 0 || specialOffer.Discount > 20)
            {
                result.Message = $"Недопустимый размер скидки — \"{specialOffer.Discount}%\"";
                return result;
            }

            Console.WriteLine("минимальная сумма");
            if (specialOffer.MinOrderAmount <= 0)
            {
                result.Message = $"Не указана минимальная сумма заказа";
                return result;
            }

            Console.WriteLine("основное блюдо");

            if (specialOffer.MainDish != null)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" недолжно быть основных блюд";
                return result;
            }


            Console.WriteLine("доп блюда");

            if (specialOffer.ExtraDish != null)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" недолжно быть доп блюд";
                return result;
            }


            Console.WriteLine("количество основных");

            if (specialOffer.RequiredNumberOfDish != 0)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" необходимое число блюд должно равняться 0";
                return result;
            }

            Console.WriteLine("количество доп");

            if (specialOffer.NumberOfExtraDish != 0)
            {
                result.Message = "В Акции типа \"GeneralDiscount\" число доп блюд должно равняться 0";
                return result;
            }


            result.IsSuccess = true;
            result.Message = "Успешно";

            return result;
        }

    }
}
