using Application.Common;
using Application.Interface;
using MediatR;

namespace Application.Businesses.Commands
{
    public class UpdateBusinessStatusHandler : IRequestHandler<UpdateBusinessStatusCommand, OperationResult<bool>>

    {
        private readonly IBusinessRepository _repository;
        
        public UpdateBusinessStatusHandler(IBusinessRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<bool>> Handle(UpdateBusinessStatusCommand request,
            CancellationToken cancellationToken)
        {
            
            var business = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (business == null)
            {
                return OperationResult<bool>.Failure("Business not found.");
            }

            business.Status = request.NewStatus;
            business.UpdatedAt = DateTime.UtcNow;
            
            await _repository.UpdateAsync(business, cancellationToken);

            return OperationResult<bool>.Success(true);
        }
    }
}