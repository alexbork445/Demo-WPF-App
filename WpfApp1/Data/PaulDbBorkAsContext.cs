using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Models;

namespace WpfApp1.Data;

public partial class PaulDbBorkAsContext : DbContext
{
    public PaulDbBorkAsContext()
    {
    }

    public PaulDbBorkAsContext(DbContextOptions<PaulDbBorkAsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-I15MDGH;Database=Exam;TrustServerCertificate=True;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PK__Equipmen__3447459991503C60");

            entity.HasIndex(e => e.Article, "IX_Equipment_Article");

            entity.HasIndex(e => e.Article, "UQ__Equipmen__4943444AA5A50452").IsUnique();

            entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            entity.Property(e => e.Article).HasMaxLength(20);
            entity.Property(e => e.Discount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Photo).HasMaxLength(100);
            entity.Property(e => e.RentalCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RentalUnit).HasMaxLength(20);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipment_Manufacturers");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipment_Suppliers");

            entity.HasOne(d => d.Type).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipment_Types");
        });

        modelBuilder.Entity<EquipmentType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Equipmen__516F03958E4C0D0E");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PK__Manufact__357E5CA13BA1B9DF");

            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.ManufacturerName).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF16032349");

            entity.HasIndex(e => e.OrderNumber, "IX_Orders_OrderNumber");

            entity.HasIndex(e => e.RentalStartDate, "IX_Orders_RentalStartDate");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ClientUserId).HasColumnName("ClientUserID");
            entity.Property(e => e.EquipmentId).HasColumnName("EquipmentID");
            entity.Property(e => e.OrderNumber).HasColumnType("decimal(10, 1)");
            entity.Property(e => e.PickupPointId).HasColumnName("PickupPointID");
            entity.Property(e => e.ReceiptCode).HasColumnType("decimal(10, 1)");
            entity.Property(e => e.RentalStartDate).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.ClientUser).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_ClientUsers");

            entity.HasOne(d => d.Equipment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EquipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Equipment");

            entity.HasOne(d => d.PickupPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickupPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_PickupPoints");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Statuses");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__C8EE2043DF95AD67");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.PointId).HasName("PK__PickupPo__40A97781796682A2");

            entity.Property(e => e.PointId).HasColumnName("PointID");
            entity.Property(e => e.Address).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A9B72DA1C");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666947487E062");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACD992035B");

            entity.HasIndex(e => e.Login, "IX_Users_Login");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
