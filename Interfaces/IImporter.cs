using System.Data;

namespace CsvToDatabaseImporter.Interfaces
{
    public interface IImporter
    {
        /// <summary>
        /// Gets or sets the delimiters used for parsing CSV or other delimited files.
        /// </summary>
        string[] Delimiters { get; set; }

        /// <summary>
        /// Gets the database connection string used for importing data.
        /// </summary>
        string DbConnectionString { get; }

        /// <summary>
        /// Checks if a file exists at the specified file path.
        /// </summary>
        /// <param name="filePath">The full path to the file</param>
        /// <returns><see langword="true"/> if the file exists, otherwise <see langword="false"/></returns>
        bool CheckFileExists(string filePath);

        /// <summary>
        /// Gets a DataTable from a specified input file (e.g. CSV, TXT).
        /// </summary>
        /// <param name="inputFilePath">The path of the input file.</param>
        /// <returns>A DataTable containing the data from the input file.</returns>
        DataTable GetDataTableFromInputFile(string inputFilePath);

        /// <summary>
        /// Inserts data into the database from a DataTable.
        /// </summary>
        /// <param name="csvFileData">The DataTable containing data to insert.</param>
        /// <param name="destinationTableName">The name of the destination table in the database.</param>
        void InsertDataIntoDatabase(DataTable csvFileData, string destinationTableName);

        /// <summary>
        /// Gets the target database name from a full CSV (or other text file type) file path.
        /// </summary>
        /// <param name="fullFileName">The full path of the file.</param>
        /// <returns>The target database name derived from the CSV or text file path.</returns>
        string GetTargetDbNameFromFullFileName(string fullFileName);
    }
}
