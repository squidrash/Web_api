using System;
using CreateDb.Storage.Models;

namespace CreateDb.Storage.DTO
{
    public class DishDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
