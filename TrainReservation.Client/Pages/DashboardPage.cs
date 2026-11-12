using TrainReservation.Client.Models; // data models
using TrainReservation.Client.Services; // the services
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Pages; // namespace for the pages

public partial class DashboardPage : UserControl // dashboard page
{
    private Ticket nextTicket; // the passenger's next ticket

    public DashboardPage() // create the page
    {
        InitializeComponent(); // build the controls
        Theme.StyleGrid(dgv); // style the table
        var now = DateTime.Now; // the current time
        var user = SampleData.CurrentUser; // the current user

        if (user.Role == UserRole.Admin) // if admin
        {
            var upcoming = SampleData.Trips.Where(t => t.Departure >= now).OrderBy(t => t.Departure).ToList(); // upcoming trips sorted by time
            SetCard(lblTitle1, lblValue1, "Upcoming trips", upcoming.Count.ToString()); // card: number of upcoming trips
            SetCard(lblTitle2, lblValue2, "Tickets sold", SampleData.Tickets.Count.ToString("N0")); // card: number of tickets sold
            SetCard(lblTitle3, lblValue3, "Active trains", SampleData.Trains.Count.ToString()); // card: number of trains
            SetCard(lblTitle4, lblValue4, "Revenue (EGP)", SampleData.Tickets.Sum(t => t.Price).ToString("N0")); // card: revenue
            lblList.Text = "Upcoming departures"; // table title
            dgv.Tag = "No upcoming trips. Schedule one on the Trips page."; // message when the table is empty
            dgv.DataSource = upcoming.Take(15).Select(t => new // show the first 15 trips
            {
                Train = t.Train.ToString(), // the train
                t.Route, // the route
                Departs = t.Departure.ToString("ddd dd MMM  HH:mm"), // departure time
                Arrives = t.Arrival.ToString("HH:mm"), // arrival time
                Booked = SampleData.Tickets.Count(k => k.Trip == t), // number booked
                Capacity = t.Train.Capacity // capacity
            }).ToList(); // turn them into a list
        }
        else // if passenger
        {
            var mine = SampleData.Tickets.Where(t => t.Passenger == user).ToList(); // my tickets
            var next = mine.Where(t => t.Departs >= now).OrderBy(t => t.Departs).ToList(); // my upcoming tickets, sorted
            SetCard(lblTitle1, lblValue1, "My tickets", mine.Count.ToString()); // card: number of my tickets
            SetCard(lblTitle2, lblValue2, "Upcoming journeys", next.Count.ToString()); // card: upcoming trips
            SetCard(lblTitle3, lblValue3, "Total spent (EGP)", mine.Sum(t => t.Price).ToString("N0")); // card: total paid
            SetCard(lblTitle4, lblValue4, "Next departure", next.Count > 0 ? next[0].Departs.ToString("dd MMM HH:mm") : "—"); // card: next trip
            lblList.Text = "Your upcoming journeys"; // table title
            dgv.Tag = "No upcoming journeys. Open Book a Ticket to find a train."; // message when the table is empty
            if (next.Count > 0) // if there is an upcoming trip
            {
                nextTicket = next[0]; // the nearest ticket
                var wait = nextTicket.Departs - now; // time left
                string inText = wait.TotalHours >= 48 ? $"in {(int)wait.TotalDays} days" // if more than 2 days, show in days
                              : wait.TotalHours >= 1 ? $"in {(int)wait.TotalHours} h {wait.Minutes} min" // if more than an hour, show in hours
                              : $"in {Math.Max(1, wait.Minutes)} min"; // otherwise in minutes
                lblNextRoute.Text = $"{nextTicket.From.Station.Name}  →  {nextTicket.To.Station.Name}"; // from where to where
                lblNextDetail.Text = $"{nextTicket.Departs:ddd dd MMM, HH:mm}  ({inText})   ·   Train {nextTicket.Trip.Train.Number}   ·   " + // the time and the train
                                     $"{nextTicket.Class} class   ·   Coach {nextTicket.Coach}, seat {nextTicket.Seat}"; // class, coach and seat
                pnlNextHost.Visible = true; // show the next trip card
            }
            dgv.DataSource = next.Select(t => new // show the upcoming tickets in the table
            {
                Ticket = t.Code, // ticket code
                Train = t.Trip.Train.ToString(), // the train
                From = t.From.Station.Name, // from
                To = t.To.Station.Name, // to
                Departs = t.Departs.ToString("ddd dd MMM  HH:mm"), // the time
                Seat = $"{t.Class} · Coach {t.Coach} · Seat {t.Seat}" // the seat
            }).ToList(); // turn them into a list
        }
    }

    private void btnNextPrint_Click(object sender, EventArgs e) => TicketPrinter.Preview(nextTicket, this); // print the next ticket

    static void SetCard(Label title, Label value, string titleText, string valueText) // fill a card with a title and a value
    {
        title.Text = titleText; // the title
        value.Text = valueText; // the value
    }
}
