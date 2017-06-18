using Raven.Client.Indexes;
using RavenDbByExample.Repository.Entities;
using System.Linq;

namespace RavenDbByExample.Repository.Indexes
{
    public class PizzaByToppingIndex : AbstractIndexCreationTask<Pizza>
    {
        public PizzaByToppingIndex()
        {
            Map = pizzas => from pizza in pizzas
                            select new { pizza.Toppings };
        }
    }
}
