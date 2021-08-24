using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexaNewProject.Models
{   
    public class SearchModel
    {
        public string ItemName { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public byte[] ArticleSmallImage { get; set; }
        public byte[] ArticleLargeImage { get; set; }
        public string Category { get; set; }
        public string PostedDate { get; set; }
        public string Readonbtn { get; set; }
        public Sitecore.Data.Items.Item sitecoreItem { get; set; }
        public string BlogURL { get; set; }
        public string BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogSImage { get; set; }
        public string PostCardButtonText { get; set; }
    }
}