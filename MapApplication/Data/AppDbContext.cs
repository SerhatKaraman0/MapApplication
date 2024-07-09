using Microsoft.EntityFrameworkCore;

namespace MapApplication.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PointDb> Points { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PointDb>(entity =>
            {
                entity.ToTable("points");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.X_coordinate).HasColumnName("X_coordinate").IsRequired();
                entity.Property(p => p.Y_coordinate).HasColumnName("Y_coordinate").IsRequired();
                entity.Property(p => p.Name).IsRequired();
            });
        }
    }
}
