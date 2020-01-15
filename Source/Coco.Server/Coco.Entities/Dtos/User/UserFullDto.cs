namespace Coco.Entities.Dtos.User
{
    public class UserFullDto : UserDto
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string GenderLabel { get; set; }
        public string StatusLabel { get; set; }
    }
}
