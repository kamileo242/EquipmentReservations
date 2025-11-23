using EquipmentReservations.DataLayer.Dbos;
using EquipmentReservations.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Główny kontekst EF Core dla systemu rezerwacji sprzętu.
    /// Zawiera tabele Identity oraz tabele domenowe systemu.
    /// Obsługuje również logowanie zmian dzięki interceptorowi.
    /// </summary>
    public class EquipmentReservationsDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        /// <summary>
        /// Interceptor odpowiedzialny za tworzenie wpisów AuditLog
        /// przy każdej operacji zapisu danych.
        /// </summary>
        private readonly AuditSaveChangeInterceptor auditInterceptor;

        /// <summary>
        /// Konstruktor kontekstu bazodanowego.
        /// Interceptor jest wstrzykiwany przez DI.
        /// </summary>
        public EquipmentReservationsDbContext(
            DbContextOptions<EquipmentReservationsDbContext> options,
            AuditSaveChangeInterceptor auditInterceptor)
            : base(options)
        {
            this.auditInterceptor = auditInterceptor;
        }

        /// <summary>
        /// Tabela sprzętu.
        /// </summary>
        public DbSet<EquipmentDbo> Equipments { get; set; } = null!;

        /// <summary>
        /// Tabela kategorii sprzętu.
        /// </summary>
        public DbSet<CategoryDbo> Categories { get; set; } = null!;

        /// <summary>
        /// Tabela rezerwacji sprzętu.
        /// </summary>
        public DbSet<ReservationDbo> Reservations { get; set; } = null!;

        /// <summary>
        /// Tabela logów audytowych.
        /// </summary>
        public DbSet<AuditLogDbo> AuditLogs { get; set; } = null!;

        /// <summary>
        /// Konfiguracja modeli domenowych.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CategoryDbo>()
                .HasMany(c => c.SubCategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Podłącza interceptor odpowiedzialny za audytowanie operacji CRUD.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(auditInterceptor);
        }
    }
}
