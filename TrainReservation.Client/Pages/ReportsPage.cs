using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Pages;

public partial class ReportsPage : UserControl
{
    private record Report(
        int Tickets, decimal Revenue, double AvgOccupancy,
        List<(string, double)> ByRoute, List<(string, double)> ByClass,
        List<(string, double)> ByDay, List<(string, double)> ByTrain);

    public ReportsPage()
    {
        InitializeComponent();
        dtpFrom.Value = DateTime.Today.AddDays(-14);
        dtpTo.Value = DateTime.Today.AddDays(6);
    }

    private void ReportsPage_Load(object sender, EventArgs e) => btnRefresh_Click(sender, e);

    // async: the report is built on a worker thread so the window stays responsive
    private async void btnRefresh_Click(object sender, EventArgs e)
    {
        DateTime from = dtpFrom.Value.Date, to = dtpTo.Value.Date.AddDays(1);
        if (from >= to) { Theme.Warn("'From' must be before 'To'."); return; }

        btnRefresh.Enabled = false;
        progress.Visible = true;
        lblStatus.Text = "Building report...";
        try
        {
            var r = await Task.Run(() => Build(from, to));
            if (IsDisposed) return; // user navigated away while it was running
            lblSummary.Text = $"Tickets sold: {r.Tickets:N0}      Revenue: {r.Revenue:N2} EGP      Average occupancy: {r.AvgOccupancy:P0}";
            chartRevenue.SetData(r.ByRoute);
            chartClass.SetData(r.ByClass);
            chartDaily.SetData(r.ByDay);
            chartOccupancy.SetData(r.ByTrain);
            lblStatus.Text = $"Updated {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            if (!IsDisposed) lblStatus.Text = "Failed: " + ex.Message;
        }
        finally
        {
            if (!IsDisposed)
            {
                btnRefresh.Enabled = true;
                progress.Visible = false;
            }
        }
    }

    private static Report Build(DateTime from, DateTime to)
    {
        // snapshot the lists so the UI thread can keep adding while we read
        var trips = SampleData.Trips.Where(t => t.Departure >= from && t.Departure < to).ToList();
        var tickets = SampleData.Tickets.ToList().Where(t => trips.Contains(t.Trip)).ToList();

        // occupancy of a trip = seat-km sold / seat-km available
        double Occupancy(Models.Trip trip)
        {
            double available = trip.Train.Capacity * (double)trip.Stops[^1].KmFromOrigin;
            double sold = tickets.Where(k => k.Trip == trip).Sum(k => (double)(k.To.KmFromOrigin - k.From.KmFromOrigin));
            return available == 0 ? 0 : sold / available;
        }
        var occ = trips.Select(t => (Trip: t, Occ: Occupancy(t))).ToList();

        return new Report(
            tickets.Count,
            tickets.Sum(t => t.Price),
            occ.Count == 0 ? 0 : occ.Average(o => o.Occ),
            tickets.GroupBy(t => t.Trip.Route).Select(g => (g.Key, (double)g.Sum(t => t.Price))).OrderByDescending(x => x.Item2).ToList(),
            tickets.GroupBy(t => t.Class).Select(g => (g.Key.ToString(), (double)g.Count())).ToList(),
            tickets.GroupBy(t => t.Trip.Departure.Date).OrderBy(g => g.Key).TakeLast(7)
                   .Select(g => (g.Key.ToString("ddd dd/MM"), (double)g.Count())).ToList(),
            occ.GroupBy(o => o.Trip.Train.ToString()).Select(g => (g.Key, Math.Round(g.Average(o => o.Occ) * 100))).ToList());
    }
}
