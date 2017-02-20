﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using eShop.Settings;

namespace eShop.Model
{
  public class FirdoosModel : DbContext
  {
    public FirdoosModel() : base(ConnectionStrings.ConnectionString)
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      throw new UnintentionalCodeFirstException();
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
  }
}