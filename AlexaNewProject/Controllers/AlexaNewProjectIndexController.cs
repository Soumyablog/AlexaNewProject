using AlexaNewProject.Models;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlexaNewProject.Controllers
{
    public class AlexaNewProjectIndexController : Controller
    {
        // GET: AlexaNewProjectIndex
        public ActionResult Index()
        {
            return View();
        }
       
        //public ActionResult DoSearch(string searchText)
        //{
        //    var myResults = new SearchResults
        //    {
        //        Results = new List<SearchResult>()
        //    };
        //    var searchIndex = ContentSearchManager.GetIndex("sitecore_web_index"); // Get the search index
        //    var searchPredicate = GetSearchPredicate(searchText); // Build the search predicate
        //    using (var searchContext = searchIndex.CreateSearchContext()) // Get a context of the search index
        //    {
        //        //Select * from Sitecore_web_index Where Author="searchText" OR Description="searchText" OR Title="searchText"
        //        //var searchResults = searchContext.GetQueryable<SearchModel>().Where(searchPredicate); // Search the index for items which match the predicate
        //        var searchResults = searchContext.GetQueryable<SearchModel>()
        //            .Where(x => x.Title.Contains(searchText) || x.ShortDescription.Contains(searchText));   //LINQ query
        //        var fullResults = searchResults.GetResults();

        //        // This is better and will get paged results - page 1 with 10 results per page
        //        //var pagedResults = searchResults.Page(1, 10).GetResults();
        //        foreach (var hit in fullResults.Hits)
        //        {
        //            myResults.Results.Add(new SearchResult
        //            {
        //                Category = hit.Document.Category,
        //                Title = hit.Document.Title,
        //                ShortDescription = hit.Document.ShortDescription,
        //                LongDescription = hit.Document.LongDescription,
        //                PostedDate = hit.Document.PostedDate,
        //                ArticleSmallImage = hit.Document.ArticleSmallImage
        //            });
        //        }
        //        return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = myResults };
        //    }
        //}

        ///// <summary>
        ///// Search logic
        ///// </summary>
        ///// <param name="searchText">Search term</param>
        ///// <returns>Search predicate object</returns>
        //public static Expression<Func<SearchModel, bool>> GetSearchPredicate(string searchText)
        //{
        //    var predicate = PredicateBuilder.True<SearchModel>(); // Items which meet the predicate
        //                                                          // Search the whole phrase - LIKE
        //                                                          // predicate = predicate.Or(x => x.DispalyName.Like(searchText)).Boost(1.2f);
        //                                                          // predicate = predicate.Or(x => x.Description.Like(searchText)).Boost(1.2f);
        //                                                          // predicate = predicate.Or(x => x.Title.Like(searchText)).Boost(1.2f);
        //                                                          // Search the whole phrase - CONTAINS
        //    predicate = predicate.Or(x => x.LongDescription.Contains(searchText)); // .Boost(2.0f);
        //    predicate = predicate.Or(x => x.ShortDescription.Contains(searchText)); // .Boost(2.0f);
        //    predicate = predicate.Or(x => x.Title.Contains(searchText)); // .Boost(2.0f);
        //    //Where Author="searchText" OR Description="searchText" OR Title="searchText"
        //    return predicate;
        //}
       
       
        public ActionResult searchPredicate(FormCollection form)
        {
            string query = form["searchInput"];
            List<Sitecore.Data.Items.Item> blogItems = new List<Sitecore.Data.Items.Item>();
            List<SearchModel> BlogCardsCollection = new List<SearchModel>();
            var rootitem = Sitecore.Context.Database.GetItem("{BD633124-74A8-4509-AC5F-156216F4BEB5}");
            var Websitesettings = Sitecore.Context.Database.GetItem("{C50D9B83-B0E3-4506-AE33-FF110A97595E}");

            blogItems = rootitem.Axes.GetDescendants().ToList();

            foreach (Sitecore.Data.Items.Item items in blogItems.ToList())
            {
                if (items.TemplateID.ToString() != "{CD905BD2-E758-46B6-98EB-6B139A4162AE}")
                {
                    blogItems.Remove(items);
                }
            }

            for (int i = 0; i < blogItems.Count; i++)
            {
                SearchModel BlogModel = new SearchModel();
                var imageUrl = string.Empty;

                Sitecore.Data.Fields.ImageField imageField = blogItems[i].Fields["ArticleSmallImage"];
                if (imageField?.MediaItem != null)
                {
                    var image = new MediaItem(imageField.MediaItem);
                    imageUrl = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(image));
                    BlogModel.BlogSImage = imageUrl;
                }
                BlogModel.Category = blogItems[i].Fields["Title"].Value;
                BlogModel.Title = blogItems[i].Fields["ShortDescription"].Value;

                Sitecore.Data.Fields.DateField dateTimeField = blogItems[i].Fields["PostedDate"];

                string dateTimeString = dateTimeField.Value;

                DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                BlogModel.PostedDate = dateTimeStruct.ToShortDateString();

                BlogModel.ShortDescription = blogItems[i].Fields["ShortDescription"].Value;
                BlogModel.LongDescription = blogItems[i].Fields["LongDescription"].Value;
                BlogModel.PostCardButtonText = Websitesettings.Fields["PostCardButtonText"].Value;
                BlogModel.sitecoreItem = blogItems[i];
                BlogModel.BlogURL = Sitecore.Links.LinkManager.GetItemUrl(blogItems[i]);


                BlogCardsCollection.Add(BlogModel);
            }

            if (query is null)
                query = "hi";

            List<SearchModel> results = BlogCardsCollection.FindAll(Findtitle);



            bool Findtitle(SearchModel bk)
            {

                if (bk.Title.Contains(query) || bk.Category.Contains(query) || bk.ShortDescription.Contains(query) || bk.LongDescription.Contains(query))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            ViewBag.searchCards = results;
            return View("~/Views/Soumya/SearchCards.cshtml", ViewBag.searchCards);
        }

    }
}