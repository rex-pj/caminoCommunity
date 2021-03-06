﻿using Camino.Shared.Enums;
using System;

namespace Camino.Shared.Results.Feed
{
    public class FeedResult
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long PictureId { get; set; }
        public long CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public FeedType FeedType { get; set; }
    }
}
