using Application.Common;
using Application.Interface;
using Application.Services.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Services.Commands.UpdateService
{
    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, OperationResult<UpdateServiceDto>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IBusinessRepository _businessRepository; // Behövs för att kontrollera ägarskap
        private readonly IUserContext _userContext;
        private readonly IValidator<UpdateServiceCommand> _validator;

        public UpdateServiceCommandHandler(
            IServiceRepository serviceRepository, 
            IBusinessRepository businessRepository, 
            IUserContext userContext, 
            IValidator<UpdateServiceCommand> validator)
        {
            _serviceRepository = serviceRepository;
            _businessRepository = businessRepository;
            _userContext = userContext;
            _validator = validator;
        }

        public async Task<OperationResult<UpdateServiceDto>> Handle(UpdateServiceCommand request, CancellationToken ct)
        {
            // 1. Validering och hämta tjänst (samma logik)
            var service = await _serviceRepository.GetServiceByIdAsync(request.Id, ct);
            if (service == null) return OperationResult<UpdateServiceDto>.Failure("Tjänsten hittades inte.");

            var business = await _businessRepository.GetByIdAsync(service.BusinessId, ct);
        
            // 2. Behörighetskontroll (samma logik)
            var currentUserId = _userContext.UserId;
            if (business?.OwnerId != currentUserId && _userContext.Role != "SuperAdmin")
                return OperationResult<UpdateServiceDto>.Failure("Access denied.");

            // 3. Uppdatera fälten
            service.Name = request.Name;
            service.Description = request.Description;
            service.DurationMinutes = request.DurationMinutes;
            service.Price = request.Price;
            service.IsActive = request.IsActive;

            await _serviceRepository.UpdateServiceAsync(service, ct);

            // 4. Mappa till DTO och returnera
            var dto = new UpdateServiceDto(
                service.Id,
                service.Name,
                service.Description,
                service.DurationMinutes,
                service.Price,
                service.IsActive,
                service.BusinessId
            );

            return OperationResult<UpdateServiceDto>.Success(dto);
        }
    }
}