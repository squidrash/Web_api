using System;
using Microsoft.EntityFrameworkCore;
using Web_api_pizza.Storage.Models;

namespace Web_api_pizza.Storage.DTO
{
    public class PizzaDbContext : DbContext
    {
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<CustomerAddressEntity> CustomerAddressEntities { get; set; }
        public DbSet<CustomerOrderEntity> CustomerOrderEntities { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderDishEntity> OrderDishEntities { get; set; }

        public DbSet<DishEntity> Dishes { get; set; }

        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<AddressOrderEntity> AddressOrderEntities { get; set; }

        public DbSet<SpecialOfferEntity> Offers { get; set; }
        public DbSet<SpecialOfferOrderEntity> SpecialOfferOrderEntities { get; set; }


        public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
            : base (options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpecialOfferEntity>()
            .HasOne(so => so.ExtraDish)
            .WithMany(d => d.OfferExtraDish)
            .HasForeignKey(so => so.ExtraDishId);


            modelBuilder.Entity<SpecialOfferEntity>()
            .HasMany(so => so.MainDishes)
            .WithMany(d => d.OfferMainDishes)
            .UsingEntity(j => j.ToTable("OfferMainDishesEntity"));


        }
    }
}
