namespace Camino.Shared.Requests.Authentication
{
    public class UserIdentifierUpdateRequest
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public long Id { get; set; }
    }
}
