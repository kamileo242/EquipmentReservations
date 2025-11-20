namespace EquipmentReservations.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EquipmentReservationsDbContext context;
        public IReservationRepository Reservations { get; }
        public IEquipmentRepository Equipments { get; }
        public ICategoryRepository Categories { get; }
        public IAuditLogRepository AuditLogs { get; }

        public UnitOfWork(
            EquipmentReservationsDbContext context,
            IReservationRepository reservations,
            IEquipmentRepository equipments,
            ICategoryRepository categories,
            IAuditLogRepository auditLogs)
        {
            this.context = context;
            Reservations = reservations;
            Equipments = equipments;
            Categories = categories;
            AuditLogs = auditLogs;
        }

        public async Task<int> SaveChangesAsync()
            => await context.SaveChangesAsync();

        public void Dispose()
            => context.Dispose();
    }
}
