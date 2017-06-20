using Raven.Client;
using Raven.Client.Bundles.Versioning;
using RavenDbByExample.Repository.Entities;
using RavenDbByExample.Repository.Indexes;
using System.Collections.Generic;
using System.Linq;

namespace RavenDbByExample.Repository
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly IDocumentStore _documentStore;

        public PizzaRepository(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
            LoadIndexes();
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

        public IEnumerable<Pizza> GetByScore(Score score)
        {
            using (var session = _documentStore.OpenSession())
            {
                return session.Query<Pizza, PizzaByScoreIndex>()
                              .Where(p => p.Rating.Score == score);
            }
        }

        public IEnumerable<Pizza> GetByTopping(Topping topping)
        {
            using (var session = _documentStore.OpenSession())
            {
                return session.Query<Pizza, PizzaByToppingIndex>()
                              .Where(p => p.Toppings.Any(t => t == topping));
            }
        }

        public IEnumerable<Pizza> GetRevisions(string id)
        {
            using (var session = _documentStore.OpenSession())
            {
                var revisions = session.Advanced.GetRevisionsFor<Pizza>(id, 0, 25);
                return revisions;
            }
        }

        private void LoadIndexes()
        {
            new PizzaByScoreIndex().Execute(_documentStore);
            new PizzaByToppingIndex().Execute(_documentStore);
        }
    }
}
