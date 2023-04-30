using ShopAdmin.Data;
using ShopAdmin.Models;

namespace ShopAdmin.Helpers
{
    public class VisitCountService
    {
        private readonly ProductDbContext _context;

        public VisitCountService(ProductDbContext context)
        {
            _context = context;
        }

        public int GetVisitCount()
        {
            var count = _context.VisitCounts.FirstOrDefault();
            if (count == null)
            {
                count = new VisitCounts();
                _context.VisitCounts.Add(count);
            }
            count.VisitCount++;
            _context.SaveChanges();
            return count.VisitCount;
        }
    }
}
