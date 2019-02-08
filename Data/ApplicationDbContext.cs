using System;
using System.Collections.Generic;
using System.Text;
using Clutchlit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Clutchlit.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<PassengerCar> PassengerCars { get; set; }
        public DbSet<PcKtype> PcKtypes { get; set; }
        public DbSet<PcCategory> PcCategories { get; set; }
        public DbSet<PdPrices> PdPrices { get; set; } 
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributorWarehouse> Warehouses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PcAttributes> PcAttributes { get; set; }
        public DbSet<AllegroAuction> AllegroAuction { get; set; }
        public DbSet<AllegroAuctionUsage> AllegroAuctionUsage { get; set; }
        public DbSet<AllegroAuctionPc> AllegroAuctionPc { get; set; }
        public DbSet<AllegroPhotos> AllegroPhotos { get; set; }
        public DbSet<AllegroAdditionalInfo> AllegroAdditional { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=185.243.55.195;Database=trimfit_clutchlit;Username=clutchlit;Password=Bazacl8!");
        }
    }
}
