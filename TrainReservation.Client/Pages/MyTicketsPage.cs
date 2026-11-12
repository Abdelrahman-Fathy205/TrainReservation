using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Pages; // namespace for the pages

public partial class MyTicketsPage : UserControl // my tickets page
{
    public MyTicketsPage() // create the page
    {
        InitializeComponent(); // build the controls
        Theme.StyleGrid(dgv); // style the table
        cboWhen.SelectedIndex = 1; // triggers the first load
    }

    private void Filter_Changed(object sender, EventArgs e) // when the filter or search changes
    {
        if (cboWhen.SelectedIndex < 0) return; // if nothing is selected, stop
        string q = txtSearch.Text.Trim(); // the search word
        var now = DateTime.Now; // the current time
        var tickets = SampleData.Tickets // from all tickets
            .Where(t => t.Passenger == SampleData.CurrentUser) // keep only the current user's tickets
            .Where(t => cboWhen.SelectedIndex == 0 || (cboWhen.SelectedIndex == 1) == (t.Departs >= now)) // all, upcoming, or past
            .Where(t => q == "" || t.Code.Contains(q, StringComparison.OrdinalIgnoreCase) // if there is no search or the code has the word
                     || t.From.Station.Name.Contains(q, StringComparison.OrdinalIgnoreCase) // or the boarding station has the word
                     || t.To.Station.Name.Contains(q, StringComparison.OrdinalIgnoreCase) // or the drop-off station
                     || t.Trip.Train.ToString().Contains(q, StringComparison.OrdinalIgnoreCase)) // or the train
            .OrderByDescending(t => t.Departs) // sort from newest to oldest
            .ToList(); // turn them into a list

        dgv.DataSource = tickets.Select(t => new // show them in the table
        {
            t.Id, // the id (hidden)
            Ticket = t.Code, // ticket code
            Train = t.Trip.Train.Number, // train number
            From = t.From.Station.Name, // from
            To = t.To.Station.Name, // to
            Departs = t.Departs.ToString("ddd dd MMM yyyy  HH:mm"), // the time
            Class = t.Class.ToString(), // the class
            Seat = $"C{t.Coach} · {t.Seat}", // coach and seat
            Price = t.Price.ToString("N2") // the price
        }).ToList(); // turn them into a list
        dgv.Columns["Id"].Visible = false; // hide the id column
        dgv.Tag = q != "" ? "No tickets match your search." // message when the search finds nothing
                : cboWhen.SelectedIndex == 1 ? "No upcoming tickets. Open Book a Ticket to find a train." // message when there are no upcoming tickets
                : "No tickets here yet."; // general message
        lblTotal.Text = $"{tickets.Count} tickets · {tickets.Sum(t => t.Price):N2} EGP"; // count and total
        btnPrint.Enabled = btnCancel.Enabled = tickets.Count > 0; // the buttons work only if there are tickets
    }

    private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e) // double click on a row
    {
        if (e.RowIndex >= 0) btnPrint_Click(sender, e); // print the ticket
    }

    // on the list: Enter prints the highlighted ticket, Delete cancels it
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // keyboard shortcuts
    {
        if (dgv.Focused && dgv.CurrentRow != null && keyData is Keys.Enter or Keys.Delete) // if the table has focus and Enter or Delete is pressed
        {
            (keyData == Keys.Enter ? btnPrint : btnCancel).PerformClick(); // Enter prints, Delete cancels
            return true; // the key was handled
        }
        return base.ProcessCmdKey(ref msg, keyData); // other keys work as normal
    }

    private void btnCancel_Click(object sender, EventArgs e) // when the cancel ticket button is clicked
    {
        if (dgv.CurrentRow == null) { Theme.Warn("Select a ticket first."); return; } // a ticket must be selected first
        int id = (int)dgv.CurrentRow.Cells["Id"].Value; // id of the selected ticket
        var ticket = SampleData.Tickets.First(t => t.Id == id); // get the ticket
        if (ticket.Departs <= DateTime.Now) { Theme.Warn("This train has already departed, so the ticket can't be cancelled."); return; } // the train already left, it cannot be canceled
        if (!Theme.Confirm($"Cancel ticket {ticket.Code} ({ticket.From.Station.Name} → {ticket.To.Station.Name}, {ticket.Departs:dd MMM HH:mm})?\n\nYour seat will be released.")) return; // ask the user to confirm
        SampleData.Tickets.Remove(ticket); // delete the ticket
        Filter_Changed(sender, e); // update the table
        Theme.Info($"Ticket {ticket.Code} cancelled. {ticket.Price:N2} EGP will be refunded."); // success message and refunded amount
    }

    private void btnPrint_Click(object sender, EventArgs e) // when the print button is clicked
    {
        if (dgv.CurrentRow == null) { Theme.Warn("Select a ticket first."); return; } // a ticket must be selected first
        int id = (int)dgv.CurrentRow.Cells["Id"].Value; // id of the selected ticket
        TicketPrinter.Preview(SampleData.Tickets.First(t => t.Id == id), this); // show the print preview
    }
}
