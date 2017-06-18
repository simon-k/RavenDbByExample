using RavenDbByExample.Repository.Entities;
using System.Collections.Generic;

namespace RavenDbByExample.Repository
{
    public interface IPizzaRepository
    {
        void Add(Pizza pizza);
        void Delete(string id);
        Pizza Get(string id);
        IEnumerable<Pizza> GetByScore(Score score);
        IEnumerable<Pizza> GetByTopping(Topping topping);
    }
}