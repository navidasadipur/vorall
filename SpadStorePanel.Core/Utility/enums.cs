using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpadStorePanel.Core.Utility
{
    public enum DiscountType
    {
        Percentage = 1,
        Amount = 2
    }
    public enum GeoDivisionType
    {
        Country = 0,
        State = 1,
        City = 2,
    }
    public enum StaticContents
    {
        AboutDescription = 15,
        firstImageAboutPage = 16,
        WorkingHours = 1008,
        Youtube = 29,
        Instagram = 28,
        Twitter = 27,
        Pinterest = 30,
        Facebook = 26,
        linkedin = 33,
        BlogImage = 1013,
        ContactUsBackImageAndContent = 2,
        companyServices = 3003,
        CopyRight = 3004,
        ImplementaitonService = 3005,

        Address = 10,
        Email = 8,
        Phone = 9,
        Map = 7,

        DiscountNews = 5027,

        Logo = 14,
        //BackGroundImage = 2,
        //NewsBackImage = 8,

        BlogAd = 32,
    }

    public enum StaticContentTypes
    {
        HeaderFooter = 9,
        About = 13,
        AboutProperties,

        HomeTopSlider = 17,
        Contact = 2,

        Guide = 9,
        Popup = 11,
        PageBanner = 12,
        OurServices = 3,
    }
}
