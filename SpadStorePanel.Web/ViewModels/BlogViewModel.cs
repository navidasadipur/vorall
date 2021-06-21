using SpadStorePanel.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpadStorePanel.Web.ViewModels
{
    public class BlogViewModel
    {
        public Article Blog { get; set; }

        public string Date { get; set; }
    }
}