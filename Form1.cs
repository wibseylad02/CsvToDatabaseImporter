using System.Data;

namespace CsvToDatabaseImporter
{
    public partial class CsvImporter : Form
    {
        private const string DefaultFilePath = @"C:\AlphaGraphics\";

        private string _selectedFolderPath = "";

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
            try
            {
                var csvFiles = Directory.GetFiles(_selectedFolderPath, "*.csv");
                if (csvFiles.Length == 0)
                {
                    MessageBox.Show("No CSV files found in the selected folder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var csvFile in csvFiles)
                {
                    DataTable csvData = Importer.GetDataTableFromCSVFile(csvFile);

                    if (csvData != null && csvData.Rows.Count > 0)
                    {
                        // Assuming the table name is derived from the CSV file name without extension and minus the selected folder name
                        Importer.InsertDataIntoSQLServerUsingSQLBulkCopy(csvData, Importer.GetTargetDbNameFromFullFileName(csvFile));
                        MessageBox.Show($"Successfully imported {csvFile}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during import: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        
    }
}
