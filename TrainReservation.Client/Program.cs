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

        SampleData.SelfCheck();
        Application.Run(new LoginForm());
    }
}
