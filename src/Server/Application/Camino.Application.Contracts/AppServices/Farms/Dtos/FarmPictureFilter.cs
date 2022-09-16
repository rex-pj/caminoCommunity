using System;

namespace Camino.Application.Contracts.AppServices.Farms.Dtos
{
    public class FarmPictureFilter : BaseFilter
    {
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public string MimeType { get; set; }
    }
}
