namespace Makta.api.Models
{
    public class AddPointsDto
    {
        public string StoreKey { get; set; }

        public string currency;
        public string Currency
        {
            get
            {
                return string.IsNullOrEmpty(currency) ? "CAD" : currency;
            }
            set
            {
                currency = value;
            }
        }
        public string CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AmountSpent { get; set; }
        public string Comments { get; set; }
    }
}
