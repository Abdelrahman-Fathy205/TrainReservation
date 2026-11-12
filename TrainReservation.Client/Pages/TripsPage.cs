using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Pages; // namespace for the pages

public partial class TripsPage : UserControl // trip management page
{
    private Trip selected; // the currently selected trip

    public TripsPage() // create the page
    {
        InitializeComponent(); // build the controls
        Theme.StyleGrid(dgv); // style the trips table
        Theme.StyleGrid(dgvStops, readOnly: false); // style the stops table (editable)

        cboTrain.DataSource = SampleData.Trains.ToList(); // fill the trains list
        cboFilterTrain.Items.Add("All trains"); // first filter option: all trains
        cboFilterTrain.Items.AddRange(SampleData.Trains.ToArray()); // add the trains to the filter
        cboFilterTrain.SelectedIndex = 0; // select "all trains"

        colStation.DataSource = SampleData.Stations.ToList(); // the station column in the stops table uses the stations
        colStation.DisplayMember = nameof(Station.Name); // shows the station name
        colStation.ValueMember = nameof(Station.Id); // and stores its id
        colStation.FlatStyle = FlatStyle.Flat; // follows the grid colours in dark mode

        dtpFilter.Checked = false; // the date filter is off at first
        LoadGrid(); // fill the table
    }

    private void LoadGrid() // load the trips into the table
    {
        var train = cboFilterTrain.SelectedItem as Train; // the train picked in the filter (or null for "all")
        var rows = SampleData.Trips // from all trips
            .Where(t => !dtpFilter.Checked || t.Departure.Date == dtpFilter.Value.Date) // filter by date if turned on
            .Where(t => train == null || t.Train == train) // filter by train
            .OrderBy(t => t.Departure) // sort by time
            .Select(t => new // row shape
            {
                t.Id, // the id (hidden)
                Train = t.Train.ToString(), // the train
                t.Route, // the route
                Departs = t.Departure.ToString("ddd dd/MM  HH:mm"), // departure time
                Arrives = t.Arrival.ToString("ddd dd/MM  HH:mm"), // arrival time
                Stops = t.Stops.Count, // number of stops
                Booked = SampleData.Tickets.Count(k => k.Trip == t) // number of booked tickets
            })
            .ToList(); // turn them into a list
        dgv.Tag = "No trips match this filter."; // empty table message
        dgv.DataSource = rows; // show the rows
        dgv.Columns["Id"].Visible = false; // hide the id column
        lblCount.Text = $"{rows.Count} trips"; // number of trips
        ClearEditor(); // clear the edit boxes
    }

    private void Filter_Changed(object sender, EventArgs e) => LoadGrid(); // when the filter changes, update the table

    private void dgv_CellClick(object sender, DataGridViewCellEventArgs e) // when a row is clicked
    {
        if (e.RowIndex < 0) return; // if the header was clicked, stop
        int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value; // trip id
        selected = SampleData.Trips.First(t => t.Id == id); // get the trip
        cboTrain.SelectedItem = selected.Train; // select the train
        dtpDeparture.Value = selected.Departure; // set the departure time
        dgvStops.Rows.Clear(); // clear the stops table
        foreach (var s in selected.Stops) // for each stop
            dgvStops.Rows.Add(s.Station.Id, s.KmFromOrigin.ToString(), s.MinutesFromStart.ToString()); // add it to the table
    }

    private void btnRemoveStop_Click(object sender, EventArgs e) // delete stop button
    {
        var row = dgvStops.CurrentRow; // the selected row
        if (row != null && !row.IsNewRow) dgvStops.Rows.Remove(row); // if it is a real row, delete it
    }

