using System;

namespace Camino.Application.Contracts.AppServices.Media.Dtos
{
    public class PictureResult
    {
        public long Id { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[] BinaryData { get; set; }
        public long UpdatedById { get; set; }
        public long CreatedById { get; set; }
    }
}
