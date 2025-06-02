using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Data;

namespace CsvToDatabaseImporter
{
    /// <summary>
    /// This helper class provides methods to import CSV files into a SQL Server database.
    /// </summary>
    internal static class Importer
    {
        public static DataTable GetDataTableFromCSVFile(string csvFilePath)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csvFilePath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = false;

                    string[] colFields = csvReader.ReadFields();

                    if (colFields == null || colFields.Length == 0)
                    {
                        throw new Exception("CSV file is empty or not properly formatted.");
                    }

                    foreach (string column in colFields)
                    {
                        DataColumn dataColumn = new DataColumn(column);
                        dataColumn.AllowDBNull = true;
                        csvData.Columns.Add(dataColumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        //Mark empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return csvData;
        }

        /// <summary>
        /// Inserts data from a DataTable into a SQL Server database using SqlBulkCopy.
        /// </summary>
        /// <param name="csvFileData">A <see cref="DataTable"/> created from parsing a .csv file</param>
        /// <param name="destinationTableName">The database table to write to</param>
        public static void InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData, string destinationTableName)
        {
            // This would typically be stored in a configuration file such as app.config or environment variable.
            var connectionString = ConfigurationManager.ConnectionStrings["VehiclesDb"].ToString();

            //using (SqlConnection dbConnection = new SqlConnection("data source=DESKTOP-7CLD65E\\SQLEXPRESS;initial catalog=Vehicles;trusted_connection=true;"))
            using (SqlConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = destinationTableName;
                    foreach (var column in csvFileData.Columns)
                    {
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    }
                    s.WriteToServer(csvFileData);
                }

                // The using statement should take care of closing the connection automatically, but if it doesn't, we can close it explicitly.
                if (dbConnection.State == ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
        }

        public static string GetTargetDbNameFromFullFileName(string fullFileName)
        {
            // Assuming the file name is in the format "TableName.csv"
            // This method extracts the table name from the file name
            var fileNameWithExtension = Path.GetFileName(fullFileName);
            var fileName = Path.GetFileNameWithoutExtension(fullFileName);
            return fileName;
        }
    }
}

