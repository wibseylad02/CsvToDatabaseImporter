using System.Data;

namespace CsvToDatabaseImporter.Interfaces
{
    public interface IDbDataTableLoader
    {
        /// <summary>
        /// Checks if a table name is valid for use with the database.
        /// </summary>
        /// <param name="specifiedTableName">The table name to validate</param>
        /// <returns><see langword="true"/> if valid, otherwise <see langword="false"/></returns>
        /// <remarks>Table names must start with a letter and can only contain letters, digits, and underscores.</remarks>
        /// <exception cref="ArgumentException"> if the specified table name is null or empty</exception>
        bool IsTableNameValid(string specifiedTableName);

        /// <summary>
        /// Gets a DataTable with the name root of the specified input data file after import to the database.
        /// </summary>
        /// <param name="specifiedTableName"></param>
        /// <returns>The <see cref="DataTable"/> of the specified name</returns>
        DataTable GetSpecifiedDataTable(string specifiedTableName);
    }
}
