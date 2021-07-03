using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SpadStorePanel.Core.Models;
using SpadStorePanel.Infrastructure.Dtos.Product;

namespace SpadStorePanel.Web.ViewModels
{
    public class ProductViewModel
    {
        public ProductWithPriceDto Product { get; set; }
        public List<ProductMainFeature> ProductMainFeatures { get; set; }
    }
    public class NewProductViewModel
    {
        public int? ProductId { get; set; }
        public string ShortDescription { get; set; }

        public string Description { get; set; }
        public string Title { get; set; }
        public string ProductCode { get; set; }
        public int Brand { get; set; }
        public int Rate { get; set; }
        public int ProductGroup { get; set; }
        public List<ProductFeaturesViewModel> ProductFeatures { get; set; }

    }

    public class ProductFeaturesViewModel
    {
        public int? ProductId { get; set; }
        public int FeatureId { get; set; }
        public int? SubFeatureId { get; set; }
        public string Value { get; set; }
        public bool IsMain { get; set; }
        public int? Quantity { get; set; }
        public long? Price { get; set; }
    }
    public class ProductCommentWithPersianDateViewModel : ProductComment
    {
        public ProductCommentWithPersianDateViewModel()
        {
        }
        public ProductCommentWithPersianDateViewModel(ProductComment comment)
        {
            this.Comment = comment;
            this.PersianDate = comment.AddedDate != null ? new PersianDateTime(comment.AddedDate.Value).ToString() : "-";
        }
        public ProductComment Comment { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public string PersianDate { get; set; }
    }

    public class GridViewModel
    {
        public int? categoryId { get; set; }
        public string searchString { get; set; }
        public long? priceFrom { get; set; }
        public long? priceTo { get; set; }
        public string brands { get; set; }
        public string subFeatures { get; set; }
        public int pageNumber { get; set; }
        public int take { get; set; }
        public string sort { get; set; }

        public string BrandIds { get; set; }
        public string GroupIds { get; set; }
        public string ProductIds { get; set; }
    }
}