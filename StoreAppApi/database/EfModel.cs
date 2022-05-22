using Microsoft.EntityFrameworkCore;
using StoreAppApi.models.product;
using StoreAppApi.models.product.review;
using StoreAppApi.models.user;
using StoreAppApi.models.сompany;

namespace FastestDeliveryApi.database
{
    public class EfModel: DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BaseUser>()
                .HasMany(m => m.ProductsDownload).WithMany(u => u.UserDownload);

            modelBuilder.Entity<Product>()
                .HasMany(m => m.UserDownload).WithMany(u => u.ProductsDownload);
        }

        public EfModel(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<BaseUser> BaseUsers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<CompanyUser> CompanyUsers { get; set; }
        public virtual DbSet<Сompany> Сompanies { get; set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
    }
}
