using System;

namespace LuxuryWatchesApiTests
{
    [Serializable]
    public class Product : IEquatable<Product>
    {
        public int id { get; set; } 
        public int category_id { get; set; } 
        public string title { get; set; } 
        public string alias { get; set; } 
        public string content { get; set; } 
        public double price { get; set; } 
        public double old_price { get; set; } 
        public int status { get; set; } 
        public string keywords { get; set; } 
        public string description { get; set; } 
        public string img { get; set; } 
        public string hit { get; set; } 
        public string cat { get; set; }

        public bool Equals(Product other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return category_id == other.category_id && title == other.title && content == other.content && price.Equals(other.price) && old_price.Equals(other.old_price) && status == other.status && keywords == other.keywords && description == other.description;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            
            return Equals((Product) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            
            hashCode.Add(category_id);
            hashCode.Add(title);
            hashCode.Add(alias);
            hashCode.Add(content);
            hashCode.Add(price);
            hashCode.Add(old_price);
            hashCode.Add(status);
            hashCode.Add(keywords);
            hashCode.Add(description);
            
            return hashCode.ToHashCode();
        }
    }
}