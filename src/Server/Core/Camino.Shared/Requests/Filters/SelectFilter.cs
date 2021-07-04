﻿namespace Camino.Shared.Requests.Filters
{
    public class SelectFilter
    {
        public SelectFilter()
        {
            Search = string.Empty;
            CurrentIds = new long[0];
        }

        public long CreatedById { get; set; }
        public long[] CurrentIds { get; set; }
        public string Search { get; set; }
    }
}
