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
        private readonly ProductsRepository _productsRepository;
        private readonly ProductGroupsRepository _productGroupsRepository;
        private readonly BrandsRepository _brandsRepository;
        private readonly FeaturesRepository _featuresRepository;
        private readonly ProductFeatureValuesRepository _productFeatureValuesRepository;
        private readonly ProductMainFeaturesRepository _productMainFeaturesRepository;
        private readonly ProductGalleriesRepository _productGalleriesRepository;
        private readonly ProductCommentsRepository _productCommentsRepository;
        private readonly StaticContentDetailsRepository _staticContentDetailsRepository;

        public ShopController
            (
            ProductService productService
            , ProductsRepository productsRepository
            , ProductMainFeaturesRepository productMainFeaturesRepository
            , ProductFeatureValuesRepository productFeatureValuesRepository
            , ProductGalleriesRepository productGalleriesRepository
            , ProductCommentsRepository productCommentsRepository
            , StaticContentDetailsRepository staticContentDetailsRepository
            , ProductGroupsRepository productGroupsRepository
            , BrandsRepository brandsRepository
            , FeaturesRepository featuresRepository
            )
        {
            _staticContentDetailsRepository = staticContentDetailsRepository;
            _productService = productService;
            _productCommentsRepository = productCommentsRepository;
            _productMainFeaturesRepository = productMainFeaturesRepository;
            _productsRepository = productsRepository;
            _productGalleriesRepository = productGalleriesRepository;
            _productFeatureValuesRepository = productFeatureValuesRepository;
            _productGroupsRepository = productGroupsRepository;
            _brandsRepository = brandsRepository;
            _featuresRepository = featuresRepository;
        }

        // GET: Shop
        public ActionResult Index(int? id, string searchString = null, string groupIds = null, string productIds = null, string brandIds = null)
        {
            //var model = _productService.GetProductsWithPrice().OrderByDescending(x => x.Id).Skip((page - 1) * 10).Take(10).ToList();

            //float d = _productService.GetProductsWithPrice().Count() / 10f;

            //ViewBag.PageCount = (int)Math.Ceiling(d);

            //return View(model);

            var vm = new ProductListViewModel();
            vm.SelectedGroupId = id ?? 0;
            var productGroups = new List<ProductGroup>();
            var banner = "";
            if (id == null)
            {
                vm.Features = _featuresRepository.GetAllFeatures();
                vm.Brands = _brandsRepository.GetAll();

                var childrenGroups = _productGroupsRepository.GetChildrenProductGroups();
                vm.ProductGroups = childrenGroups;
                ViewBag.Title = "محصولات";
                //try
                //{
                //    banner = _staticContentDetailsRepository.GetSingleContentDetailByTitle("سربرگ محصولات").Image;
                //    banner = "/Files/StaticContentImages/Image/" + banner;
                //}
                //catch
                //{

                //}
            }
            else
            {

                vm.Features = _featuresRepository.GetAllGroupFeatures(id.Value);
                vm.Brands = _brandsRepository.GetAllGroupBrands(id.Value);
                var selectedProductGroup = _productGroupsRepository.Get(id.Value);
                var childrenGroups = _productGroupsRepository.GetChildrenProductGroups(id.Value);

                vm.ProductGroups = childrenGroups;
                ViewBag.ProductGroupName = selectedProductGroup.Title;
                ViewBag.ProductGroupId = selectedProductGroup.Id;
                ViewBag.Title = $"محصولات {selectedProductGroup.Title}";

            }

            ViewBag.MinPrice = _productMainFeaturesRepository.GetMinPrice();
            ViewBag.MaxPrice = _productMainFeaturesRepository.GetMaxPrice();

            if (searchString != null)
                ViewBag.SearchString = searchString;

            if (groupIds != null)
                ViewBag.GroupIds = groupIds;

            if (productIds != null)
                ViewBag.ProductIds = productIds;

            if (brandIds != null)
                ViewBag.BrandIds = brandIds;

            ViewBag.SearchString = searchString;

            ViewBag.Banner = banner;

            ViewBag.BanerImage = _staticContentDetailsRepository.GetStaticContentDetail(13).Image;

            return View(vm);
        }

        //[Route("ProductsGrid")]
        public ActionResult ProductsGrid(GridViewModel grid)
        {
            var products = new List<Product>();

            var brandsIntArr = new List<int>();

            if (string.IsNullOrEmpty(grid.brands) == false)
            {
                var brandsArr = grid.brands.Split('-').ToList();
                brandsArr.ForEach(b => brandsIntArr.Add(Convert.ToInt32(b)));
            }

            var subFeaturesIntArr = new List<int>();
            if (string.IsNullOrEmpty(grid.subFeatures) == false)
            {
                var subFeaturesArr = grid.subFeatures.Split('-').ToList();
                subFeaturesArr.ForEach(b => subFeaturesIntArr.Add(Convert.ToInt32(b)));
            }

            products = _productService.GetProductsGrid(grid.categoryId, brandsIntArr, subFeaturesIntArr, grid.priceFrom, grid.priceTo, grid.searchString);

            #region Get Products Base on Group, Brand and Products of "offer"

            var allSearchedTargetProducts = new List<Product>();

            if (grid.GroupIds != null || grid.ProductIds != null || grid.BrandIds != null)
            {
                //search based on multiple group ids
                if (string.IsNullOrEmpty(grid.GroupIds) == false)
                {
                    var groupIdsIntArr = new List<int>();

                    var groupIdsArr = grid.GroupIds.Split('-').ToList();
                    groupIdsArr.ForEach(g => groupIdsIntArr.Add(Convert.ToInt32(g)));

                    var allTargetGroups = new List<ProductGroup>();

                    foreach (var id in groupIdsIntArr)
                    {
                        var group = _productGroupsRepository.GetProductGroup(id);

                        allTargetGroups.Add(group);
                    }

                    foreach (var group in allTargetGroups)
                    {
                        if (group != null)
                        {
                            var allProductsOfOneGroup = _productsRepository.getProductsByGroupId(group.Id);

                            foreach (var product in allProductsOfOneGroup)
                            {
                                allSearchedTargetProducts.Add(product);
                            }
                        }
                    }
                }

                //search based on multiple brand ids
                if (string.IsNullOrEmpty(grid.BrandIds) == false)
                {
                    var brandIdsIntArr = new List<int>();

                    var brandIdsArr = grid.BrandIds.Split('-').ToList();
                    brandIdsArr.ForEach(b => brandIdsIntArr.Add(Convert.ToInt32(b)));

                    var allTargetBrands = new List<Brand>();

                    foreach (var id in brandIdsIntArr)
                    {
                        var brand = _brandsRepository.GetBrand(id);

                        allTargetBrands.Add(brand);
                    }

                    foreach (var brand in allTargetBrands)
                    {
                        if (brand != null)
                        {
                            var allProductsOfOneBrand = _productsRepository.getProductsByBrandId(brand.Id);

                            foreach (var product in allProductsOfOneBrand)
                            {
                                allSearchedTargetProducts.Add(product);
                            }
                        }
                    }
                }

                //search based on multiple product ids
                if (string.IsNullOrEmpty(grid.ProductIds) == false)
                {
                    var productIdsIntArr = new List<int>();

                    var productIdsArr = grid.ProductIds.Split('-').ToList();
                    productIdsArr.ForEach(b => productIdsIntArr.Add(Convert.ToInt32(b)));

                    foreach (var id in productIdsIntArr)
                    {
                        var product = _productsRepository.GetProduct(id);

                        //if product not found in allSearchedTargetProducts
                        if (!allSearchedTargetProducts.Contains(product))
                        {
                            allSearchedTargetProducts.Add(product);
                        }
                    }
                }

                products = allSearchedTargetProducts;
            }

            #endregion

            #region Sorting

            if (grid.sort != "date")
            {
                switch (grid.sort)
                {
                    case "name":
                        products = products.OrderBy(p => p.Title).ToList();
                        break;
                    case "sale":
                        products = products.OrderByDescending(p => _productService.GetProductSoldCount(p)).ToList();
                        break;
                    case "price-high-to-low":
                        products = products.OrderByDescending(p => _productService.GetProductPriceAfterDiscount(p)).ToList();
                        break;
                    case "price-low-to-high":
                        products = products.OrderBy(p => _productService.GetProductPriceAfterDiscount(p)).ToList();
                        break;
                }
            }
            #endregion

            var count = products.Count;
            var skip = grid.pageNumber * grid.take - grid.take;
            int pageCount = (int)Math.Ceiling((double)count / grid.take);
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = grid.pageNumber;

            products = products.Skip(skip).Take(grid.take).ToList();

            var vm = new List<ProductWithPriceDto>();
            foreach (var product in products)
                vm.Add(_productService.CreateProductWithPriceDto(product));

            return PartialView(vm);
        }

        public ActionResult Detail(int id)
        {
            var product = _productsRepository.GetProduct(id);
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

        public string GetProductPrice(int productId, int mainFeatureId)
        {
            var product = _productsRepository.Get(productId);
            var price = _productService.GetProductPrice(product, mainFeatureId);
            var priceAfterDiscount = _productService.GetProductPriceAfterDiscount(product, mainFeatureId);
            var result = new
            {
                price = price.ToString("##,###"),
                priceAfterDiscount = priceAfterDiscount.ToString("##,###")
            };
            var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return jsonStr;
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
                var product = _productsRepository.GetProduct(Convert.ToInt32(item));
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
            var product = _productsRepository.GetProduct(id);
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
    }
}
