using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;

namespace Module.Api.Farm.Models
{
    public class FarmModel
    {
        public FarmModel()
        {
            Pictures = new List<PictureRequestModel>();
        }

        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [GraphQLType(typeof(DateTimeType))]
        public DateTimeOffset CreatedDate { get; set; }
        [GraphQLType(typeof(LongType))]
        public long CreatedById { get; set; }
        [GraphQLType(typeof(DateTimeType))]
        public DateTimeOffset UpdatedDate { get; set; }
        [GraphQLType(typeof(LongType))]
        public long UpdatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string CreatedByIdentityId { get; set; }
        public long FarmTypeId { get; set; }
        public string FarmTypeName { get; set; }
        public string Address { get; set; }
        public IEnumerable<PictureRequestModel> Pictures { get; set; }
    }
}
