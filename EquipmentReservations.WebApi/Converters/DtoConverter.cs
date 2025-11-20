using EquipmentReservations.Models;
using EquipmentReservations.WebApi.Dtos;

namespace EquipmentReservations.WebApi.Converters
{
    public class DtoConverter : IDtoConverter
    {
        public CategoryDto Convert(Category model)
            => new()
            {
                Id = model.Id.GuidToText(),
                Name = model.Name
            };
        public CategoryTreeDto ConvertTree(Category model)
            => new()
            {
                Id = model.Id.GuidToText(),
                Name = model.Name,
                SubCategories = model.SubCategories?.Select(ConvertTree).ToList()
            };

        public Category Convert(StoreCategoryDto dto)
            => new()
            {
                Name = dto.Name ?? string.Empty,
                ParentCategoryId = dto.ParentCategoryId?.TextToGuidOrNull(),
            };

        public EquipmentDto Convert(Equipment model)
            => new()
            {
                Id = model.Id.GuidToText(),
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId.GuidToText(),
                CategoryName = model.Category?.Name
            };

        public FullEquipmentDto ConvertFull(Equipment model)
            => new()
            {
                Id = model.Id.GuidToText(),
                Name = model.Name,
                Description = model.Description,
                Category = model.Category is not null ? Convert(model.Category) : null!,
                Reservations = model.Reservations?
                    .Select(Convert)
                    .ToList() ?? new List<ReservationDto>()
            };

        public Equipment Convert(StoreEquipmentDto dto)
            => new()
            {
                Name = dto.Name ?? string.Empty,
                Description = dto.Description,
                CategoryId = dto.CategoryId?.TextToGuidOrNull() ?? Guid.Empty,
            };

        public ReservationDto Convert(Reservation model)
            => new()
            {
                Id = model.Id.GuidToText(),
                EquipmentId = model.EquipmentId.GuidToText(),
                EquipmentName = model.Equipment?.Name,
                UserId = model.UserId.ToString(),
                Start = model.Start,
                End = model.End,
                ReservationStatus = model.ReservationStatus.ToString(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

        public ReservationWithEquipmentDto ConvertWithEquipment(Reservation model)
            => new()
            {
                Id = model.Id.GuidToText(),
                Equipment = Convert(model.Equipment),
                UserId = model.UserId,
                Start = model.Start,
                End = model.End,
                ReservationStatus = model.ReservationStatus.ToString(),
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

        public Reservation Convert(ReservationStoreDto dto)
            => new()
            {
                EquipmentId = dto.EquipmentId?.TextToGuid() ?? Guid.Empty,
                UserId = dto.UserId ?? string.Empty,
                Start = dto.Start ?? DateTime.Now,
                End = dto.End
            };

        public AuditLogDto Convert(AuditLog model)
            => new()
            {
                Id = model.Id.GuidToText(),
                EntityName = model.EntityName,
                EntityId = model.EntityId,
                Action = model.Action,
                ChangeJson = model.ChangeJson,
                ChangedBy = model.ChangedBy,
                ChangedAt = model.ChangedAt
            };
    }
}
