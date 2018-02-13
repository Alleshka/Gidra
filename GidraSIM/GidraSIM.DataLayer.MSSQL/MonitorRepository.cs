using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSIM.Core.Model.Resources;

namespace GidraSIM.DataLayer.MSSQL
{
    public class MonitorRepository:IResourcesRepository<Monitor>
    {
        private readonly string _connectionString;

        public MonitorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Monitor Create(Monitor newResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Monitors_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Diagonal", newResources.Diagonal);
                    sqlCommand.Parameters.AddWithValue("@Price", newResources.Price);
                    var result = newResources;
                    result.ID = Convert.ToInt16(sqlCommand.ExecuteScalar());
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
                    sqlCommand.CommandText = "Resources.Monitors_Delete";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@MonitorId", id);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public Monitor Update(Monitor updateResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Monitors_Update ";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@MonitorId", updateResources.ID);
                    sqlCommand.Parameters.AddWithValue("@Diagonal", updateResources.Diagonal);
                    sqlCommand.Parameters.AddWithValue("@Price", updateResources.Price);
                    sqlCommand.ExecuteNonQuery();
                    return updateResources;
                }
            }
        }

        public IEnumerable<Monitor> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Monitors_Getall";
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

        public IEnumerable<Monitor> Get(short id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Monitors_Get";
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

        public Monitor Parse(SqlDataReader reader)
        {
            return new Monitor
            {
                ID = reader.GetInt16(reader.GetOrdinal("MonitorId")),
                Diagonal = reader.GetByte(reader.GetOrdinal("Diagonal")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            };
        }
    }
}
