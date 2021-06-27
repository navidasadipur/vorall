using SpadStorePanel.Core.Models;
using SpadStorePanel.Core.Utility;
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

            model.Phone = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.Phone).ShortDescription;

            model.Email = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.Email).ShortDescription;

            model.Address = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.Address).ShortDescription;

            model.Map = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.Map).Description;

            ViewBag.SidebarShortDescription = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.ContactUsBackImageAndContent).ShortDescription;

            ViewBag.SidebarDescription = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.ContactUsBackImageAndContent).Description;

            ViewBag.HeaderImage = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.ContactUsBackImageAndContent).Image;

            return View(model);
        }

        public ActionResult FormSection()
        {
            var model = new ContactForm();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult FormSection(ContactForm model)
        {
            //try
            //{
            //    _contactFormsRepository.Add(form);
            //    return "success";
            //}
            //catch
            //{
            //    return "fail";
            //}

            if (ModelState.IsValid)
            {
                _contactFormsRepository.Add(model);
                return RedirectToAction("ContactUsSummary");
            }
            return RedirectToAction("Index");
        }

        public ActionResult ContactUsSummary()
        {
            ViewBag.HeaderImage = _staticContentDetailsRepository.GetStaticContentDetail((int)StaticContents.ContactUsBackImageAndContent).Image;

            return View();
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
    }
}