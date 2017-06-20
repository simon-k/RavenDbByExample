using FluentAssertions;
using Raven.Client.Document;
using Raven.Client.Embedded;
using RavenDbByExample.Repository.Entities;
using System.Collections.Generic;
using System.Linq;
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
                },
            };
            _documentStore.Configuration.Storage.Voron.AllowOn32Bits = true;
            _documentStore.Initialize();
            _documentStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;

            _repository = new PizzaRepository(_documentStore);
        }

        [Fact]
        public void Add_Pizza_AddsPizzaToRepository()
        {
            var expectedId = "Hawaii";
            Pizza expectedPizza = ArrangePizza(expectedId);

            _repository.Add(expectedPizza);

            Pizza actualPizza;
            using (var session = _documentStore.OpenSession())
            {
                actualPizza = session.Load<Pizza>(expectedId);
            }

            actualPizza.Should().Be(expectedPizza);
        }

        [Fact]
        public void Get_Pizza_ReturnsPizzaFromRepository()
        {
            var expectedId = "Hawaii";
            Pizza expectedPizza = ArrangePizza(expectedId);

            using (var session = _documentStore.OpenSession())
            {
                session.Store(expectedPizza);
                session.SaveChanges();
            }
            
            var actualPizza = _repository.Get(expectedId);

            actualPizza.Should().Be(expectedPizza);
        }

        [Fact]
        public void Delete_Pizza_RemovesPizzaFromRepository()
        {
            var expectedId = "Hawaii";
            Pizza expectedPizza = ArrangePizza(expectedId);

            using (var session = _documentStore.OpenSession())
            {
                session.Store(expectedPizza);
                session.SaveChanges();
            }

             _repository.Delete(expectedId);

            Pizza actualPizza;
            using (var session = _documentStore.OpenSession())
            {
                actualPizza = session.Load<Pizza>(expectedId);
            }

            actualPizza.Should().BeNull();
        }

        [Fact]
        public void GetByScore_Score_ReturnsPizzasWithGivenScore()
        {
            var hawaiiPizza = ArrangePizza("Hawaii", Score.Perfect);
            var margaritaPizza = ArrangePizza("Margarita", Score.Perfect);
            var vesuvioPizza = ArrangePizza("Vesuvio", Score.Horrible);

            using (var session = _documentStore.OpenSession())
            {
                session.Store(hawaiiPizza);
                session.Store(margaritaPizza);
                session.Store(vesuvioPizza);
                session.SaveChanges();
            }

            var actualPizzas = _repository.GetByScore(Score.Perfect);

            actualPizzas.Count().Should().Be(2);
            actualPizzas.Should().Contain(margaritaPizza);
            actualPizzas.Should().Contain(hawaiiPizza);
        }

        [Fact]
        public void GetByTopping_Topping_ReturnsPizzasWithGivenTopping()
        {
            var hawaiiPizza = ArrangePizza("Hawaii", toppings: new List<Topping> { Topping.Tomatoes, Topping.Cheese, Topping.Ham, Topping.Pineapple });
            var margaritaPizza = ArrangePizza("Margarita", toppings: new List<Topping>{Topping.Tomatoes, Topping.Cheese});
            var vesuvioPizza = ArrangePizza("Vesuvio", toppings: new List<Topping> { Topping.Tomatoes, Topping.Cheese, Topping.Ham });

            using (var session = _documentStore.OpenSession())
            {
                session.Store(hawaiiPizza);
                session.Store(margaritaPizza);
                session.Store(vesuvioPizza);
                session.SaveChanges();
            }

            var actualPizzas = _repository.GetByTopping(Topping.Ham);

            actualPizzas.Count().Should().Be(2);
            actualPizzas.Should().Contain(hawaiiPizza);
            actualPizzas.Should().Contain(vesuvioPizza);
        }

        private static Pizza ArrangePizza(string id, Score score = Score.OK, List<Topping> toppings = null)
        {
            return new Pizza
            {
                Id = id,
                Toppings = toppings ?? new List<Topping>
                {
                    Topping.Tomatoes, Topping.Cheese, Topping.Ham, Topping.Pineapple
                },
                Description = "Classic pizza.",
                Rating = new Rating
                {
                    Score = score,
                    Reason = "Just plain regular",
                }
            };
        }
    }
}
