using System.Text.Json; // library to convert data to JSON and back
using TrainReservation.Client.Models; // use the data models

namespace TrainReservation.Client.Services; // namespace for the services

/// <summary>Saves SampleData to a JSON file and loads it back. Object links (trip → train, ticket → stop)
/// are written as ids / stop indexes and rebuilt on load.</summary>
public static class DataStore // class that saves and loads data from a file
{
    public static string FilePath { get; set; } = Path.Combine( // path of the data file
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TrainReservation", "data.json"); // inside the AppData\Local\TrainReservation folder

    record StopDto(int StationId, int Km, int Min); // how a stop is stored in the file (station id instead of the whole station)
    record TripDto(int Id, int TrainId, DateTime Departure, List<StopDto> Stops); // how a trip is stored in the file
    record TicketDto(int Id, int TripId, int PassengerId, int FromStop, int ToStop, SeatClass Class, int Coach, int Seat, decimal Price, DateTime BookedAt); // how a ticket is stored in the file
    record Snapshot(List<User> Users, List<Station> Stations, List<Train> Trains, List<TripDto> Trips, List<TicketDto> Tickets); // a full copy of all the data

    public static void Save() // save the data to the file
    {
        var snap = new Snapshot( // make a copy of the current data
            SampleData.Users, SampleData.Stations, SampleData.Trains, // users, stations and trains as they are
            SampleData.Trips.Select(t => new TripDto(t.Id, t.Train.Id, t.Departure, // convert each trip to the file format
                t.Stops.Select(s => new StopDto(s.Station.Id, s.KmFromOrigin, s.MinutesFromStart)).ToList())).ToList(), // and convert its stops too
            SampleData.Tickets.Select(k => new TicketDto(k.Id, k.Trip.Id, k.Passenger.Id, // convert each ticket to the file format
                k.Trip.Stops.IndexOf(k.From), k.Trip.Stops.IndexOf(k.To), k.Class, k.Coach, k.Seat, k.Price, k.BookedAt)).ToList()); // boarding and drop-off stations are saved as an index

        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)); // create the folder if it does not exist
        // write to a temp file first so a crash mid-write never leaves a half-written data file
        string tmp = FilePath + ".tmp"; // temp file name
        File.WriteAllText(tmp, JsonSerializer.Serialize(snap)); // write the data to the temp file
        File.Move(tmp, FilePath, overwrite: true); // move the temp file over the real one (so the file is not broken by a sudden shutdown)
    }

    /// <summary>Fills SampleData from the file. Returns false when there is no file yet.
    /// A corrupt file is moved aside (data.json.bad) so it is never overwritten, then false is returned.</summary>
    public static bool TryLoad() // load the data from the file
    {
        if (!File.Exists(FilePath)) return false; // if there is no file, return false
        try // try to read the file
        {
            var snap = JsonSerializer.Deserialize<Snapshot>(File.ReadAllText(FilePath)); // read the file and turn it into the data copy
            var users = snap.Users.ToDictionary(u => u.Id); // users by id, for fast lookup
            var stations = snap.Stations.ToDictionary(s => s.Id); // stations by id
            var trains = snap.Trains.ToDictionary(t => t.Id); // trains by id
            var trips = snap.Trips.Select(t => new Trip // rebuild the trips
            {
                Id = t.Id, Train = trains[t.TrainId], Departure = t.Departure, // link the trip to the real train
                Stops = t.Stops.Select(s => new TripStop { Station = stations[s.StationId], KmFromOrigin = s.Km, MinutesFromStart = s.Min }).ToList() // rebuild the stops and link them to stations
            }).ToDictionary(t => t.Id); // and make a lookup by id from them
            var tickets = snap.Tickets.Select(k => // rebuild the tickets
            {
                var trip = trips[k.TripId]; // get the ticket's trip
                return new Ticket // make the ticket
                {
                    Id = k.Id, Trip = trip, Passenger = users[k.PassengerId], // link it to the trip and the passenger
                    From = trip.Stops[k.FromStop], To = trip.Stops[k.ToStop], // get the boarding and drop-off stations from the index
                    Class = k.Class, Coach = k.Coach, Seat = k.Seat, Price = k.Price, BookedAt = k.BookedAt // the rest of the ticket data
                };
            }).ToList(); // turn them into a list

            SampleData.Users.AddRange(users.Values); // add the users to the data
            SampleData.Stations.AddRange(stations.Values); // add the stations
            SampleData.Trains.AddRange(trains.Values); // add the trains
            SampleData.Trips.AddRange(trips.Values); // add the trips
            SampleData.Tickets.AddRange(tickets); // add the tickets
            return true; // loading worked
        }
        catch (Exception ex) when (ex is JsonException or KeyNotFoundException or ArgumentOutOfRangeException or NullReferenceException) // if the file is broken
        {
            File.Move(FilePath, FilePath + ".bad", overwrite: true); // rename the broken file to .bad so it is not lost
            MessageBox.Show($"The saved data could not be read and was moved to:\n{FilePath}.bad\n\nStarting with demo data.", // tell the user
                "Train Reservation", MessageBoxButtons.OK, MessageBoxIcon.Warning); // with a warning message
            return false; // loading failed, start with sample data
        }
    }
}
