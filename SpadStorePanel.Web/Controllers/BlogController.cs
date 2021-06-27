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
    public class BlogController : Controller
    {
        private readonly ArticlesRepository _articlesRepository;

        private readonly ArticleCategoriesRepository _articleCategoriesRepository;

        private readonly ArticleCommentsRepository _articleCommentsRepository;

        private readonly ArticlesTagRepositoriy _articlesTagRepositoriy;

        private readonly StaticContentDetailsRepository _staticContentDetailsRepo;

        public BlogController(
            ArticlesRepository articlesRepository
            ,ArticleCategoriesRepository articleCategoriesRepository
            ,ArticleCommentsRepository articleCommentsRepository
            ,ArticlesTagRepositoriy articlesTagRepositoriy
            , StaticContentDetailsRepository staticContentDetailsRepo
        )
        {
            _articlesRepository = articlesRepository;

            _articleCategoriesRepository = articleCategoriesRepository;

            _articlesTagRepositoriy = articlesTagRepositoriy;

            _staticContentDetailsRepo = staticContentDetailsRepo;
            _articleCommentsRepository = articleCommentsRepository;
        }

        // GET: Blog
        public ActionResult Index(int page=0)
        {
            List<BlogViewModel> list = new List<BlogViewModel>();
            var take = 9;
            var skip = (page + 1) * take - take;
            var count = 0;

            float d = _articlesRepository.GetAll().Count() / 3f;

            ViewBag.PageCount = (int)Math.Ceiling(d);

            var test = _articlesRepository.GetAll().OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();

            foreach (var item in test)
            {
                list.Add(new BlogViewModel()
                {
                    Blog = item,
                    Date = DateFormater.ConvertToPersian(item.AddedDate ?? DateTime.Now)
                });
            }

            ViewBag.HeaderImage = _staticContentDetailsRepo.GetStaticContentDetail((int)StaticContents.BlogBackImage).Image;

            return View(list);
        }

        public ActionResult NewBlogSection(int take)
        {
            List<BlogViewModel> list = new List<BlogViewModel>();

            var test = _articlesRepository.GetAll().OrderByDescending(x => x.Id).Take(take).ToList();

            foreach (var item in test)
            {
                list.Add(new BlogViewModel()
                {
                    Blog = item,
                    Date = DateFormater.ConvertToPersian(item.AddedDate ?? DateTime.Now)
                });
            }

            return PartialView("NewBlogSection",list);
        }

        public ActionResult Detail(int id)
        {
            BlogDetailViewModel model = new BlogDetailViewModel();

            model.Blog = _articlesRepository.Get(id);

            model.Comments = _articleCommentsRepository.GetArticleComments(id);

            model.Tags = _articlesTagRepositoriy.GetAll();

            ViewBag.HeaderImage = _staticContentDetailsRepo.GetStaticContentDetail((int)StaticContents.BlogBackImage).Image;

            return View(model);
        }

        public ActionResult CategorySection()
        {
            return PartialView("CategorySection",_articleCategoriesRepository.GetAll());
        }

        public ActionResult SendCommentSection()
        {
            var model = new ArticleComment();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SendCommentSection(ArticleComment form)
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
            form.AddedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _articleCommentsRepository.Add(form);
                return RedirectToAction("ContactUsSummary");
            }
            return RedirectToAction("Detail");
        }

        public ActionResult ContactUsSummary()
        {
            ViewBag.HeaderImage = _staticContentDetailsRepo.GetStaticContentDetail((int)StaticContents.BlogBackImage).Image;

            return View();
        }

        public ActionResult NewArticle(int take)
        {
            List<BlogViewModel> list = new List<BlogViewModel>();

            var test = _articlesRepository.GetAll().OrderByDescending(x => x.Id).Take(take).ToList();

            foreach (var item in test)
            {
                list.Add(new BlogViewModel()
                {
                    Blog = item,
                    Date = DateFormater.ConvertToPersian(item.AddedDate ?? DateTime.Now)
                });
            }

            return PartialView("NewArticle",list);
        }
    }
}