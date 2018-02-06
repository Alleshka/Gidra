using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSim.Model.Resources;

namespace GidraSIM.DataLayer.MSSQL
{
    public class GpuRepository:IResourcesRepository<Gpu>
    {

        private readonly string _connectionString;

        public GpuRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Gpu Create(Gpu newResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.CPU_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Memory", newResources.Memory);
                    sqlCommand.Parameters.AddWithValue("@Frequency", newResources.Frequency);
                    sqlCommand.Parameters.AddWithValue("@Price", newResources.Price);
                    var result = newResources;
                    result.GpuId = Convert.ToInt16(sqlCommand.ExecuteScalar());
                    return result;
                }
            }
        }

        public void Delete(short id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.GPU_Delete";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@GPUId", id);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public Gpu Update(Gpu updateResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.GggggggggggPU_Update";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@GPUId", updateResources.GpuId);
                    sqlCommand.Parameters.AddWithValue("@Memory", updateResources.Memory);
                    sqlCommand.Parameters.AddWithValue("@Frequency", updateResources.Frequency);
                    sqlCommand.Parameters.AddWithValue("@Price", updateResources.Price);
                    sqlCommand.ExecuteNonQuery();
                    return updateResources;
                }
            }
        }

        public IEnumerable<Gpu> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.GPU_Getall";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return Parse(reader);
                        }
                    }
                }
            }
        }

        public IEnumerable<Gpu> Get(short id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.GPU_Get";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@TechnicalSupportId", id);
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return Parse(reader);
                        }
                    }
                }
            }
        }

        public Gpu Parse(SqlDataReader reader)
        {
            return new Gpu
            {
                GpuId = reader.GetInt16(reader.GetOrdinal("GPUId")),
                Memory = reader.GetByte(reader.GetOrdinal("Memory")),
                Frequency = reader.GetInt16(reader.GetOrdinal("Frequency")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            };
        }
}
