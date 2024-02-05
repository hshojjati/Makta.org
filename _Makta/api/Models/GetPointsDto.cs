namespace Makta.api.Models
{
    public class GetPointsDto
    {
        public string StoreKey { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public int PageNumber { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
    }
}
