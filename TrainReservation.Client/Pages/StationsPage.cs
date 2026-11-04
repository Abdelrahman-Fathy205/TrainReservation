using System.Text.RegularExpressions;
using TrainReservation.Client.Models;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Pages;

public partial class StationsPage : UserControl
{
    private Station selected;

    public StationsPage()
    {
        InitializeComponent();
        Theme.StyleGrid(dgv);
        LoadGrid();
    }

    private void LoadGrid()
    {
        string q = txtSearch.Text.Trim();
        var rows = SampleData.Stations
            .Where(s => q == "" || s.Code.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || s.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                     || s.City.Contains(q, StringComparison.OrdinalIgnoreCase))
            .Select(s => new { s.Id, s.Code, s.Name, s.City, Trips = SampleData.Trips.Count(t => t.StopAt(s) != null) })
            .ToList();
        dgv.DataSource = rows;
        dgv.Columns["Id"].Visible = false;
        lblCount.Text = $"{rows.Count} stations";
        ClearEditor();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e) => LoadGrid();

    private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        int id = (int)dgv.Rows[e.RowIndex].Cells["Id"].Value;
        selected = SampleData.Stations.First(s => s.Id == id);
        txtCode.Text = selected.Code;
        txtName.Text = selected.Name;
        txtCity.Text = selected.City;
        errorProvider.Clear();
    }

    private bool ValidateInput()
    {
        errorProvider.Clear();
        bool ok = true;
        if (!Regex.IsMatch(txtCode.Text, "^[A-Z]{3}$")) { errorProvider.SetError(txtCode, "Exactly 3 letters."); ok = false; }
        else if (SampleData.Stations.Any(s => s != selected && s.Code == txtCode.Text)) { errorProvider.SetError(txtCode, "Code already used."); ok = false; }
        if (txtName.Text.Trim().Length < 2) { errorProvider.SetError(txtName, "Name is required."); ok = false; }
        if (txtCity.Text.Trim().Length < 2) { errorProvider.SetError(txtCity, "City is required."); ok = false; }
        return ok;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        selected = null; // adding never collides with "itself"
        if (!ValidateInput()) return;
        SampleData.Stations.Add(new Station
        {
            Id = SampleData.NextId(SampleData.Stations, s => s.Id),
            Code = txtCode.Text, Name = txtName.Text.Trim(), City = txtCity.Text.Trim()
        });
        LoadGrid();
        Theme.Info("Station added.");
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
        if (selected == null) { Theme.Warn("Select a station from the list first."); return; }
        if (!ValidateInput()) return;
        selected.Code = txtCode.Text;
        selected.Name = txtName.Text.Trim();
        selected.City = txtCity.Text.Trim();
        LoadGrid();
        Theme.Info("Station updated.");
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (selected == null) { Theme.Warn("Select a station from the list first."); return; }
        if (SampleData.Trips.Any(t => t.StopAt(selected) != null))
        {
            Theme.Warn("This station is used by scheduled trips and cannot be deleted.");
            return;
        }
        if (!Theme.Confirm($"Delete station '{selected.Name}'?")) return;
        SampleData.Stations.Remove(selected);
        LoadGrid();
    }

    private void btnClear_Click(object sender, EventArgs e) => ClearEditor();

    private void ClearEditor()
    {
        selected = null;
        txtCode.Clear();
        txtName.Clear();
        txtCity.Clear();
        errorProvider.Clear();
        dgv.ClearSelection();
    }
}
