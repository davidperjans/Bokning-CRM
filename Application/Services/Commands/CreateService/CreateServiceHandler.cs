using Application.Common;
using Application.Interface;
using Application.Services.DTOs;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.Services.Commands.CreateService
{
    public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, OperationResult<Guid>>
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserContext _userContext;
        private readonly IValidator<CreateServiceCommand> _validator;
        
        public CreateServiceHandler(IBusinessRepository businessRepository, IUserContext userContext, IValidator<CreateServiceCommand> validator)
        {
            _businessRepository = businessRepository;
            _userContext = userContext;
            _validator = validator;
        }

        public async Task<OperationResult<Guid>> Handle(CreateServiceCommand request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _userContext.UserId;
            if (currentUserId == null) return OperationResult<Guid>.Failure("User context missing.");

            var business = await _businessRepository.GetByIdAsync(request.BusinessId);
            if (business == null) return OperationResult<Guid>.Failure("Business not found.");

            // Kontrollera behörighet: Ägare ELLER SuperAdmin
            bool isOwner = business.OwnerId == currentUserId;
            bool isSuperAdmin = _userContext.Role == "SuperAdmin";
            
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return OperationResult<Guid>.Failure(validationResult.Errors.First().ErrorMessage);
            }

            if (!isOwner && !isSuperAdmin)
            {
                return OperationResult<Guid>.Failure("Access denied.");
            }
            
            var service = new Service
            {
                Id = Guid.NewGuid(),
                BusinessId = request.BusinessId,
                Name = request.Name,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                Price = request.Price,
                IsActive = request.IsActive
            };
            
            await _businessRepository.AddServiceAsync(service);
            
            var dto = new ServiceDto(
                service.Id,
                service.Name,
                service.Description,
                service.DurationMinutes,
                service.Price,
                service.IsActive,
                service.BusinessId
            );
            
            return OperationResult<Guid>.Success(service.Id);


        }
    }
}