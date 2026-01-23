using Application.Admin.DTOs;
using Application.Common;
using Application.Interface;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Admin.Commands.CreateStaff
{
    public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, OperationResult<StaffDto>>
    {
        private readonly IStaffRepository _staffRepo;
        private readonly IServiceRepository _serivceRepo;

        public CreateStaffCommandHandler(IStaffRepository staffRepo, IServiceRepository serviceRepo)
        {
            _staffRepo = staffRepo;
            _serivceRepo = serviceRepo;
        }

        public async Task<OperationResult<StaffDto>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {
            var serviceIds = request.QualifiedServiceIds ?? Array.Empty<Guid>();
            var services = serviceIds.Count == 0
                ? new List<Service>()
                : await _serivceRepo.GetByIdsAsync(serviceIds, cancellationToken);

            if (serviceIds.Count > 0 && services.Count != serviceIds.Count)
            {
                var missing = serviceIds.Except(services.Select(s => s.Id)).ToArray();
                return OperationResult<StaffDto>.Failure($"One or more services do not exist: {string.Join(", ", missing)}");
            }

            var staff = new Staff
            {
                Id = Guid.NewGuid(),
                BusinessId = request.BusinessId,
                Name = request.Name.Trim(),
                Title = request.Title.Trim(),
                ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim(),
                Bio = string.IsNullOrWhiteSpace(request.Bio) ? null : request.Bio.Trim(),
                IsActive = request.IsActive,
                QualifiedServices = services // many-to-many
            };

            await _staffRepo.AddAsync(staff, cancellationToken);

            // 5) Return DTO
            var dto = new StaffDto
            {
                Id = staff.Id,
                BusinessId = staff.BusinessId,
                Name = staff.Name,
                Title = staff.Title,
                ImageUrl = staff.ImageUrl,
                Bio = staff.Bio,
                IsActive = staff.IsActive,
                QualifiedServiceIds = staff.QualifiedServices.Select(s => s.Id).ToArray()
            };

            return OperationResult<StaffDto>.Success(dto);
        }
    }
}
