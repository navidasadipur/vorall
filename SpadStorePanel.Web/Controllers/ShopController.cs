using SpadStorePanel.Core.Models;
using SpadStorePanel.Infrastructure.Dtos.Product;
using SpadStorePanel.Infrastructure.Repositories;
using SpadStorePanel.Infrastructure.Services;
using SpadStorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpadStorePanel.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly ProductService _productService;

        private readonly ProductsRepository _productRepository;

        private readonly ProductGroupsRepository _productGroupsRepository;

        private readonly ProductFeatureValuesRepository _productFeatureValuesRepository;

        private readonly ProductMainFeaturesRepository _productMainFeaturesRepository;

        private readonly ProductGalleriesRepository _productGalleriesRepository;

        private readonly ProductCommentsRepository _productCommentsRepository;

        private readonly StaticContentDetailsRepository _staticContentDetailsRepository;

        public ShopController(ProductService productService, ProductsRepository productsRepository, ProductMainFeaturesRepository productMainFeaturesRepository
        , ProductFeatureValuesRepository productFeatureValuesRepository, ProductGalleriesRepository productGalleriesRepository
        , ProductCommentsRepository productCommentsRepository
        , StaticContentDetailsRepository staticContentDetailsRepository
        , ProductGroupsRepository productGroupsRepository)
        {
            _staticContentDetailsRepository = staticContentDetailsRepository;

            _productService = productService;

            _productCommentsRepository = productCommentsRepository;

            _productMainFeaturesRepository = productMainFeaturesRepository;

            _productRepository = productsRepository;

            _productGalleriesRepository = productGalleriesRepository;

            _productFeatureValuesRepository = productFeatureValuesRepository;

            _productGroupsRepository = productGroupsRepository;
        }

        // GET: Shop
        public ActionResult Index(int pageNumber = 1, long minPrice = -1, long maxPrice = -1)
        {
            //var model = _productService.GetProductsWithPrice().OrderByDescending(x => x.Id).Skip((page - 1) * 10).Take(10).ToList();

            //float d = _productService.GetProductsWithPrice().Count() / 10f;

            ////ViewBag.PageCount = (int)Math.Ceiling(d);
            ///

            var products = _productService.GetProductsWithPrice().OrderByDescending(x => x.Id).ToList();

            //if minPrice or maxPrice does not specified
            if (minPrice == -1)
            {
                minPrice = _productMainFeaturesRepository.GetMinPrice();
            }
            if (maxPrice == -1)
            {
                maxPrice = _productMainFeaturesRepository.GetMaxPrice();
            }

            targetProductsPriceFilted = FilteringByPrice(model.MinPrice, model.MaxPrice, allSearchedTargetProducts);

            allTargetProducts = targetProductsPriceFilted;

            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            

            //take number of products in every page
            var take = 9;

            var count = products.Count;
            var skip = pageNumber * take - take;
            int pageCount = (int)Math.Ceiling((double)count / take);
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageNumber;

            products = products.Skip(skip).Take(take).ToList();

            return View(products);
        }

        public ActionResult Detail(int id)
        {
            var product = _productRepository.GetProduct(id);
            var productGallery = _productGalleriesRepository.GetProductGalleries(id);
            var productMainFeatures = _productMainFeaturesRepository.GetProductMainFeatures(id);
            var productFeatureValues = _productFeatureValuesRepository.GetProductFeatures(id);
            var price = _productService.GetProductPrice(product);
            var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
            var Productcomments = _productCommentsRepository.GetProductComments(id);


            var vm = new ProductDetailViewModel()
            {
                Product = product,
                ProductGalleries = productGallery,
                ProductMainFeatures = productMainFeatures,
                ProductFeatureValues = productFeatureValues,
                Price = price,
                Comments = Productcomments,
                PriceAfterDiscount = priceAfterDiscount,
                DiscountPercentage = (int)(priceAfterDiscount * 100 / price)
            };
            return View(vm);
        }

        public ActionResult CategorySection()
        {
            return PartialView("CategorySection", _productGroupsRepository.GetAllGroupsWithProducts());
        }

        public ActionResult ColorsSection()
        {
            return PartialView("ColorsSection");
        }

        public ActionResult NewProductSection()
        {
            var model = _productService.GetProductsWithPrice().OrderByDescending(x => x.Id).Take(3).ToList();

            return PartialView("NewProductSection", model);
        }

        public ActionResult PriceSection()
        {
            return PartialView("PriceSection");
        }

        public ActionResult SocialSection()
        {
            SocialViewModel model = new SocialViewModel();

            model.Facebook = _staticContentDetailsRepository.Get(11).Link;

            model.Linkdin = _staticContentDetailsRepository.Get(15).Link;

            model.GooglePlus = _staticContentDetailsRepository.Get(14).Link;

            model.Pintrest = _staticContentDetailsRepository.Get(13).Link;

            model.twitter = _staticContentDetailsRepository.Get(12).Link;

            return PartialView("SocialSection", model);
        }

        [HttpPost]
        public string SendComment(ProductComment comment)
        {
            try
            {
                _productCommentsRepository.Add(comment);

                return "success";
            }
            catch
            {
                return "fail";
            }
        }

        public ActionResult CartSection()
        {
            return PartialView("CartSectionShare");
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            var model = _productService.GetProductsWithPrice().OrderByDescending(x => x.Id).Where(x => x.ShortTitle.Contains(search)).ToList();

            float d = _productService.GetProductsWithPrice().Count() / 10f;

            ViewBag.PageCount = (int)Math.Ceiling(d);

            return View("Index", model);
        }

        public ActionResult GetByCategory(int id)
        {
            var model = _productService.GetProductsWithPrice().Where(x => x.ProductGroupId == id).ToList();
            ViewBag.PageCount = 0;
            return View("Index", model);
        }

        public ActionResult SetPrice(int min, int Max)
        {
            var model = _productService.GetProductsWithPrice().Where(x => x.Price < Max*1000 && x.Price > min*1000).ToList();
            ViewBag.PageCount = 0;
            return View("Index", model);
        }

        public ActionResult Compare(string CompareList)
        {
            var productsId = CompareList.Split('-');
            List<ProductDetailViewModel> Model = new List<ProductDetailViewModel>();

            foreach (var item in productsId)
            {
                var product = _productRepository.GetProduct(Convert.ToInt32(item));
                var productGallery = _productGalleriesRepository.GetProductGalleries(Convert.ToInt32(item));
                var productMainFeatures = _productMainFeaturesRepository.GetProductMainFeatures(Convert.ToInt32(item));
                var productFeatureValues = _productFeatureValuesRepository.GetProductFeatures(Convert.ToInt32(item));
                var price = _productService.GetProductPrice(product);
                var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
                var Productcomments = _productCommentsRepository.GetProductComments(Convert.ToInt32(item));


                var vm = new ProductDetailViewModel()
                {
                    Product = product,
                    ProductGalleries = productGallery,
                    ProductMainFeatures = productMainFeatures,
                    ProductFeatureValues = productFeatureValues,
                    Price = price,
                    Comments = Productcomments,
                    PriceAfterDiscount = priceAfterDiscount,
                    DiscountPercentage = (int)(priceAfterDiscount * 100 / price)
                };

                Model.Add(vm);
            }

            return View(Model);
        }

        public ActionResult PopupData(int id)
        {
            var product = _productRepository.GetProduct(id);
            var productGallery = _productGalleriesRepository.GetProductGalleries(id);
            var productMainFeatures = _productMainFeaturesRepository.GetProductMainFeatures(id);
            var productFeatureValues = _productFeatureValuesRepository.GetProductFeatures(id);
            var price = _productService.GetProductPrice(product);
            var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
            var Productcomments = _productCommentsRepository.GetProductComments(id);

            var vm = new ProductDetailViewModel()
            {
                Product = product,
                ProductGalleries = productGallery,
                ProductMainFeatures = productMainFeatures,
                ProductFeatureValues = productFeatureValues,
                Price = price,
                Comments = Productcomments,
                PriceAfterDiscount = priceAfterDiscount,
                DiscountPercentage = (int)(priceAfterDiscount * 100 / price)
            };
            return PartialView("PopupSection", vm);
        }

        public ActionResult PopupSection()
        {
            var product = new Product() 
            {Id=10000,
            Title="test",
            BrandId=2,
            ProductGroupId=15,
            Rate=1,
            InsertUser="test",
            InsertDate=null,
            IsDeleted=true,
            };
            //var productGallery = _productGalleriesRepository.GetProductGalleries(product.Id);
            //var productMainFeatures = _productMainFeaturesRepository.GetProductMainFeatures(product.Id);
            //var productFeatureValues = _productFeatureValuesRepository.GetProductFeatures(product.Id);
            //var price = _productService.GetProductPrice(product);
            //var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product);
            //var Productcomments = _productCommentsRepository.GetProductComments(product.Id);


            var vm = new ProductDetailViewModel()
            {
                Product = product,
                ProductGalleries = null,
                ProductMainFeatures = null,
                ProductFeatureValues = null,
                Price = 1,
                Comments = null,
                PriceAfterDiscount = 1,
                DiscountPercentage =1
            };

            return PartialView("PopupSection",vm);
        }

        public ActionResult Cart()
        {
            ViewBag.BanerImage = _staticContentDetailsRepository.GetStaticContentDetail(13).Image;

            return View();
        }
        public ActionResult CartTable()
        {
            var cartModel = new CartModel();

            HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

            if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
            {
                string cartJsonStr = cartCookie.Values["cart"];
                cartModel = new CartModel(cartJsonStr);
            }

            return PartialView(cartModel);
        }

        #region Helpers
        private List<Product> FilteringByPrice(int minFilterPrice, int maxFilterPrice, List<Product> allTargetProducts)
        {
            var targetProducts = new List<Product>();

            foreach (var product in allTargetProducts)
            {
                product.ProductMainFeatures = new List<ProductMainFeature>();

                product.ProductMainFeatures = (_productMainFeaturesRepository.GetProductMainFeatures(product.Id));

                var targetProductId = product.ProductMainFeatures.Where(pmf => pmf.Price >= minFilterPrice && pmf.Price <= maxFilterPrice).Select(pmf => pmf.ProductId).FirstOrDefault();

                if (targetProductId != 0)
                {
                    targetProducts.Add(_productRepository.GetProduct(targetProductId));
                }
            }

            return targetProducts;
        }
        #endregion
    }
}
