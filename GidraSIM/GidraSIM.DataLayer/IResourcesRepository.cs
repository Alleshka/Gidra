using System.Collections.Generic;
using System.Data.SqlClient;

namespace GidraSIM.DataLayer
{
    public interface IResourcesRepository<T>
    {
        T Create(T newResources);   

        void Delete(short id);

        T Update(T updateResources);

        IEnumerable<T> GetAll();

        IEnumerable<T> Get(short id);
        
        T Parse(SqlDataReader reader);
    }       
}
