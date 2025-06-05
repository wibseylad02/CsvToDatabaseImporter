namespace CsvToDatabaseImporter
{
    public partial class DisplayDataForm : Form
    {
        public string InputFileName { get; set; } = "Vehicles.csv"; // Default input file name
        public string SelectedFolderPath { get; set; } = @"C:\AlphaGraphicsTechTest\"; // Default folder path

        public DisplayDataForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayDataForm_Load(object sender, EventArgs e)
        {
            var importerController = new ImporterController(SelectedFolderPath);
            importerController.ShowMessages = true;

            var dataTable = importerController.GetImportedDataTableFromInputFile(InputFileName);

            Text = dataTable.TableName;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                dataGridView1.DataSource = dataTable;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            else
            {
                importerController.CreateMessageReporting(true).ShowMessage($"No data found in the specified file {InputFileName}.", "Information");
            }
        }
    }
}
