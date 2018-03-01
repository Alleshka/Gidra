using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.DataLayer.MSSQL
{
    public class InputRepository : IResourcesRepository<InputDevices>
    {
        private readonly string _connectionString;

        public InputRepository(String connectionString)
        {
            _connectionString = connectionString;
        }

        public InputDevices Create(InputDevices newResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "InputDevices_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Type", EnumExtension.Description<TypeInputDevices>(newResources.Type));
                    sqlCommand.Parameters.AddWithValue("@Price", newResources.Price);
                    var result = newResources;
                    result.ID = Convert.ToInt16(sqlCommand.ExecuteScalar());
                    return result;
                }
            }
        }

        public void Delete(short id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InputDevices> Get(short id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InputDevices> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "InputDevices_Getall";
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

        public InputDevices Parse(SqlDataReader reader)
        {
            return new InputDevices()
            {
                ID = reader.GetInt16(reader.GetOrdinal("InputDevicesId")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Type = EnumExtension.GetEnum<TypeInputDevices>(reader.GetString(reader.GetOrdinal("Type")))
            };
        }

        public InputDevices Update(InputDevices updateResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "InputDevices_Update";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Type", EnumExtension.Description<TypeInputDevices>(updateResources.Type));
                    sqlCommand.Parameters.AddWithValue("@Price", updateResources.Price);
                    sqlCommand.ExecuteNonQuery();
                    return updateResources;
                }
            }
        }
    }
}