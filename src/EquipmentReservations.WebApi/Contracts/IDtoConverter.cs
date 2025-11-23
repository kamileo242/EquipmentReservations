using EquipmentReservations.Models;
using EquipmentReservations.WebApi.Dtos;

namespace EquipmentReservations.WebApi
{
    /// <summary>
    /// Zbiór operacji konwertujących modele biznesowego na modele dto oraz modele dto na modele biznesowe. 
    /// </summary>
    public interface IDtoConverter
    {
        /// <summary>
        /// Konwertuje model domenowy kategorii na obiekt DTO.
        /// </summary>
        /// <param name="model">Model kategorii domenowej.</param>
        /// <returns>Obiekt DTO kategorii.</returns>
        CategoryDto Convert(Category model);

        /// <summary>
        /// Konwertuje model domenowy kategorii na obiekt DTO.
        /// </summary>
        /// <param name="model">Model kategorii domenowej.</param>
        /// <returns>Obiekt DTO drzewa kategorii.</returns>
        CategoryTreeDto ConvertTree(Category model);

        /// <summary>
        /// Konwertuje obiekt DTO kategorii przesyłany z klienta (StoreCategoryDto) na model domenowy.
        /// </summary>
        /// <param name="dto">Obiekt DTO kategorii.</param>
        /// <returns>Model domenowy kategorii.</returns>
        Category Convert(StoreCategoryDto dto);

        /// <summary>
        /// Konwertuje model domenowy sprzętu na obiekt DTO.
        /// </summary>
        /// <param name="model">Model sprzętu domenowego.</param>
        /// <returns>Obiekt DTO sprzętu.</returns>
        EquipmentDto Convert(Equipment model);

        /// <summary>
        /// Konwertuje model domenowy sprzętu na obiekt DTO.
        /// </summary>
        /// <param name="model">Model sprzętu domenowego.</param>
        /// <returns>Obiekt DTO sprzętu.</returns>
        FullEquipmentDto ConvertFull(Equipment model);

        /// <summary>
        /// Konwertuje obiekt DTO sprzętu przesyłany z klienta (StoreEquipmentDto) na model domenowy.
        /// </summary>
        /// <param name="dto">Obiekt DTO sprzętu.</param>
        /// <returns>Model domenowy sprzętu.</returns>
        Equipment Convert(StoreEquipmentDto dto);

        /// <summary>
        /// Konwertuje model domenowy rezerwacji na obiekt DTO.
        /// </summary>
        /// <param name="model">Model rezerwacji domenowej.</param>
        /// <returns>Obiekt DTO rezerwacji.</returns>
        ReservationDto Convert(Reservation model);

        /// <summary>
        /// Konwertuje model domenowy rezerwacji na obiekt DTO.
        /// </summary>
        /// <param name="model">Model rezerwacji domenowej.</param>
        /// <returns>Obiekt DTO rezerwacji.</returns>
        ReservationWithEquipmentDto ConvertWithEquipment(Reservation model);

        /// <summary>
        /// Konwertuje obiekt DTO rezerwacji przesyłany z klienta (ReservationStoreDto) na model domenowy.
        /// </summary>
        /// <param name="dto">Obiekt DTO rezerwacji.</param>
        /// <returns>Model domenowy rezerwacji.</returns>
        Reservation Convert(ReservationStoreDto dto);

        /// <summary>
        /// Konwertuje model domenowy logu audytu na obiekt DTO.
        /// </summary>
        /// <param name="model">Model logu audytu domenowego.</param>
        /// <returns>Obiekt DTO logu audytu.</returns>
        AuditLogDto Convert(AuditLog model);
    }
}
