using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        public static async Task RunQueryAsync(string sql, DatabaseEnvironment env, CancellationToken ct, Dictionary<string, object>? parameters = null)
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
                await cmd.ExecuteNonQueryAsync(ct);
            }
            catch (Exception ex)
            {
                await Logger.Log(ex, Logger.LogLevel.Error, ct);
            }
            finally { await connection.CloseAsync(); }
        }

        public enum DatabaseEnvironment
        {
            Test,
            Production
        }

    }
}
