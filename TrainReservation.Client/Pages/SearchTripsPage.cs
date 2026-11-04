using TrainReservation.Client.Forms;
using TrainReservation.Client.Models;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Pages;

public partial class SearchTripsPage : UserControl
{
    private Station from, to;
    private SeatClass seatClass;

    public SearchTripsPage()
    {
        InitializeComponent();
        Theme.StyleGrid(dgv);
        cboFrom.DataSource = SampleData.Stations.ToList();
        cboTo.DataSource = SampleData.Stations.ToList();
        cboTo.SelectedIndex = Math.Min(5, cboTo.Items.Count - 1);
        cboClass.DataSource = Enum.GetValues<SeatClass>();
        cboClass.SelectedItem = SeatClass.Second;
        dtpDate.MinDate = DateTime.Today;
    }

    private void btnSwap_Click(object sender, EventArgs e)
    {
        var f = cboFrom.SelectedItem;
        cboFrom.SelectedItem = cboTo.SelectedItem;
        cboTo.SelectedItem = f;
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
        from = cboFrom.SelectedItem as Station;
        to = cboTo.SelectedItem as Station;
        seatClass = (SeatClass)cboClass.SelectedItem;
        if (from == null || to == null) { Theme.Warn("Choose both stations."); return; }
        if (from == to) { Theme.Warn("Departure and arrival stations must be different."); return; }
        RunSearch();
    }

    private void RunSearch()
    {
        var now = DateTime.Now;
        var rows = SampleData.Trips
            .Where(t => t.Departure.Date == dtpDate.Value.Date)
            .Select(t => (Trip: t, A: t.StopAt(from), B: t.StopAt(to)))
            .Where(x => x.A != null && x.B != null && x.A.KmFromOrigin < x.B.KmFromOrigin && x.Trip.TimeAt(x.A) > now)
            .OrderBy(x => x.Trip.TimeAt(x.A))
            .Select(x => new
            {
                x.Trip.Id,
                Train = x.Trip.Train.ToString(),
                Departs = x.Trip.TimeAt(x.A).ToString("HH:mm"),
                Arrives = x.Trip.TimeAt(x.B).ToString("HH:mm"),
                Duration = TimeSpan.FromMinutes(x.B.MinutesFromStart - x.A.MinutesFromStart).ToString(@"h\h\ mm\m"),
                Distance = $"{x.B.KmFromOrigin - x.A.KmFromOrigin} km",
                Fare = $"{Fares.Calc(x.A, x.B, seatClass):N2} EGP",
                FreeSeats = SampleData.FreeSeats(x.Trip, seatClass, x.A, x.B)
            })
            .ToList();

        dgv.DataSource = rows;
        dgv.Columns["Id"].Visible = false;
        dgv.Columns["FreeSeats"].HeaderText = "Free seats";
        lblResult.Text = rows.Count == 0
            ? $"No trains from {from.Name} to {to.Name} on {dtpDate.Value:dd MMM}. Try another date."
            : $"{rows.Count} train(s) found · {seatClass} class";
        btnBook.Enabled = rows.Count > 0;
    }

    private void dgv_SelectionChanged(object sender, EventArgs e) => btnBook.Enabled = dgv.CurrentRow != null;

    private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnBook_Click(sender, e);
    }

    private void btnBook_Click(object sender, EventArgs e)
    {
        if (dgv.CurrentRow == null) return;
        var trip = SampleData.Trips.First(t => t.Id == (int)dgv.CurrentRow.Cells["Id"].Value);
        var a = trip.StopAt(from);
        var b = trip.StopAt(to);
        if (SampleData.FreeSeats(trip, seatClass, a, b) <= 0)
        {
            Theme.Warn($"No free {seatClass} seats on this train. Try another class or train.");
            return;
        }

        Ticket ticket;
        using (var form = new SeatSelectionForm(trip, a, b, seatClass))
        {
            if (form.ShowDialog(this) != DialogResult.OK) return;
            ticket = form.BookedTicket;
        }
        RunSearch();
        if (Theme.Confirm($"Ticket {ticket.Code} booked!\n\nDo you want to print it now?"))
            TicketPrinter.Preview(ticket, this);
    }
}
