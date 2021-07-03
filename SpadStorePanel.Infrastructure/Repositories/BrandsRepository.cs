using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpadStorePanel.Core.Models;

namespace SpadStorePanel.Infrastructure.Repositories
{
    public class BrandsRepository : BaseRepository<Brand, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public BrandsRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Brand> GetAllGroupBrands(int groupId)
        {
            var pgBrands = _context.ProductGroupBrands.Where(f => f.ProductGroupId == groupId).ToList();
            var brands = pgBrands.Select(item => _context.Brands.Find(item.BrandId)).ToList();
            return brands;
        }

        public Brand GetBrand(int id)
        {
            return _context.Brands.Where(b => b.IsDeleted == false && b.Id == id).FirstOrDefault();
        }
    }
}
