using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using CsvToDatabaseImporter.Interfaces;

namespace CsvToDatabaseImporter
{
    public class DbDataTableLoader : IDbDataTableLoader
    {
        public string DbConnectionString { get; private set; } = "";

        public bool IsTableNameValid(string specifiedTableName)
        {
            if (string.IsNullOrWhiteSpace(specifiedTableName))
            {
                throw new ArgumentException("The specified table name cannot be null or empty.", nameof(specifiedTableName));
            }

            if (!char.IsLetter(specifiedTableName[0]) || specifiedTableName.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            {
                throw new ArgumentException("The specified table name must start with a letter and can only contain letters, digits, and underscores.", 
                    nameof(specifiedTableName));
            }

            return true;
        }

        /// <summary>
        /// Gets a DataTable with the name root of the specified input data file after import to the database.
        /// </summary>
        /// <param name="specifiedTableName">The name of the database table, derived from the name of the original input data file</param>
        /// <returns>The <see cref="DataTable"/> of the specified name</returns>
        public DataTable GetSpecifiedDataTable(string specifiedTableName)
        {
            try
            {
                if (IsTableNameValid(specifiedTableName) == false)
                    throw new ArgumentException($"The specified table name '{specifiedTableName}' is not valid.", nameof(specifiedTableName));
            }
            catch (Exception)
            {
                throw;
            }


            // This would typically be stored in app.config or an environment variable.
            // The name of this connection string might need to be updated to match the database setup.
            // For the purposes of this example, we assume it is named "VehiclesDb".
            DbConnectionString = ConfigurationManager.ConnectionStrings["VehiclesDb"].ToString();

            // DEVELOPER NOTE: Although you no longer need to enlcose the contents of a using statement in curly braces,
            // it is still a good practice to do so for clarity of the scope and to ensure proper disposal of resources.
            using (var connection = new SqlConnection(DbConnectionString))
            {
                using (var command = new SqlCommand($"SELECT * FROM [{specifiedTableName}]", connection))
                {
                    connection.Open();

                    using var reader = command.ExecuteReader();
                    var dataTable = new DataTable(specifiedTableName);
                    dataTable.Load(reader);

                    return dataTable;
                }
            }
        }
    }
}
