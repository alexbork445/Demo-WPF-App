using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1.Data;

public partial class ExampleDbContext : DbContext
{
    public ExampleDbContext()
    {
    }

    public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Manufacturer> Manufacturer { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetails> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatus { get; set; }

    public virtual DbSet<PickupPoint> PickupPoint { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductType> ProductType { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Supplier> Supplier { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ARTHUR\\SQLEXPRESS;Database=Example_DB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC0738B542EC");

            entity.Property(e => e.ManufacturerName).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC07516D609B");

            entity.HasIndex(e => e.OrderStatusId, "IX_Order_OrderStatusId");

            entity.HasIndex(e => e.PickupPointId, "IX_Order_PickupPointId");

            entity.HasIndex(e => e.UserId, "IX_Order_UserId");

            entity.Property(e => e.Code).HasMaxLength(255);

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Order)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK__Order__OrderStat__4E88ABD4");

            entity.HasOne(d => d.PickupPoint).WithMany(p => p.Order)
                .HasForeignKey(d => d.PickupPointId)
                .HasConstraintName("FK__Order__PickupPoi__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.Order)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserId__4D94879B");
        });

        modelBuilder.Entity<OrderDetails>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC0730083D54");

            entity.HasIndex(e => e.OrderId, "IX_OrderDetails_OrderId");

            entity.HasIndex(e => e.ProductId, "IX_OrderDetails_ProductId");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__5165187F");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__Produ__52593CB8");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC072BBB4058");

            entity.Property(e => e.StatusName).HasMaxLength(255);
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PickupPo__3214EC072B0834DC");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC0767A43587");

            entity.HasIndex(e => e.ManufacturerId, "IX_Product_ManufacturerId");

            entity.HasIndex(e => e.ProductTypeId, "IX_Product_ProductTypeId");

            entity.HasIndex(e => e.SupplierId, "IX_Product_SupplierId");

            entity.Property(e => e.Article).HasMaxLength(255);
            entity.Property(e => e.Photo).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UnitOfMeasure).HasMaxLength(255);

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Product)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK__Product__Manufac__44FF419A");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Product)
                .HasForeignKey(d => d.ProductTypeId)
                .HasConstraintName("FK__Product__Product__45F365D3");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Product)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__Product__Supplie__440B1D61");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC07E908A462");

            entity.Property(e => e.TypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC073084625B");

            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC0792FB1747");

            entity.Property(e => e.SupplierName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07E1ADCA48");

            entity.HasIndex(e => e.RoleId, "IX_User_RoleId");

            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.User)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleId__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
