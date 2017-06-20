using System.Collections.Generic;
using System.Linq;

namespace RavenDbByExample.Repository.Entities
{
    public class Pizza
    {
        public string Id { get; set; }
        public Rating Rating { get; set; }
        public List<Topping> Toppings { get; set; }
        public string Description { get; set; }

        protected bool Equals(Pizza other)
        {
            return string.Equals(Id, other.Id) && Equals(Rating, other.Rating) && Toppings.SequenceEqual(other.Toppings) && string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pizza) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Rating != null ? Rating.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Toppings != null ? Toppings.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
