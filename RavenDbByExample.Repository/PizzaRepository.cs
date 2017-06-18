using Raven.Client;
using RavenDbByExample.Repository.Entities;
using RavenDbByExample.Repository.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RavenDbByExample.Repository
{
    public class PizzaRepository : IPizzaRepository
    {
        private IDocumentStore _documentStore;

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

        private void LoadIndexes()
        {
            new PizzaByScoreIndex().Execute(_documentStore);
            new PizzaByToppingIndex().Execute(_documentStore);
            WaitForNonStaleIndexes(_documentStore, TimeSpan.FromSeconds(30));
        }



        public static bool WaitForNonStaleIndexes(IDocumentStore documentStore, TimeSpan timeout)
        {
            string[] staleIndexes;
            return WaitForNonStaleIndexes(documentStore, timeout, out staleIndexes);
        }

        public static bool WaitForNonStaleIndexes(IDocumentStore documentStore, TimeSpan timeout, out string[] staleIndexes)
        {
            var time = DateTime.Now;

            staleIndexes = ExcludeAutoIndexes(GetStaleIndexes(documentStore));

            while (staleIndexes.Length > 0)
            {
                if (DateTime.Now - time > timeout)
                {
                    return false;
                }
                Thread.Sleep(50);

                staleIndexes = ExcludeAutoIndexes(GetStaleIndexes(documentStore));
            }

            return true;
        }

        private static string[] GetStaleIndexes(IDocumentStore documentStore)
        {
            return documentStore.DatabaseCommands.GetStatistics().StaleIndexes;
        }

        private static string[] ExcludeAutoIndexes(string[] staleIndexes)
        {
            return staleIndexes.Where(x => !x.StartsWith("Auto/")).ToArray();
        }



        //TODO: Search pizza by name
    }
}
