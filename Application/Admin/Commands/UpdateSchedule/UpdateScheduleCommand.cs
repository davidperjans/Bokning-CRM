using Application.Admin.DTOs;
using Application.Common;
using MediatR;

namespace Application.Admin.Commands.UpdateSchedule
{
    public record UpdateStaffScheduleCommand(
        Guid StaffId,
        List<WorkingHourDto> Days
    ) : IRequest<OperationResult<List<WorkingHourDto>>>;
}