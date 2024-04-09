using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace COLLEGE_PROJECT.Model
{


    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("booktitle")]
        public string BookTitle { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("bookcover")]
        public string BookCover
        { get; set; }

        [BsonElement("ISBN")]
        public string ISBN { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

        [BsonElement("timestamps")]
        public DateTime Timestamps { get; set; }
    }
}
