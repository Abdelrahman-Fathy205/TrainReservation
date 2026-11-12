using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Forms; // namespace for the screens

public partial class SeatSelectionForm : Form // seat selection screen
{
    private readonly Trip trip; // the trip
    private readonly TripStop from, to; // boarding and drop-off stations
    private readonly SeatClass seatClass; // the class
    private readonly decimal fare; // price of one seat

    public List<Ticket> BookedTickets { get; } = new(); // the tickets that were booked

    private int Coach => cboCoach.SelectedIndex + 1; // number of the selected coach

    // parameterless constructor keeps the Visual Studio designer working
    public SeatSelectionForm() => InitializeComponent(); // build the controls

    public SeatSelectionForm(Trip trip, TripStop from, TripStop to, SeatClass seatClass) : this() // create the screen for a given trip
    {
        this.trip = trip; // save the trip
        this.from = from; // save the boarding station
        this.to = to; // save the drop-off station
        this.seatClass = seatClass; // save the class
        fare = Fares.Calc(from, to, seatClass); // calculate the price

        lblTrip.Text = $"Train {trip.Train}  ·  {seatClass} class"; // write the train and class
        lblSegment.Text = $"{from.Station.Name}  →  {to.Station.Name}     " + // from station to station
                          $"{trip.TimeAt(from):ddd dd MMM  HH:mm} – {trip.TimeAt(to):HH:mm}     " + // departure and arrival times
                          $"{to.KmFromOrigin - from.KmFromOrigin} km"; // distance in km
        lblPrice.Text = $"Fare: {fare:N2} EGP"; // write the price

        seatMap.SeatCount = trip.Train.SeatsPerCoach; // number of seats in the coach map
        for (int c = 1; c <= trip.Train.CoachCount(seatClass); c++) // for each coach in this class
            cboCoach.Items.Add($"{seatClass} coach {c}"); // add it to the list
        cboCoach.SelectedIndex = 0; // select the first coach

        AcceptButton = btnConfirm; // Enter books the picked seats, Esc cancels
        ActiveControl = seatMap;   // arrow keys work straight away
        Theme.Apply(this); // apply the colors
    }

    private void cboCoach_SelectedIndexChanged(object sender, EventArgs e) // when the selected coach changes
    {
        var taken = Enumerable.Range(1, trip.Train.SeatsPerCoach) // all seat numbers
            .Where(s => SampleData.IsSeatTaken(trip, seatClass, Coach, s, from, to)) // keep only the booked ones
            .ToList(); // turn them into a list
        seatMap.SetTaken(taken); // mark the booked seats on the map
        lblFree.Text = $"{trip.Train.SeatsPerCoach - taken.Count} free seats in this coach"; // write the number of free seats
        UpdateSelection(); // update the selection info
    }

    private void seatMap_SelectedSeatChanged(object sender, EventArgs e) => UpdateSelection(); // when the picked seats change, update the info

    private void UpdateSelection() // update the selection info
    {
        var seats = seatMap.SelectedSeats; // the picked seats
        btnConfirm.Enabled = seats.Count > 0; // the confirm button works only if a seat is picked
        lblSelected.Text = seats.Count == 0 ? "No seat selected" // if none: "no seat selected"
            : $"Coach {Coach}, seat{(seats.Count > 1 ? "s" : "")} {string.Join(", ", seats)}"; // otherwise write the coach and seats
        lblPrice.Text = seats.Count > 1 ? $"Fare: {seats.Count} × {fare:N2} = {seats.Count * fare:N2} EGP" : $"Fare: {fare:N2} EGP"; // total price
    }

    private void btnConfirm_Click(object sender, EventArgs e) // when the confirm booking button is clicked
    {
        var seats = seatMap.SelectedSeats; // the picked seats
        if (seats.Count == 0) return; // if none, stop
        // re-check right before saving: a seat may have been taken meanwhile
        if (seats.Any(s => SampleData.IsSeatTaken(trip, seatClass, Coach, s, from, to))) // if any seat got booked in the meantime
        {
            Theme.Warn("Sorry, one of those seats was just taken. Please pick again."); // tell the user the seat was booked
            cboCoach_SelectedIndexChanged(sender, e); // update the map
            return; // stop
        }
        if (!Theme.Confirm($"Book coach {Coach}, seat{(seats.Count > 1 ? "s" : "")} {string.Join(", ", seats)} " + // ask the user to confirm the booking
                           $"for {seats.Count * fare:N2} EGP?")) return; // with the total price, if they say no, stop

        foreach (int seat in seats) // for each picked seat
        {
            var ticket = new Ticket // make a ticket
            {
                Id = SampleData.NextId(SampleData.Tickets, t => t.Id), // new id
                Trip = trip, Passenger = SampleData.CurrentUser, // the trip and the passenger
                From = from, To = to, Class = seatClass, Coach = Coach, Seat = seat, // the stations and the seat
                Price = fare, BookedAt = DateTime.Now // the price and booking time
            };
            SampleData.Tickets.Add(ticket); // add it to all tickets
            BookedTickets.Add(ticket); // and add it to the tickets booked now
        }
        DialogResult = DialogResult.OK; // close the screen with success
    }
}
