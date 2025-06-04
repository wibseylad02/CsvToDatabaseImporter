namespace CsvToDatabaseImporter
{
    /// <summary>
    /// Class is for controlling how messages are reported during the import process.
    /// </summary>
    /// <remarks>Defaults to showing <see cref="MessageBox"/></remarks>
    public class MessageReporting
    {
        /// <summary>
        /// Flag to control whether messages are shown to the user (<see langword="true"/>), or logged instead (<see langword="false"/>).
        /// </summary>
        public bool ShowMessages { get; set; } // Future proof in case external logging will be needed.

        public MessageReporting() 
        {
            ShowMessages = true;
        }

        public MessageReporting(bool showMessages)
        {
            ShowMessages = showMessages;
        }

        /// <summary>
        /// Display or log a message to the user.
        /// </summary>
        /// <param name="message">The message to report</param>
        /// <param name="caption">The message heading (e.g. "Error") - defaults to "Information"</param>
        public void ShowMessage(string message, string caption = "Information")
        {
            if (ShowMessages)
            {
                MessageBox.Show(message, caption, MessageBoxButtons.OK, caption == "Error" ? MessageBoxIcon.Error : MessageBoxIcon.Information);
            }
            else
            {
                // Log the message instead of showing it in a message box.
                // This could be extended to log to a file, console, Windows Event Log etc.
                Console.WriteLine($"{caption}: {message}");
            }
        }
    }
}
