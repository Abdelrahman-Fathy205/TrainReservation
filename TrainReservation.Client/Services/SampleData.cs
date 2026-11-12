using System.Diagnostics; // checking library (Debug.Assert)
using TrainReservation.Client.Models; // use the data models

namespace TrainReservation.Client.Services; // namespace for the services

/// <summary>
/// In-memory data so the GUI can be demoed on its own.
/// Saved to a JSON file on exit (see DataStore) and loaded on the next start; the demo data is only generated on first run.
/// shortcut: one local file per Windows user; replace with calls to the TCP server (which owns SQL Server) in the next phase.
/// </summary>
public static class SampleData // class that holds all the program data in memory
{
    public static readonly List<User> Users = new(); // list of users
    public static readonly List<Station> Stations = new(); // list of stations
    public static readonly List<Train> Trains = new(); // list of trains
    public static readonly List<Trip> Trips = new(); // list of trips
    public static readonly List<Ticket> Tickets = new(); // list of tickets

    public static User CurrentUser { get; set; } // the user who is logged in now

    public static int NextId<T>(List<T> list, Func<T, int> id) => list.Count == 0 ? 1 : list.Max(id) + 1; // gets a new id: biggest id + 1 (or 1 if the list is empty)

    /// <summary>Two bookings clash only if their segments share at least one km of track.</summary>
    static bool Overlaps(Ticket t, TripStop from, TripStop to) => // does this ticket overlap the requested part of the route?
        t.From.KmFromOrigin < to.KmFromOrigin && from.KmFromOrigin < t.To.KmFromOrigin; // they overlap if they share part of the route

    public static bool IsSeatTaken(Trip trip, SeatClass c, int coach, int seat, TripStop from, TripStop to) => // is this seat booked?
        Tickets.Any(t => t.Trip == trip && t.Class == c && t.Coach == coach && t.Seat == seat && Overlaps(t, from, to)); // booked if a ticket for the same seat overlaps the route part

    public static int FreeSeats(Trip trip, SeatClass c, TripStop from, TripStop to) // number of free seats in a class
    {
        int capacity = trip.Train.CoachCount(c) * trip.Train.SeatsPerCoach; // capacity = coaches × seats
        int taken = Tickets.Where(t => t.Trip == trip && t.Class == c && Overlaps(t, from, to)) // the tickets that overlap the route part
                           .Select(t => (t.Coach, t.Seat)).Distinct().Count(); // number of booked seats without duplicates
        return capacity - taken; // free = capacity - booked
    }

    [Conditional("DEBUG")] // this method runs only in debug mode
    public static void SelfCheck() // self check that the calculations are correct
    {
        var s = Trips[0].Stops; // Cairo, Banha, Tanta, Damanhour, Alexandria
        var cairoTanta = new Ticket { From = s[0], To = s[2] }; // sample ticket from Cairo to Tanta
        Debug.Assert(!Overlaps(cairoTanta, s[2], s[4]), "Cairo→Tanta must not block Tanta→Alexandria"); // Cairo-Tanta does not block Tanta-Alexandria
        Debug.Assert(Overlaps(cairoTanta, s[1], s[3]), "Cairo→Tanta must block Banha→Damanhour"); // Cairo-Tanta must block Benha-Damanhour
        Debug.Assert(Fares.Calc(s[0], s[4], SeatClass.Second) == 208m, "208 km at 1.00/km"); // 208 km in second class = 208 pounds

        var hash = Passwords.Hash("secret1"); // hash a sample password
        Debug.Assert(Passwords.Verify("secret1", hash) && !Passwords.Verify("secret2", hash), "password hash round trip"); // the right password works and the wrong one fails
        Debug.Assert(!Passwords.Verify("", ""), "empty hash never matches"); // an empty hash never works

        // save → reload into empty lists must give the same data back
        string realPath = DataStore.FilePath; // save the real path
        DataStore.FilePath = Path.Combine(Path.GetTempPath(), $"tr-selfcheck-{Environment.ProcessId}.json"); // use a temp file for the test
        var before = (Users.Count, Trips.Count, Tickets.Count, Tickets[^1].Code, Tickets[^1].From.Station.Name, Tickets[^1].Passenger.Username); // record the data before saving
        DataStore.Save(); // save
        Users.Clear(); Stations.Clear(); Trains.Clear(); Trips.Clear(); Tickets.Clear(); // clear all the lists
        Debug.Assert(DataStore.TryLoad(), "self-check file loads"); // load again, it must work
        var after = (Users.Count, Trips.Count, Tickets.Count, Tickets[^1].Code, Tickets[^1].From.Station.Name, Tickets[^1].Passenger.Username); // record the data after loading
        Debug.Assert(before == after, "data survives save/load"); // it must be the same
        File.Delete(DataStore.FilePath); // delete the temp file
        DataStore.FilePath = realPath; // put back the real path
    }

    static SampleData() // runs once, the first time the class is used
    {
        if (!DataStore.TryLoad()) Seed(); // if there is no saved file, make sample data
    }

