using System.Collections.Generic;

namespace RavenDbByExample.Repository.Entities
{
    public class Pizza
    {
        public string Id { get; set; }
        public Rating Rating { get; set; }
        public List<Topping> Toppings { get; set; }
        public string Description { get; set; }
    }
}
