using Application.Common;
using Application.Interface;
using MediatR;


namespace Application.Admin.Commands.UpdateBookingStatus
{
    public sealed class UpdateBookingStatusHandler
        : IRequestHandler<UpdateBookingStatusCommand, OperationResult<bool>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContext _userContext;

        public UpdateBookingStatusHandler(
            IBookingRepository bookingRepository,
            IUserContext userContext)
        {
            _bookingRepository = bookingRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<bool>> Handle(
            UpdateBookingStatusCommand request,
            CancellationToken ct)
        {
            if (_userContext.BusinessId is null)
                return OperationResult<bool>.Failure("Business saknas i användarkontexten.");

            var businessId = _userContext.BusinessId.Value;

            var updated = await _bookingRepository.UpdateStatusAsync(
                businessId,
                request.BookingId,
                request.NewStatus,
                ct);

            if (!updated)
                return OperationResult<bool>.Failure("Bokningen hittades inte.");

            return OperationResult<bool>.Success(true);
        }
    }
}
