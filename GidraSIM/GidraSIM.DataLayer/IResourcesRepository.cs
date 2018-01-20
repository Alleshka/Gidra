using System.Collections.Generic;

namespace GidraSIM.DataLayer
{
    public interface IResourcesRepository<T>
    {
        T Create(T newResources);   

        void Delete(short id);

        T Update(T updateResources);

        IEnumerable<T> GetAll();

        IEnumerable<T> Get(short id);
    }       
}
