using Application.Interface;
using Domain.Models;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingTypeRepository : IBookingTypeRepository
    {
        private readonly AppDbContext _context;

        public BookingTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookingType?> GetByIdAsync(Guid businessId, Guid bookingTypeId, CancellationToken ct)
        {
            // Hämtar booking type inom rätt business + ignorerar soft-deletade (om query filter finns)
            return await _context.BookingTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(bt =>
                    bt.Id == bookingTypeId &&
                    bt.BusinessId == businessId,
                    ct);
        }
    }
}
