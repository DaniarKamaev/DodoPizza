using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace DodoPizza.Models;

public partial class PizzaDbContext : DbContext
{
    public PizzaDbContext()
    {
    }

    public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<BasketItem> BasketItems { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("host=localhost;user=root;password=rootpassword;database=PizzaDb", ServerVersion.Parse("9.4.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.BasketId).HasName("PRIMARY");

            entity.ToTable("Basket");

            entity.HasIndex(e => e.UserId, "UserId_idx");

            entity.HasOne(d => d.User).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserId");
        });

        modelBuilder.Entity<BasketItem>(entity =>
        {
            entity.HasKey(e => e.BasketItemId).HasName("PRIMARY");

            entity.HasIndex(e => e.BasketId, "BasketId_idx");

            entity.HasIndex(e => e.MenuId, "MenuId_idx");

            entity.Property(e => e.AddedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Basket).WithMany(p => p.BasketItems)
                .HasForeignKey(d => d.BasketId)
                .HasConstraintName("FK_BasketItems_Basket");

            entity.HasOne(d => d.Menu).WithMany(p => p.BasketItems)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BasketItems_Menu");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Menu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
