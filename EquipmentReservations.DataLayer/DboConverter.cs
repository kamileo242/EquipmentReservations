using AutoMapper;
using EquipmentReservations.DataLayer.Dbos;
using EquipmentReservations.Models;

namespace EquipmentReservations.DataLayer
{
    public class DboConverter : IDboConverter
    {
        private readonly IMapper mapper;

        public DboConverter()
        {
            mapper = CreateMapper();
        }
        public TResult Convert<TResult>(object source)
        {
            return mapper.Map<TResult>(source);
        }

        private IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.ClearPrefixes();
                cfg.AllowNullCollections = true;

                cfg.CreateMap<EquipmentDbo, Equipment>();
                cfg.CreateMap<Equipment, EquipmentDbo>();

                cfg.CreateMap<CategoryDbo, Category>();
                cfg.CreateMap<Category, CategoryDbo>();

                cfg.CreateMap<ReservationDbo, Reservation>()
                    .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => Enum.Parse<ReservationStatus>(src.ReservationStatus)));
                cfg.CreateMap<Reservation, ReservationDbo>()
                    .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.ReservationStatus.ToString()));

                cfg.CreateMap<AuditLogDbo, AuditLog>();
                cfg.CreateMap<AuditLog, AuditLogDbo>();
            });

            return configuration.CreateMapper();
        }
    }
}
