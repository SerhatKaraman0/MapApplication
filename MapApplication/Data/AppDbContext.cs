using Microsoft.EntityFrameworkCore;

namespace MapApplication.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PointDb> Points { get; set; }
        public DbSet<WktDb> Wkt { get; set; }
        public DbSet<UsersDb> Users { get; set; }
        public DbSet<TabsDb> Tabs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure PointDb
            modelBuilder.Entity<PointDb>(entity =>
            {
                entity.ToTable("points");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.X_coordinate)
                      .HasColumnName("x_coordinate")
                      .IsRequired();
                entity.Property(p => p.Y_coordinate)
                      .HasColumnName("y_coordinate")
                      .IsRequired();
                entity.Property(p => p.Name)
                      .HasColumnName("name")
                      .IsRequired();
                entity.Property(p => p.Date)
                      .HasColumnName("date")
                      .IsRequired();
                entity.Property(p => p.OwnerId)
                      .HasColumnName("owner_id")
                      .IsRequired();

                // Configure the many-to-one relationship with UsersDb
                entity.HasOne<UsersDb>()
                      .WithMany(u => u.UserPoints)
                      .HasForeignKey(p => p.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure WktDb
            modelBuilder.Entity<WktDb>(entity =>
            {
                entity.ToTable("Wkt");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                      .HasColumnName("Name")
                      .IsRequired();
                entity.Property(p => p.Description)
                      .HasColumnName("Description")
                      .IsRequired();
                entity.Property(p => p.WKT)
                      .HasColumnName("WKT")
                      .IsRequired();
                entity.Property(p => p.PhotoLocation)
                      .HasColumnName("PhotoLocation")
                      .IsRequired();
                entity.Property(p => p.Color)
                      .HasColumnName("Color")
                      .IsRequired();
                entity.Property(p => p.Date)
                      .HasColumnName("Date")
                      .IsRequired();
                entity.Property(p => p.OwnerId)
                      .HasColumnName("OwnerId")
                      .IsRequired();

                // Configure the many-to-one relationship with UsersDb
                entity.HasOne<UsersDb>()
                      .WithMany(u => u.UserShapes)
                      .HasForeignKey(w => w.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure UsersDb
            modelBuilder.Entity<UsersDb>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(p => p.UserId);
                entity.Property(p => p.UserName)
                      .HasColumnName("userName")
                      .IsRequired();
                entity.Property(p => p.UserEmail)
                      .HasColumnName("userEmail")
                      .IsRequired();
                entity.Property(p => p.UserPassword)
                      .HasColumnName("userPassword")
                      .IsRequired();
                entity.Property(p => p.createdDate)
                      .HasColumnName("createdDate")
                      .IsRequired();
            });

            // Configure TabsDb
            modelBuilder.Entity<TabsDb>(entity =>
            {
                entity.ToTable("tabs");
                entity.HasKey(p => p.TabId);
                entity.Property(p => p.TabName)
                      .HasColumnName("tabName")
                      .IsRequired();
                entity.Property(p => p.TabColor)
                      .HasColumnName("tabColor")
                      .IsRequired();
                entity.Property(p => p.OwnerId)
                      .HasColumnName("OwnerId")
                      .IsRequired();
                entity.Property(p => p.createdDate)
                      .HasColumnName("createdDate")
                      .IsRequired();

                // Configure the many-to-one relationship with UsersDb
                entity.HasOne<UsersDb>()
                      .WithMany(u => u.UserTabs)
                      .HasForeignKey(t => t.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Other entity configurations
        }

    }
}
