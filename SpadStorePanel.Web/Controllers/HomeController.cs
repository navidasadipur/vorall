using SpadStorePanel.Core.Models;
using SpadStorePanel.Infrastructure.Repositories;
using SpadStorePanel.Infrastructure.Services;
using SpadStorePanel.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SpadStorePanel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly StaticContentDetailsRepository _staticContentDetailsRepository;

        private readonly ProductService _productService;

        private readonly ProductFeatureValuesRepository _productFeatureValuesRepository;

        private readonly ProductMainFeaturesRepository _productMainFeaturesRepository;

        private readonly ProductGroupsRepository _productGroupsRepository;

        public HomeController(StaticContentDetailsRepository staticContentDetailsRepository
        , ProductService productService
        , ProductFeatureValuesRepository productFeatureValuesRepository
        , ProductMainFeaturesRepository productMainFeaturesRepository
        , ProductGroupsRepository productGroupsRepository)
        {
            _staticContentDetailsRepository = staticContentDetailsRepository;

            _productGroupsRepository = productGroupsRepository;

            _productService = productService;

            _productFeatureValuesRepository = productFeatureValuesRepository;

            _productMainFeaturesRepository = productMainFeaturesRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult FooterSection()
        {
            SocialViewModel socialViewModel = new SocialViewModel();

            FooterViewModel model = new FooterViewModel(socialViewModel);

            model._SocialLinks.Facebook = _staticContentDetailsRepository.Get(11).Link;

            model._SocialLinks.Linkdin = _staticContentDetailsRepository.Get(15).Link;

            model._SocialLinks.GooglePlus = _staticContentDetailsRepository.Get(14).Link;

            model._SocialLinks.Pintrest = _staticContentDetailsRepository.Get(13).Link;

            model._SocialLinks.twitter = _staticContentDetailsRepository.Get(12).Link;

            model.Phone = _staticContentDetailsRepository.Get(9).ShortDescription;

            model.Email = _staticContentDetailsRepository.Get(8).ShortDescription;

            model.Address = _staticContentDetailsRepository.Get(10).ShortDescription;

            return PartialView("FooterSection", model);
        }

        public ActionResult GetNewsSection()
        {
            return PartialView("GetNewsSection");
        }

        public ActionResult NewProductSection()
        {
            List<ProductViewModel> model = new List<ProductViewModel>();

            var listProduct = _productService.GetLatestProductsWithPrice(10);

            foreach (var item in listProduct)
            {
                var feature = _productMainFeaturesRepository.GetAll().Where(x => x.ProductId == item.Id).ToList();

                model.Add(new ProductViewModel() { Product = item, ProductMainFeatures = feature });
            }



            return PartialView("NewProductSection", model);
        }

        public ActionResult PopularProductSection()
        {
            List<ProductViewModel> model = new List<ProductViewModel>();

            var listProduct = _productService.GetPopularProductsWithPrice(10);

            foreach (var item in listProduct)
            {
                var feature = _productMainFeaturesRepository.GetAll().Where(x => x.ProductId == item.Id).ToList();

                model.Add(new ProductViewModel() { Product = item, ProductMainFeatures = feature });
            }

            return PartialView("PopularProductSection", model);
        }

        public ActionResult BestSellerSection()
        {
            List<ProductViewModel> model = new List<ProductViewModel>();

            var listProduct = _productService.GetTopSoldProductsWithPrice(10);

            foreach (var item in listProduct)
            {
                var feature = _productMainFeaturesRepository.GetAll().Where(x => x.ProductId == item.Id).ToList();

                model.Add(new ProductViewModel() { Product = item, ProductMainFeatures = feature });
            }

            return PartialView("BestSellerSection", model);
        }

        public ActionResult DealSection()
        {
            List<ProductViewModel> model = new List<ProductViewModel>();

            var listProduct = _productService.GetProductsWithPrice().Where(x => x.Price > x.PriceAfterDiscount).OrderByDescending(x => x.Id).Take(10);

            foreach (var item in listProduct)
            {
                var feature = _productMainFeaturesRepository.GetAll().Where(x => x.ProductId == item.Id).ToList();

                model.Add(new ProductViewModel() { Product = item, ProductMainFeatures = feature });
            }


            return PartialView("DealSection", model);
        }


        public ActionResult SliderSection()
        {
            List<StaticContentDetail> model = new List<StaticContentDetail>()
            {
                _staticContentDetailsRepository.Get(3),
                _staticContentDetailsRepository.Get(4),
                _staticContentDetailsRepository.Get(5),
            };

            return PartialView("SliderSection", model);
        }

        public ActionResult ServiceSection()
        {
            return PartialView("ServiceSection");
        }

        public ActionResult HeaderSection()
        {
            return PartialView("HeaderSection", _productGroupsRepository.GetAll());
        }

        public ActionResult BreadCrumbSection()
        {
            return PartialView("BreadCrumbSection");
        }

        public ActionResult HeaderShareSection()
        {
            HeaderShareSectionViewModel model = new HeaderShareSectionViewModel();
            model.Email = _staticContentDetailsRepository.Get(8);
            model.Phone = _staticContentDetailsRepository.Get(9);
            return PartialView("HeaderShareSection", model);
        }

        public ActionResult CounterSection()
        {
            List<StaticContentDetail> List = new List<StaticContentDetail>()
            {
                _staticContentDetailsRepository.Get(25),

                _staticContentDetailsRepository.Get(26),

                _staticContentDetailsRepository.Get(27),
            };

            return PartialView("CounterSection", List);
        }

        public ActionResult CustomerCommentSection()
        {
            List<StaticContentDetail> List = new List<StaticContentDetail>()
            {
                _staticContentDetailsRepository.Get(31),
                _staticContentDetailsRepository.Get(32),
                _staticContentDetailsRepository.Get(33),
            };

            return PartialView("CustomerCommentSection", List);
        }

        public ActionResult AboutSection()
        {
            return PartialView("AboutSection", _staticContentDetailsRepository.Get(24));
        }

        public ActionResult EventSection()
        {
            List<StaticContentDetail> List = new List<StaticContentDetail>()
            {
                _staticContentDetailsRepository.Get(28),

                _staticContentDetailsRepository.Get(29),

                _staticContentDetailsRepository.Get(30),
            };

            return PartialView("EventSection", List);

        }

        public ActionResult Error404()
        {
            return View();
        }

        //public ActionResult CartSection()
        //{
        //    return PartialView("CartSection");
        //}

        public ActionResult privacy()
        {
            return View(_staticContentDetailsRepository.Get(21));
        }

        public ActionResult Law()
        {
            return View(_staticContentDetailsRepository.Get(23));
        }

        public ActionResult Recovery()
        {
            return View(_staticContentDetailsRepository.Get(20));
        }

        public ActionResult SendingRule()
        {
            return View(_staticContentDetailsRepository.Get(19));
        }

        public ActionResult LoginSection()
        {
            return PartialView("LoginSection");
        }

        public ActionResult RegisterSection()
        {
            return PartialView("RegisterSection");
        }


    }
}