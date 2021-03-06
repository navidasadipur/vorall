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
        public ActionResult Index(int pageNumber = 1, string searchString = null, int? category = null)
        {
            var articles = new List<Article>();

            var take = 9;
            var skip = pageNumber * take - take;
            var count = 0;

            //List<BlogViewModel> list = new List<BlogViewModel>();

            //float d = _articlesRepository.GetAll().Count() / 3f;

            //ViewBag.PageCount = (int)Math.Ceiling(d);

            //var test = _articlesRepository.GetAll().OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();

            //foreach (var item in test)
            //{
            //    list.Add(new BlogViewModel()
            //    {
            //        Blog = item,
            //        Date = DateFormater.ConvertToPersian(item.AddedDate ?? DateTime.Now)
            //    });
            //}

            if (category != null)
            {
                articles = _articlesRepository.GetArticlesList(skip, take, category.Value);
                count = _articlesRepository.GetArticlesCount(category.Value);
                var cat = _articleCategoriesRepository.Get(category.Value);
                ViewBag.CategoryId = category;
                ViewBag.Title = $"دسته {cat.Title}";
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                articles = _articlesRepository.GetArticlesList(skip, take, searchString);
                count = _articlesRepository.GetArticlesCount(searchString);
                ViewBag.SearchString = searchString;
                ViewBag.Title = $"جستجو: {searchString}";
            }
            else
            {
                articles = _articlesRepository.GetArticlesList(skip, take);
                count = _articlesRepository.GetArticlesCount();
                ViewBag.Title = "بلاگ";
            }

            var pageCount = (int)Math.Ceiling((double)count / take);
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageNumber;

            var vm = new List<LatestArticlesViewModel>();
            foreach (var item in articles)
                vm.Add(new LatestArticlesViewModel(item));

            ViewBag.HeaderImage = _staticContentDetailsRepo.GetStaticContentDetail((int)StaticContents.BlogBackImage).Image;

            return View(vm);
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
            return PartialView("CategorySection",_articleCategoriesRepository.GetAllCategoriesWithArticles());
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