using TrainReservation.Client.Models;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Forms;

public partial class SeatSelectionForm : Form
{
    private readonly Trip trip;
    private readonly TripStop from, to;
    private readonly SeatClass seatClass;
    private readonly decimal fare;

    public Ticket BookedTicket { get; private set; }

    private int Coach => cboCoach.SelectedIndex + 1;

    // parameterless constructor keeps the Visual Studio designer working
    public SeatSelectionForm() => InitializeComponent();

    public SeatSelectionForm(Trip trip, TripStop from, TripStop to, SeatClass seatClass) : this()
    {
        this.trip = trip;
        this.from = from;
        this.to = to;
        this.seatClass = seatClass;
        fare = Fares.Calc(from, to, seatClass);

        lblTrip.Text = $"Train {trip.Train}  ·  {seatClass} class";
        lblSegment.Text = $"{from.Station.Name}  →  {to.Station.Name}     " +
                          $"{trip.TimeAt(from):ddd dd MMM  HH:mm} – {trip.TimeAt(to):HH:mm}     " +
                          $"{to.KmFromOrigin - from.KmFromOrigin} km";
        lblPrice.Text = $"Fare: {fare:N2} EGP";

        seatMap.SeatCount = trip.Train.SeatsPerCoach;
        for (int c = 1; c <= trip.Train.CoachCount(seatClass); c++)
            cboCoach.Items.Add($"{seatClass} coach {c}");
        cboCoach.SelectedIndex = 0;
    }

    private void cboCoach_SelectedIndexChanged(object sender, EventArgs e)
    {
        var taken = Enumerable.Range(1, trip.Train.SeatsPerCoach)
            .Where(s => SampleData.IsSeatTaken(trip, seatClass, Coach, s, from, to))
            .ToList();
        seatMap.SetTaken(taken);
        lblFree.Text = $"{trip.Train.SeatsPerCoach - taken.Count} free seats in this coach";
        UpdateSelection();
    }

    private void seatMap_SelectedSeatChanged(object sender, EventArgs e) => UpdateSelection();

    private void UpdateSelection()
    {
        int seat = seatMap.SelectedSeat;
        btnConfirm.Enabled = seat > 0;
        lblSelected.Text = seat > 0 ? $"Coach {Coach}, seat {seat}" : "No seat selected";
    }

    private void btnConfirm_Click(object sender, EventArgs e)
    {
        int seat = seatMap.SelectedSeat;
        if (seat == 0) return;
        // re-check right before saving: the seat may have been taken meanwhile
        if (SampleData.IsSeatTaken(trip, seatClass, Coach, seat, from, to))
        {
            Theme.Warn("Sorry, that seat was just taken. Please pick another one.");
            cboCoach_SelectedIndexChanged(sender, e);
            return;
        }
        if (!Theme.Confirm($"Book coach {Coach}, seat {seat} for {fare:N2} EGP?")) return;

        BookedTicket = new Ticket
        {
            Id = SampleData.NextId(SampleData.Tickets, t => t.Id),
            Trip = trip, Passenger = SampleData.CurrentUser,
            From = from, To = to, Class = seatClass, Coach = Coach, Seat = seat,
            Price = fare, BookedAt = DateTime.Now
        };
        SampleData.Tickets.Add(BookedTicket);
        DialogResult = DialogResult.OK;
    }
}
