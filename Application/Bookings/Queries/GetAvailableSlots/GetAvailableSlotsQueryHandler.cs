
using Application.Bookings.DTOs;
using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Bookings.Queries.GetAvailableSlots
{
    public sealed class GetAvailableSlotsHandler
        : IRequestHandler<GetAvailableSlotsQuery, OperationResult<List<AvailableSlotDto>>>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContext _userContext;

        public GetAvailableSlotsHandler(
            IResourceRepository resourceRepository,
            IBookingRepository bookingRepository,
            IUserContext userContext)
        {
            _resourceRepository = resourceRepository;
            _bookingRepository = bookingRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<List<AvailableSlotDto>>> Handle(GetAvailableSlotsQuery request, CancellationToken ct)
        {
            if (_userContext.BusinessId is null)
                return OperationResult<List<AvailableSlotDto>>.Failure("Business saknas i användarkontexten.");

            var businessId = _userContext.BusinessId.Value;

            // 1) Hämta alla resurser som är kopplade till bokningstypen
            var resources = await _resourceRepository.GetResourcesForBookingTypeAsync(
                businessId,
                request.BookingTypeId,
                ct);

            if (resources.Count == 0)
                return OperationResult<List<AvailableSlotDto>>.Success(new List<AvailableSlotDto>());

            var startUtc = request.StartDateUtc;
            var endUtc = request.EndDateUtc;

            // (Validator borde redan skydda, men vi är defensiva)
            if (endUtc < startUtc)
                return OperationResult<List<AvailableSlotDto>>.Failure("Slutdatum måste vara samma eller efter startdatum.");

            var slots = new List<AvailableSlotDto>();

            // 2) För varje resurs: om ingen overlap i hela intervallet → en slot
            foreach (var resource in resources)
            {
                // Om ResourceId skickas in i queryn, filtrera på den
                if (request.ResourceId.HasValue && resource.Id != request.ResourceId.Value)
                    continue;

                var isTaken = await _bookingRepository.ExistsOverlappingAsync(
                    businessId,
                    resource.Id,
                    startUtc,
                    endUtc,
                    ct);

                if (!isTaken)
                {
                    slots.Add(new AvailableSlotDto
                    {
                        ResourceId = resource.Id,
                        StartTimeUtc = startUtc,
                        EndTimeUtc = endUtc
                    });
                }
            }

            return OperationResult<List<AvailableSlotDto>>.Success(slots);
        }
    }
}
