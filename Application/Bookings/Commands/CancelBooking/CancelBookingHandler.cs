using Application.Common;
using Application.Interface;
using Domain.Enum;
using MediatR;

namespace Application.Bookings.Commands.CancelBooking
{
    public sealed class CancelBookingHandler : IRequestHandler<CancelBookingCommand, OperationResult<bool>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContext _userContext;

        public CancelBookingHandler(IBookingRepository bookingRepository, IUserContext userContext)
        {
            _bookingRepository = bookingRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<bool>> Handle(CancelBookingCommand request, CancellationToken ct)
        {
            // 0) Säkerställ att vi har användare + business i context
            if (_userContext.UserId is null)
                return OperationResult<bool>.Failure("Du måste vara inloggad.");

            if (_userContext.BusinessId is null)
                return OperationResult<bool>.Failure("Business saknas i användarkontexten.");

            var userId = _userContext.UserId.Value;
            var businessId = _userContext.BusinessId.Value;

            // 1) Hämta bokningen
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, ct);
            if (booking is null)
                return OperationResult<bool>.Failure("Bokningen finns inte.");

            // 2) Multi-tenant säkerhet
            if (booking.BusinessId != businessId)
                return OperationResult<bool>.Failure("Du har inte åtkomst till denna bokning.");

            // 3) Ägarskap (MVP: endast den som bokat får avboka)
            // (Sen kan du lägga till admin-regel)
            if (booking.UserId != userId)
                return OperationResult<bool>.Failure("Du kan bara avboka dina egna bokningar.");

            // 4) Idempotent: redan avbokad → OK
            if (booking.Status == BookingStatus.Cancelled)
                return OperationResult<bool>.Success(true);

            // 5) Avboka
            booking.Status = BookingStatus.Cancelled;
            booking.CancelledAtUtc = DateTime.UtcNow;
            booking.CancelledByUserId = userId;
            booking.CancellationReason = request.CancellationReason;
            booking.UpdatedAtUtc = DateTime.UtcNow;

            // 6) Spara
            await _bookingRepository.SaveChangesAsync(ct);

            return OperationResult<bool>.Success(true);
        }
    }
}
