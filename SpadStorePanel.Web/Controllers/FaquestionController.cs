using SpadStorePanel.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpadStorePanel.Web.Controllers
{
    public class FaquestionController : Controller
    {
        private readonly FaqRepository _faqRepository;

        public FaquestionController(FaqRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        // GET: Faq
        public ActionResult Index()
        {
            return View(_faqRepository.GetAll());
        }
    }
}