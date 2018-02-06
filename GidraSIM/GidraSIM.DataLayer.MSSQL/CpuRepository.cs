using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                    sqlCommand.CommandText = "Resources.CPU_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@QuantityCore", newResources.QuantityCore);
                    sqlCommand.Parameters.AddWithValue("@Frequency", newResources.Frequency);
                    sqlCommand.Parameters.AddWithValue("@Price", newResources.Price);
                    var result = newResources;
                    result.CpuId = Convert.ToInt16(sqlCommand.ExecuteScalar());
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
                    sqlCommand.CommandText = "Resources.CPU_Delete";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CPUId", id);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public Cpu Update(Cpu updateResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.CPU_Update";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CPUId", updateResources.CpuId);
                    sqlCommand.Parameters.AddWithValue("@QuantityCore", updateResources.QuantityCore);
                    sqlCommand.Parameters.AddWithValue("@Frequency", updateResources.Frequency);
                    sqlCommand.Parameters.AddWithValue("@Price", updateResources.Price);
                    sqlCommand.ExecuteNonQuery();
                    return updateResources;
                }
            }
        }

        public IEnumerable<Cpu> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.CPU_Getall";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    using (var reader=sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return Parse(reader);
                        }
                    }
                }
            }
        }

        public IEnumerable<Cpu> Get(short id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.CPU_Get";
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

        public Cpu Parse(SqlDataReader reader)
        {
            return new Cpu
            {
                CpuId = reader.GetInt16(reader.GetOrdinal("CPUId")),
                QuantityCore = reader.GetByte(reader.GetOrdinal("QuantityCore")),
                Frequency = reader.GetInt16(reader.GetOrdinal("Frequency")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            }; 
        }
    }
}
