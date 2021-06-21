using SpadStorePanel.Core.Models;
using System.Collections.Generic;

namespace SpadStorePanel.Web.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<ProductGallery> ProductGalleries { get; set; }
        public List<ProductMainFeature> ProductMainFeatures { get; set; }
        public List<ProductFeatureValue> ProductFeatureValues { get; set; }
        public List<ProductComment> Comments { get; set; }
        public long Price { get; set; }
        public long PriceAfterDiscount { get; set; }
        public int DiscountPercentage { get; set; }

    }
}