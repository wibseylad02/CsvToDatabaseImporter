using CsvToDatabaseImporter.Interfaces;

namespace CsvToDatabaseImporter
{
    /// <summary>   
    /// This class is responsible for importing CSV files into a database.
    /// </summary>    
    public class ImporterController : IImporterController
    {
        public const string DefaultFileExtension = "*.csv"; // Default file extension for CSV files

        private string _selectedFolderPath = string.Empty;
        private string _selectedFileExtension = DefaultFileExtension;

        /// <summary>
        /// Flag to control whether messages are shown to the user, or logged instead.
        /// </summary>
        public bool ShowMessages { get; set; } = true; // for future-proofing, this could be set to false to log messages instead of showing them in a message box

        /// <summary>
        /// Instantiate a new <see cref="ImporterController"/> with a specified folder path and default file extension of <see cref="DefaultFileExtension"/>.
        /// </summary>
        /// <param name="selectedFolderPath">The path of the folder containing CSV files.</param>
        public ImporterController(string selectedFolderPath)
        {
            _selectedFolderPath = selectedFolderPath;
        }

        /// <summary>
        /// Instantiate a new <see cref="ImporterController"/> with a specified folder path and file extension.
        /// </summary>
        /// <param name="selectedFolderPath">The path of the folder containing delimited files.</param>
        /// <param name="selectedFileExtension">The file extension of the delimited files to import (default is "*.csv").</param>
        public ImporterController(string selectedFolderPath, string selectedFileExtension) : this(selectedFolderPath)
        {
            _selectedFileExtension = selectedFileExtension;
        }


        /// <summary>
        /// Imports CSV files from the specified folder into the database using the default file extension of "*.csv".
        /// </summary>
        /// <param name="selectedFolderPath"></param>
        public void ImportCsvFiles(string selectedFolderPath)
        {
            ImportCsvFiles(selectedFolderPath, DefaultFileExtension);
        }

        /// <summary>
        /// Imports CSV files from the specified folder into the database.
        /// </summary>
        /// <param name="selectedFolderPath">The path of the folder containing CSV files.</param>
        /// <exception cref="ArgumentException"> if the folder name is blank</exception>
        /// <exception cref="DirectoryNotFoundException"> if the selected folder cannot be found</exception>
        public void ImportCsvFiles(string selectedFolderPath, string selectedFileExtension = DefaultFileExtension)
        {
            if (string.IsNullOrWhiteSpace(selectedFolderPath))
            {
                throw new ArgumentException("The selected folder path cannot be null or empty.", nameof(selectedFolderPath));
            }
            _selectedFolderPath = selectedFolderPath;
            _selectedFileExtension = selectedFileExtension;

            // Throw error if the directory exists, though this should not happen if the user has selected correctly in the UI
            if (!Directory.Exists(_selectedFolderPath))
            {
                throw new DirectoryNotFoundException($"The directory '{_selectedFolderPath}' does not exist.");
            }

            try
            {
                var importFiles = Directory.GetFiles(_selectedFolderPath, _selectedFileExtension);

                if (importFiles.Length == 0)
                {
                    CreateMessageReporting(ShowMessages).ShowMessage("No import files found in the selected folder.", "Information");
                    return;
                }

                foreach (var importFile in importFiles)
                {
                    var importer = CreateImporter();

                    if (_selectedFileExtension.Equals(".csv", StringComparison.OrdinalIgnoreCase) == false)
                    {
                        // If the file is not a CSV, you might want to set different delimiters or handle it differently
                        importer.Delimiters = new string[] { "\t" }; // Example for tab-delimited files, adjust as necessary
                    }

                    var importData = importer.GetDataTableFromInputFile(importFile);

                    if (importData != null && importData.Rows.Count > 0)
                    {
                        // Assuming the table name is derived from the CSV file name without extension and minus the selected folder name
                        importer.InsertDataIntoDatabase(importData, importer.GetTargetDbNameFromFullFileName(importFile));
                        CreateMessageReporting(ShowMessages).ShowMessage($"Successfully imported {importFile}", "Success");
                    }
                }
            }
            catch (Exception ex)
            {
                CreateMessageReporting(ShowMessages).ShowMessage($"An error occurred during import: {ex.Message}", "Error");
            }
        }

        /// <summary>
        /// Creates an instance of an <see cref="IImporter"/> to handle the import process for a data file.
        /// </summary>
        /// <returns>A <see cref="IImporter"/> instance, with a comma as the default delimiter</returns>
        public IImporter CreateImporter() => (IImporter)new Importer();

        /// <summary>
        /// Creates an instance of a <see cref="MessageReporting"/> to handle message reporting during the import process.
        /// </summary>
        /// <param name="showMessages">Indicates whether messages should be shown to the user via a <see cref="MessageBox"/> or logged.</param>
        /// <returns></returns>
        public MessageReporting CreateMessageReporting(bool showMessages)
        {
            return new MessageReporting(showMessages);
        }
    }
}
