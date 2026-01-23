using Application.Admin.DTOs;
using Application.Common;
using Application.Interface;
using Domain.Models;
using MediatR;

namespace Application.Admin.Commands.UpdateSchedule
{
    public class UpdateStaffScheduleHandler : IRequestHandler<UpdateStaffScheduleCommand, OperationResult<List<WorkingHourDto>>>
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUserContext _userContext;

        public UpdateStaffScheduleHandler(IStaffRepository staffRepository, IBusinessRepository businessRepository, IUserContext userContext)
        {
            _staffRepository = staffRepository;
            _businessRepository = businessRepository;
            _userContext = userContext;
        }

        public async Task<OperationResult<List<WorkingHourDto>>> Handle(UpdateStaffScheduleCommand request, CancellationToken ct)
        {
            // 1. Fetch staff and include current working hours
            var staff = await _staffRepository.GetByIdAsync(request.StaffId, ct);
            if (staff == null) 
                return OperationResult<List<WorkingHourDto>>.Failure("Staff member not found.");
            
            // 2. Authorization check via Business ownership
            var business = await _businessRepository.GetByIdAsync(staff.BusinessId, ct);
            bool isOwner = business?.OwnerId == _userContext.UserId;
            bool isSuperAdmin = _userContext.Role == "SuperAdmin";

            if (!isOwner && !isSuperAdmin) 
                return OperationResult<List<WorkingHourDto>>.Failure("Access denied.");

            // 3. Map DTOs to Domain Models and update
            // We create a new list to pass to the repository
            var newWorkingHours = request.Days.Select(day => new WorkingHour
            {
                Id = Guid.NewGuid(),
                StaffId = staff.Id,
                DayOfWeek = day.DayOfWeek,
                StartTime = TimeSpan.Parse(day.StartTime ?? "08:00"),
                EndTime = TimeSpan.Parse(day.EndTime ?? "17:00"),
                IsClosed = day.IsClosed
            }).ToList();

            // 4. Persist changes
            await _staffRepository.UpdateScheduleAsync(staff.Id, newWorkingHours, ct);

            // 5. Return the updated list as a confirmation to the frontend
            return OperationResult<List<WorkingHourDto>>.Success(request.Days);
        }
    }
}