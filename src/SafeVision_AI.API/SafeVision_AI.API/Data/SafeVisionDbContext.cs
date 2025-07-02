using Microsoft.EntityFrameworkCore;
using SafeVisionAI.Core.Entities;

namespace SafeVision_AI.API.Data
{
    public class SafeVisionDbContext : DbContext
    {
        public SafeVisionDbContext(DbContextOptions<SafeVisionDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentAlert> IncidentAlerts { get; set; }
        public DbSet<DailyReport> DailyReports { get; set; }
        public DbSet<ProcessingQueue> ProcessingQueue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Camera Configuration
            modelBuilder.Entity<Camera>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.StreamUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Location).HasMaxLength(255);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.LastSeen).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Incident Configuration
            modelBuilder.Entity<Incident>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Notes).HasMaxLength(2000);
                entity.Property(e => e.DetectedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.ConfidenceScore).HasPrecision(5, 4); // 99.99%

                // Foreign Key
                entity.HasOne(e => e.Camera)
                    .WithMany(c => c.Incidents)
                    .HasForeignKey(e => e.CameraId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes for performance
                entity.HasIndex(e => e.DetectedAt);
                entity.HasIndex(e => e.Type);
                entity.HasIndex(e => e.Severity);
                entity.HasIndex(e => e.IsResolved);
                entity.HasIndex(e => new { e.CameraId, e.DetectedAt });
            });

            // IncidentAlert Configuration
            modelBuilder.Entity<IncidentAlert>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RecipientEmail).IsRequired().HasMaxLength(200);
                entity.Property(e => e.RecipientPhone).HasMaxLength(20);
                entity.Property(e => e.ErrorMessage).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Foreign Key
                entity.HasOne(e => e.Incident)
                    .WithMany(i => i.Alerts)
                    .HasForeignKey(e => e.IncidentId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            // DailyReport Configuration
            modelBuilder.Entity<DailyReport>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Summary).IsRequired();
                entity.Property(e => e.GeneratedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.ReportDate).IsUnique();
            });

            // ProcessingQueue Configuration
            modelBuilder.Entity<ProcessingQueue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);

                // Foreign Key
                entity.HasOne(e => e.Camera)
                    .WithMany()
                    .HasForeignKey(e => e.CameraId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Indexes for queue processing
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => new { e.Status, e.CreatedAt });
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var createdAt1 = new DateTime(2024, 1, 1, 8, 0, 0, DateTimeKind.Utc);
            var createdAt2 = new DateTime(2024, 1, 1, 8, 5, 0, DateTimeKind.Utc);
            var createdAt3 = new DateTime(2024, 1, 1, 8, 10, 0, DateTimeKind.Utc);
            var lastSeen1 = createdAt1;
            var lastSeen2 = createdAt2;
            var lastSeen3 = createdAt3.AddHours(-2);

            modelBuilder.Entity<Camera>().HasData(
                new Camera
                {
                    Id = 1,
                    Name = "Main Entrance",
                    StreamUrl = "rtsp://demo:demo@ipvmdemo.dyndns.org:5541/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast",
                    Location = "Building A - Main Entrance",
                    IsActive = true,
                    CreatedAt = createdAt1,
                    LastSeen = lastSeen1,
                    FrameRate = 30,
                    AnalysisInterval = 3
                },
                new Camera
                {
                    Id = 2,
                    Name = "Parking Lot",
                    StreamUrl = "rtsp://demo:demo@ipvmdemo.dyndns.org:5542/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast",
                    Location = "Building A - Parking Area",
                    IsActive = true,
                    CreatedAt = createdAt2,
                    LastSeen = lastSeen2,
                    FrameRate = 30,
                    AnalysisInterval = 5
                },
                new Camera
                {
                    Id = 3,
                    Name = "Cafeteria",
                    StreamUrl = "rtsp://demo:demo@ipvmdemo.dyndns.org:5543/onvif-media/media.amp?profile=profile_1_h264&sessiontimeout=60&streamtype=unicast",
                    Location = "Building B - Cafeteria",
                    IsActive = false, // Offline for testing
                    CreatedAt = createdAt3,
                    LastSeen = lastSeen3,
                    FrameRate = 25,
                    AnalysisInterval = 4
                }
            );
        }
    }
}