    static void Seed() // create sample data
    {
        Users.AddRange(new[] // add sample users
        {
            new User { Id = 1, Username = "admin", PasswordHash = Passwords.Hash("admin123"), FullName = "System Admin", Email = "admin@rail.eg", Phone = "01000000000", Role = UserRole.Admin }, // admin account
            new User { Id = 2, Username = "abdelrahman", PasswordHash = Passwords.Hash("123456"), FullName = "Abdelrahman Fathy", Email = "abdelrahman@mail.com", Phone = "01012345678", Role = UserRole.Passenger }, // passenger
            new User { Id = 3, Username = "mona", PasswordHash = Passwords.Hash("123456"), FullName = "Mona Ali", Email = "mona@mail.com", Phone = "01122334455", Role = UserRole.Passenger }, // passenger
            new User { Id = 4, Username = "omar", PasswordHash = Passwords.Hash("123456"), FullName = "Omar Khaled", Email = "omar@mail.com", Phone = "01233445566", Role = UserRole.Passenger }, // passenger
        });

        string[,] st = // stations table: code, name, city
        {
            { "CAI", "Cairo (Ramses)", "Cairo" }, { "GIZ", "Giza", "Giza" }, { "BNH", "Banha", "Qalyubia" }, // Cairo, Giza, Benha
            { "TNT", "Tanta", "Gharbia" }, { "DMN", "Damanhour", "Beheira" }, { "ALX", "Alexandria (Misr)", "Alexandria" }, // Tanta, Damanhour, Alexandria
            { "MNY", "Minya", "Minya" }, { "AST", "Assiut", "Assiut" }, { "SHG", "Sohag", "Sohag" }, // Minya, Asyut, Sohag
            { "LXR", "Luxor", "Luxor" }, { "ASW", "Aswan", "Aswan" }, // Luxor, Aswan
        };
        for (int i = 0; i < st.GetLength(0); i++) // loop over each row in the table
            Stations.Add(new Station { Id = i + 1, Code = st[i, 0], Name = st[i, 1], City = st[i, 2] }); // add the station

        Trains.AddRange(new[] // add sample trains
        {
            new Train { Id = 1, Number = "905", Name = "Alexandria Express", FirstCoaches = 1, SecondCoaches = 2, EconomyCoaches = 3, SeatsPerCoach = 40 }, // Alexandria express train
            new Train { Id = 2, Number = "2008", Name = "Upper Egypt Sleeper", FirstCoaches = 2, SecondCoaches = 2, EconomyCoaches = 4, SeatsPerCoach = 40 }, // sleeper train to Upper Egypt
            new Train { Id = 3, Number = "934", Name = "Delta Spanish", FirstCoaches = 0, SecondCoaches = 2, EconomyCoaches = 4, SeatsPerCoach = 48 }, // Spanish train to the Delta
        });

        var north = new (string Code, int Km, int Min)[] { ("CAI", 0, 0), ("BNH", 48, 40), ("TNT", 93, 75), ("DMN", 160, 125), ("ALX", 208, 165) }; // north line: station, km, minutes
        var south = new (string Code, int Km, int Min)[] { ("CAI", 0, 0), ("GIZ", 5, 15), ("MNY", 245, 210), ("AST", 375, 330), ("SHG", 470, 420), ("LXR", 670, 600), ("ASW", 880, 780) }; // south line

        void AddTrip(Train train, DateTime dep, (string Code, int Km, int Min)[] route) => Trips.Add(new Trip // small function that adds a trip
        {
            Id = Trips.Count + 1, // trip id
            Train = train, // the train
            Departure = dep, // departure time
            Stops = route.Select(r => new TripStop { Station = Stations.First(s => s.Code == r.Code), KmFromOrigin = r.Km, MinutesFromStart = r.Min }).ToList() // turn the route into real stops
        });

        for (int d = -14; d <= 6; d++) // for each day from two weeks ago to 6 days ahead
        {
            var day = DateTime.Today.AddDays(d); // the date of that day
            AddTrip(Trains[0], day.AddHours(8), north); // 8 AM trip to Alexandria
            AddTrip(Trains[2], day.AddHours(14.5), north); // 2:30 PM trip to Alexandria
            AddTrip(Trains[1], day.AddHours(20), south); // 8 PM trip to Upper Egypt
        }

        // background bookings belong to a crowd account that cannot log in (empty username and no password hash)
        var crowd = new User { Id = 99, Username = "", FullName = "Other passengers", Role = UserRole.Passenger }; // fake account for the other passengers (cannot log in)
        var rnd = new Random(42); // random number generator with a fixed seed so the result is always the same
        foreach (var trip in Trips) // for each trip
        {
            int target = rnd.Next(30, 120); // number of bookings wanted for this trip
            for (int k = 0, booked = 0; k < target * 2 && booked < target; k++) // keep trying until the count is reached (or attempts run out)
            {
                var cls = (SeatClass)rnd.Next(3); // pick a random class
                int coaches = trip.Train.CoachCount(cls); // number of coaches in this class
                if (coaches == 0) continue; // if this class has no coaches, try again
                int coach = rnd.Next(1, coaches + 1), seat = rnd.Next(1, trip.Train.SeatsPerCoach + 1); // pick a random coach and seat
                int a = rnd.Next(0, trip.Stops.Count - 1), b = rnd.Next(a + 1, trip.Stops.Count); // pick a boarding station and a drop-off station after it
                var from = trip.Stops[a]; // boarding station
                var to = trip.Stops[b]; // drop-off station
                if (IsSeatTaken(trip, cls, coach, seat, from, to)) continue; // if the seat is booked, try again
                Tickets.Add(new Ticket // add the ticket
                {
                    Id = Tickets.Count + 1, Trip = trip, Passenger = rnd.Next(100) == 0 ? Users[rnd.Next(1, Users.Count)] : crowd, // rarely for a real passenger, mostly for the fake account
                    From = from, To = to, Class = cls, Coach = coach, Seat = seat, // seat info
                    Price = Fares.Calc(from, to, cls), BookedAt = trip.Departure.AddDays(-rnd.Next(1, 10)) // price and booking time, some days before the trip
                });
                booked++; // increase the booking counter
            }
        }
        Users.Add(crowd); // added last so the random picks above only hit real passengers; saved so tickets can point at it
    }
}
