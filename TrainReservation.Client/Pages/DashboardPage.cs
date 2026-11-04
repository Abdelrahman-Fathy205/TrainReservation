using TrainReservation.Client.Models;
using TrainReservation.Client.Services;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Pages;

public partial class DashboardPage : UserControl
{
    public DashboardPage()
    {
        InitializeComponent();
        Theme.StyleGrid(dgv);
        var now = DateTime.Now;
        var user = SampleData.CurrentUser;

        if (user.Role == UserRole.Admin)
        {
            var upcoming = SampleData.Trips.Where(t => t.Departure >= now).OrderBy(t => t.Departure).ToList();
            SetCard(lblTitle1, lblValue1, "Upcoming trips", upcoming.Count.ToString());
            SetCard(lblTitle2, lblValue2, "Tickets sold", SampleData.Tickets.Count.ToString("N0"));
            SetCard(lblTitle3, lblValue3, "Active trains", SampleData.Trains.Count.ToString());
            SetCard(lblTitle4, lblValue4, "Revenue (EGP)", SampleData.Tickets.Sum(t => t.Price).ToString("N0"));
            lblList.Text = "Upcoming departures";
            dgv.DataSource = upcoming.Take(15).Select(t => new
            {
                Train = t.Train.ToString(),
                t.Route,
                Departs = t.Departure.ToString("ddd dd MMM  HH:mm"),
                Arrives = t.Arrival.ToString("HH:mm"),
                Booked = SampleData.Tickets.Count(k => k.Trip == t),
                Capacity = t.Train.Capacity
            }).ToList();
        }
        else
        {
            var mine = SampleData.Tickets.Where(t => t.Passenger == user).ToList();
            var next = mine.Where(t => t.Departs >= now).OrderBy(t => t.Departs).ToList();
            SetCard(lblTitle1, lblValue1, "My tickets", mine.Count.ToString());
            SetCard(lblTitle2, lblValue2, "Upcoming journeys", next.Count.ToString());
            SetCard(lblTitle3, lblValue3, "Total spent (EGP)", mine.Sum(t => t.Price).ToString("N0"));
            SetCard(lblTitle4, lblValue4, "Next departure", next.Count > 0 ? next[0].Departs.ToString("dd MMM HH:mm") : "—");
            lblList.Text = "Your upcoming journeys";
            dgv.DataSource = next.Select(t => new
            {
                Ticket = t.Code,
                Train = t.Trip.Train.ToString(),
                From = t.From.Station.Name,
                To = t.To.Station.Name,
                Departs = t.Departs.ToString("ddd dd MMM  HH:mm"),
                Seat = $"{t.Class} · Coach {t.Coach} · Seat {t.Seat}"
            }).ToList();
        }
    }

    static void SetCard(Label title, Label value, string titleText, string valueText)
    {
        title.Text = titleText;
        value.Text = valueText;
    }
}
