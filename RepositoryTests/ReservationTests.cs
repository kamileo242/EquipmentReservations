using System.Reflection;
using EquipmentReservations.DataLayer;
using EquipmentReservations.DataLayer.Dbos;
using EquipmentReservations.DataLayer.Repositories;
using EquipmentReservations.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EquipmentReservations.Tests.Repositories
{
    [TestFixture]
    public class ReservationRepositoryTests
    {
        private EquipmentReservationsDbContext context;
        private ReservationRepository repository;
        private Mock<IDboConverter> converterMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<EquipmentReservationsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new EquipmentReservationsDbContext(options);
            converterMock = new Mock<IDboConverter>();

            converterMock
                .Setup(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()))
                .Returns((ReservationDbo dbo) => new Reservation
                {
                    Id = dbo.Id,
                    Equipment = new Equipment { Id = dbo.EquipmentId, Name = "Test Equipment" },
                    UserId = dbo.UserId,
                    Start = dbo.Start,
                    End = dbo.End,
                    ReservationStatus = Enum.TryParse<ReservationStatus>(dbo.ReservationStatus, out var status) ? status : ReservationStatus.Pending,
                    CreatedAt = dbo.CreatedAt,
                    UpdatedAt = dbo.UpdatedAt
                });

            converterMock
                .Setup(c => c.Convert<ReservationDbo>(It.IsAny<Reservation>()))
                .Returns((Reservation model) => new ReservationDbo
                {
                    Id = model.Id,
                    EquipmentId = model.Equipment.Id,
                    UserId = model.UserId,
                    Start = model.Start,
                    End = model.End,
                    ReservationStatus = model.ReservationStatus.ToString(),
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt,
                });

            repository = new ReservationRepository(context, converterMock.Object);
        }

        [TearDown]
        public void TearDown() => context.Dispose();

        [Test]
        public async Task AddAsync_Should_throw_argument_null_exception_when_entity_is_null()
        {
            Func<Task> act = async () => await repository.AddAsync(null!);

            await act.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task AddAsync_Should_throw_argumentnullexception_from_ef_when_converter_returns_null()
        {
            var model = CreateValidReservation();
            converterMock.Setup(c => c.Convert<ReservationDbo>(model)).Returns((ReservationDbo?)null);

            Func<Task> act = async () => await repository.AddAsync(model);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task AddAsync_Should_throw_dbupdateexception_when_required_field_missing_()
        {
            var model = CreateValidReservation();
            model.UserId = null!;

            Func<Task> act = async () => await repository.AddAsync(model);

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [Test]
        public async Task AddAsync_Should_add_to_database_and_return_model_when_valid_reservation()
        {
            var model = CreateValidReservation();

            var result = await repository.AddAsync(model);

            result.Should().NotBeNull();
            result.UserId.Should().Be(model.UserId);
            var dboInDb = await context.Reservations.FirstOrDefaultAsync(x => x.Id == model.Id);
            dboInDb.Should().NotBeNull();
            dboInDb!.UserId.Should().Be(model.UserId);
            converterMock.Verify(c => c.Convert<ReservationDbo>(It.IsAny<Reservation>()), Times.Once);
            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Once);
        }


        [Test]
        public async Task GetByIDAsync_Should_return_null_when_entity_not_found()
        {
            var missingId = new Guid("00000000-0000-0000-0000-000000000999");

            var result = await repository.GetByIDAsync(missingId);

            result.Should().BeNull();
            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Never);
        }

        [Test]
        public async Task GetByIDAsync_Should_return_model_when_entity_exists()
        {
            var reservation = CreateValidReservation();
            var dbo = converterMock.Object.Convert<ReservationDbo>(reservation);
            await context.Reservations.AddAsync(dbo);
            await context.SaveChangesAsync();

            var result = await repository.GetByIDAsync(reservation.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(reservation.Id);
            result.UserId.Should().Be(reservation.UserId);
            result.Equipment.Id.Should().Be(reservation.Equipment.Id);

            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Once);
        }

        [Test]
        public async Task GetByIDAsync_Should_not_throw_when_db_is_empty()
        {
            Func<Task> act = async () => await repository.GetByIDAsync(new Guid("00000000-0000-0000-0000-000000000555"));

            await act.Should().NotThrowAsync();
        }

        [Test]
        public async Task GetAllAsync_Should_return_empty_list_when_no_entities()
        {
            var result = await repository.GetAllAsync();

            result.Should().BeEmpty();
            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Never);
        }

        [Test]
        public async Task GetAllAsync_Should_return_all_entities()
        {
            var res1 = CreateValidReservation();
            var res2 = CreateValidReservation();
            res2.Id = new Guid("00000000-0000-0000-0000-000000000023");
            res2.UserId = "user2";
            var dbo1 = converterMock.Object.Convert<ReservationDbo>(res1);
            var dbo2 = converterMock.Object.Convert<ReservationDbo>(res2);
            await context.Reservations.AddRangeAsync(dbo1, dbo2);
            await context.SaveChangesAsync();

            var result = (await repository.GetAllAsync()).ToList();

            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == res1.Id);
            result.Should().Contain(x => x.Id == res2.Id);

            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Exactly(2));
        }

        [Test]
        public async Task DeleteAsync_Should_do_nothing_when_entity_not_found()
        {
            Func<Task> act = async () => await repository.DeleteAsync(new Guid("00000000-0000-0000-0000-000000000012"));

            await act.Should().NotThrowAsync();
        }

        [Test]
        public async Task DeleteAsync_Should_remove_existing_entity()
        {
            var reservation = CreateValidReservation();
            var dbo = converterMock.Object.Convert<ReservationDbo>(reservation);
            await context.Reservations.AddAsync(dbo);
            await context.SaveChangesAsync();

            await repository.DeleteAsync(reservation.Id);

            var dboInDb = await context.Reservations.FindAsync(reservation.Id);
            dboInDb.Should().BeNull();
        }

        [Test]
        public async Task UpdateAsync_Should_throw_when_entity_is_null()
        {
            Func<Task> act = async () => await repository.UpdateAsync(null!);

            await act.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task UpdateAsync_Should_throw_when_converter_returns_null()
        {
            var model = CreateValidReservation();
            converterMock.Setup(c => c.Convert<ReservationDbo>(model)).Returns((ReservationDbo?)null);

            Func<Task> act = async () => await repository.UpdateAsync(model);

            await act.Should().ThrowAsync<TargetException>();
        }

        [Test]
        public async Task UpdateAsync_Should_update_existing_record_and_return_updated_model()
        {
            var reservation = CreateValidReservation();
            var dbo = converterMock.Object.Convert<ReservationDbo>(reservation);
            await context.Reservations.AddAsync(dbo);
            await context.SaveChangesAsync();

            reservation.UserId = "updatedUser";
            var result = await repository.UpdateAsync(reservation);

            result.Should().NotBeNull();
            result.UserId.Should().Be("updatedUser");

            var dboInDb = await context.Reservations.FirstOrDefaultAsync(x => x.Id == reservation.Id);
            dboInDb.Should().NotBeNull();
            dboInDb!.UserId.Should().Be("updatedUser");

            converterMock.Verify(c => c.Convert<ReservationDbo>(It.IsAny<Reservation>()), Times.AtLeastOnce);
            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task UpdateAsync_Should_throw_when_entity_not_found()
        {
            var reservation = CreateValidReservation();

            Func<Task> act = async () => await repository.UpdateAsync(reservation);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task GetActiveAsync_Should_return_only_active_reservations()
        {
            var now = DateTime.Now;
            var active = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000000112"),
                EquipmentId = new Guid("00000000-0000-0000-0000-000000000113"),
                UserId = "user1",
                Start = now.AddHours(-1),
                End = now.AddHours(1),
                ReservationStatus = "Pending",
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1)
            };

            var past = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000000222"),
                EquipmentId = new Guid("00000000-0000-0000-0000-000000000931"),
                UserId = "user2",
                Start = now.AddHours(-3),
                End = now.AddHours(-2),
                ReservationStatus = "Pending",
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1)
            };

            await context.Reservations.AddRangeAsync(active, past);
            await context.SaveChangesAsync();

            var result = (await repository.GetActiveAsync()).ToList();

            result.Should().HaveCount(1);
            result.First().Id.Should().Be(active.Id);
            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Once);
        }

        [Test]
        public async Task GetActiveAsync_Should_return_empty_when_none_active()
        {
            var now = DateTime.Parse("2025-10-10");
            var past = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000000058"),
                EquipmentId = new Guid("00000000-0000-0000-0000-000000000822"),
                UserId = "user2",
                Start = now.AddHours(-5),
                End = now.AddHours(-4),
                ReservationStatus = "Pending",
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1)
            };
            await context.Reservations.AddAsync(past);
            await context.SaveChangesAsync();

            var result = await repository.GetActiveAsync();

            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetByUserAsync_Should_return_reservations_for_specified_user()
        {
            var userId = "targetUser";
            var dbo1 = converterMock.Object.Convert<ReservationDbo>(CreateValidReservation());
            dbo1.UserId = userId;

            var dbo2 = converterMock.Object.Convert<ReservationDbo>(CreateValidReservation());
            dbo2.Id = new Guid("00000000-0000-0000-0000-000000000952");
            dbo2.UserId = "otherUser";

            await context.Reservations.AddRangeAsync(dbo1, dbo2);
            await context.SaveChangesAsync();

            var result = (await repository.GetByUserAsync(userId)).ToList();

            result.Should().HaveCount(1);
            result.First().UserId.Should().Be(userId);
            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Once);
        }

        [Test]
        public async Task GetByUserAsync_Should_return_empty_when_user_has_no_reservations()
        {
            var result = await repository.GetByUserAsync("nonExistingUser");

            result.Should().BeEmpty();
        }

        [Test]
        public async Task IsEquipmentAvailableAsync_Should_return_false_when_overlap_exists()
        {
            var now = DateTime.Parse("2025-10-05");
            var equipmentId = new Guid("00000000-0000-0000-0000-000000001052");

            var dbo = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000001053"),
                EquipmentId = equipmentId,
                UserId = "user",
                Start = now.AddHours(-1),
                End = now.AddHours(1),
                ReservationStatus = "Pending",
                CreatedAt = now,
                UpdatedAt = now
            };
            await context.Reservations.AddAsync(dbo);
            await context.SaveChangesAsync();

            var result = await repository.IsEquipmentAvailableAsync(equipmentId, now, now.AddHours(2));

            result.Should().BeFalse();
        }

        [Test]
        public async Task IsEquipmentAvailableAsync_Should_return_true_when_no_overlap()
        {
            var now = DateTime.Parse("2025-10-05");
            var equipmentId = new Guid("00000000-0000-0000-0000-000000001054");

            var dbo = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000001055"),
                EquipmentId = equipmentId,
                UserId = "user",
                Start = now.AddHours(-4),
                End = now.AddHours(-3),
                ReservationStatus = "Pending",
                CreatedAt = now,
                UpdatedAt = now
            };
            await context.Reservations.AddAsync(dbo);
            await context.SaveChangesAsync();

            var result = await repository.IsEquipmentAvailableAsync(equipmentId, now, now.AddHours(1));

            result.Should().BeTrue();
        }

        [Test]
        public async Task SearchAsync_Should_return_matching_reservations_by_equipment_name()
        {
            var dbo1 = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000001056"),
                EquipmentId = new Guid("00000000-0000-0000-0000-000000001057"),
                UserId = "user1",
                Start = DateTime.Parse("2025-10-05"),
                End = DateTime.Parse("2025-10-05").AddHours(1),
                ReservationStatus = "Pending",
                CreatedAt = DateTime.Parse("2025-10-05"),
                UpdatedAt = DateTime.Parse("2025-10-05"),
                Equipment = new EquipmentDbo { Id = Guid.NewGuid(), Name = "Laptop Dell" }
            };

            var dbo2 = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000001057"),
                EquipmentId = new Guid("00000000-0000-0000-0000-000000001057"),
                UserId = "user2",
                Start = DateTime.Parse("2025-10-05"),
                End = DateTime.Parse("2025-10-05").AddHours(1),
                ReservationStatus = "Pending",
                CreatedAt = DateTime.Parse("2025-10-05"),
                UpdatedAt = DateTime.Parse("2025-10-05"),
                Equipment = new EquipmentDbo { Id = Guid.NewGuid(), Name = "Projector Epson" }
            };

            await context.Reservations.AddRangeAsync(dbo1, dbo2);
            await context.SaveChangesAsync();

            var result = (await repository.SearchAsync("Laptop")).ToList();

            result.Should().HaveCount(1);
            result.First().Id.Should().Be(dbo1.Id);
            converterMock.Verify(c => c.Convert<Reservation>(It.IsAny<ReservationDbo>()), Times.Once);
        }

        [Test]
        public async Task SearchAsync_Should_return_empty_when_no_match_found()
        {
            var dbo = new ReservationDbo
            {
                Id = new Guid("00000000-0000-0000-0000-000000001088"),
                EquipmentId = new Guid("00000000-0000-0000-0000-000000001099"),
                UserId = "user",
                Start = DateTime.Parse("2025-10-05"),
                End = DateTime.Parse("2025-10-05").AddHours(1),
                ReservationStatus = "Pending",
                CreatedAt = DateTime.Parse("2025-10-05"),
                UpdatedAt = DateTime.Parse("2025-10-05"),
                Equipment = new EquipmentDbo { Id = Guid.NewGuid(), Name = "Camera Sony" }
            };
            await context.Reservations.AddAsync(dbo);
            await context.SaveChangesAsync();

            var result = await repository.SearchAsync("Printer");

            result.Should().BeEmpty();
        }

        private Reservation CreateValidReservation()
        {
            var reservationId = new Guid("00000000-0000-0000-0000-000000011111");
            var equipmentId = new Guid("00000000-0000-0000-0000-000000022222");
            return new Reservation
            {
                Id = reservationId,
                Equipment = new Equipment { Id = equipmentId, Name = "Laptop" },
                UserId = "123",
                Start = DateTime.Parse("2025-10-10"),
                End = DateTime.Parse("2025-10-10").AddHours(2),
                ReservationStatus = ReservationStatus.Pending,
                CreatedAt = DateTime.Parse("2025-10-05"),
                UpdatedAt = DateTime.Parse("2025-10-05")
            };
        }
    }
}
