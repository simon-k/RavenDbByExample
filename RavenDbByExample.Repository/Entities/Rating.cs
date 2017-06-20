using System.Collections.Generic;

namespace RavenDbByExample.Repository.Entities
{
    public class Rating
    {
        public Score Score { get; set; }
        public string Reason { get; set; }

        protected bool Equals(Rating other)
        {
            return Score == other.Score && string.Equals(Reason, other.Reason);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rating) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Score * 397) ^ (Reason != null ? Reason.GetHashCode() : 0);
            }
        }
    }
}
