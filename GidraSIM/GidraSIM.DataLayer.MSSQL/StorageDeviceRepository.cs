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
    public class StorageDeviceRepository:IResourcesRepository<StorageDevice>
    {

        private readonly string _connectionString;

        public StorageDeviceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }   

        public StorageDevice Create(StorageDevice newResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.StorageDevices_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@SpeedRead", newResources.SpeedRead);
                    sqlCommand.Parameters.AddWithValue("@SpeedWrite", newResources.SpeedWrite);
                    sqlCommand.Parameters.AddWithValue("@Size", newResources.Size);
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
                    sqlCommand.CommandText = "Resources.StorageDevices_Delete";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@StorageDeviceId", id);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public StorageDevice Update(StorageDevice updateResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.StorageDevices_Update";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@StorageDeviceId", updateResources.ID);
                    sqlCommand.Parameters.AddWithValue("@SpeedWrite", updateResources.SpeedWrite);
                    sqlCommand.Parameters.AddWithValue("@SpeedRead", updateResources.SpeedRead);
                    sqlCommand.Parameters.AddWithValue("@SpeedRead", updateResources.Size);
                    sqlCommand.Parameters.AddWithValue("@Price", updateResources.Price);
                    sqlCommand.ExecuteNonQuery();
                    return updateResources;
                }
            }
        }

        public IEnumerable<StorageDevice> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.StorageDevices_Getall";
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

        public IEnumerable<StorageDevice> Get(short id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.StorageDevices_Get";
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

        public StorageDevice Parse(SqlDataReader reader)
        {
            return new StorageDevice
            {
                ID = reader.GetInt16(reader.GetOrdinal("StorageDeviceId")),
                SpeedRead = reader.GetInt16(reader.GetOrdinal("SpeedRead")),
                SpeedWrite = reader.GetInt16(reader.GetOrdinal("SpeedWrite")),
                Size = reader.GetInt16(reader.GetOrdinal("Size")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            };
        }
    }
}
