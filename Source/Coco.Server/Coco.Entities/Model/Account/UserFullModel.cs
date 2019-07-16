namespace Coco.Entities.Model.Account
{
    public class UserFullModel : UserModel
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string GenderLabel { get; set; }
        public string StatusLabel { get; set; }
        public string Photo { get; set; }
        public string CoverPhoto { get; set; }
    }
}
