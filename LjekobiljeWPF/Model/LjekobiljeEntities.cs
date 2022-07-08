using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Ljekobilje
{
    public partial class LjekobiljeEntities : DbContext
    {
        public LjekobiljeEntities()
        {
        }

        public LjekobiljeEntities(DbContextOptions<LjekobiljeEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Cooperant> Cooperants { get; set; }
        public virtual DbSet<CooperantBankAccount> CooperantBankAccounts { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Plant> Plants { get; set; }
        public virtual DbSet<PlantPurchase> PlantPurchases { get; set; }
        public virtual DbSet<PlantPurchaseEntry> PlantPurchaseEntries { get; set; }
        public virtual DbSet<Purchasespaid> Purchasespaids { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<Stationsview> Stationsviews { get; set; }
        public virtual DbSet<Totalplantpurchased> Totalplantpurchaseds { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL(ConfigurationManager.ConnectionStrings["Ljekobilje"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PRIMARY");

                entity.ToTable("BankAccount", "Ljekobilje");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Bank)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Cooperant>(entity =>
            {
                entity.ToTable("Cooperant", "Ljekobilje");

                entity.HasIndex(e => e.StationId, "fk_KOOPERANT_OTKUPNA_STANICA1_idx");

                entity.Property(e => e.CooperantId).HasColumnName("CooperantID");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.Cooperants)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_KOOPERANT_OTKUPNA_STANICA1");
            });

            modelBuilder.Entity<CooperantBankAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PRIMARY");

                entity.ToTable("CooperantBankAccount", "Ljekobilje");

                entity.HasIndex(e => e.CooperantId, "fk_RAČUN_KOOPERANTA_KOOPERANT1_idx");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CooperantId).HasColumnName("CooperantID");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.CooperantBankAccount)
                    .HasForeignKey<CooperantBankAccount>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_RAČUN_KOOPERANTA_RAČUN1");

                entity.HasOne(d => d.Cooperant)
                    .WithMany(p => p.CooperantBankAccounts)
                    .HasForeignKey(d => d.CooperantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_RAČUN_KOOPERANTA_KOOPERANT1");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment", "Ljekobilje");

                entity.HasIndex(e => e.PlantPurchaseId, "fk_ISPLATA_OTKUP1_idx");

                entity.HasIndex(e => e.CooperantAccountId, "fk_ISPLATA_RAČUN_KOOPERANTA1_idx");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.CooperantAccountId).HasColumnName("CooperantAccountID");

                entity.Property(e => e.PlantPurchaseId).HasColumnName("PlantPurchaseID");

                entity.Property(e => e.Sum).HasColumnType("decimal(7,2)");

                entity.HasOne(d => d.CooperantAccount)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CooperantAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ISPLATA_RAČUN_KOOPERANTA1");

                entity.HasOne(d => d.PlantPurchase)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PlantPurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ISPLATA_OTKUP1");
            });

            modelBuilder.Entity<Plant>(entity =>
            {
                entity.ToTable("Plant", "Ljekobilje");

                entity.Property(e => e.PlantId).HasColumnName("PlantID");

                entity.Property(e => e.LatinName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MedicName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PlantName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RetailPrice).HasColumnType("decimal(5,2)");
            });

            modelBuilder.Entity<PlantPurchase>(entity =>
            {
                entity.ToTable("PlantPurchase", "Ljekobilje");

                entity.HasIndex(e => e.StationId, "OS_VRŠI");

                entity.HasIndex(e => e.CooperantId, "fk_OTKUP_KOOPERANT1_idx");

                entity.Property(e => e.PlantPurchaseId).HasColumnName("PlantPurchaseID");

                entity.Property(e => e.CooperantId).HasColumnName("CooperantID");

                entity.Property(e => e.DatePurchased).HasColumnType("date");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.TotalValue)
                    .HasColumnType("decimal(7,2)")
                    .HasDefaultValueSql("'0.00'");

                entity.HasOne(d => d.Cooperant)
                    .WithMany(p => p.PlantPurchases)
                    .HasForeignKey(d => d.CooperantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_OTKUP_KOOPERANT1");

                entity.HasOne(d => d.Station)
                    .WithMany(p => p.PlantPurchases)
                    .HasForeignKey(d => d.StationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OS_VRŠI");
            });

            modelBuilder.Entity<PlantPurchaseEntry>(entity =>
            {
                entity.HasKey(e => new { e.PlantPurchaseId, e.PlantId })
                    .HasName("PRIMARY");

                entity.ToTable("PlantPurchaseEntry", "Ljekobilje");

                entity.HasIndex(e => e.PlantId, "B_OS");

                entity.Property(e => e.PlantPurchaseId).HasColumnName("PlantPurchaseID");

                entity.Property(e => e.PlantId).HasColumnName("PlantID");

                entity.Property(e => e.Quantity).HasColumnType("decimal(7,2)");

                entity.Property(e => e.RetailPrice).HasColumnType("decimal(7,2)");

                entity.HasOne(d => d.Plant)
                    .WithMany(p => p.PlantPurchaseEntries)
                    .HasForeignKey(d => d.PlantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("B_OS");

                entity.HasOne(d => d.PlantPurchase)
                    .WithMany(p => p.PlantPurchaseEntries)
                    .HasForeignKey(d => d.PlantPurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OTKUP_SADRŽI");
            });

            modelBuilder.Entity<Purchasespaid>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("purchasespaid", "Ljekobilje");

                entity.Property(e => e.CooperantId).HasColumnName("CooperantID");

                entity.Property(e => e.CooperantName).HasMaxLength(91);

                entity.Property(e => e.DatePurchased).HasColumnType("date");

                entity.Property(e => e.PlantPurchaseId).HasColumnName("PlantPurchaseID");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.TotalValue)
                    .HasColumnType("decimal(7,2)")
                    .HasDefaultValueSql("'0.00'");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("Station", "Ljekobilje");

                entity.Property(e => e.StationId).HasColumnName("StationID");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Stationsview>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("stationsview", "Ljekobilje");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<Totalplantpurchased>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("totalplantpurchased", "Ljekobilje");

                entity.Property(e => e.PlantId).HasColumnName("PlantID");

                entity.Property(e => e.Quantity).HasColumnType("decimal(29,2)");

                entity.Property(e => e.StationId).HasColumnName("StationID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "Ljekobilje");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
