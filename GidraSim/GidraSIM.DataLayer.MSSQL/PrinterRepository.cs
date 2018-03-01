using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GidraSIM.Core.Model.Resources;
using GidraSIM.Core.Model;

namespace GidraSIM.DataLayer.MSSQL
{
    public class PrinterRepository : IResourcesRepository<Printer>
    {
        private readonly string _connectionString;

        public PrinterRepository(String connectionString)
        {
            _connectionString = connectionString;
        }

        public Printer Create(Printer newResources)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Printers_Create";
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@Type", EnumExtension.Description<TypePrinter>(newResources.Type));
                    sqlCommand.Parameters.AddWithValue("@Speed", newResources.Speed);
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

        public IEnumerable<Printer> Get(short id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Printer> GetAll()
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = "Printers_Getall";
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

        public Printer Parse(SqlDataReader reader)
        {
            return new Printer()
            {
                ID = reader.GetInt16(reader.GetOrdinal("PrinterId")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                Speed = reader.GetByte(reader.GetOrdinal("Speed")),
                Type = EnumExtension.GetEnum<TypePrinter>(reader.GetString(reader.GetOrdinal("Type")))
            };
        }

        public Printer Update(Printer updateResources)
        {
            throw new NotImplementedException();
        }
    }
}
