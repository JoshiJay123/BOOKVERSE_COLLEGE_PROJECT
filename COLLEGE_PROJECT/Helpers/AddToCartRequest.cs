namespace COLLEGE_PROJECT.Helpers
{
    public class AddToCartRequest
    {
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public string BookCover { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
    }
}
    