using System.Text.RegularExpressions; // format checking library
using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Pages; // namespace for the pages

public partial class StationsPage : UserControl // station management page
{
    private Station selected; // the currently selected station

    public StationsPage() // create the page
    {
        InitializeComponent(); // build the controls
        Theme.StyleGrid(dgv); // style the table
        LoadGrid(); // fill the table
    }

    private void LoadGrid() // load the stations into the table
    {
        string q = txtSearch.Text.Trim(); // the search word
        var rows = SampleData.Stations // from all stations
            .Where(s => q == "" || s.Code.Contains(q, StringComparison.OrdinalIgnoreCase) // if there is no search or the code has the word
                     || s.Name.Contains(q, StringComparison.OrdinalIgnoreCase) // or the name
                     || s.City.Contains(q, StringComparison.OrdinalIgnoreCase)) // or the city
            .Select(s => new { s.Id, s.Code, s.Name, s.City, Trips = SampleData.Trips.Count(t => t.StopAt(s) != null) }) // row data + number of trips that pass through it
            .ToList(); // turn them into a list
        dgv.Tag = q == "" ? "No stations yet. Fill in the form and press Add." : "No stations match your search."; // empty table message
        dgv.DataSource = rows; // show the rows
        dgv.Columns["Id"].Visible = false; // hide the id column
        lblCount.Text = $"{rows.Count} stations"; // number of stations
        ClearEditor(); // clear the edit boxes
    }

    private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid(); // when the search word changes, update the table

    private void dgv_CellClick(object sender, DataGridViewCellEventArgs e) // when a row is clicked
    {
        if (e.RowIndex < 0) return; // if the header was clicked, stop
        int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value; // station id
        selected = SampleData.Stations.First(s => s.Id == id); // get the station
        txtCode.Text = selected.Code; // fill the code
        txtName.Text = selected.Name; // fill the name
        txtCity.Text = selected.City; // fill the city
        errorProvider.Clear(); // clear the error marks
    }

    private bool ValidateInput() // check the typed data
    {
        errorProvider.Clear(); // clear the old errors
        bool ok = true; // is the data valid?
        if (!Regex.IsMatch(txtCode.Text, "^[A-Z]{3}$")) { errorProvider.SetError(txtCode, "Exactly 3 letters."); ok = false; } // the code must be 3 capital letters
        else if (SampleData.Stations.Any(s => s != selected && s.Code == txtCode.Text)) { errorProvider.SetError(txtCode, "Code already used."); ok = false; } // the code must not repeat
        if (txtName.Text.Trim().Length < 2) { errorProvider.SetError(txtName, "Name is required."); ok = false; } // the name is required
        if (txtCity.Text.Trim().Length < 2) { errorProvider.SetError(txtCity, "City is required."); ok = false; } // the city is required
        return ok; // return the result
    }

    private void btnAdd_Click(object sender, EventArgs e) // add button
    {
        selected = null; // adding never collides with "itself"
        if (!ValidateInput()) return; // if the data is wrong, stop
        SampleData.Stations.Add(new Station // add a new station
        {
            Id = SampleData.NextId(SampleData.Stations, s => s.Id), // new id
            Code = txtCode.Text, Name = txtName.Text.Trim(), City = txtCity.Text.Trim() // the data
        });
        LoadGrid(); // update the table
        Theme.Info("Station added."); // success message
    }

    private void btnUpdate_Click(object sender, EventArgs e) // edit button
    {
        if (selected == null) { Theme.Warn("Select a station from the list first."); return; } // a station must be selected first
        if (!ValidateInput()) return; // if the data is wrong, stop
        selected.Code = txtCode.Text; // update the code
        selected.Name = txtName.Text.Trim(); // update the name
        selected.City = txtCity.Text.Trim(); // update the city
        LoadGrid(); // update the table
        Theme.Info("Station updated."); // success message
    }

    private void btnDelete_Click(object sender, EventArgs e) // delete button
    {
        if (selected == null) { Theme.Warn("Select a station from the list first."); return; } // a station must be selected first
        if (SampleData.Trips.Any(t => t.StopAt(selected) != null)) // if trips pass through it
        {
            Theme.Warn("This station is used by scheduled trips and cannot be deleted."); // it cannot be deleted
            return; // stop
        }
        if (!Theme.Confirm($"Delete station '{selected.Name}'?")) return; // ask the user to confirm
        SampleData.Stations.Remove(selected); // delete the station
        LoadGrid(); // update the table
    }

    private void btnClear_Click(object sender, EventArgs e) => ClearEditor(); // clear button

    private void ClearEditor() // clear the edit boxes
    {
        selected = null; // no station selected
        txtCode.Clear(); // clear the code
        txtName.Clear(); // clear the name
        txtCity.Clear(); // clear the city
        errorProvider.Clear(); // clear the errors
        dgv.ClearSelection(); // unselect the row
    }
}
