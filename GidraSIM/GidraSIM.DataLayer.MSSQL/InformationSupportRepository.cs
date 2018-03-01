using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.DataLayer.MSSQL
{
    public class InformationSupportRepository: IResourcesRepository<InformationSupport>
    {

        private readonly string _connectionString;

        public InformationSupportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public InformationSupport Create(InformationSupport newResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.InformationSupports_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@MultiClientUse", newResources.MultiClientUse);
                    sqlCommand.Parameters.AddWithValue("@Type", EnumExtension.Description<TypeIS>(newResources.Type));
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
                    sqlCommand.CommandText = "Resources.InformationSupports_Delete";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@InformationSupportId", id);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public InformationSupport Update(InformationSupport updateResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.InformationSupports_Update";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@InformationSupportId", updateResources.ID);
                    sqlCommand.Parameters.AddWithValue("@MultiClientUse", updateResources.MultiClientUse);
                    sqlCommand.Parameters.AddWithValue("@Type", EnumExtension.Description<TypeIS>(updateResources.Type));
                    sqlCommand.Parameters.AddWithValue("@Price", updateResources.Price);
                    sqlCommand.ExecuteNonQuery();
                    return updateResources;
                }
            }
        }

        public IEnumerable<InformationSupport> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.InformationSupports_Getall";
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

        public IEnumerable<InformationSupport> Get(short id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.InformationSupports_Get";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ProcedureId", id);
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

        public InformationSupport Parse(SqlDataReader reader)
        {
            return new InformationSupport
            {
                ID = reader.GetInt16(reader.GetOrdinal("InformationSupportId")),
                MultiClientUse = reader.GetBoolean(reader.GetOrdinal("MultiClientUse")),
                Type = EnumExtension.GetEnum<TypeIS>(reader.GetString(reader.GetOrdinal("Type"))),
                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            };
        }
    }
}
