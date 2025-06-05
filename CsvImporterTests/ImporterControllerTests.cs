using CsvToDatabaseImporter;
using CsvToDatabaseImporter.Interfaces;
using Moq;

namespace CsvImporterTests
{
    [TestFixture]
    public class ImporterControllerTests
    {
        private const string DefaultFilePathRoot = @"C:\AlphaGraphicsTechTest\"; // Root path for test files
        private const string DefaultFileExtension = ImporterController.DefaultFileExtension; 

        private Mock<IImporterController> _importerControllerMock;
        private IImporterController _systemUnderTest;
        private string _fullPathToImporterAssembly;

        [SetUp]
        public void Setup()
        {
            _fullPathToImporterAssembly = DefaultFilePathRoot;
            _importerControllerMock = new Mock<IImporterController>();

            // Initialize the real ImporterController with a default file path and file extension
            _systemUnderTest = new ImporterController(_fullPathToImporterAssembly, DefaultFileExtension);
            _systemUnderTest.ShowMessages = false; // Don't show message boxes in unit tests!!!
        }

        [Test, Description("Test importing text files into the database with matching table names")]
        public void ImportCsvFiles_ValidFiles_Success()
        {
            // Arrange
            string selectedFolderPath = _fullPathToImporterAssembly;

            // Act
            _systemUnderTest.ImportCsvFiles(selectedFolderPath);

            var databaseResults = _systemUnderTest.GetImportedDataTableFromInputFile("Vehicles.csv");

            // Assert
            Assert.That(databaseResults, Is.Not.Null, "The DataTable should not be null after import.");
            Assert.That(databaseResults.TableName, Is.EqualTo("Vehicles"), "The DataTable should match the input name stem after import.");
        }
    }
}
