using Microsoft.VisualBasic;

namespace ShopClient.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<CartItem> Carts { get; set; }
        public double TotalPrice { get; set; }
        public double Delivery { get; set; }
        public DateTime DateAndTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address1 { get; set; }
        public string Address2 { get; set; } = string.Empty;
        public string Country { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public string Payment { get; set; }
        public int OrderStatus { get; set; }
    }
}
