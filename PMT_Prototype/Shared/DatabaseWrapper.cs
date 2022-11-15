using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PMT_Prototype.Shared
{
    public static class DatabaseWrapper
    {
        private static Dictionary<DatabaseEnvironment, string> connectionStrings = new()
        {
            {DatabaseEnvironment.Test, "Server=SIPMTPD01;Database=AAIC_DRAE;Integrated Security=True" }
        };

        public static async Task RunNonQueryAsync(string sql, DatabaseEnvironment env, CancellationToken ct, Dictionary<string, object>? parameters = null)
        {
            using SqlConnection connection = new(connectionStrings[env]);
            await connection.OpenAsync();

            try
            {
                SqlCommand cmd = new(sql, connection);
                if(parameters != null && parameters.Count > 0) 
                {
                    
                    foreach (var parameter in parameters)
                    {
                        if (!parameter.Key.Contains('@')) throw new InvalidDataException($"Parameter {parameter.Key} formatted incorrectly. Please reformat the paramter.");

                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }
                await cmd.ExecuteNonQueryAsync(ct);
            }
            catch (Exception ex)
            {
                await Logger.Log(ex, Logger.LogLevel.Error, ct);
            }
            finally { await connection.CloseAsync(); }

        }

        public static async Task<DataTable> RunQueryAsync(string sql, DatabaseEnvironment env, CancellationToken ct, Dictionary<string, object>? parameters = null)
        {
            using SqlConnection connection = new(connectionStrings[env]);
            await connection.OpenAsync();

            try
            {
                SqlCommand cmd = new(sql, connection);
                if (parameters != null && parameters.Count > 0)
                {

                    foreach (var parameter in parameters)
                    {
                        if (!parameter.Key.Contains('@')) throw new InvalidDataException($"Parameter {parameter.Key} formatted incorrectly. Please reformat the paramter.");

                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                using SqlDataReader reader = await cmd.ExecuteReaderAsync(ct);

                var resultTable = new DataTable();
                resultTable.Load(reader);
                return resultTable;

            }
            catch (Exception ex)
            {
                await Logger.Log(ex, Logger.LogLevel.Error, ct);
            }
            finally { await connection.CloseAsync(); }

            return new DataTable();
        }

        public static async Task<List<T>> RunQueryAsync<T>(string sql, DatabaseEnvironment env, CancellationToken ct, Dictionary<string, object>? parameters = null) where T : new()
        {
            using SqlConnection connection = new(connectionStrings[env]);
            await connection.OpenAsync();

            try
            {
                SqlCommand cmd = new(sql, connection);
                if (parameters != null && parameters.Count > 0)
                {

                    foreach (var parameter in parameters)
                    {
                        if (!parameter.Key.Contains('@')) throw new InvalidDataException($"Parameter {parameter.Key} formatted incorrectly. Please reformat the paramter.");

                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }

                using SqlDataReader reader = await cmd.ExecuteReaderAsync(ct);

                var resultTable = new DataTable();
                resultTable.Load(reader);

                return CreateListFromTable<T>(resultTable);

            }
            catch (Exception ex)
            {
                await Logger.Log(ex, Logger.LogLevel.Error, ct);
            }
            finally { await connection.CloseAsync(); }

            return new List<T>();
        }

        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r));
            }

            // return the list
            return lst;
        }

        // function that creates an object from the given data row
        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                if (item == null)
                {
                    throw new ArgumentNullException("item", "Null item passed in SetItemFromRow");
                }

                if (c == null || string.IsNullOrWhiteSpace(c.ColumnName))
                {
                    throw new ArgumentNullException("ColumnName", "Null item in row.Table.Columns in SetItemFromRow");
                }

                try
                {
                    PropertyInfo p = item.GetType().GetProperty(c.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    // if exists, set the value
                    if (p != null && row[c] != DBNull.Value)
                    {
                        p.SetValue(item, row[c], null);
                    }

                }
                catch (ArgumentNullException)
                {
                    throw new ArgumentNullException("name", $"Property with name {c.ColumnName} not found");
                }
                


            }
        }

        

        public enum DatabaseEnvironment
        {
            Test,
            Production
        }

    }
}
