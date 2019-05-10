using Helpers.Settings;
using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.DB
{
    public class DbEntityInit
    {
        public DbEntityInit(SqlConnection connection)
        {
            _connection = Guard.GetNotNull(connection, "connection");
        }

        public int Init()
        {
            try
            {
                return _connection.ExecuteAutoOpenClose(() =>
                {
                    using (var cmd = _connection.CreateCommand())
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.Init";
                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@spid",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output
                        });
                        cmd.Parameters.Add(new SqlParameter
                        {
                            ParameterName = "@version",
                            Value = CommonSettings.AssVersion,
                            SqlDbType = SqlDbType.NVarChar,
                            Size = 100,
                            Direction = ParameterDirection.Input
                        });

                        cmd.ExecuteNonQuery();

                        return (int)cmd.Parameters["@spid"].Value;
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка инициализации БД ДВК. " + ex.Message);
            }
        }

        private readonly SqlConnection _connection;


    }


}
