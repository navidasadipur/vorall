using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpadStorePanel.Web.ViewModels
{
    public class FooterViewModel
    {
        public FooterViewModel(SocialViewModel socialViewModel)
        {
            _SocialLinks = socialViewModel;   
        }
        public SocialViewModel _SocialLinks { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}