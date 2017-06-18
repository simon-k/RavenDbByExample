using Raven.Client.Indexes;
using RavenDbByExample.Repository.Entities;
using System.Linq;

namespace RavenDbByExample.Repository.Indexes
{
    public class PizzaByScoreIndex : AbstractIndexCreationTask<Pizza>
    {
        public PizzaByScoreIndex()
        {
            Map = pizzas => from pizza in pizzas
                             select new { Rating_Score = pizza.Rating.Score };
        }
    }
}
