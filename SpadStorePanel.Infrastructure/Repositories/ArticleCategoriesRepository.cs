using SpadStorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpadStorePanel.Infrastructure.Repositories
{
    public class ArticleCategoriesRepository : BaseRepository<ArticleCategory, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public ArticleCategoriesRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<ArticleCategory> GetAllCategoriesWithArticles()
        {
            var allCategories = _context.ArticleCategories.Where(c => c.IsDeleted == false).OrderByDescending(c => c.InsertDate).ToList();

            foreach (var category in allCategories)
            {
                category.Articles = _context.Articles.Where(p => p.IsDeleted == false && p.ArticleCategoryId == category.Id).ToList();
            }

            return allCategories;
        }
    }
}
