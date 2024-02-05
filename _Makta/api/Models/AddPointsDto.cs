namespace Makta.api.Models
{
    public class AddPointsDto
    {
        public string StoreKey { get; set; }
        public string Currency { get; set; } = "CAD";
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AmountSpent { get; set; }
        public string Comments { get; set; }
    }
}
