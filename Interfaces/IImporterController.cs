using System.Data;

namespace CsvToDatabaseImporter.Interfaces
{
    /// <summary>
    /// Interface for functionality for importing CSV files into a database.
    /// </summary>
    public interface IImporterController
    {
        /// <summary>
        /// Flag to control whether messages are shown to the user, or logged instead.
        /// </summary>
        bool ShowMessages { get; set; }

        /// <summary>
        /// Imports CSV files from the specified folder into the database.
        /// </summary>
        void ImportCsvFiles(string selectedFolderPath);

        /// <summary>
        /// Gets the DataTable with the name root of the specified input data file after import to the database.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        DataTable GetImportedDataTableFromInputFile(string inputFile);

        /// <summary>
        /// Creates an instance of an <see cref="IImporter"/> to handle the import process for a data file.
        /// </summary>
        /// <returns>A <see cref="IImporter"/> instance</returns>
        IImporter CreateImporter();

        /// <summary>
        /// Creates an instance of a <see cref="IDbDataTableLoader"/> to handle loading DataTables from the database.
        /// </summary>
        /// <returns>A <see cref="IDbDataTableLoader"/> instance</returns>
        IDbDataTableLoader CreateDataTableLoader();

        /// <summary>
        /// Creates an instance of a <see cref="MessageReporting"/> to handle message reporting during the import process.
        /// </summary>
        /// <param name="showMessages">Indicates whether messages should be shown to the user or logged.</param>
        /// <returns></returns>
        MessageReporting CreateMessageReporting(bool showMessages);
    }
}
