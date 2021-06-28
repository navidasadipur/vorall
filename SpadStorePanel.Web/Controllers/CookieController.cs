using Newtonsoft.Json;
using SpadStorePanel.Core.Models;
using SpadStorePanel.Core.Utility;
using SpadStorePanel.Infrastructure.Dtos.Product;
using SpadStorePanel.Infrastructure.Repositories;
using SpadStorePanel.Infrastructure.Services;
using SpadStorePanel.Web.Providers;
using SpadStorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpadStorePanel.Web.Controllers
{
    public class CookieController : Controller
    {
        private readonly ProductService _productService;

        private readonly ProductMainFeaturesRepository _productMainFeaturesRepo;

        private readonly ProductsRepository _productsRepository;
        private readonly CustomersRepository _customerRepo;
        private readonly InvoicesRepository _invoicesRepository;
        private readonly StaticContentDetailsRepository _staticContentRepo;
        private readonly ShoppingRepository _shoppingRepo;
        private readonly GeoDivisionsRepository _geoDivisionRepo;

        public CookieController(
              ProductService productService
            , ProductMainFeaturesRepository productMainFeaturesRepository
            , ProductsRepository productsRepository
            , CustomersRepository customerRepo
            , InvoicesRepository invoicesRepository
            , StaticContentDetailsRepository staticContentDetailsRepository
            , ShoppingRepository shoppingRepo
            , GeoDivisionsRepository geoDivisionRepo
            )
        {
            _productService = productService;

            _productMainFeaturesRepo = productMainFeaturesRepository;

            _productsRepository = productsRepository;
            this._customerRepo = customerRepo;
            this._invoicesRepository = invoicesRepository;
            this._staticContentRepo = staticContentDetailsRepository;
            this._shoppingRepo = shoppingRepo;
            this._geoDivisionRepo = geoDivisionRepo;
        }

        [HttpPost]
        public void AddToCart(int productId, int? mainFeatureId)
        {
            try
            {
                var cartModel = new CartModel();
                var cartItemsModel = new List<CartItemModel>();

                #region Checking for cookie
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                {
                    string cartJsonStr = cartCookie.Values["cart"];
                    cartModel = new CartModel(cartJsonStr);
                    cartItemsModel = cartModel.CartItems;
                }
                #endregion

                ProductWithPriceDto product;
                int productStockCount;
                if (mainFeatureId == null)
                {
                    mainFeatureId = _productMainFeaturesRepo.GetByProductId(productId).Id;
                }
                product = _productService.CreateProductWithPriceDto(productId, mainFeatureId.Value);
                productStockCount = _productService.GetProductStockCount(productId, mainFeatureId.Value);

                if (productStockCount > 0)
                {
                    if (cartItemsModel.Any(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value))
                    {
                        if (cartItemsModel.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value).Quantity < productStockCount)
                        {
                            cartItemsModel.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId.Value).Quantity += 1;
                            cartModel.TotalPrice += product.PriceAfterDiscount;
                        }
                    }
                    else
                    {
                        cartItemsModel.Add(new CartItemModel()
                        {
                            Id = product.Id,
                            ProductName = product.ShortTitle,
                            Price = product.PriceAfterDiscount,
                            Quantity = 1,
                            MainFeatureId = mainFeatureId.Value,
                            Image = product.Image
                        });
                        cartModel.TotalPrice += product.PriceAfterDiscount;
                    }
                    cartModel.CartItems = cartItemsModel;
                    var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);
                    cartCookie.Values.Set("cart", jsonStr);

                    cartCookie.Expires = DateTime.Now.AddHours(12);
                    cartCookie.SameSite = SameSiteMode.Lax;
                    Response.Cookies.Add(cartCookie);
                }
            }
            catch (Exception e)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                cartCookie.Values.Set("cart", "");

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }
        }

        public ActionResult CartSection()
       {
            try
            {
                var cartModel = new CartModel();
                cartModel.CartItems = new List<CartItemModel>();

                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                {
                    string cartJsonStr = cartCookie.Values["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }
                return PartialView(cartModel);

            }
            catch (Exception e)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                cartCookie.Values.Set("cart", "");

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;


                var cartModel = new CartModel();
                cartModel.CartItems = new List<CartItemModel>();

                if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                {
                    string cartJsonStr = cartCookie.Values["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }
                return PartialView(cartModel);

            }
        }

        [HttpPost]
        public void RemoveFromCart(int productId, int? mainFeatureId, string complete = null)
        {
            try
            {
                var cartModel = new CartModel();

                #region Checking for cookie
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                {
                    string cartJsonStr = cartCookie.Values["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }
                #endregion

                if (cartModel.CartItems.Any(i => i.Id == productId && i.MainFeatureId == mainFeatureId))
                {
                    var itemToRemove = cartModel.CartItems.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId);
                    if (complete == "true" || itemToRemove.Quantity < 2)
                    {
                        cartModel.TotalPrice -= itemToRemove.Price * itemToRemove.Quantity;
                        cartModel.CartItems.Remove(itemToRemove);
                    }
                    else if (complete == "false")
                    {
                        cartModel.TotalPrice -= itemToRemove.Price;
                        cartModel.CartItems.FirstOrDefault(i => i.Id == productId && i.MainFeatureId == mainFeatureId).Quantity -= 1;
                    }
                }
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);
                cartCookie.Values.Set("cart", jsonStr);
                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }
            catch (Exception e)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                cartCookie.Values.Set("cart", "");

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }
        }


        public void AddToWishList(int productId)
        {
            try
            {
                var withListModel = new WishListModel();
                var withListItemsModel = new List<WishListItemModel>();

                #region Checking for cookie
                HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

                if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
                {
                    string cartJsonStr = cartCookie.Values["wishList"];
                    withListModel = new WishListModel(cartJsonStr);
                    withListItemsModel = withListModel.WishListItems;
                }
                #endregion

                var product = _productsRepository.Get(productId);
                if (withListItemsModel.Any(i => i.Id == productId) == false)
                {
                    withListItemsModel.Add(new WishListItemModel()
                    {
                        Id = product.Id,
                        ProductName = product.Title,
                        Image = product.Image
                    });
                }
                withListModel.WishListItems = withListItemsModel;
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(withListModel);
                cartCookie.Values.Set("wishList", jsonStr);

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }
            catch (Exception e)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                cartCookie.Values.Set("wishList", "");

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }
        }

        public ActionResult WishList()
        {
            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View();
        }

        public string WishListTable()
        {
            var wishListModel = new WishListModel();

            try
            {
                HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

                if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
                {
                    string cartJsonStr = cartCookie.Values["wishList"];
                    wishListModel = new WishListModel(cartJsonStr);
                }

            }
            catch (Exception e)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                cartCookie.Values.Set("wishList", "");

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }

            
            return JsonConvert.SerializeObject(wishListModel);
        }

        public ActionResult WishListPage()
        {
            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View();
        }

        public ActionResult WishListTablePage()
        {
            var wishListModel = new WishListModel();

            HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

            if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
            {
                string cartJsonStr = cartCookie.Values["wishList"];
                wishListModel = new WishListModel(cartJsonStr);
            }

            return PartialView(wishListModel);
        }

        [HttpPost]
        public void RemoveFromWishList(int productId)
        {
            try
            {
                var withListModel = new WishListModel();

                #region Checking for cookie
                HttpCookie cartCookie = Request.Cookies["wishList"] ?? new HttpCookie("wishList");

                if (!string.IsNullOrEmpty(cartCookie.Values["wishList"]))
                {
                    string cartJsonStr = cartCookie.Values["wishList"];
                    withListModel = new WishListModel(cartJsonStr);
                }
                #endregion

                if (withListModel.WishListItems.Any(i => i.Id == productId))
                {
                    var itemToRemove = withListModel.WishListItems.FirstOrDefault(i => i.Id == productId);
                    withListModel.WishListItems.Remove(itemToRemove);
                }
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(withListModel);
                cartCookie.Values.Set("wishList", jsonStr);
                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }
            catch (Exception e)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                cartCookie.Values.Set("wishList", "");

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;
                Response.Cookies.Add(cartCookie);
            }
        }

        [CustomerAuthorize]
        public ActionResult Checkout(string invoiceNumber = "")
        {
            var cartModel = new CartModel();
            cartModel.CartItems = new List<CartItemModel>();
            List<string> errors = new List<string>();
            long totalPrice = 0;
            string discountCode = "";
            long discountAmount = 0;
            var customer = _customerRepo.GetCurrentCustomer();

            if (invoiceNumber.Equals(""))
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");

                if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                {
                    string cartJsonStr = cartCookie.Values["cart"];
                    cartModel = new CartModel(cartJsonStr);
                }

                var cartItems = cartModel.CartItems ?? new List<CartItemModel>();
                foreach (var item in cartItems)
                {

                    var mainFeature = _productMainFeaturesRepo.GetLastActiveMainFeature(item.Id, item.MainFeatureId);
                    var product = _productService.CreateProductWithPriceDto(item.Id, item.MainFeatureId);

                    var productPrice = product.PriceAfterDiscount > 0 ? product.PriceAfterDiscount : product.Price;

                    if (mainFeature == null || (productPrice != item.Price))
                    {
                        errors.Add("ویژگی های محصول '" + item.ProductName + "' تغییر کرده و امکان ثبت سفارش وجود ندارد. لطفا این محصول را از سبد خود حذف و در صورت تمایل مجدد از فروشگاه آن را انتخاب کنید.");
                    }

                    var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                    if (item.Quantity > productStockCount)
                    {
                        errors.Add("در حال حاظر تنها " + productStockCount + " از محصول " + item.ProductName + "در انبار موجود است. لطفا سبد خرید خود را به روز کنید.");

                    }

                    item.Price = product.PriceAfterDiscount;
                    totalPrice += item.Quantity * item.Price;
                }

                cartModel.CartItems = cartItems;
                cartModel.TotalPrice = totalPrice;
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cartModel);
                cartCookie.Values.Set("cart", jsonStr);

                cartCookie.Expires = DateTime.Now.AddHours(12);
                cartCookie.SameSite = SameSiteMode.Lax;

                Response.Cookies.Add(cartCookie);
                invoiceNumber = GenerateInvoiceNumber();
            } // new order
            else
            {

                // Reading from database 
                var invoice = _invoicesRepository.GetInvoice(invoiceNumber, customer.Id);


                if (DateTime.Now.Subtract(invoice.AddedDate).TotalDays > 1)
                {
                    return Redirect("/Shop/Expired");
                }

                if (invoice.DiscountAmount > 0)
                {
                    discountAmount = invoice.DiscountAmount;
                    discountCode = invoice.DiscountCode.DiscountCodeStr;

                }

                foreach (var item in invoice.InvoiceItems)
                {
                    var product = _productService.CreateProductWithPriceDto(item.ProductId, item.MainFeatureId);
                    var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                    if (item.Quantity > productStockCount)
                    {
                        errors.Add("امکان ثبت این سفارش وجود ندارد. در حال حاظر تنها تعداد " + productStockCount + " مورد از محصول " + product.ShortTitle + " در انبار موجود است. ");

                    }

                    CartItemModel cartItem = new CartItemModel();
                    cartItem.Id = item.ProductId;
                    cartItem.Quantity = item.Quantity;
                    cartItem.MainFeatureId = item.MainFeatureId;
                    cartItem.Price = item.Price;
                    cartItem.ProductName = product.ShortTitle;
                    cartItem.Image = product.Image;

                    totalPrice += cartItem.Quantity * cartItem.Price;


                    cartModel.CartItems.Add(cartItem);
                }
                cartModel.TotalPrice = totalPrice;
            } // existing order


            cartModel.TotalPrice -= discountAmount;

            ViewBag.Today = new PersianDateTime(DateTime.Now).ToString("dddd d MMMM yyyy");
            ViewBag.InvoiceNumber = invoiceNumber;
            ViewBag.Errors = errors;
            ViewBag.DiscountCode = discountCode;
            ViewBag.DiscountAmount = discountAmount;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(cartModel);
        }

        [CustomerAuthorize]
        [HttpPost]
        public ActionResult Checkout(CheckoutForm checkoutForm)
        {

            if (ModelState.IsValid)
            {
                HttpCookie cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");
                var customer = _customerRepo.GetCurrentCustomer();


                // Update customer info with latest information
                customer.Address = checkoutForm.Address;
                _customerRepo.Update(customer);

                // checkin' for discount code validity
                var discountCode = _shoppingRepo.GetActiveDiscountCode(checkoutForm.DiscountCode, customer.Id);
                long discountCodeAmount = 0;
                int? discountCodeId = null;
                if (discountCode != null)
                {
                    discountCodeAmount = discountCode.Value;
                    discountCodeId = discountCode.Id;
                }

                // Add a new (if not exists, according to invoice number) or update invoice
                var cartModel = new CartModel();
                long totalPricebeforeDiscountCode = 0;

                Invoice currentInvoice = _invoicesRepository.GetInvoice(checkoutForm.InvoiceNumber);
                if (currentInvoice == null)
                {
                    // adding new order
                    currentInvoice = new Invoice();
                    currentInvoice.InvoiceItems = new List<InvoiceItem>();

                    currentInvoice.AddedDate = DateTime.Now;
                    currentInvoice.CustomerId = customer.Id;
                    currentInvoice.CustomerName = checkoutForm.Name;
                    currentInvoice.CompanyName = checkoutForm.CompanyName;
                    currentInvoice.Country = checkoutForm.Country;
                    currentInvoice.GeoDivisionId = checkoutForm.GeoDivisionId;
                    currentInvoice.City = checkoutForm.City;
                    currentInvoice.Address = checkoutForm.Address;
                    currentInvoice.Phone = checkoutForm.Phone;
                    currentInvoice.PostalCode = checkoutForm.PostalCode;
                    currentInvoice.Email = checkoutForm.Email;
                    currentInvoice.IsPayed = false;
                    currentInvoice.InvoiceNumber = checkoutForm.InvoiceNumber;


                    // calculate price for order and adding items to invoice

                    if (!string.IsNullOrEmpty(cartCookie.Values["cart"]))
                    {
                        string cartJsonStr = cartCookie.Values["cart"];
                        cartModel = new CartModel(cartJsonStr);
                    }
                    else
                    {
                        return Redirect("/Shop/Checkout"); // basket is empty
                    }

                    var cartItems = cartModel.CartItems;
                    foreach (var item in cartItems)
                    {
                        InvoiceItem invoiceItem = new InvoiceItem();

                        var mainFeature = _productMainFeaturesRepo.GetLastActiveMainFeature(item.Id, item.MainFeatureId);
                        var product = _productService.CreateProductWithPriceDto(item.Id, item.MainFeatureId);

                        var productPrice = product.PriceAfterDiscount > 0 ? product.PriceAfterDiscount : product.Price;

                        if (mainFeature == null || (productPrice != item.Price))
                        {
                            // product features have changed
                            return Redirect("/Shop/Checkout");
                        }

                        var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                        if (item.Quantity > productStockCount)
                        {
                            return Redirect("/Shop/Checkout"); // out of product 

                        }

                        item.Price = product.PriceAfterDiscount;
                        totalPricebeforeDiscountCode += item.Quantity * item.Price;

                        invoiceItem.Quantity = item.Quantity;
                        invoiceItem.Price = item.Price;
                        invoiceItem.ProductId = product.Id;
                        invoiceItem.MainFeatureId = item.MainFeatureId;
                        invoiceItem.InsertDate = DateTime.Now;
                        invoiceItem.InsertUser = customer.User.UserName;

                        currentInvoice.InvoiceItems.Add(invoiceItem);

                    }

                    currentInvoice.DiscountAmount = discountCodeAmount;
                    currentInvoice.TotalPriceBeforeDiscount = totalPricebeforeDiscountCode;
                    currentInvoice.TotalPrice = totalPricebeforeDiscountCode - discountCodeAmount;
                    currentInvoice.DiscountCodeId = discountCodeId;

                    _invoicesRepository.Add(currentInvoice);

                    if (discountCode != null) _shoppingRepo.DeactiveDiscountCode(discountCode.Id); // deactive discount code



                    // remove all items from cart
                    cartCookie = Request.Cookies["cart"] ?? new HttpCookie("cart");
                    var cart = new CartModel();
                    cart.CartItems = new List<CartItemModel>();
                    cart.TotalPrice = 0;
                    var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(cart);
                    cartCookie.Values.Set("cart", jsonStr);

                    cartCookie.Expires = DateTime.Now.AddHours(-12);
                    cartCookie.SameSite = SameSiteMode.Lax;

                    Response.Cookies.Add(cartCookie);

                }
                else
                {
                    // using existing order
                    if (DateTime.Now.Subtract(currentInvoice.AddedDate).TotalDays > 1) // check if invoice is not expired
                    {
                        return Redirect("/Shop/Expired");
                    }

                    // recalculate price for order

                    foreach (var item in currentInvoice.InvoiceItems)
                    {
                        var product = _productService.CreateProductWithPriceDto(item.ProductId, item.MainFeatureId);
                        var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                        if (item.Quantity > productStockCount)
                        {
                            return Redirect("/Shop/Checkout"); // out of product 
                        }
                        item.Price = product.PriceAfterDiscount;
                        totalPricebeforeDiscountCode += item.Quantity * item.Price;


                    }

                    // updating info
                    currentInvoice.AddedDate = DateTime.Now;
                    currentInvoice.CustomerId = customer.Id;
                    currentInvoice.CustomerName = checkoutForm.Name;
                    currentInvoice.CompanyName = checkoutForm.CompanyName;
                    currentInvoice.Country = checkoutForm.Country;
                    currentInvoice.GeoDivisionId = checkoutForm.GeoDivisionId;
                    currentInvoice.City = checkoutForm.City;
                    currentInvoice.Address = checkoutForm.Address;
                    currentInvoice.Phone = checkoutForm.Phone;
                    currentInvoice.PostalCode = checkoutForm.PostalCode;
                    currentInvoice.Email = checkoutForm.Email;
                    currentInvoice.IsPayed = false;

                    if (discountCode != null)
                    {
                        currentInvoice.DiscountAmount = discountCodeAmount;
                        currentInvoice.DiscountCodeId = discountCodeId;
                    }

                    currentInvoice.TotalPriceBeforeDiscount = totalPricebeforeDiscountCode;
                    currentInvoice.TotalPrice = totalPricebeforeDiscountCode - currentInvoice.DiscountAmount;

                    _invoicesRepository.Update(currentInvoice);
                    if (discountCode != null) _shoppingRepo.DeactiveDiscountCode(discountCode.Id); // deactive discount code

                }

            }

            return Redirect("/Shop/ConfirmOrder/?invoiceNumber=" + checkoutForm.InvoiceNumber);
        }

        [CustomerAuthorize]
        public ActionResult ConfirmOrder(string invoiceNumber = "")
        {
            if (invoiceNumber.Equals(""))
                return Redirect("/Shop/Checkout");


            var customer = _customerRepo.GetCurrentCustomer();
            var invoice = _invoicesRepository.GetInvoice(invoiceNumber, customer.Id);


            var cartModel = new CartModel();
            cartModel.CartItems = new List<CartItemModel>();
            long totalPrice = 0;
            string discountCode = "";
            long discountAmount = 0;




            if (DateTime.Now.Subtract(invoice.AddedDate).TotalDays > 1)
            {
                return Redirect("/Shop/Expired");
            }

            if (invoice.DiscountAmount > 0)
            {
                discountAmount = invoice.DiscountAmount;
                discountCode = invoice.DiscountCode.DiscountCodeStr;

            }

            foreach (var item in invoice.InvoiceItems)
            {
                var product = _productService.CreateProductWithPriceDto(item.ProductId, item.MainFeatureId);
                var productStockCount = _productService.GetProductStockCount(item.Id, item.MainFeatureId);
                if (item.Quantity > productStockCount)
                {
                    return Redirect("/Shop/Expired"); // since order been registered we can't change the number of products and because we don't have enough products in the stock, we can't process the order

                }

                CartItemModel cartItem = new CartItemModel();
                cartItem.Id = item.ProductId;
                cartItem.Quantity = item.Quantity;
                cartItem.MainFeatureId = item.MainFeatureId;
                cartItem.Price = item.Price;
                cartItem.ProductName = product.ShortTitle;
                cartItem.Image = product.Image;

                totalPrice += cartItem.Quantity * cartItem.Price;


                cartModel.CartItems.Add(cartItem);
            }
            cartModel.TotalPrice = totalPrice;


            cartModel.TotalPrice -= discountAmount;


            ViewBag.CustomerInfoView = new CustomerInfoView(invoice, _geoDivisionRepo);
            ViewBag.Today = new PersianDateTime(invoice.AddedDate).ToString("dddd d MMMM yyyy");
            ViewBag.InvoiceNumber = invoiceNumber;
            ViewBag.DiscountCode = discountCode;
            ViewBag.DiscountAmount = discountAmount;

            ViewBag.BanerImage = _staticContentRepo.GetStaticContentDetail(13).Image;

            return View(cartModel);


        }

        public ActionResult Expired()
        {
            return View();
        }

        [CustomerAuthorize]
        public ActionResult CheckoutForm(string invoiceNumber)
        {
            var customer = _customerRepo.GetCurrentCustomer();
            Invoice invoice = _invoicesRepository.GetLatestInvoice(customer.Id);

            CheckoutForm CheckoutForm = new CheckoutForm();
            CheckoutForm.InvoiceNumber = invoiceNumber;
            CheckoutForm.Address = invoice != null ? invoice.Address : customer.Address;
            CheckoutForm.Email = invoice != null ? invoice.Email : customer.User.Email;
            CheckoutForm.Name = invoice != null ? invoice.CustomerName : customer.User.FirstName + " " + customer.User.LastName;
            CheckoutForm.PostalCode = invoice != null ? invoice.PostalCode : customer.PostalCode;
            CheckoutForm.Phone = invoice != null ? invoice.Phone : customer.User.PhoneNumber;
            CheckoutForm.GeoDivisionId = invoice != null ? invoice.GeoDivisionId.Value : (customer.GeoDivisionId ?? 1);
            CheckoutForm.DiscountCode = invoice != null ? (invoice.DiscountCode != null ? invoice.DiscountCode.DiscountCodeStr : "") : "";


            ViewBag.GeoDivisionIds = new SelectList(_geoDivisionRepo.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", CheckoutForm.GeoDivisionId);

            return PartialView(CheckoutForm);
        }

        public string GenerateInvoiceNumber()
        {
            var bytes = Guid.NewGuid().ToByteArray();
            var code = "";
            for (int i = 0; code.Length <= 16 && i < bytes.Length; i++)
            {
                code += (bytes[i] % 10).ToString();
            }

            return code;
        }
    }
}