    // a combo cell whose station was deleted would otherwise throw a dialog storm
    private void dgvStops_DataError(object sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false; // ignore display errors in the stops table

    /// <summary>Reads the stops grid; returns null and shows why when it is invalid.</summary>
    private List<TripStop> ReadStops() // read the stops from the table and check them
    {
        dgvStops.EndEdit(); // finish any edit in progress in the cell
        var stops = new List<TripStop>(); // list of stops
        foreach (DataGridViewRow r in dgvStops.Rows) // for each row
        {
            if (r.IsNewRow) continue; // skip the empty row at the end
            if (r.Cells[0].Value is not int stationId) // if no station is selected
                return Fail($"Row {r.Index + 1}: choose a station."); // error: pick a station
            if (!int.TryParse(Convert.ToString(r.Cells[1].Value), out int km) || km < 0) // if km is not a positive whole number
                return Fail($"Row {r.Index + 1}: km must be a whole number ≥ 0."); // km error
            if (!int.TryParse(Convert.ToString(r.Cells[2].Value), out int min) || min < 0) // if minutes is not a positive whole number
                return Fail($"Row {r.Index + 1}: minutes must be a whole number ≥ 0."); // minutes error
            stops.Add(new TripStop { Station = SampleData.Stations.First(s => s.Id == stationId), KmFromOrigin = km, MinutesFromStart = min }); // add the stop
        }
        if (stops.Count < 2) return Fail("A trip needs at least 2 stops."); // at least two stops are needed
        if (stops[0].KmFromOrigin != 0 || stops[0].MinutesFromStart != 0) return Fail("The first stop must have 0 km and 0 minutes."); // the first stop must be 0 km and 0 minutes
        if (stops.Select(s => s.Station).Distinct().Count() != stops.Count) return Fail("A station can appear only once in a trip."); // a station must not repeat
        for (int i = 1; i < stops.Count; i++) // for each stop after the first
            if (stops[i].KmFromOrigin <= stops[i - 1].KmFromOrigin || stops[i].MinutesFromStart <= stops[i - 1].MinutesFromStart) // if it is not farther and later than the one before
                return Fail($"Stop {i + 1} must be further and later than stop {i}."); // order error
        return stops; // all good, return the stops

        static List<TripStop> Fail(string message) { Theme.Warn(message); return null; } // show the error and return null
    }

    /// <summary>A train can't run two trips at the same time.</summary>
    private static bool TrainIsFree(Train train, DateTime departure, List<TripStop> stops, Trip except) // is the train free at this time?
    {
        var arrival = departure.AddMinutes(stops[^1].MinutesFromStart); // arrival time
        var clash = SampleData.Trips.FirstOrDefault(t => t != except && t.Train == train && t.Departure < arrival && departure < t.Arrival); // look for a trip on the same train that overlaps in time
        if (clash == null) return true; // no conflict
        Theme.Warn($"Train {train.Number} is already running {clash.Route} from {clash.Departure:dd/MM HH:mm} to {clash.Arrival:dd/MM HH:mm}."); // show the conflicting trip
        return false; // the train is not free
    }

    private void btnAdd_Click(object sender, EventArgs e) // add button
    {
        if (cboTrain.SelectedItem is not Train train) { Theme.Warn("Choose a train."); return; } // a train must be selected
        if (dtpDeparture.Value < DateTime.Now) { Theme.Warn("Departure must be in the future."); return; } // the time must be in the future
        var stops = ReadStops(); // read the stops
        if (stops == null || !TrainIsFree(train, dtpDeparture.Value, stops, null)) return; // if the stops are wrong or the train is busy, stop
        SampleData.Trips.Add(new Trip // add a new trip
        {
            Id = SampleData.NextId(SampleData.Trips, t => t.Id), // new id
            Train = train, Departure = dtpDeparture.Value, Stops = stops // the data
        });
        LoadGrid(); // update the table
        Theme.Info("Trip scheduled."); // success message
    }

    private void btnUpdate_Click(object sender, EventArgs e) // edit button
    {
        if (selected == null) { Theme.Warn("Select a trip from the list first."); return; } // a trip must be selected first
        if (SampleData.Tickets.Any(k => k.Trip == selected)) // if the trip has tickets
        {
            Theme.Warn("This trip already has booked tickets, so its schedule can't be changed."); // it cannot be edited
            return; // stop
        }
        if (cboTrain.SelectedItem is not Train train) { Theme.Warn("Choose a train."); return; } // a train must be selected
        if (dtpDeparture.Value < DateTime.Now) { Theme.Warn("Departure must be in the future."); return; } // the time must be in the future
        var stops = ReadStops(); // read the stops
        if (stops == null || !TrainIsFree(train, dtpDeparture.Value, stops, selected)) return; // if the stops are wrong or the train is busy, stop
        selected.Train = train; // update the train
        selected.Departure = dtpDeparture.Value; // update the time
        selected.Stops = stops; // update the stops
        LoadGrid(); // update the table
        Theme.Info("Trip updated."); // success message
    }

    private void btnDelete_Click(object sender, EventArgs e) // delete button
    {
        if (selected == null) { Theme.Warn("Select a trip from the list first."); return; } // a trip must be selected first
        if (SampleData.Tickets.Any(k => k.Trip == selected)) // if the trip has tickets
        {
            Theme.Warn("This trip has booked tickets and cannot be deleted."); // it cannot be deleted
            return; // stop
        }
        if (!Theme.Confirm($"Delete trip {selected.Route} on {selected.Departure:dd/MM HH:mm}?")) return; // ask the user to confirm
        SampleData.Trips.Remove(selected); // delete the trip
        LoadGrid(); // update the table
    }

    private void btnClear_Click(object sender, EventArgs e) => ClearEditor(); // clear button

    private void ClearEditor() // clear the edit boxes
    {
        selected = null; // no trip selected
        dgvStops.Rows.Clear(); // clear the stops table
        dtpDeparture.Value = DateTime.Today.AddDays(1).AddHours(8); // default time: tomorrow at 8
        dgv.ClearSelection(); // unselect the row
    }
}
