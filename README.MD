AUTHOR
David Grant

VERSION
1.0.0

OVERVIEW
This is an application which allows you to import a data text file (e.g. CSV) into a SQL Server database of the same name.

INSTALLATION INSTRUCTIONS

Using the GitHub repository: 
https://github.com/wibseylad02/ CsvToDatabaseImporter

1. Download the repository as a ZIP file or clone it using Git, to C:\AlphaGraphicsTechTest.
2. Ensure that there is at least one CSV or tab delimited TXT file in the DataImportFiles folder.  The file should contain a header row with the field names. A sample Vehicles.csv file is included with this project.
3. In the app.config file, amend the Server name in the connection string to point to your SQL Server instance. The default is set to use Windows Authentication.


RUNNING THE APPLICATION
1. Run the application, either as the compiled .exe or via Visual Studio. 

The application will read the files in the DataImportFiles folder and import them into a SQL Server database with the same name as the file (without the extension). 
If the database does not exist, it will be created.


UNIT TESTS
These should be run within Visual Studio and the Test Explorer, using the CsvImporterTests project.


KNOWN ISSUES
1. The unit tests are unable to get the correct database connection string from the app.config file, so they are currently failing. However, the same connection string works correctly in the app.
2. The app is currently compiled against .NET 8.0 rather than .NET Framework 4.8.1.


CONTACT DETAILS
Email: wibseylad02@gmail.com