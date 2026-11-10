using TrainReservation.Client.Forms;
using TrainReservation.Client.Services;

namespace TrainReservation.Client;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Last line of defence: any unhandled UI exception is shown instead of crashing the app.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (s, e) =>
            MessageBox.Show("Unexpected error:\n" + e.Exception.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

        try
        {
            Db.Init();
        }
        catch (Exception ex) // SqlException, LocalDB not installed, etc.
        {
            SampleData.Seed();
            MessageBox.Show("Could not connect to the database, so the app runs on demo data and changes will not be saved." +
                Environment.NewLine + Environment.NewLine + ex.Message,
                "Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        SampleData.SelfCheck();
        Application.Run(new LoginForm());
    }
}
