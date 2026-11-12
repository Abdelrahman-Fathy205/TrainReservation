using TrainReservation.Client.Forms; // the screens
using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Pages; // namespace for the pages

public partial class SearchTripsPage : UserControl // page to search trips and book a ticket
{
    private Station from, to; // the boarding and drop-off stations used in the search
    private SeatClass seatClass; // the class used in the search

    public SearchTripsPage() // create the page
    {
        InitializeComponent(); // build the controls
        Theme.StyleGrid(dgv); // style the table
        cboFrom.DataSource = SampleData.Stations.ToList(); // fill the "from" list with stations
        cboTo.DataSource = SampleData.Stations.ToList(); // fill the "to" list with stations
        cboTo.SelectedIndex = Math.Min(5, cboTo.Items.Count - 1); // pick the sixth station as default (Alexandria)
        cboClass.DataSource = Enum.GetValues<SeatClass>(); // fill the class list
        cboClass.SelectedItem = SeatClass.Second; // default class: second
        dtpDate.MinDate = DateTime.Today; // a past date cannot be picked
        dgv.Tag = "Choose your stations and date, then press Enter or Search trains."; // empty table message
    }

    // Enter searches; on the results list it books the highlighted train
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // keyboard shortcuts
    {
        if (keyData == Keys.Enter && ActiveControl is not Button && ActiveControl is not ComboBox { DroppedDown: true }) // if Enter is pressed and not on a button or an open list
        {
            if (dgv.Focused && dgv.CurrentRow != null) btnBook.PerformClick(); else btnSearch.PerformClick(); // on the table: book, otherwise: search
            return true; // the key was handled
        }
        return base.ProcessCmdKey(ref msg, keyData); // other keys work as normal
    }

    private void btnSwap_Click(object sender, EventArgs e) // button to swap the two stations
    {
        var f = cboFrom.SelectedItem; // save the "from" station
        cboFrom.SelectedItem = cboTo.SelectedItem; // "from" takes the value of "to"
        cboTo.SelectedItem = f; // "to" takes the old value of "from"
    }

    private void btnSearch_Click(object sender, EventArgs e) // when the search button is clicked
    {
        from = cboFrom.SelectedItem as Station; // boarding station
        to = cboTo.SelectedItem as Station; // drop-off station
        seatClass = (SeatClass)cboClass.SelectedItem; // the class
        if (from == null || to == null) { Theme.Warn("Choose both stations."); return; } // both stations must be selected
        if (from == to) { Theme.Warn("Departure and arrival stations must be different."); return; } // the two stations must be different
        RunSearch(); // search
    }

    private void RunSearch() // run the search
    {
        var now = DateTime.Now; // the current time
        var rows = SampleData.Trips // from all trips
            .Where(t => t.Departure.Date == dtpDate.Value.Date) // trips on the selected date
            .Select(t => (Trip: t, A: t.StopAt(from), B: t.StopAt(to))) // get the boarding and drop-off stops
            .Where(x => x.A != null && x.B != null && x.A.KmFromOrigin < x.B.KmFromOrigin && x.Trip.TimeAt(x.A) > now) // the trip passes both stations in the right order and has not left yet
            .OrderBy(x => x.Trip.TimeAt(x.A)) // sort by departure time
            .Select(x => new // shape of a table row
            {
                x.Trip.Id, // trip id (hidden)
                Train = x.Trip.Train.ToString(), // the train
                Departs = x.Trip.TimeAt(x.A).ToString("HH:mm"), // departure time
                Arrives = x.Trip.TimeAt(x.B).ToString("HH:mm"), // arrival time
                Duration = TimeSpan.FromMinutes(x.B.MinutesFromStart - x.A.MinutesFromStart).ToString(@"h\h\ mm\m"), // trip duration
                Distance = $"{x.B.KmFromOrigin - x.A.KmFromOrigin} km", // the distance
                Fare = $"{Fares.Calc(x.A, x.B, seatClass):N2} EGP", // the price
                FreeSeats = SampleData.FreeSeats(x.Trip, seatClass, x.A, x.B) // free seats
            })
            .ToList(); // turn them into a list

        dgv.Tag = "No trains found. Try another date, or swap the stations."; // message when there are no results
        dgv.DataSource = rows; // show the results
        dgv.Columns["Id"].Visible = false; // hide the id column
        dgv.Columns["FreeSeats"].HeaderText = "Free seats"; // title of the free seats column
        lblResult.Text = rows.Count == 0 // if there are no results
            ? $"No trains from {from.Name} to {to.Name} on {dtpDate.Value:dd MMM}. Try another date." // write "no trains"
            : $"{rows.Count} train(s) found · {seatClass} class"; // otherwise write the count
        btnBook.Enabled = rows.Count > 0; // the book button works only if there are results
    }

    private void dgv_SelectionChanged(object sender, EventArgs e) => btnBook.Enabled = dgv.CurrentRow != null; // the book button works if a row is selected

    private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e) // double click on a row
    {
        if (e.RowIndex >= 0) btnBook_Click(sender, e); // book this trip
    }

    private void btnBook_Click(object sender, EventArgs e) // when the book button is clicked
    {
        if (dgv.CurrentRow == null) return; // if no row is selected, stop
        var trip = SampleData.Trips.First(t => t.Id == (int)dgv.CurrentRow.Cells["Id"].Value); // get the selected trip
        var a = trip.StopAt(from); // boarding stop
        var b = trip.StopAt(to); // drop-off stop
        if (SampleData.FreeSeats(trip, seatClass, a, b) <= 0) // if there are no free seats
        {
            Theme.Warn($"No free {seatClass} seats on this train. Try another class or train."); // tell the user
            return; // stop
        }

        List<Ticket> tickets; // the tickets that will be booked
        using (var form = new SeatSelectionForm(trip, a, b, seatClass)) // open the seat selection screen
        {
            if (form.ShowDialog(this) != DialogResult.OK) return; // if canceled, stop
            tickets = form.BookedTickets; // get the booked tickets
        }
        RunSearch(); // update the results (the free seat count changed)
        string codes = string.Join(", ", tickets.Select(t => t.Code)); // ticket codes
        if (Theme.Confirm($"{(tickets.Count > 1 ? "Tickets" : "Ticket")} {codes} booked!\n\nDo you want to print {(tickets.Count > 1 ? "them" : "it")} now?")) // ask if they want to print
            TicketPrinter.Preview(tickets, this); // show the print preview
    }
}
