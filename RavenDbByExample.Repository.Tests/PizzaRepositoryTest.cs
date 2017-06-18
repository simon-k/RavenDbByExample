using FluentAssertions;
using Raven.Client.Document;
using Raven.Client.Embedded;
using RavenDbByExample.Repository.Entities;
using System.Collections.Generic;
using Xunit;

namespace RavenDbByExample.Repository.Tests
{
    public class PizzaRepositoryTest
    {
        private readonly EmbeddableDocumentStore _documentStore;
        private readonly IPizzaRepository _repository;

        public PizzaRepositoryTest()
        {
            _documentStore = new EmbeddableDocumentStore
            {
                Configuration =
                {
                    RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
                    RunInMemory = true,
                }
            };
            _documentStore.Configuration.Storage.Voron.AllowOn32Bits = true;
            _documentStore.Initialize();
            _documentStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;

            _repository = new PizzaRepository(_documentStore);
        }

        [Fact]
        public void Add_Pizza_AddsPizzaToRepository()
        {
            var expectedPizza = new Pizza
            {
                Id = "Hawaii",
                Toppings = new List<Topping>
                {
                    Topping.Tomatoes, Topping.Cheese, Topping.Ham, Topping.Pineapple
                },
                Description = "The classic pizza with ham and pineapple.",
                Rating = new Rating
                {
                    Score = Score.Horrible,
                    Reason = "Nobody likes pineapple on a pizza.",
                }
            };

            _repository.Add(expectedPizza);

            Pizza actualPizza;
            using (var session = _documentStore.OpenSession())
            {
                actualPizza = session.Load<Pizza>("Hawaii");
            }

            //TODO: Add comparator to entities and make below assert better.
            actualPizza.Id.Should().Be(expectedPizza.Id);
        }
    }
}
