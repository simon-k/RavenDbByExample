using Raven.Client;
using Raven.Client.Document;
using RavenDbByExample.Repository;
using RavenDbByExample.Repository.Entities;
using System;
using System.Collections.Generic;

namespace RavenByExample.Program
{
    class Program
    {
        private static IDocumentStore _documentStore;
        private static IPizzaRepository _pizzaRepository;

        static void Main(string[] args)
        {
            Setup();

            char key = PrintMenu();
            while (key != '0')
            {
                PerformAction(key);
                key = PrintMenu();
            }

            Teardown();
        }

        private static void PerformAction(char key)
        {
            switch (key)
            {
                case '1': 
                    AddPizza();
                    break;
                case '2': 
                    GetPizza();
                    break;
                case '3':
                    GetRevisions();
                    break;
            }
        }

        private static void GetPizza()
        {
            Console.Write("\n\nGet Pizza\nName: ");
            string name;
            while ((name = Console.ReadLine()).Length == 0) ;

            var pizza = _pizzaRepository.Get(name);
            Console.WriteLine("{0}, {1}\n", pizza.Id, pizza.Description);
        }

        private static void GetRevisions()
        {
            Console.Write("\n\nGet Pizza Revisions\nName: ");
            string name;
            while ((name = Console.ReadLine()).Length == 0);

            var revisions = _pizzaRepository.GetRevisions(name);
            
            foreach(var revision in revisions)
            {
                Console.WriteLine("{0}, {1}", revision.Id, revision.Description);
            }
            Console.WriteLine("\n");
        }

        private static void AddPizza()
        {
            Console.Write("\n\nEnter Pizza\nName: ");
            string name;
            while ((name = Console.ReadLine()).Length == 0) ;

            Console.Write("Description: ");
            string description;
            while ((description = Console.ReadLine()).Length == 0) ;

            var pizza = new Pizza
            {
                Id = name,
                Description = description,
                Toppings = new List<Topping> { Topping.Tomatoes, Topping.Cheese, Topping.Pepperoni },
                Rating = new Rating
                {
                    Score = Score.OK,
                    Reason = "Just for some reason",
                }
            };
            _pizzaRepository.Add(pizza);
            Console.WriteLine("Pizza Added\n");
        }

        private static char PrintMenu()
        {
            Console.WriteLine("Menu:\n1) Add Pizza\n2) Get pizza\n3) Get Revisions");
            Console.Write("Select: ");
            return Console.ReadKey().KeyChar;
        }

        private static void Setup()
        {
            _documentStore = new DocumentStore
            {
                Url = "http://localhost:8080/",
                DefaultDatabase = "PizzaDB"
            }.Initialize();

            _pizzaRepository = new PizzaRepository(_documentStore);
        }

        private static void Teardown()
        {
            _documentStore.Dispose();
        }
    }
}
