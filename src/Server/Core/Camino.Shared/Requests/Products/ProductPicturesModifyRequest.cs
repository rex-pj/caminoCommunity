using Camino.Shared.Requests.Media;
using System;
using System.Collections.Generic;

namespace Camino.Shared.Requests.Products
{
    public class ProductPicturesModifyRequest
    {
        public ProductPicturesModifyRequest()
        {
            Pictures = new List<PictureRequest>();
        }

        public long CreatedById { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        public long ProductId { get; set; }

        public IEnumerable<PictureRequest> Pictures { get; set; }
    }
}
