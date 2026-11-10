using System.Diagnostics;
using TrainReservation.Client.Models;

namespace TrainReservation.Client.Services;

/// <summary>
/// In-memory cache of the data. <see cref="Db"/> loads it from SQL Server at startup and writes every change back;
/// <see cref="Seed"/> fills it with demo data the first time (or when the database is unavailable).
/// </summary>
public static class SampleData
{
    public static readonly List<User> Users = new();
    public static readonly List<Station> Stations = new();
    public static readonly List<Train> Trains = new();
    public static readonly List<Trip> Trips = new();
    public static readonly List<Ticket> Tickets = new();

    public static User CurrentUser { get; set; }

    public static int NextId<T>(List<T> list, Func<T, int> id) => list.Count == 0 ? 1 : list.Max(id) + 1;

    /// <summary>Two bookings clash only if their segments share at least one km of track.</summary>
    static bool Overlaps(Ticket t, TripStop from, TripStop to) =>
        t.From.KmFromOrigin < to.KmFromOrigin && from.KmFromOrigin < t.To.KmFromOrigin;

    public static bool IsSeatTaken(Trip trip, SeatClass c, int coach, int seat, TripStop from, TripStop to) =>
        Tickets.Any(t => t.Trip == trip && t.Class == c && t.Coach == coach && t.Seat == seat && Overlaps(t, from, to));

    public static int FreeSeats(Trip trip, SeatClass c, TripStop from, TripStop to)
    {
        int capacity = trip.Train.CoachCount(c) * trip.Train.SeatsPerCoach;
        int taken = Tickets.Where(t => t.Trip == trip && t.Class == c && Overlaps(t, from, to))
                           .Select(t => (t.Coach, t.Seat)).Distinct().Count();
        return capacity - taken;
    }

    [Conditional("DEBUG")]
    public static void SelfCheck()
    {
        var s = Trips[0].Stops; // Cairo, Banha, Tanta, Damanhour, Alexandria
        var cairoTanta = new Ticket { From = s[0], To = s[2] };
        Debug.Assert(!Overlaps(cairoTanta, s[2], s[4]), "Cairo→Tanta must not block Tanta→Alexandria");
        Debug.Assert(Overlaps(cairoTanta, s[1], s[3]), "Cairo→Tanta must block Banha→Damanhour");
        Debug.Assert(Fares.Calc(s[0], s[4], SeatClass.Second) == 208m, "208 km at 1.00/km");
    }

    public static void Seed()
    {
        Users.Clear(); Stations.Clear(); Trains.Clear(); Trips.Clear(); Tickets.Clear();
        Users.AddRange(new[]
        {
            new User { Id = 1, Username = "admin", Password = "admin123", FullName = "System Admin", Email = "admin@rail.eg", Phone = "01000000000", Role = UserRole.Admin },
            new User { Id = 2, Username = "abdelrahman", Password = "123456", FullName = "Abdelrahman Fathy", Email = "abdelrahman@mail.com", Phone = "01012345678", Role = UserRole.Passenger },
            new User { Id = 3, Username = "mona", Password = "123456", FullName = "Mona Ali", Email = "mona@mail.com", Phone = "01122334455", Role = UserRole.Passenger },
            new User { Id = 4, Username = "omar", Password = "123456", FullName = "Omar Khaled", Email = "omar@mail.com", Phone = "01233445566", Role = UserRole.Passenger },
        });

        string[,] st =
        {
            { "CAI", "Cairo (Ramses)", "Cairo" }, { "GIZ", "Giza", "Giza" }, { "BNH", "Banha", "Qalyubia" },
            { "TNT", "Tanta", "Gharbia" }, { "DMN", "Damanhour", "Beheira" }, { "ALX", "Alexandria (Misr)", "Alexandria" },
            { "MNY", "Minya", "Minya" }, { "AST", "Assiut", "Assiut" }, { "SHG", "Sohag", "Sohag" },
            { "LXR", "Luxor", "Luxor" }, { "ASW", "Aswan", "Aswan" },
        };
        for (int i = 0; i < st.GetLength(0); i++)
            Stations.Add(new Station { Id = i + 1, Code = st[i, 0], Name = st[i, 1], City = st[i, 2] });

        Trains.AddRange(new[]
        {
            new Train { Id = 1, Number = "905", Name = "Alexandria Express", FirstCoaches = 1, SecondCoaches = 2, EconomyCoaches = 3, SeatsPerCoach = 40 },
            new Train { Id = 2, Number = "2008", Name = "Upper Egypt Sleeper", FirstCoaches = 2, SecondCoaches = 2, EconomyCoaches = 4, SeatsPerCoach = 40 },
            new Train { Id = 3, Number = "934", Name = "Delta Spanish", FirstCoaches = 0, SecondCoaches = 2, EconomyCoaches = 4, SeatsPerCoach = 48 },
        });

        var north = new (string Code, int Km, int Min)[] { ("CAI", 0, 0), ("BNH", 48, 40), ("TNT", 93, 75), ("DMN", 160, 125), ("ALX", 208, 165) };
        var south = new (string Code, int Km, int Min)[] { ("CAI", 0, 0), ("GIZ", 5, 15), ("MNY", 245, 210), ("AST", 375, 330), ("SHG", 470, 420), ("LXR", 670, 600), ("ASW", 880, 780) };

        void AddTrip(Train train, DateTime dep, (string Code, int Km, int Min)[] route) => Trips.Add(new Trip
        {
            Id = Trips.Count + 1,
            Train = train,
            Departure = dep,
            Stops = route.Select(r => new TripStop { Station = Stations.First(s => s.Code == r.Code), KmFromOrigin = r.Km, MinutesFromStart = r.Min }).ToList()
        });

        for (int d = -14; d <= 6; d++)
        {
            var day = DateTime.Today.AddDays(d);
            AddTrip(Trains[0], day.AddHours(8), north);
            AddTrip(Trains[2], day.AddHours(14.5), north);
            AddTrip(Trains[1], day.AddHours(20), south);
        }

        // background bookings belong to a crowd account that cannot log in (empty password fails login validation)
        var crowd = new User { Id = 99, Username = "", FullName = "Other passengers", Role = UserRole.Passenger };
        var rnd = new Random(42);
        foreach (var trip in Trips)
        {
            int target = rnd.Next(30, 120);
            for (int k = 0, booked = 0; k < target * 2 && booked < target; k++)
            {
                var cls = (SeatClass)rnd.Next(3);
                int coaches = trip.Train.CoachCount(cls);
                if (coaches == 0) continue;
                int coach = rnd.Next(1, coaches + 1), seat = rnd.Next(1, trip.Train.SeatsPerCoach + 1);
                int a = rnd.Next(0, trip.Stops.Count - 1), b = rnd.Next(a + 1, trip.Stops.Count);
                var from = trip.Stops[a];
                var to = trip.Stops[b];
                if (IsSeatTaken(trip, cls, coach, seat, from, to)) continue;
                Tickets.Add(new Ticket
                {
                    Id = Tickets.Count + 1, Trip = trip, Passenger = rnd.Next(100) == 0 ? Users[rnd.Next(1, Users.Count)] : crowd,
                    From = from, To = to, Class = cls, Coach = coach, Seat = seat,
                    Price = Fares.Calc(from, to, cls), BookedAt = trip.Departure.AddDays(-rnd.Next(1, 10))
                });
                booked++;
            }
        }
        Users.Add(crowd); // added last so the random passenger pick above never chooses it; the DB needs it for the ticket foreign key
    }
}
