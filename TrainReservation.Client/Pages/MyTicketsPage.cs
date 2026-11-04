using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Pages;

public partial class MyTicketsPage : UserControl
{
    public MyTicketsPage()
    {
        InitializeComponent();
        Theme.StyleGrid(dgv);
        cboWhen.SelectedIndex = 1; // triggers the first load
    }

    private void Filter_Changed(object sender, EventArgs e)
    {
        if (cboWhen.SelectedIndex < 0) return;
        string q = txtSearch.Text.Trim();
        var now = DateTime.Now;
        var tickets = SampleData.Tickets
            .Where(t => t.Passenger == SampleData.CurrentUser)
            .Where(t => cboWhen.SelectedIndex == 0 || (cboWhen.SelectedIndex == 1) == (t.Departs >= now))
            .Where(t => q == "" || t.Code.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || t.From.Station.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || t.To.Station.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || t.Trip.Train.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(t => t.Departs)
            .ToList();

        dgv.DataSource = tickets.Select(t => new
        {
            t.Id,
            Ticket = t.Code,
            Train = t.Trip.Train.Number,
            From = t.From.Station.Name,
            To = t.To.Station.Name,
            Departs = t.Departs.ToString("ddd dd MMM yyyy  HH:mm"),
            Class = t.Class.ToString(),
            Seat = $"C{t.Coach} · {t.Seat}",
            Price = t.Price.ToString("N2")
        }).ToList();
        dgv.Columns["Id"].Visible = false;
        lblTotal.Text = $"{tickets.Count} tickets · {tickets.Sum(t => t.Price):N2} EGP";
        btnPrint.Enabled = tickets.Count > 0;
    }

    private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnPrint_Click(sender, e);
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
        if (dgv.CurrentRow == null) { Theme.Warn("Select a ticket first."); return; }
        int id = (int)dgv.CurrentRow.Cells["Id"].Value;
        TicketPrinter.Preview(SampleData.Tickets.First(t => t.Id == id), this);
    }
}
