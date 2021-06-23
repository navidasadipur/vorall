using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SpadStorePanel.Core.Models;

namespace SpadStorePanel.Web.ViewModels
{
    public class DiscountFormViewModel
    {
        [DisplayName("عنوان تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید کمتر از 500 کارکتر باشد")]
        public string Title { get; set; }
        public int DiscountType { get; set; }
        [DisplayName("میزان تخفیف")]
        [Required(ErrorMessage = "لطفا میزان تخفیف را وارد کنید")]
        public long Amount { get; set; }
        public List<int> BrandIds { get; set; }
        public List<int> ProductIds { get; set; }
        public List<int> ProductGroupIds { get; set; }
        public bool IsOffer { get; set; }
        public int? OfferId { get; set; }

        // Edit Props
        public string GroupIdentifier { get; set; }
        public List<Discount> PreviousDiscounts { get; set; }
    }

    public class CheckoutForm
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل نا معتبر")]
        [MaxLength(400, ErrorMessage = "{0} باید کمتر از 400 کارکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "{0} باید کمتر از 400 کارکتر باشد")]
        public string Phone { get; set; }

        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }

        [Display(Name = "کشور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Country { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string City { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PostalCode { get; set; }

        public int GeoDivisionId { get; set; }

        [Display(Name = "توضیحات(اختیاری)")]
        [DataType(DataType.MultilineText)]
        [MaxLength(800, ErrorMessage = "{0} باید کمتر از 800 کارکتر باشد")]
        public string Message { get; set; }

        public string DiscountCode { get; set; }
        public string InvoiceNumber { get; set; }

    }
}