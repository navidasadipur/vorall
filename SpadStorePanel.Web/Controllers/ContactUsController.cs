using SpadStorePanel.Core.Models;
using SpadStorePanel.Infrastructure.Repositories;
using SpadStorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpadStorePanel.Web.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly StaticContentDetailsRepository _staticContentDetailsRepository;

        private readonly ContactFormsRepository _contactFormsRepository;

        public ContactUsController(StaticContentDetailsRepository staticContentDetailsRepository
        , ContactFormsRepository contactFormsRepository)
        {
            _contactFormsRepository = contactFormsRepository;

            _staticContentDetailsRepository = staticContentDetailsRepository;
        }

        // GET: ContactUs
        public ActionResult Index()
        {
            ContactUsViewModel model = new ContactUsViewModel();

            model.Phone = _staticContentDetailsRepository.Get(9).ShortDescription;

            model.Email = _staticContentDetailsRepository.Get(8).ShortDescription;

            model.Address = _staticContentDetailsRepository.Get(10).ShortDescription;

            model.Map = _staticContentDetailsRepository.Get(7).Description;

            return View(model);
        }

        public ActionResult FormSection()
        {
            return PartialView("FormSection");
        }

        [HttpPost]
        public string GetNews(ContactForm form)
        {
            var contactForm = _contactFormsRepository.GetContatctFormByEmail(form.Email);
            try
            {
                if (contactForm == null)
                {

                    form.Name = "فرم دریافت خبرنامه";
                    form.Message = "دریافت خبرنامه";
                    form.Phone = "_";

                    form.IsDeleted = false;
                    form.InsertUser = "_";

                    _contactFormsRepository.Add(form);
                    return "success";
                }
                else if (contactForm.Name != "فرم دریافت خبرنامه")
                {
                    form.Name = "فرم دریافت خبرنامه";
                    form.Message = "دریافت خبرنامه";
                    form.Phone = "_";

                    form.IsDeleted = false;
                    form.InsertUser = "_";

                    _contactFormsRepository.Add(form);
                    return "success";
                }
                else
                {
                    return "fail";
                }
            }
            catch
            {
                return "fail";
            }

        }

        [HttpPost]
        public string ContactUS(ContactForm form)
        {
            try
            {
                _contactFormsRepository.Add(form);
                return "success";
            }
            catch
            {
                return "fail";
            }

        }
    }
}