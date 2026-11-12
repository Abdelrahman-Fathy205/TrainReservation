using TrainReservation.Client.Forms; // use the Forms folder (the screens)
using TrainReservation.Client.Services; // use the Services folder (data and saving)

namespace TrainReservation.Client; // the namespace of the program

static class Program // the main class where the program starts
{
    [STAThread] // needed by Windows Forms so the UI works correctly
    static void Main() // main method: the first code that runs when the program opens
    {
        ApplicationConfiguration.Initialize(); // set up UI settings (fonts and screen DPI)

        // Last line of defence: any unhandled UI exception is shown instead of crashing the app.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException); // catch any unexpected error instead of closing the program
        Application.ThreadException += (s, e) => // run this code when a UI error happens
            MessageBox.Show("Unexpected error:\n" + e.Exception.Message, "Error", // show a message with the error text
                MessageBoxButtons.OK, MessageBoxIcon.Error); // with an OK button and an error icon

        SampleData.SelfCheck(); // quick check that the data and accounts are valid
        Application.Run(new LoginForm()); // open the login screen and keep the program running until it closes
        try { DataStore.Save(); } // after the program closes: save all data to the file
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) // if saving fails because of the file or permissions
        {
            MessageBox.Show("Your changes could not be saved:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // tell the user the changes were not saved
        }
    }
}
