using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace NPSLCore.Models.DB
{
    public class BaseDataAccess : DbContext
    {
        protected string ConnectionString { get; set; }
        private readonly IConfiguration _Connectionstring;

        public BaseDataAccess(IConfiguration connectionstring)
        {
            _Connectionstring = connectionstring;
            this.ConnectionString = _Connectionstring["ConnectionString:DBConnection"];
        }


        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        protected DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }

        protected SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
            parameterObject.Direction = ParameterDirection.Input;
            return parameterObject;
        }

        protected SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type); ;

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            if (value != null)
            {
                parameterObject.Value = value;
            }
            else
            {
                parameterObject.Value = DBNull.Value;
            }

            return parameterObject;
        }
        public object ExecuteProcedure(string procedureName, List<DbParameter> parameters)
        {
            object returnObject = null;

            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, CommandType.StoredProcedure);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnObject = cmd.ExecuteReader();
                }
            }
            catch (Exception)
            {
                //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);  
                throw;
            }

           
            return returnObject;
        }
      
        public List<T> ExecuteList<T>(string procedureName, List<DbParameter> parameters) where T : new()
        {
            List<T> objects = new List<T>();
            object returnObject = null;

            using (SqlConnection connection = this.GetConnection())
            {
                DbCommand cmd = this.GetCommand(connection, procedureName, CommandType.StoredProcedure);
                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                returnObject = cmd.ExecuteReader();
                IDataReader reader = (IDataReader)returnObject;
                while (reader.Read())
                {
                    T tempObject = new T();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.GetValue(i) != DBNull.Value)
                        {
                            PropertyInfo propertyInfo = typeof(T).GetProperty(reader.GetName(i));
                            propertyInfo.SetValue(tempObject, reader.GetValue(i), null);
                        }
                    }
                    objects.Add(tempObject);
                }
                reader.Close();
            }
            return objects;
        }

        protected int ExecuteNonQuery(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue = -1;

            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);  
                throw;
            }

            return returnValue;
        }

        protected object ExecuteScalar(string procedureName, List<SqlParameter> parameters)
        {
            object returnValue = null;

            try
            {
                using (DbConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, CommandType.StoredProcedure);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);  
                throw;
            }

            return returnValue;
        }

        public DbDataReader GetDataReader(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DbDataReader ds;

            try
            {
                DbConnection connection = this.GetConnection();
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    ds = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception)
            {
                //LogException("Failed to GetDataReader for " + procedureName, ex, parameters);  
                throw;
            }

            return ds;
        }
    }
}
