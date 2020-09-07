using Camino.Framework.Models;
using System;

namespace Module.Web.FarmManagement.Models
{
    public class FarmPictureFilterModel : BaseFilterModel
    {
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public string MimeType { get; set; }
    }
}
