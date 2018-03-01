using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.DataLayer.MSSQL
{
    public class SoftwareRepository:IResourcesRepository<Software>
    {
        private readonly string _connectionString;

        public SoftwareRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Software Create(Software newResources)
        {

            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Softwares_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Type", EnumExtension.Description<TypeSoftware>(newResources.Type));
                    sqlCommand.Parameters.AddWithValue("@Name", newResources.Name);
                    sqlCommand.Parameters.AddWithValue("@LicenseForm", EnumExtension.Description<TypeLicenseForm>(newResources.LicenseForm));
                    sqlCommand.Parameters.AddWithValue("@LicenseStatus", newResources.LicenseStatus.ToString());
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
                    sqlCommand.CommandText = "Resources.Softwares_Delete";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@SoftwareId", id);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public Software Update(Software updateResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Softwares_Update";
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@SoftwareId", updateResources.ID);
                    sqlCommand.Parameters.AddWithValue("@Type", EnumExtension.Description<TypeSoftware>(updateResources.Type));
                    sqlCommand.Parameters.AddWithValue("@Name", updateResources.Name);
                    sqlCommand.Parameters.AddWithValue("@LicenseForm", EnumExtension.Description<TypeLicenseForm>(updateResources.LicenseForm));
                    sqlCommand.Parameters.AddWithValue("@LicenseStatus", updateResources.LicenseStatus);
                    sqlCommand.Parameters.AddWithValue("@Price", updateResources.Price);
                    sqlCommand.ExecuteNonQuery();
                    return updateResources;
                }
            }
        }

        public IEnumerable<Software> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Softwares_Getall";
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

        public IEnumerable<Software> Get(short id)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Resources.Softwares_Get";
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

        public Software Parse(SqlDataReader reader)
        {
            return new Software
            {
                ID = reader.GetInt16(reader.GetOrdinal("SoftwareId")),
                Type = EnumExtension.GetEnum<TypeSoftware>(reader.GetString(reader.GetOrdinal("Type"))),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                LicenseForm = EnumExtension.GetEnum<TypeLicenseForm>(reader.GetString(reader.GetOrdinal("LicenseForm"))),
                LicenseStatus = reader.GetString(reader.GetOrdinal("LicenseStatus")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price"))
            };
        }
    }
}
