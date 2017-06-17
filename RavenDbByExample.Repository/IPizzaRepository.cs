using RavenDbByExample.Repository.Entities;

namespace RavenDbByExample.Repository
{
    public interface IPizzaRepository
    {
        void Add(Pizza pizza);
        void Delete(string id);
        Pizza Get(string id);
    }
}