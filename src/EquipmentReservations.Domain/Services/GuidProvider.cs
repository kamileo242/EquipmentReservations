namespace EquipmentReservations.Domain.Services
{
  public class GuidProvider : IGuidProvider
  {
    public Guid GenerateGuid()
      => Guid.NewGuid();
  }
}
