using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.EnumTypes;

namespace My_Company.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<AppUser,AppRole,string,IdentityUserClaim<string>, 
            AppUserRole, IdentityUserLogin<string>,
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<ProductSector> ProductSectors { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseSector> WarehouseSectors { get; set; }
        public DbSet<WarehouseRow> WarehouseRows { get; set; }
        public DbSet<VATRate> VATRates { get; set; }
        public DbSet<Models.Attribute> Attributes { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<AttributeDictionaryValues> AttributeDictionaryValues { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });
            }); 
            
            builder.Entity<ProductSector>(entity =>
            {
                entity.HasKey(e => new { e.SectorId, e.ProductId });
            });
            
            builder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.CategoryId });
            });

             builder.Entity<ProductAttribute>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.AttributeId });
            });

            builder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });

            builder.Entity<Models.Attribute>(entity =>
            {
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Attributes)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<AttributeDictionaryValues>(entity =>
            {
                entity.HasOne(e => e.Attribute)
                    .WithMany(a => a.AttributeDictionaryValues)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ProductAttribute>(entity =>
            {
                entity.HasOne(e => e.Attribute)
                    .WithMany(a => a.ProductAttributes)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Product>(entity =>
            {
                entity.HasIndex(e => e.EANCode)
                    .IsUnique();

                entity.Property(e => e.Status)
                    .HasDefaultValue(ProductStatus.Active);
            });
        }

    }
}
