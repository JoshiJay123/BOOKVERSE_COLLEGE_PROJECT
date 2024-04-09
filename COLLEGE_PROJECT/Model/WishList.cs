using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using COLLEGE_PROJECT.Data;

namespace COLLEGE_PROJECT.Model
{
    public class WishList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("booktitle")]
        public string BookTitle { get; set; }

        [Required]
        [BsonElement("author")]
        public string Author { get; set; }

        [Required]
        [BsonElement("price")]
        public double Price { get; set; }

        [Required]
        [BsonElement("bookcover")]
        public string BookCover { get; set; }

        [Required]
        [BsonElement("ISBN")]
        public string ISBN { get; set; }

        [Required]
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

    }
}
