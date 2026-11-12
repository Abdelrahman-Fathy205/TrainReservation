using System.Text.RegularExpressions; // format checking library
using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Pages; // namespace for the pages

public partial class TrainsPage : UserControl // train management page
{
    private Train selected; // the currently selected train

    public TrainsPage() // create the page
    {
        InitializeComponent(); // build the controls
        Theme.StyleGrid(dgv); // style the table
        LoadGrid(); // fill the table
    }

    private void LoadGrid() // load the trains into the table
    {
        string q = txtSearch.Text.Trim(); // the search word
        var rows = SampleData.Trains // from all trains
            .Where(t => q == "" || t.Number.Contains(q) || t.Name.Contains(q, StringComparison.OrdinalIgnoreCase)) // filter by number or name
            .Select(t => new // row shape
            {
                t.Id, t.Number, t.Name, // internal id, train number, name
                First = t.FirstCoaches, Second = t.SecondCoaches, Economy = t.EconomyCoaches, // number of coaches for each class
                SeatsPerCoach = t.SeatsPerCoach, t.Capacity // seats per coach and capacity
            })
            .ToList(); // turn them into a list
        dgv.Tag = q == "" ? "No trains yet. Fill in the form and press Add." : "No trains match your search."; // empty table message
        dgv.DataSource = rows; // show the rows
        dgv.Columns["Id"].Visible = false; // hide the id column
        lblCount.Text = $"{rows.Count} trains"; // number of trains
        ClearEditor(); // clear the edit boxes
    }

    private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid(); // when the search changes, update the table

    private void Capacity_Changed(object sender, EventArgs e) => // when any coach or seat number changes
        lblCapacity.Text = $"Total capacity: {(numFirst.Value + numSecond.Value + numEconomy.Value) * numSeats.Value} seats"; // calculate and write the total capacity

    private void dgv_CellClick(object sender, DataGridViewCellEventArgs e) // when a row is clicked
    {
        if (e.RowIndex < 0) return; // if the header was clicked, stop
        int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value; // train number
        selected = SampleData.Trains.First(t => t.Id == id); // get the train
        txtNumber.Text = selected.Number; // fill the number
        txtName.Text = selected.Name; // fill the name
        numFirst.Value = selected.FirstCoaches; // first class coaches
        numSecond.Value = selected.SecondCoaches; // second class coaches
        numEconomy.Value = selected.EconomyCoaches; // economy coaches
        numSeats.Value = selected.SeatsPerCoach; // seats per coach
        errorProvider.Clear(); // clear the errors
    }

    private bool ValidateInput() // check the data
    {
        errorProvider.Clear(); // clear the old errors
        bool ok = true; // is the data valid?
        if (!Regex.IsMatch(txtNumber.Text.Trim(), @"^\d{2,5}$")) { errorProvider.SetError(txtNumber, "2-5 digits."); ok = false; } // the number has 2 to 5 digits
        else if (SampleData.Trains.Any(t => t != selected && t.Number == txtNumber.Text.Trim())) { errorProvider.SetError(txtNumber, "Number already used."); ok = false; } // the number must not repeat
        if (txtName.Text.Trim().Length < 3) { errorProvider.SetError(txtName, "Name is required."); ok = false; } // the name is required
        if (numFirst.Value + numSecond.Value + numEconomy.Value == 0) { errorProvider.SetError(numEconomy, "Add at least one coach."); ok = false; } // at least one coach is needed
        if (numSeats.Value % 4 != 0) { errorProvider.SetError(numSeats, "Must be a multiple of 4 (2 + 2 layout)."); ok = false; } // seats must be a multiple of 4
        return ok; // return the result
    }

    private void Apply(Train t) // copy the data from the boxes to the train
    {
        t.Number = txtNumber.Text.Trim(); // the number
        t.Name = txtName.Text.Trim(); // the name
        t.FirstCoaches = (int)numFirst.Value; // first class coaches
        t.SecondCoaches = (int)numSecond.Value; // second class coaches
        t.EconomyCoaches = (int)numEconomy.Value; // economy coaches
        t.SeatsPerCoach = (int)numSeats.Value; // seats per coach
    }

    private void btnAdd_Click(object sender, EventArgs e) // add button
    {
        selected = null; // no train selected (for the duplicate check)
        if (!ValidateInput()) return; // if the data is wrong, stop
        var t = new Train { Id = SampleData.NextId(SampleData.Trains, x => x.Id) }; // a new train with a new id
        Apply(t); // fill its data
        SampleData.Trains.Add(t); // add it
        LoadGrid(); // update the table
        Theme.Info("Train added."); // success message
    }

    private void btnUpdate_Click(object sender, EventArgs e) // edit button
    {
        if (selected == null) { Theme.Warn("Select a train from the list first."); return; } // a train must be selected first
        if (!ValidateInput()) return; // if the data is wrong, stop
        if (SampleData.Tickets.Any(k => k.Trip.Train == selected) && // if the train has booked tickets
            (numSeats.Value != selected.SeatsPerCoach || // and the seat count changed
             numFirst.Value < selected.FirstCoaches || numSecond.Value < selected.SecondCoaches || numEconomy.Value < selected.EconomyCoaches)) // or coaches were reduced
        {
            Theme.Warn("This train has booked tickets: you can add coaches but not remove coaches or change seats per coach."); // not allowed (you can only add more)
            return; // stop
        }
        Apply(selected); // save the changes
        LoadGrid(); // update the table
        Theme.Info("Train updated."); // success message
    }

    private void btnDelete_Click(object sender, EventArgs e) // delete button
    {
        if (selected == null) { Theme.Warn("Select a train from the list first."); return; } // a train must be selected first
        if (SampleData.Trips.Any(t => t.Train == selected)) // if the train has trips
        {
            Theme.Warn("This train has scheduled trips and cannot be deleted."); // it cannot be deleted
            return; // stop
        }
        if (!Theme.Confirm($"Delete train {selected}?")) return; // ask the user to confirm
        SampleData.Trains.Remove(selected); // delete the train
        LoadGrid(); // update the table
    }

    private void btnClear_Click(object sender, EventArgs e) => ClearEditor(); // clear button

    private void ClearEditor() // clear the edit boxes
    {
        selected = null; // no train selected
        txtNumber.Clear(); // clear the number
        txtName.Clear(); // clear the name
        numFirst.Value = 0; // set first class coaches to zero
        numSecond.Value = 0; // set second class coaches to zero
        numEconomy.Value = 0; // set economy coaches to zero
        numSeats.Value = 40; // default seats: 40
        errorProvider.Clear(); // clear the errors
        dgv.ClearSelection(); // unselect the row
    }
}
