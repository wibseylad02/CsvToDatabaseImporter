AUTHOR
David Grant

VERSION
1.0.0

OVERVIEW
This is an application which allows you to import a data text file (e.g. CSV) into a SQL Server database of the same name.

INSTALLATION INSTRUCTIONS

1. Ensure that there is at least one CSV or tab delimited TXT file in the DataImportFiles folder.  The file should contain a header row with the field names. A sample Vehicles.csv file is included with this project.
2. In the app.config file, amend the Server name in the connection string to point to your SQL Server instance. The default is set to use Windows Authentication.


RUNNING THE APPLICATION
1. Run the application, either as the compiled .exe or via Visual Studio. 

The application will read the files in the DataImportFiles folder and import them into a SQL Server database with the same name as the file (without the extension). 
If the database does not exist, it will be created.


UNIT TESTS
Awaiting implementation.


KNOWN ISSUES
The app is currently compiled against .NET 8.0.  I'm looking to convert it to run against .NET Framework 4.8.1, however.


CONTACT DETAILS
Email: wibseylad02@gmail.com