using Application.Businesses.DTOs;
using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Businesses.Queries.GetDetailsBySlug
{
    public sealed record GetDetailsBySlugQuery(
        string Slug
    ) : IRequest<OperationResult<BusinessDetailsDto>>;
}
