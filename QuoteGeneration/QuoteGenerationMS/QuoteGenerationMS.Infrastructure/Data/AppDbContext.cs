
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Domain.Entities;

namespace QuoteGenerationMS.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<quote> quote => Set<quote>();
        public DbSet<premium_calculation> premium_calculation => Set<premium_calculation>();
        public DbSet<risk_assessment> risk_assessment => Set<risk_assessment>();
        public DbSet<quote_rider> quote_rider => Set<quote_rider>();
        public DbSet<discount_master> discount_master => Set<discount_master>();
        public DbSet<quote_history> quote_history => Set<quote_history>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quote
            modelBuilder.Entity<quote>(entity =>
            {
                entity.ToTable("quote");

                entity.HasKey(x => x.quote_id);

                entity.Property(x => x.quote_id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.quote_number)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.HasIndex(x => x.quote_number)
                    .IsUnique();

                entity.Property(x => x.customer_id)
                    .IsRequired();

                entity.Property(x => x.product_id)
                    .IsRequired();

                entity.Property(x => x.premium_frequency_id)
                    .IsRequired();

                entity.Property(x => x.policy_term)
                    .IsRequired();

                entity.Property(x => x.sum_insured)
                    .HasPrecision(15, 2)
                    .IsRequired();

                entity.Property(x => x.quote_date)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(x => x.expiry_date)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(x => x.quote_status)
                    .HasMaxLength(20)
                    .IsRequired();
            });

            // Premium Calculation
            modelBuilder.Entity<premium_calculation>(entity =>
            {
                entity.ToTable("premium_calculation");

                entity.HasKey(x => x.calculation_id);

                entity.Property(x => x.calculation_id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.base_premium)
                    .HasPrecision(15, 2);

                entity.Property(x => x.risk_loading)
                    .HasPrecision(15, 2);

                entity.Property(x => x.rider_premium)
                    .HasPrecision(15, 2);

                entity.Property(x => x.discount_amount)
                    .HasPrecision(15, 2);

                entity.Property(x => x.gst_amount)
                    .HasPrecision(15, 2);

                entity.Property(x => x.final_premium)
                    .HasPrecision(15, 2);

                entity.HasOne<quote>()
                    .WithMany()
                    .HasForeignKey(x => x.quote_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Risk Assessment
            modelBuilder.Entity<risk_assessment>(entity =>
            {
                entity.ToTable("risk_assessment");

                entity.HasKey(x => x.risk_id);

                entity.Property(x => x.risk_id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.risk_category)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasOne<quote>()
                    .WithMany()
                    .HasForeignKey(x => x.quote_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Quote Rider
            modelBuilder.Entity<quote_rider>(entity =>
            {
                entity.ToTable("quote_rider");

                entity.HasKey(x => x.rider_selection_id);

                entity.Property(x => x.rider_selection_id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.rider_premium)
                    .HasPrecision(15, 2);

                entity.HasOne<quote>()
                    .WithMany()
                    .HasForeignKey(x => x.quote_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Discount Master
            modelBuilder.Entity<discount_master>(entity =>
            {
                entity.ToTable("discount_master");

                entity.HasKey(x => x.discount_id);

                entity.Property(x => x.discount_id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.discount_name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.discount_percentage)
                    .HasPrecision(5, 2);

                entity.Property(x => x.valid_from)
                    .HasColumnType("date");

                entity.Property(x => x.valid_to)
                    .HasColumnType("date");

                entity.Property(x => x.status)
                    .HasMaxLength(20)
                    .IsRequired();
            });

            // Quote History
            modelBuilder.Entity<quote_history>(entity =>
            {
                entity.ToTable("quote_history");

                entity.HasKey(x => x.history_id);

                entity.Property(x => x.history_id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.action)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.remarks)
                    .HasMaxLength(500);

                entity.Property(x => x.action_date)
                    .IsRequired();

                entity.HasOne<quote>()
                    .WithMany()
                    .HasForeignKey(x => x.quote_id)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
