using Application.Bookings.Dtos;
using Application.Common;
using Application.Interface;
using Domain.Enum;
using Domain.Models;
using MediatR;

namespace Application.Bookings.Commands.CreateBooking
{
    public sealed class CreateBookingHandler : IRequestHandler<CreateBookingCommand, OperationResult<BookingDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingTypeRepository _bookingTypeRepository;
        private readonly IResourceRepository _resourceRepository;
        private readonly IUserContext _userContext;

        public CreateBookingHandler(
            IBookingRepository bookingRepository,
            IBookingTypeRepository bookingTypeRepository,
            IResourceRepository resourceRepository,
            IUserContext userContext)
        {
            _bookingRepository = bookingRepository;
            _bookingTypeRepository = bookingTypeRepository;
            _resourceRepository = resourceRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<BookingDto>> Handle(CreateBookingCommand request, CancellationToken ct)
        {
            if (_userContext.UserId is null)
                return OperationResult<BookingDto>.Failure("Du måste vara inloggad.");

            if (_userContext.BusinessId is null)
                return OperationResult<BookingDto>.Failure("Business saknas i användarkontexten.");

            var userId = _userContext.UserId.Value;
            var businessId = _userContext.BusinessId.Value;


            // 1) Hämta booking type (regler)
            var bookingType = await _bookingTypeRepository.GetByIdAsync(businessId, request.BookingTypeId, ct);
            if (bookingType is null)
                return OperationResult<BookingDto>.Failure("Bokningstypen finns inte.");

            if (!bookingType.IsActive)
                return OperationResult<BookingDto>.Failure("Bokningstypen är inte aktiv.");

            // 2) Hämta resurs
            var resource = await _resourceRepository.GetByIdAsync(businessId, request.ResourceId, ct);
            if (resource is null)
                return OperationResult<BookingDto>.Failure("Resursen finns inte.");

            if (!resource.IsActive)
                return OperationResult<BookingDto>.Failure("Resursen är inte aktiv.");

            // 3) Kontrollera att resursen är tillåten för booking type (junction-tabell)
            var allowed = await _resourceRepository.IsAllowedForBookingTypeAsync(request.BookingTypeId, request.ResourceId, ct);
            if (!allowed)
                return OperationResult<BookingDto>.Failure("Resursen är inte tillåten för den valda bokningstypen.");

            // 4) Beräkna sluttid via duration
            var startUtc = request.StartTimeUtc;
            var endUtc = startUtc.AddMinutes(bookingType.DurationMinutes);

            // 5) Max bokningsfönster framåt
            var latestAllowed = DateTime.UtcNow.AddDays(bookingType.MaxAdvanceBookingDays);
            if (startUtc > latestAllowed)
                return OperationResult<BookingDto>.Failure($"Du kan max boka {bookingType.MaxAdvanceBookingDays} dagar framåt.");

            // (valfritt men bra) Förhindra bokning bakåt i tiden
            if (startUtc <= DateTime.UtcNow)
                return OperationResult<BookingDto>.Failure("Starttiden måste ligga i framtiden.");

            // 6) Max bokningar per user (om satt)
            if (bookingType.MaxBookingsPerUser.HasValue)
            {
                var activeCount = await _bookingRepository.CountActiveForUserAsync(businessId, userId, ct);
                if (activeCount >= bookingType.MaxBookingsPerUser.Value)
                    return OperationResult<BookingDto>.Failure("Du har nått max antal aktiva bokningar för denna bokningstyp.");
            }

            // 7) Overlap-check (dubbelbokning på samma resurs)
            var overlapping = await _bookingRepository.ExistsOverlappingAsync(
                businessId,
                request.ResourceId,
                startUtc,
                endUtc,
                ct);

            if (overlapping)
                return OperationResult<BookingDto>.Failure("Tiden är redan bokad.");

            // 8) Skapa bokning
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                BusinessId = businessId,
                UserId = userId,
                BookingTypeId = request.BookingTypeId,
                ResourceId = request.ResourceId,
                StartTimeUtc = startUtc,
                EndTimeUtc = endUtc,
                Notes = request.Notes,
                Status = bookingType.RequiresApproval ? BookingStatus.Pending : BookingStatus.Confirmed,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // 9) Spara
            await _bookingRepository.AddAsync(booking, ct);
            await _bookingRepository.SaveChangesAsync(ct);


            // 10) Returnera DTO
            var dto = new BookingDto
            {
                Id = booking.Id,
                BusinessId = booking.BusinessId,
                UserId = booking.UserId,
                BookingTypeId = booking.BookingTypeId,
                ResourceId = booking.ResourceId,
                StartTimeUtc = booking.StartTimeUtc,
                EndTimeUtc = booking.EndTimeUtc,
                Status = booking.Status,
                Notes = booking.Notes
            };

            return OperationResult<BookingDto>.Success(dto);
        }
    }
}
