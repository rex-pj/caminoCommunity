﻿namespace Coco.Entities.Dtos.Auth
{
    public class RoleClaimDto
    {
        public int Id { get; set; }
        public long RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
