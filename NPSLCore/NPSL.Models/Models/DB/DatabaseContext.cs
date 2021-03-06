﻿using NPSLCore.Models.DB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace NPSL.Models.Models.DB
{
    public class DatabaseContext
    {
        private readonly NPSLContext _DataAccess;

        public DatabaseContext(NPSLContext dataaccess)
        {
            _DataAccess = dataaccess;
        }
        public List<T> ExecuteTransactional<T>(string commandName, List<SqlParameter> parameters = null, string outputPara = "") where T : new()
        {
            List<T> objects = new List<T>();
            using (var command = _DataAccess.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = commandName;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                _DataAccess.Database.GetDbConnection().Open();
                var trans = _DataAccess.Database.GetDbConnection().BeginTransaction(IsolationLevel.ReadUncommitted);
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
                                        PropertyInfo propertyInfo = typeof(T).GetProperty(reader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
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
                    _DataAccess.Database.GetDbConnection().Close();
                }
            }
        }
        public async Task ExecuteTransactionalNonQueryAsync(string commandName, List<SqlParameter> parameters = null, string outputPara = "")
        {
            using (var command = _DataAccess.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = commandName;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());

                }
             
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                var trans = _DataAccess.Database.GetDbConnection().BeginTransaction(IsolationLevel.ReadUncommitted);
                try
                {
                    command.Transaction = trans;
                    await command.ExecuteNonQueryAsync();
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
                    _DataAccess.Database.GetDbConnection().Close();
                }
            }
        }
        public int  ExecuteTransactionalNonQuery(string commandName, List<SqlParameter> parameters = null, string outputPara = "")
        {
            using (var command = _DataAccess.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = commandName;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());

                }
                _DataAccess.Database.GetDbConnection().Open();
                var trans = _DataAccess.Database.GetDbConnection().BeginTransaction(IsolationLevel.ReadUncommitted);
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
                    _DataAccess.Database.GetDbConnection().Close();
                }
            }
        }

    }
}
