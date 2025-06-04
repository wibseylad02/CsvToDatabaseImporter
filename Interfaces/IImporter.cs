using System.Data;

namespace CsvToDatabaseImporter.Interfaces
{
    public interface IImporter
    {
        string[] Delimiters { get; set; }

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
