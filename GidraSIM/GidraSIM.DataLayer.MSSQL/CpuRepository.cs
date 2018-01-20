using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GidraSim.Model.Resources;

namespace GidraSIM.DataLayer.MSSQL
{
    public class CpuRepository:IResourcesRepository<Cpu>
    {

        private readonly string _connectionString;

        public CpuRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Cpu Create(Cpu newResources)
        {
            using (var sqlConnection=new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand=sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = $"";
                    return new Cpu();
                }
            }
        }

        public void Delete(short id)
        {
            throw new NotImplementedException();
        }

        public Cpu Update(Cpu updateResources)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cpu> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cpu> Get(short id)
        {
            throw new NotImplementedException();
        }
    }
}
