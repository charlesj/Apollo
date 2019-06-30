using System;
using System.Collections.Generic;

namespace Apollo.External.SyndicationToolbox
{
    public class Feed
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string WebUri { get; set; }
        public string HubbubUri { get; set; }
        public string Generator { get; set; }
        public List<FeedArticle> Articles { get; set; }
        public List<FeedCategory> Categories { get; set; }
    }

    public class FeedArticle
    {
        public string ServerId { get; set; }
        public string Title { get; set; }
        public string WebUri { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string Content { get; set; }
        public string CommentsUri { get; set; }
        public List<FeedCategory> Categories { get; set; }
        public int? ParentFeedId { get; set; }
    }

    public class FeedCategory
    {
        public string Term { get; set; }
        public string Label { get; set; }
    }
}
