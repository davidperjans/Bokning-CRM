using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Admin.DTOs
{
    public sealed class StaffDto
    {
        public Guid Id { get; init; }
        public Guid BusinessId { get; init; }
        public string Name { get; init; } = default!;
        public string Title { get; init; } = default!;
        public string? ImageUrl { get; init; }
        public string? Bio { get; init; }
        public bool IsActive { get; init; }
        public IReadOnlyCollection<Guid> QualifiedServiceIds { get; init; } = Array.Empty<Guid>();
    }
}
