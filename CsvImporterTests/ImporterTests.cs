using CsvToDatabaseImporter;
using CsvToDatabaseImporter.Interfaces;
using Moq;
using System.Data;

namespace CsvImporterTests
{
    [TestFixture]
    public class ImporterTests
    {
        private const string DefaultFilePathRoot = @"C:\AlphaGraphicsTechTest\"; // Root path for test files;

        private Mock<IImporter> _importerMock;
        private IImporter _systemUnderTest;

        private string _fullPathToImporterAssembly;


        [SetUp]
        public void Setup()
        {
            _importerMock = new Mock<IImporter>();
            _systemUnderTest = new Importer();

            _fullPathToImporterAssembly = DefaultFilePathRoot;
        }

        [Test, Description("Test parsing the input filename into a database table name")]
        [TestCase(@"C:\AlphaGraphicsTechTest\Customers.csv", "Customers")]
        [TestCase(@"Vehicles.csv", "Vehicles")]
        [TestCase(@"Vehicles.txt", "Vehicles")]
        [TestCase(@"Vehicles", "Vehicles")] // no file extension
        [TestCase("", "")]  // empty file name
        public void GetTargetDbNameFromFullFileNameTest(string inputFileName, string expectedValue)
        {
            var actualValue = _systemUnderTest.GetTargetDbNameFromFullFileName(inputFileName);
            Assert.That(expectedValue, Is.EqualTo(actualValue));
        }

        [Test, Description("Test parsing the input filename into a database table name using the mock")]
        public void GetDataTableFromInputFile_ExistingFile_Success()
        {
            // Arrange
            const string inputFileName = "Vehicles.csv";
            var inputFilePath = DefaultFilePathRoot + inputFileName;

            if (!_systemUnderTest.CheckFileExists(inputFilePath))
            {
                Assert.Fail($"Test file '{inputFilePath}' does not exist. Please ensure the file is present in the specified path.");
            }          

            // Act
            DataTable actualDataTable = _systemUnderTest.GetDataTableFromInputFile(inputFilePath);

            // Assert
            Assert.That(actualDataTable, Is.Not.Null, $"DataTable derived from '{inputFileName}' should not be null");
            Assert.That(actualDataTable.Rows.Count, Is.GreaterThan(0));
            Assert.That(actualDataTable.Columns.Count, Is.GreaterThan(0));
        }

        [Test, Description("Test a non-existent or blank input filename into a database table name using the mock")]
        [TestCase("NonExistentFile.csv")]
        [TestCase("NonExistentFile.txt")]
        [TestCase("")] // no file name
        public void GetDataTableFromInputFile_NonExistingFile_Failure(string inputFileName)
        {
            // Arrange
            var inputFilePath = DefaultFilePathRoot + inputFileName;

            // Act
            var fileExists = _systemUnderTest.CheckFileExists(inputFilePath);

            // Assert
            Assert.That(fileExists, Is.False, $"Test file '{inputFilePath}' should not exist for this test case.");
        }

        [Test, Description("Test parsing the input filename into a database table name using the mock")]
        public void GetDataTableFromInputFile_NonExistentFileWithMocks_Success()
        {
            // Arrange
            // non existent file
            const string nonExistentFile = "Vehicles3";
            var inputFilePath = _fullPathToImporterAssembly + nonExistentFile + ".csv";

            DataTable expectedDataTable = GenerateMockDataTable(nonExistentFile);

            _importerMock.Setup(i => i.GetDataTableFromInputFile(inputFilePath)).Returns(expectedDataTable);

            // Act
            DataTable actualDataTable = _importerMock.Object.GetDataTableFromInputFile(inputFilePath);

            // Assert
            Assert.That(actualDataTable, Is.Not.Null, $"DataTable derived from '{nonExistentFile}' should not be null");
            Assert.That(actualDataTable.Rows.Count, Is.EqualTo(expectedDataTable.Rows.Count));
            Assert.That(actualDataTable.Columns.Count, Is.EqualTo(expectedDataTable.Columns.Count));
        }


        [Test, Description("Test importing a database table name into SQL Server using the mock")]
        public void BulkCopyDataIntoDatabase_Success()
        {
            // Arrange
            var inputFilePath = _fullPathToImporterAssembly + "Vehicles.csv";
            if (!File.Exists(inputFilePath))
            {
                Assert.Fail($"Test file '{inputFilePath}' does not exist. Please ensure the file is present in the specified path.");
            }

            DataTable csvData = _systemUnderTest.GetDataTableFromInputFile(inputFilePath);
            string destinationTableName = _systemUnderTest.GetTargetDbNameFromFullFileName(inputFilePath);

            // Act
            try
            {
                _systemUnderTest.InsertDataIntoDatabase(csvData, destinationTableName);

                // Assert
                // Here you would typically check the database to confirm the data was inserted correctly.
                // This is a placeholder assertion as we cannot check the database in this unit test.
                Assert.Pass("Data inserted into database successfully.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Data insertion into database failed: {ex.Message}");
            }
        }


        [Test, Description("Test importing a database table name into SQL Server using the mock")]
        public void BulkCopyDataIntoDatabase_MockConnection_Success()
        {
            // Arrange
            const string mockFileName = "Vehicles4";
            var inputFilePath = _fullPathToImporterAssembly + mockFileName + ".csv";

            DataTable csvData = GenerateMockDataTable(mockFileName);

            string destinationTableName = _systemUnderTest.GetTargetDbNameFromFullFileName(inputFilePath);

            _importerMock.SetupGet(i => i.DbConnectionString).Returns($"Server=localhost;Database={destinationTableName};Trusted_Connection=True;");

            // Act
            _importerMock.Object.InsertDataIntoDatabase(csvData, destinationTableName);
            //_importerReal.InsertDataIntoDatabase(csvData, destinationTableName);

            // Assert
            // Here you would typically check the database to confirm the data was inserted correctly.
            // This is a placeholder assertion as we cannot check the database in this unit test.
            Assert.Pass("Data inserted into database successfully.");            
        }


        // make this internal so that other tests can use it
        internal static DataTable GenerateMockDataTable(string tableName = "")
        {
            DataTable expectedDataTable = new DataTable(tableName);
            expectedDataTable.Columns.Add("Make");
            expectedDataTable.Columns.Add("Model");
            expectedDataTable.Columns.Add("Colour");
            expectedDataTable.Columns.Add("Registration");

            expectedDataTable.Rows.Add("Ford", "Fiesta", "Blue", "FF72 BLU");
            expectedDataTable.Rows.Add("Audi", "A1", "Green", "AU71 GRE");
            return expectedDataTable;
        }
    }
}