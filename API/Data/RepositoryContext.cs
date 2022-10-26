using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.Data
{
    public partial class RepositoryContext : DbContext
    {

        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Fridge> Fridges { get; set; }
        public virtual DbSet<FridgeModel> FridgeModels { get; set; }
        public virtual DbSet<FridgeProduct> FridgeProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Fridge>(entity =>
            {
                entity.ToTable("Fridge");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OwnerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.ModelNavigation)
                    .WithMany(p => p.Fridges)
                    .HasForeignKey(d => d.Model)
                    .HasConstraintName("Fridge_Model_FK");
            });

            modelBuilder.Entity<FridgeModel>(entity =>
            {
                entity.ToTable("FridgeModel");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<FridgeProduct>(entity =>
            {
                entity.ToTable("FridgeProduct");

                entity.HasOne(d => d.FridgeNavigation)
                    .WithMany(p => p.FridgeProducts)
                    .HasForeignKey(d => d.Fridge)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FridgeProduct_Fridge_FK");

                entity.HasOne(d => d.ProductNavigation)
                    .WithMany(p => p.FridgeProducts)
                    .HasForeignKey(d => d.Product)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FridgeProduct_Product_FK");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Image)
                    .HasForeignKey<Image>(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Image_Product_FK");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(30);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Role)
                .HasDefaultValue(10);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("User_Role_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
