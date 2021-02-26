﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CreateDb.Storage.Models
{
    public class DishEntity
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public int Price { get; set; }

        public List<OrderDishEntity> Orders { get; set; }

    }
}
