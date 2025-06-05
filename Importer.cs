using CsvToDatabaseImporter.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Data;

namespace CsvToDatabaseImporter
{
    /// <summary>
    /// Worker class for importing data from CSV or other delimited files into a SQL Server database.
    /// </summary>
    public class Importer : IImporter
    {
        public string[] Delimiters { get; set; } = new string[] { "," }; // Default delimiter for CSV files
        public string DbConnectionString { get ; private set; } = ""; // Default connection string, can be set externally when mocking

        public bool CheckFileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// Gets a DataTable from a specified input file (e.g. CSV, TXT).
        /// </summary>
        /// <param name="inputFilePath">The path of the input file.</param>
        /// <returns>A DataTable containing the data from the input file.</returns>
        public DataTable GetDataTableFromInputFile(string inputFilePath)
        {
            DataTable? csvData = null;
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(inputFilePath))
                {
                    csvReader.SetDelimiters(Delimiters);
                    csvReader.HasFieldsEnclosedInQuotes = false;

                    var colFields = csvReader.ReadFields();

                    if (colFields == null || colFields.Length == 0)
                    {
                        throw new Exception("Data file to import is empty or not properly formatted.");
                    }

                    csvData = new DataTable();

                    foreach (string column in colFields)
                    {
                        DataColumn dataColumn = new DataColumn(column);
                        dataColumn.AllowDBNull = true;
                        csvData.Columns.Add(dataColumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        var fieldData = csvReader.ReadFields();

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
            catch (Exception)
            {
                throw;
            }

            return csvData;

        }

        /// <summary>
        /// Inserts data from a DataTable into a SQL Server database using SqlBulkCopy.
        /// </summary>
        /// <param name="csvFileData">A <see cref="DataTable"/> created from parsing a .csv file</param>
        /// <param name="destinationTableName">The database table to write to</param>
        public void InsertDataIntoDatabase(DataTable csvFileData, string destinationTableName)
        {
            // This would typically be stored in app.config or an environment variable.
            DbConnectionString = ConfigurationManager.ConnectionStrings["VehiclesDb"].ToString();

            using (SqlConnection dbConnection = new SqlConnection(DbConnectionString))
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

        /// <summary>
        /// Gets the target database name from a full CSV (or other text file type) file path.
        /// </summary>
        /// <param name="fullFileName">The full path of the file.</param>
        /// <returns>The target database name derived from the CSV or text file path.</returns>
        public string GetTargetDbNameFromFullFileName(string fullFileName)
        {
            // Assuming the file name is in the format "TableName.csv"
            // This method extracts the table name from the file name
            var fileNameWithExtension = Path.GetFileName(fullFileName);
            var fileName = Path.GetFileNameWithoutExtension(fullFileName);
            return fileName;
        }
    }
}
