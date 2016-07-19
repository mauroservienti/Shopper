﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sales.Data.Migrations;
using Sales.Data.Models;

namespace Sales.Data.Context
{
    public interface ISalesContext
    {
        IDbSet<Product> Products { get; set; }
        IDbSet<Order> Orders { get; set; }

        Task<int> SaveChangesAsync();
    }

    [DbConfigurationType(typeof(SqLiteConfig))]
    public class SalesContext : DbContext, ISalesContext
    {
        public SalesContext() : base("Sales")
        {
        }

        public IDbSet<Product> Products { get; set; }
        public IDbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DatabaseInitializer(modelBuilder));

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Items)
                .WithRequired()
                .HasForeignKey(k => k.OrderId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
