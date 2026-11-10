using System.Text.RegularExpressions;
using TrainReservation.Client.Models;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Pages;

public partial class TrainsPage : UserControl
{
    private Train selected;

    public TrainsPage()
    {
        InitializeComponent();
        Theme.StyleGrid(dgv);
        LoadGrid();
    }

    private void LoadGrid()
    {
        string q = txtSearch.Text.Trim();
        var rows = SampleData.Trains
            .Where(t => q == "" || t.Number.Contains(q) || t.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
            .Select(t => new
            {
                t.Id, t.Number, t.Name,
                First = t.FirstCoaches, Second = t.SecondCoaches, Economy = t.EconomyCoaches,
                SeatsPerCoach = t.SeatsPerCoach, t.Capacity
            })
            .ToList();
        dgv.DataSource = rows;
        dgv.Columns["Id"].Visible = false;
        lblCount.Text = $"{rows.Count} trains";
        ClearEditor();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid();

    private void Capacity_Changed(object sender, EventArgs e) =>
        lblCapacity.Text = $"Total capacity: {(numFirst.Value + numSecond.Value + numEconomy.Value) * numSeats.Value} seats";

    private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value;
        selected = SampleData.Trains.First(t => t.Id == id);
        txtNumber.Text = selected.Number;
        txtName.Text = selected.Name;
        numFirst.Value = selected.FirstCoaches;
        numSecond.Value = selected.SecondCoaches;
        numEconomy.Value = selected.EconomyCoaches;
        numSeats.Value = selected.SeatsPerCoach;
        errorProvider.Clear();
    }

    private bool ValidateInput()
    {
        errorProvider.Clear();
        bool ok = true;
        if (!Regex.IsMatch(txtNumber.Text.Trim(), @"^\d{2,5}$")) { errorProvider.SetError(txtNumber, "2-5 digits."); ok = false; }
        else if (SampleData.Trains.Any(t => t != selected && t.Number == txtNumber.Text.Trim())) { errorProvider.SetError(txtNumber, "Number already used."); ok = false; }
        if (txtName.Text.Trim().Length < 3) { errorProvider.SetError(txtName, "Name is required."); ok = false; }
        if (numFirst.Value + numSecond.Value + numEconomy.Value == 0) { errorProvider.SetError(numEconomy, "Add at least one coach."); ok = false; }
        if (numSeats.Value % 4 != 0) { errorProvider.SetError(numSeats, "Must be a multiple of 4 (2 + 2 layout)."); ok = false; }
        return ok;
    }

    private void Apply(Train t)
    {
        t.Number = txtNumber.Text.Trim();
        t.Name = txtName.Text.Trim();
        t.FirstCoaches = (int)numFirst.Value;
        t.SecondCoaches = (int)numSecond.Value;
        t.EconomyCoaches = (int)numEconomy.Value;
        t.SeatsPerCoach = (int)numSeats.Value;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        selected = null;
        if (!ValidateInput()) return;
        var t = new Train { Id = SampleData.NextId(SampleData.Trains, x => x.Id) };
        Apply(t);
        Db.Save(t);
        SampleData.Trains.Add(t);
        LoadGrid();
        Theme.Info("Train added.");
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
        if (selected == null) { Theme.Warn("Select a train from the list first."); return; }
        if (!ValidateInput()) return;
        if (SampleData.Tickets.Any(k => k.Trip.Train == selected) &&
            (numSeats.Value != selected.SeatsPerCoach ||
             numFirst.Value < selected.FirstCoaches || numSecond.Value < selected.SecondCoaches || numEconomy.Value < selected.EconomyCoaches))
        {
            Theme.Warn("This train has booked tickets: you can add coaches but not remove coaches or change seats per coach.");
            return;
        }
        Apply(selected);
        Db.Save(selected);
        LoadGrid();
        Theme.Info("Train updated.");
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (selected == null) { Theme.Warn("Select a train from the list first."); return; }
        if (SampleData.Trips.Any(t => t.Train == selected))
        {
            Theme.Warn("This train has scheduled trips and cannot be deleted.");
            return;
        }
        if (!Theme.Confirm($"Delete train {selected}?")) return;
        Db.Delete("Trains", selected.Id);
        SampleData.Trains.Remove(selected);
        LoadGrid();
    }

    private void btnClear_Click(object sender, EventArgs e) => ClearEditor();

    private void ClearEditor()
    {
        selected = null;
        txtNumber.Clear();
        txtName.Clear();
        numFirst.Value = 0;
        numSecond.Value = 0;
        numEconomy.Value = 0;
        numSeats.Value = 40;
        errorProvider.Clear();
        dgv.ClearSelection();
    }
}
