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

        public BlogController(ArticlesRepository articlesRepository
        ,ArticleCategoriesRepository articleCategoriesRepository
        ,ArticleCommentsRepository articleCommentsRepository
        ,ArticlesTagRepositoriy articlesTagRepositoriy)
        {
            _articlesRepository = articlesRepository;

            _articleCategoriesRepository = articleCategoriesRepository;

            _articlesTagRepositoriy = articlesTagRepositoriy;

            _articleCommentsRepository = articleCommentsRepository;
        }

        // GET: Blog
        public ActionResult Index(int page=0)
        {
            List<BlogViewModel> list = new List<BlogViewModel>();

            float d = _articlesRepository.GetAll().Count() / 3f;

            ViewBag.PageCount = (int)Math.Ceiling(d);

            var test = _articlesRepository.GetAll().OrderByDescending(x => x.Id).Skip((page - 1) * 3).Take(3).ToList();

            foreach (var item in test)
            {
                list.Add(new BlogViewModel()
                {
                    Blog = item,
                    Date = DateFormater.ConvertToPersian(item.AddedDate ?? DateTime.Now)
                });
            }


            return View(list);
        }

        public ActionResult NewBlogSection()
        {
            List<BlogViewModel> list = new List<BlogViewModel>();

            var test = _articlesRepository.GetAll().OrderByDescending(x => x.Id).Take(3).ToList();

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

            return View(model);
        }

        public ActionResult CategorySection()
        {
            return PartialView("CategorySection",_articleCategoriesRepository.GetAll());
        }

        public ActionResult SendCommentSection()
        {
            return PartialView("SendCommentSection");
        }

        public ActionResult NewArticle()
        {
            List<BlogViewModel> list = new List<BlogViewModel>();

            var test = _articlesRepository.GetAll().OrderByDescending(x => x.Id).Take(3).ToList();

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