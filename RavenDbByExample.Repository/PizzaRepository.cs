using Raven.Client;
using RavenDbByExample.Repository.Entities;

namespace RavenDbByExample.Repository
{
    public class PizzaRepository : IPizzaRepository
    {
        private IDocumentStore _documentStore;

        public PizzaRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Add(Pizza pizza)
        {
            using(var session = _documentStore.OpenSession())
            {
                session.Store(pizza);
                session.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            using (var session = _documentStore.OpenSession())
            {
                var pizza = session.Load<Pizza>(id);
                session.Delete(pizza);
                session.SaveChanges();
            }
        }

        public Pizza Get(string id)
        {
            using (var session = _documentStore.OpenSession())
            {
                var pizza = session.Load<Pizza>(id);
                return pizza;
            }
        }

        //TODO: Get Number of Pizzas 
        //TODO: Get All Pizzas
        //TODO: Get pizzas with topping
        //TODO: Get pizza with rating 
        //TODO: Search pizza by name
    }
}
