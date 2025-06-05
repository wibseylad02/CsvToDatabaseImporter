using CsvToDatabaseImporter;
using NUnit.Framework;

namespace CsvImporterTests
{
    [TestFixture]
    public class DbDataTableLoaderTests
    {
        private const string DefaultFilePathRoot = @"C:\AlphaGraphicsTechTest\"; // Root path for test files    

        private DbDataTableLoader _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _systemUnderTest = new DbDataTableLoader();
        }


        [Test, Description("Test loading a SQL Server table of a given name")]
        [TestCase("Vehicles", true)] // Assuming this table exists in the database
        [TestCase("NonExistentTable", false)] // Assuming this table does not exist in the database
        public void LoadDataTableFromDatabase_ValidTableName_Success(string tableName, bool hasRows)
        {
            // TODO: Get the actual database connection loaded correctly from the unit tests app.config within the DbDataTableLoader function.
            // Arrange

            // Act
            var dataTable = _systemUnderTest.GetSpecifiedDataTable(tableName);
            
            // Assert
            Assert.That(dataTable, Is.Not.Null, "The DataTable should not be null after loading from the database.");
            Assert.That(dataTable.TableName, Is.EqualTo(tableName), "The DataTable should match the specified table name.");
            Assert.That(dataTable.Rows.Count, hasRows ? Is.GreaterThan(0) : Is.EqualTo(0), "The DataTable should contain rows.");
        }

        [Test, Description("Test an invalid or blank input table name should throw an ArgumentException")]
        [TestCase("NonExistentFile.csv")]   // comma is invalid
        [TestCase("NonExistentFile#txt")]   // hash is invalid
        [TestCase("NonExistentFile&")]      // ampersand is invalid
        [TestCase("2NonExistentTable")]     // initial digit is invalid
        [TestCase("23NonExistentTable")]    // initial digits are invalid
        [TestCase("")] // no table name
        public void IsTableNameValid_InvalidOrBlankName_Failure(string inputTableName)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _systemUnderTest.GetSpecifiedDataTable(inputTableName),
                $"Expected an ArgumentException for the table name '{inputTableName}'.");
        }

        [Test, Description("Test a valid input table name should be acceptable")]
        [TestCase("NonExistentTable")]   // letters only
        [TestCase("NonExistentTable2")]  // letters and non-initial digits
        [TestCase("NonExistent_Table")]  // underscore
        public void IsTableNameValid_ValidName_Success(string inputTableName)
        {
            // Act
            var isValid = _systemUnderTest.IsTableNameValid(inputTableName);

            // Assert
            Assert.That(isValid, Is.True, $"Input table name '{inputTableName}' should be valid");
        }
    }
}
