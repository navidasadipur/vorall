using SpadStorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpadStorePanel.Web.ViewModels
{
    public class BlogDetailViewModel
    {
        public Article Blog { get; set; }

        public List<ArticleComment> Comments { get; set; }

        public List<ArticleTag> Tags { get; set; }
    }
}