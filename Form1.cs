using CsvToDatabaseImporter.Interfaces;

namespace CsvToDatabaseImporter
{
    public partial class CsvImporter : Form
    {
        private const string DefaultFilePath = @"C:\AlphaGraphics\";

        private string _selectedFolderPath = "";

        // TODO - Flag to control whether messages are shown to the user or logged instead.  Probably best set to true, since this is a GUI app, but review its usage
        private bool _showMessages = true; // Flag to control whether messages are shown to the user or logged instead.  Probably best set to true, since this is a GUI app

        public CsvImporter()
        {
            InitializeComponent();
        }

        private void CsvImporter_Load(object sender, EventArgs e)
        {
            btnImportFiles.Enabled = false;
            btnShowData.Enabled = false;

            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.InitialDirectory = DefaultFilePath;
            folderBrowserDialog1.Description = "Select the folder containing the CSV files to import.";
            folderBrowserDialog1.ShowNewFolderButton = true;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _selectedFolderPath = folderBrowserDialog1.SelectedPath;
                txtFolderPath.Text = _selectedFolderPath;

                btnImportFiles.Enabled = true;
            }
            else
            {
                MessageBox.Show("No folder selected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnShowData_Click(object sender, EventArgs e)
        {
            // TODO: Implement logic to show data from the selected CSV files.
        }

        private void btnImportFiles_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedFolderPath))
            {
                MessageBox.Show("Please select a folder first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                
            IImporterController importerController = new ImporterController(_selectedFolderPath);
            importerController.ShowMessages = _showMessages; 
            
            try
            {
                importerController.ImportCsvFiles(_selectedFolderPath);
            }
            catch (Exception ex)
            {
                importerController.CreateMessageReporting(_showMessages).ShowMessage($"An error occurred during import: {ex.Message}", "Error"); // Ensure messages are shown
            }
        }        
    }
}
