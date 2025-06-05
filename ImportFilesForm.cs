using CsvToDatabaseImporter.Interfaces;

namespace CsvToDatabaseImporter
{
    public partial class ImportFilesForm : Form
    {
        private const string DefaultFilePath = @"C:\AlphaGraphicsTechTest\";

        private string _selectedFolderPath = "";

        // TODO - Flag to control whether messages are shown to the user or logged instead.  Probably best set to true, since this is a GUI app, but review its usage
        private bool _showMessages = true; // Flag to control whether messages are shown to the user or logged instead.  Probably best set to true, since this is a GUI app

        public ImportFilesForm()
        {
            InitializeComponent();
        }

        private void CsvImporter_Load(object sender, EventArgs e)
        {
            btnImportFiles.Enabled = false;

            // left as true during development, though ideally it would be false until the data had been imported into the database successfully
            btnShowData.Enabled = true;     

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
            var displayDataForm = new DisplayDataForm();
            displayDataForm.SelectedFolderPath = _selectedFolderPath; // Pass the selected folder path to the display form
            displayDataForm.ShowDialog(); // Show the form modally to allow user interaction
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
                btnShowData.Enabled = true; // Enable the button to show data after successful import
            }
            catch (Exception ex)
            {
                importerController.CreateMessageReporting(_showMessages).ShowMessage($"An error occurred during import: {ex.Message}", "Error"); 
            }
        }        
    }
}
