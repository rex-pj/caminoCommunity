namespace Camino.Application.Contracts.AppServices.Users.Dtos
{
    public class UserFullResult : UserResult
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string GenderLabel { get; set; }
        public string StatusLabel { get; set; }
    }
}
