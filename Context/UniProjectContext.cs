using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.Core.EntityValues;
using UniProject.DataLayer.Entity;
using UniProject.DataLayer.Entity.Order;
using UniProject.DataLayer.Entity.Permission;
using UniProject.DataLayer.Entity.Product;
using UniProject.DataLayer.EntityValues;

namespace UniProject.DataLayer.Context
{
    public class UniProjectContext : DbContext
    {
        public UniProjectContext(DbContextOptions<UniProjectContext> options) : base(options)
        {

        }

        #region Users
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        #endregion

        #region Wallet
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletType> WalletTypes { get; set; }
        #endregion

        #region Permissions
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        #endregion

        #region Product
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        #endregion

        #region Order
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        #endregion

        #region HasQueryFilter
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Role>()
               .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<ProductCategory>()
               .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Product>()
               .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<ProductFeature>()
              .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<ProductGallery>()
              .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<OrderDetail>()
              .HasQueryFilter(u => !u.IsRemoved);

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region SeeData
        private void SeedData(ModelBuilder modelBuilder)
        {
            #region Roles
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = 1, RoleTitle = typeof(RoleValues).GetField("Admin").GetValue(null).ToString() });
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = 2, RoleTitle = typeof(RoleValues).GetField("Seller").GetValue(null).ToString() });
            modelBuilder.Entity<Role>().HasData(new Role { RoleId = 3, RoleTitle = typeof(RoleValues).GetField("Customer").GetValue(null).ToString() });
            #endregion

            #region Permissions
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 1, PermissionTitle = typeof(PermissionValues).GetField("AdminPanel").GetValue(null).ToString(), ParentId = null });

            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 2, PermissionTitle = typeof(PermissionValues).GetField("ManagementUsers").GetValue(null).ToString(), ParentId = 1 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 3, PermissionTitle = typeof(PermissionValues).GetField("AddUsers").GetValue(null).ToString(), ParentId = 2 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 4, PermissionTitle = typeof(PermissionValues).GetField("EditUsers").GetValue(null).ToString(), ParentId = 2 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 5, PermissionTitle = typeof(PermissionValues).GetField("DeleteUsers").GetValue(null).ToString(), ParentId = 2 });

            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 6, PermissionTitle = typeof(PermissionValues).GetField("ManagementRoles").GetValue(null).ToString(), ParentId = 1 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 7, PermissionTitle = typeof(PermissionValues).GetField("AddRoles").GetValue(null).ToString(), ParentId = 6 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 8, PermissionTitle = typeof(PermissionValues).GetField("EditRoles").GetValue(null).ToString(), ParentId = 6 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 9, PermissionTitle = typeof(PermissionValues).GetField("DeleteRoles").GetValue(null).ToString(), ParentId = 6 });

            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 10, PermissionTitle = typeof(PermissionValues).GetField("ManagementCategories").GetValue(null).ToString(), ParentId = 1 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 11, PermissionTitle = typeof(PermissionValues).GetField("AddCategories").GetValue(null).ToString(), ParentId = 10 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 12, PermissionTitle = typeof(PermissionValues).GetField("EditCategories").GetValue(null).ToString(), ParentId = 10 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 13, PermissionTitle = typeof(PermissionValues).GetField("DeleteCategories").GetValue(null).ToString(), ParentId = 10 });

            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 14, PermissionTitle = typeof(PermissionValues).GetField("ManagementProducts").GetValue(null).ToString(), ParentId = 1 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 15, PermissionTitle = typeof(PermissionValues).GetField("AddProducts").GetValue(null).ToString(), ParentId = 14 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 16, PermissionTitle = typeof(PermissionValues).GetField("EditProducts").GetValue(null).ToString(), ParentId = 14 });
            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 17, PermissionTitle = typeof(PermissionValues).GetField("DeleteProducts").GetValue(null).ToString(), ParentId = 14 });

            modelBuilder.Entity<Permission>().HasData(new Permission { PermissionId = 18, PermissionTitle = typeof(PermissionValues).GetField("Accounting").GetValue(null).ToString(), ParentId = 1 });
            #endregion

            #region RolePermissions
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 1, RoleId = 1, PermissionId = 1 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 2, RoleId = 1, PermissionId = 2 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 3, RoleId = 1, PermissionId = 3 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 4, RoleId = 1, PermissionId = 4 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 5, RoleId = 1, PermissionId = 5 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 6, RoleId = 1, PermissionId = 6 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 7, RoleId = 1, PermissionId = 7 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 8, RoleId = 1, PermissionId = 8 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 9, RoleId = 1, PermissionId = 9 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 10, RoleId = 1, PermissionId = 10 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 11, RoleId = 1, PermissionId = 11 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 12, RoleId = 1, PermissionId = 12 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 13, RoleId = 1, PermissionId = 13 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 14, RoleId = 1, PermissionId = 14 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 15, RoleId = 1, PermissionId = 15 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 16, RoleId = 1, PermissionId = 16 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 17, RoleId = 1, PermissionId = 17 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 18, RoleId = 1, PermissionId = 18 });

            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 19, RoleId = 2, PermissionId = 14 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 20, RoleId = 2, PermissionId = 15 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 21, RoleId = 2, PermissionId = 16 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 22, RoleId = 2, PermissionId = 17 });
            modelBuilder.Entity<RolePermission>().HasData(new RolePermission { RPID = 23, RoleId = 2, PermissionId = 18 });
            #endregion

            #region Users
            modelBuilder.Entity<User>().HasData(new User { UserId = 1, UserName = "mehrsa", Email = "mehrsaamini92@gmail.com", Password = "25-F9-E7-94-32-3B-45-38-85-F5-18-1F-1B-62-4D-0B", ActiveCode = "86f12c62ce75471ca7358d4906c69be5", Avatar = "man.png", IsActive = true, RegisterDate = DateTime.Parse("2022-08-05 23:50:33.7898385"), IsDelete = false, RemainderWallet = 99897000 });
            #endregion

            #region UserRoles
            modelBuilder.Entity<UserRole>().HasData(new UserRole { URId = 1, UserId = 1, RoleId = 1 });
            modelBuilder.Entity<UserRole>().HasData(new UserRole { URId = 2, UserId = 1, RoleId = 2 });
            modelBuilder.Entity<UserRole>().HasData(new UserRole { URId = 3, UserId = 1, RoleId = 3 });
            #endregion
        }
        #endregion
    }
}
