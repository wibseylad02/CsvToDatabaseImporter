namespace CsvToDatabaseImporter
{
    partial class CsvImporter
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            folderBrowserDialog1 = new FolderBrowserDialog();
            txtFolderPath = new TextBox();
            lblSelectedFile = new Label();
            btnImportFiles = new Button();
            btnClose = new Button();
            btnShowData = new Button();
            SuspendLayout();
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(12, 32);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.Size = new Size(266, 23);
            txtFolderPath.TabIndex = 0;
            // 
            // lblSelectedFile
            // 
            lblSelectedFile.AutoSize = true;
            lblSelectedFile.Location = new Point(12, 9);
            lblSelectedFile.Name = "lblSelectedFile";
            lblSelectedFile.Size = new Size(72, 15);
            lblSelectedFile.TabIndex = 1;
            lblSelectedFile.Text = "Selected File";
            // 
            // btnImportFiles
            // 
            btnImportFiles.Location = new Point(12, 70);
            btnImportFiles.Name = "btnImportFiles";
            btnImportFiles.Size = new Size(112, 23);
            btnImportFiles.TabIndex = 2;
            btnImportFiles.Text = "Import Data Files";
            btnImportFiles.UseVisualStyleBackColor = true;
            btnImportFiles.Click += btnImportFiles_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(12, 108);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 3;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnShowData
            // 
            btnShowData.Location = new Point(142, 71);
            btnShowData.Name = "btnShowData";
            btnShowData.Size = new Size(136, 23);
            btnShowData.TabIndex = 4;
            btnShowData.Text = "Show Imported Data";
            btnShowData.UseVisualStyleBackColor = true;
            btnShowData.Click += btnShowData_Click;
            // 
            // CsvImporter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(301, 143);
            Controls.Add(btnShowData);
            Controls.Add(btnClose);
            Controls.Add(btnImportFiles);
            Controls.Add(lblSelectedFile);
            Controls.Add(txtFolderPath);
            Name = "CsvImporter";
            Text = "CSV File to Database Importer";
            Load += CsvImporter_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FolderBrowserDialog folderBrowserDialog1;
        private TextBox txtFolderPath;
        private Label lblSelectedFile;
        private Button btnImportFiles;
        private Button btnClose;
        private Button btnShowData;
    }
}
