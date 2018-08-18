using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace ReconsileProcess
{
    public static class DBContext
    {
       readonly static string oConnString = ConfigurationManager.ConnectionStrings["ReconsileConnection"].ConnectionString;
        public static List<T>  ExecuteTransactional<T>(string commandName, List<SqlParameter> parameters = null, string outputPara = "") where T : new()
        {
            
            List<T> objects = new List<T>();
            using (SqlConnection con = new SqlConnection(oConnString))
            {
                using (SqlCommand command = new SqlCommand(commandName, con))
                {
                    command.CommandText = commandName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    con.Open();
                    var trans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
                    try
                    {
                        command.Transaction = trans;
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
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
                            }
                        }
                        return objects;
                    }
                    finally
                    {
                        try
                        {
                            trans.Commit();
                        }
                        catch (InvalidOperationException)
                        {
                            // transaction was already closed or disposed... moving along...
                            // TODO: this was always happening when calling from ClipFunctions.GetClipsFromMaterial. Could not find out why...
                        }
                        con.Close();
                    }
                }
            }
        }
        public static int ExecuteTransactionalNonQuery(string commandName, List<SqlParameter> parameters = null, string outputPara = "")
        {
            using (SqlConnection con = new SqlConnection(oConnString))
            {
                using (var command = con.CreateCommand())
                {
                    command.CommandText = commandName;
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters.ToArray());

                    }
                    con.Open();
                    var trans = con.BeginTransaction(IsolationLevel.ReadUncommitted);
                    try
                    {
                        command.Transaction = trans;
                        return command.ExecuteNonQuery();
                    }
                    finally
                    {
                        try
                        {
                            trans.Commit();
                        }
                        catch (InvalidOperationException)
                        {
                            // transaction was already closed or disposed... moving along...
                            // TODO: this was always happening when calling from ClipFunctions.GetClipsFromMaterial. Could not find out why...
                        }
                        con.Close();
                    }
                }
            }
        }
    }
}
