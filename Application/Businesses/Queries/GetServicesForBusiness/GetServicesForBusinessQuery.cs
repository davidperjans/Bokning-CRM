using Application.Businesses.DTOs;
using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.Queries.GetServicesForBusiness
{
    public sealed record GetServicesForBusinessQuery(    
        string Slug
    ) : IRequest<OperationResult<BusinessWithServicesDto>>;
}
