using System;

namespace Coco.Entities.Dtos.General
{
    public class SetupDto
    {
        public string AdminEmail { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateTime BirthDate { get; set; }
        public string AdminPassword { get; set; }
        public string AdminConfirmPassword { get; set; }
    }
}
