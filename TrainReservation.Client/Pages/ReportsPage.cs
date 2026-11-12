using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Pages; // namespace for the pages

public partial class ReportsPage : UserControl // reports page
{
    private record Report( // shape of the report result
        int Tickets, decimal Revenue, double AvgOccupancy, // number of tickets, revenue, average occupancy
        List<(string, double)> ByRoute, List<(string, double)> ByClass, // by route, by class
        List<(string, double)> ByDay, List<(string, double)> ByTrain); // by day, by train

    public ReportsPage() // create the page
    {
        InitializeComponent(); // build the controls
        dtpFrom.Value = DateTime.Today.AddDays(-14); // start date: two weeks ago
        dtpTo.Value = DateTime.Today.AddDays(6); // end date: 6 days from now
    }

    private void ReportsPage_Load(object sender, EventArgs e) => btnRefresh_Click(sender, e); // build the report as soon as the page opens

    // async: the report is built on a worker thread so the window stays responsive
    private async void btnRefresh_Click(object sender, EventArgs e) // when the refresh button is clicked
    {
        DateTime from = dtpFrom.Value.Date, to = dtpTo.Value.Date.AddDays(1); // the period (the end includes the whole day)
        if (from >= to) { Theme.Warn("'From' must be before 'To'."); return; } // the start must be before the end

        btnRefresh.Enabled = false; // disable the button until it finishes
        progress.Visible = true; // show the loading bar
        lblStatus.Text = "Building report..."; // "building the report"
        try // try
        {
            var r = await Task.Run(() => Build(from, to)); // build the report in the background and wait for the result
            if (IsDisposed) return; // user navigated away while it was running
            lblSummary.Text = $"Tickets sold: {r.Tickets:N0}      Revenue: {r.Revenue:N2} EGP      Average occupancy: {r.AvgOccupancy:P0}"; // the summary
            chartRevenue.SetData(r.ByRoute); // chart: revenue by route
            chartClass.SetData(r.ByClass); // chart: tickets by class
            chartDaily.SetData(r.ByDay); // chart: tickets by day
            chartOccupancy.SetData(r.ByTrain); // chart: occupancy by train
            lblStatus.Text = $"Updated {DateTime.Now:HH:mm:ss}"; // last update time
        }
        catch (Exception ex) // if an error happens
        {
            if (!IsDisposed) lblStatus.Text = "Failed: " + ex.Message; // write the error
        }
        finally // in all cases
        {
            if (!IsDisposed) // if the page is still open
            {
                btnRefresh.Enabled = true; // enable the button again
                progress.Visible = false; // hide the loading bar
            }
        }
    }

    private static Report Build(DateTime from, DateTime to) // calculate the report
    {
        // snapshot the lists so the UI thread can keep adding while we read
        var trips = SampleData.Trips.Where(t => t.Departure >= from && t.Departure < to).ToList(); // the trips in the period
        var tickets = SampleData.Tickets.ToList().Where(t => trips.Contains(t.Trip)).ToList(); // the tickets of these trips

        // occupancy of a trip = seat-km sold / seat-km available
        double Occupancy(Models.Trip trip) // calculate the occupancy of one trip
        {
            double available = trip.Train.Capacity * (double)trip.Stops[^1].KmFromOrigin; // available = capacity × trip length
            double sold = tickets.Where(k => k.Trip == trip).Sum(k => (double)(k.To.KmFromOrigin - k.From.KmFromOrigin)); // sold = sum of ticket distances
            return available == 0 ? 0 : sold / available; // the percent
        }
        var occ = trips.Select(t => (Trip: t, Occ: Occupancy(t))).ToList(); // occupancy of each trip

        return new Report( // return the report
            tickets.Count, // number of tickets
            tickets.Sum(t => t.Price), // total revenue
            occ.Count == 0 ? 0 : occ.Average(o => o.Occ), // average occupancy
            tickets.GroupBy(t => t.Trip.Route).Select(g => (g.Key, (double)g.Sum(t => t.Price))).OrderByDescending(x => x.Item2).ToList(), // revenue per route, from biggest to smallest
            tickets.GroupBy(t => t.Class).Select(g => (g.Key.ToString(), (double)g.Count())).ToList(), // number of tickets per class
            tickets.GroupBy(t => t.Trip.Departure.Date).OrderBy(g => g.Key).TakeLast(7) // tickets per day (last 7 days)
                   .Select(g => (g.Key.ToString("ddd dd/MM"), (double)g.Count())).ToList(), // day name and count
            occ.GroupBy(o => o.Trip.Train.ToString()).Select(g => (g.Key, Math.Round(g.Average(o => o.Occ) * 100))).ToList()); // average occupancy of each train in percent
    }
}
