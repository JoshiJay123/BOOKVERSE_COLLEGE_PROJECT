namespace COLLEGE_PROJECT.Helpers
{
    public class AddToWishListRequest
    {
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string BookCover { get; set; }
        public string ISBN { get; set; }
    }
}
