using SpadStorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpadStorePanel.Web.ViewModels
{
    public class CategorySectionViewModel
    {
        public List<ArticleCategory> ArticleCategories { get; set; }

        public List<int> Count { get; set; }
    }
}