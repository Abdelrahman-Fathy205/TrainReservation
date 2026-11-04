using TrainReservation.Client.Models;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Pages;

public partial class TripsPage : UserControl
{
    private Trip selected;

    public TripsPage()
    {
        InitializeComponent();
        Theme.StyleGrid(dgv);
        Theme.StyleGrid(dgvStops, readOnly: false);

        cboTrain.DataSource = SampleData.Trains.ToList();
        cboFilterTrain.Items.Add("All trains");
        cboFilterTrain.Items.AddRange(SampleData.Trains.ToArray());
        cboFilterTrain.SelectedIndex = 0;

        colStation.DataSource = SampleData.Stations.ToList();
        colStation.DisplayMember = nameof(Station.Name);
        colStation.ValueMember = nameof(Station.Id);

        dtpFilter.Checked = false;
        LoadGrid();
    }

    private void LoadGrid()
    {
        var train = cboFilterTrain.SelectedItem as Train;
        var rows = SampleData.Trips
            .Where(t => !dtpFilter.Checked || t.Departure.Date == dtpFilter.Value.Date)
            .Where(t => train == null || t.Train == train)
            .OrderBy(t => t.Departure)
            .Select(t => new
            {
                t.Id,
                Train = t.Train.ToString(),
                t.Route,
                Departs = t.Departure.ToString("ddd dd/MM  HH:mm"),
                Arrives = t.Arrival.ToString("ddd dd/MM  HH:mm"),
                Stops = t.Stops.Count,
                Booked = SampleData.Tickets.Count(k => k.Trip == t)
            })
            .ToList();
        dgv.DataSource = rows;
        dgv.Columns["Id"].Visible = false;
        lblCount.Text = $"{rows.Count} trips";
        ClearEditor();
    }

    private void Filter_Changed(object sender, EventArgs e) => LoadGrid();

    private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value;
        selected = SampleData.Trips.First(t => t.Id == id);
        cboTrain.SelectedItem = selected.Train;
        dtpDeparture.Value = selected.Departure;
        dgvStops.Rows.Clear();
        foreach (var s in selected.Stops)
            dgvStops.Rows.Add(s.Station.Id, s.KmFromOrigin.ToString(), s.MinutesFromStart.ToString());
    }

    private void btnRemoveStop_Click(object sender, EventArgs e)
    {
        var row = dgvStops.CurrentRow;
        if (row != null && !row.IsNewRow) dgvStops.Rows.Remove(row);
    }

    // a combo cell whose station was deleted would otherwise throw a dialog storm
    private void dgvStops_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

    /// <summary>Reads the stops grid; returns null and shows why when it is invalid.</summary>
    private List<TripStop> ReadStops()
    {
        dgvStops.EndEdit();
        var stops = new List<TripStop>();
        foreach (DataGridViewRow r in dgvStops.Rows)
        {
            if (r.IsNewRow) continue;
            if (r.Cells[0].Value is not int stationId)
                return Fail($"Row {r.Index + 1}: choose a station.");
            if (!int.TryParse(Convert.ToString(r.Cells[1].Value), out int km) || km < 0)
                return Fail($"Row {r.Index + 1}: km must be a whole number ≥ 0.");
            if (!int.TryParse(Convert.ToString(r.Cells[2].Value), out int min) || min < 0)
                return Fail($"Row {r.Index + 1}: minutes must be a whole number ≥ 0.");
            stops.Add(new TripStop { Station = SampleData.Stations.First(s => s.Id == stationId), KmFromOrigin = km, MinutesFromStart = min });
        }
        if (stops.Count < 2) return Fail("A trip needs at least 2 stops.");
        if (stops[0].KmFromOrigin != 0 || stops[0].MinutesFromStart != 0) return Fail("The first stop must have 0 km and 0 minutes.");
        if (stops.Select(s => s.Station).Distinct().Count() != stops.Count) return Fail("A station can appear only once in a trip.");
        for (int i = 1; i < stops.Count; i++)
            if (stops[i].KmFromOrigin <= stops[i - 1].KmFromOrigin || stops[i].MinutesFromStart <= stops[i - 1].MinutesFromStart)
                return Fail($"Stop {i + 1} must be further and later than stop {i}.");
        return stops;

        static List<TripStop> Fail(string message) { Theme.Warn(message); return null; }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (cboTrain.SelectedItem is not Train train) { Theme.Warn("Choose a train."); return; }
        if (dtpDeparture.Value < DateTime.Now) { Theme.Warn("Departure must be in the future."); return; }
        var stops = ReadStops();
        if (stops == null) return;
        SampleData.Trips.Add(new Trip
        {
            Id = SampleData.NextId(SampleData.Trips, t => t.Id),
            Train = train, Departure = dtpDeparture.Value, Stops = stops
        });
        LoadGrid();
        Theme.Info("Trip scheduled.");
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
        if (selected == null) { Theme.Warn("Select a trip from the list first."); return; }
        if (SampleData.Tickets.Any(k => k.Trip == selected))
        {
            Theme.Warn("This trip already has booked tickets, so its schedule can't be changed.");
            return;
        }
        if (cboTrain.SelectedItem is not Train train) { Theme.Warn("Choose a train."); return; }
        var stops = ReadStops();
        if (stops == null) return;
        selected.Train = train;
        selected.Departure = dtpDeparture.Value;
        selected.Stops = stops;
        LoadGrid();
        Theme.Info("Trip updated.");
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (selected == null) { Theme.Warn("Select a trip from the list first."); return; }
        if (SampleData.Tickets.Any(k => k.Trip == selected))
        {
            Theme.Warn("This trip has booked tickets and cannot be deleted.");
            return;
        }
        if (!Theme.Confirm($"Delete trip {selected.Route} on {selected.Departure:dd/MM HH:mm}?")) return;
        SampleData.Trips.Remove(selected);
        LoadGrid();
    }

    private void btnClear_Click(object sender, EventArgs e) => ClearEditor();

    private void ClearEditor()
    {
        selected = null;
        dgvStops.Rows.Clear();
        dtpDeparture.Value = DateTime.Today.AddDays(1).AddHours(8);
        dgv.ClearSelection();
    }
}
