using Microsoft.Data.SqlClient;
using TrainReservation.Client.Models;

namespace TrainReservation.Client.Services;

/// <summary>
/// SQL Server (LocalDB) persistence. The app works on the in-memory lists in <see cref="SampleData"/>;
/// this class loads them at startup and writes each add / update / delete back with parameterized commands.
/// </summary>
public static class Db
{
    const string Server = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;TrustServerCertificate=true";
    const string ConnStr = Server + ";Database=TrainReservation";

    /// <summary>False when the database could not be reached; writes are then skipped (demo mode).</summary>
    public static bool Online { get; private set; }

    const string Schema = """
        IF OBJECT_ID('Users') IS NULL CREATE TABLE Users(Id INT PRIMARY KEY, Username NVARCHAR(50) NOT NULL, FullName NVARCHAR(100) NOT NULL,
            Email NVARCHAR(100) NOT NULL, Phone NVARCHAR(20) NOT NULL, Password NVARCHAR(100) NOT NULL, Role INT NOT NULL);
        IF OBJECT_ID('Stations') IS NULL CREATE TABLE Stations(Id INT PRIMARY KEY, Code NCHAR(3) NOT NULL, Name NVARCHAR(100) NOT NULL, City NVARCHAR(100) NOT NULL);
        IF OBJECT_ID('Trains') IS NULL CREATE TABLE Trains(Id INT PRIMARY KEY, Number NVARCHAR(10) NOT NULL, Name NVARCHAR(100) NOT NULL,
            FirstCoaches INT NOT NULL, SecondCoaches INT NOT NULL, EconomyCoaches INT NOT NULL, SeatsPerCoach INT NOT NULL);
        IF OBJECT_ID('Trips') IS NULL CREATE TABLE Trips(Id INT PRIMARY KEY, TrainId INT NOT NULL REFERENCES Trains(Id), Departure DATETIME2 NOT NULL);
        IF OBJECT_ID('TripStops') IS NULL CREATE TABLE TripStops(TripId INT NOT NULL REFERENCES Trips(Id) ON DELETE CASCADE, Seq INT NOT NULL,
            StationId INT NOT NULL REFERENCES Stations(Id), Km INT NOT NULL, Minutes INT NOT NULL, PRIMARY KEY(TripId, Seq));
        IF OBJECT_ID('Tickets') IS NULL CREATE TABLE Tickets(Id INT PRIMARY KEY, TripId INT NOT NULL REFERENCES Trips(Id), PassengerId INT NOT NULL REFERENCES Users(Id),
            FromSeq INT NOT NULL, ToSeq INT NOT NULL, Class INT NOT NULL, Coach INT NOT NULL, Seat INT NOT NULL, Price DECIMAL(10,2) NOT NULL, BookedAt DATETIME2 NOT NULL);
        """;

    /// <summary>Creates the database on first run (seeded with demo data), otherwise loads it. Throws if SQL Server is unreachable.</summary>
    public static void Init()
    {
        using (var master = new SqlConnection(Server))
        {
            master.Open();
            Exec(master, null, "IF DB_ID('TrainReservation') IS NULL CREATE DATABASE TrainReservation");
        }
        using var cn = new SqlConnection(ConnStr);
        cn.Open();
        Exec(cn, null, Schema);
        if ((int)new SqlCommand("SELECT COUNT(*) FROM Users", cn).ExecuteScalar() == 0)
        {
            SampleData.Seed();
            using var tx = cn.BeginTransaction(); // all or nothing, so a failed seed never leaves half the data
            SampleData.Users.ForEach(u => Save(cn, tx, u));
            SampleData.Stations.ForEach(s => Save(cn, tx, s));
            SampleData.Trains.ForEach(t => Save(cn, tx, t));
            SampleData.Trips.ForEach(t => Save(cn, tx, t));
            SampleData.Tickets.ForEach(t => Save(cn, tx, t));
            tx.Commit();
        }
        else Load(cn);
        Online = true;
    }

    static void Load(SqlConnection cn)
    {
        var d = SampleData.Users; d.Clear();
        Read(cn, "SELECT Id, Username, FullName, Email, Phone, Password, Role FROM Users", r => d.Add(new User
        {
            Id = r.GetInt32(0), Username = r.GetString(1), FullName = r.GetString(2), Email = r.GetString(3),
            Phone = r.GetString(4), Password = r.GetString(5), Role = (UserRole)r.GetInt32(6)
        }));
        SampleData.Stations.Clear();
        Read(cn, "SELECT Id, Code, Name, City FROM Stations", r => SampleData.Stations.Add(new Station
        {
            Id = r.GetInt32(0), Code = r.GetString(1), Name = r.GetString(2), City = r.GetString(3)
        }));
        SampleData.Trains.Clear();
        Read(cn, "SELECT Id, Number, Name, FirstCoaches, SecondCoaches, EconomyCoaches, SeatsPerCoach FROM Trains", r => SampleData.Trains.Add(new Train
        {
            Id = r.GetInt32(0), Number = r.GetString(1), Name = r.GetString(2), FirstCoaches = r.GetInt32(3),
            SecondCoaches = r.GetInt32(4), EconomyCoaches = r.GetInt32(5), SeatsPerCoach = r.GetInt32(6)
        }));
        var trains = SampleData.Trains.ToDictionary(t => t.Id);
        var stations = SampleData.Stations.ToDictionary(s => s.Id);
        var users = SampleData.Users.ToDictionary(u => u.Id);

        SampleData.Trips.Clear();
        Read(cn, "SELECT Id, TrainId, Departure FROM Trips ORDER BY Id", r => SampleData.Trips.Add(new Trip
        {
            Id = r.GetInt32(0), Train = trains[r.GetInt32(1)], Departure = r.GetDateTime(2)
        }));
        var trips = SampleData.Trips.ToDictionary(t => t.Id);
        Read(cn, "SELECT TripId, StationId, Km, Minutes FROM TripStops ORDER BY TripId, Seq", r => trips[r.GetInt32(0)].Stops.Add(new TripStop
        {
            Station = stations[r.GetInt32(1)], KmFromOrigin = r.GetInt32(2), MinutesFromStart = r.GetInt32(3)
        }));

        SampleData.Tickets.Clear();
        Read(cn, "SELECT Id, TripId, PassengerId, FromSeq, ToSeq, Class, Coach, Seat, Price, BookedAt FROM Tickets ORDER BY Id", r =>
        {
            var trip = trips[r.GetInt32(1)];
            SampleData.Tickets.Add(new Ticket
            {
                Id = r.GetInt32(0), Trip = trip, Passenger = users[r.GetInt32(2)],
                From = trip.Stops[r.GetInt32(3)], To = trip.Stops[r.GetInt32(4)], Class = (SeatClass)r.GetInt32(5),
                Coach = r.GetInt32(6), Seat = r.GetInt32(7), Price = r.GetDecimal(8), BookedAt = r.GetDateTime(9)
            });
        });
    }

    // ---- public write API: one call per add / update / delete ----

    public static void Save(User u) => Run((cn, tx) => Save(cn, tx, u));
    public static void Save(Station s) => Run((cn, tx) => Save(cn, tx, s));
    public static void Save(Train t) => Run((cn, tx) => Save(cn, tx, t));
    public static void Save(Trip t) => Run((cn, tx) => Save(cn, tx, t));
    public static void Save(Ticket t) => Run((cn, tx) => Save(cn, tx, t));

    /// <summary>Deletes one row by id; table is always a constant from our own code, never user input.</summary>
    public static void Delete(string table, int id) =>
        Run((cn, tx) => Exec(cn, tx, $"DELETE FROM {table} WHERE Id=@Id", ("Id", id)));

    static void Save(SqlConnection cn, SqlTransaction tx, User u) => Upsert(cn, tx, "Users", u.Id,
        ("Username", u.Username), ("FullName", u.FullName), ("Email", u.Email), ("Phone", u.Phone), ("Password", u.Password), ("Role", (int)u.Role));

    static void Save(SqlConnection cn, SqlTransaction tx, Station s) => Upsert(cn, tx, "Stations", s.Id,
        ("Code", s.Code), ("Name", s.Name), ("City", s.City));

    static void Save(SqlConnection cn, SqlTransaction tx, Train t) => Upsert(cn, tx, "Trains", t.Id,
        ("Number", t.Number), ("Name", t.Name), ("FirstCoaches", t.FirstCoaches), ("SecondCoaches", t.SecondCoaches),
        ("EconomyCoaches", t.EconomyCoaches), ("SeatsPerCoach", t.SeatsPerCoach));

    static void Save(SqlConnection cn, SqlTransaction tx, Trip t)
    {
        Upsert(cn, tx, "Trips", t.Id, ("TrainId", t.Train.Id), ("Departure", t.Departure));
        Exec(cn, tx, "DELETE FROM TripStops WHERE TripId=@Id", ("Id", t.Id));
        for (int i = 0; i < t.Stops.Count; i++)
            Exec(cn, tx, "INSERT INTO TripStops(TripId, Seq, StationId, Km, Minutes) VALUES(@T, @S, @St, @K, @M)",
                ("T", t.Id), ("S", i), ("St", t.Stops[i].Station.Id), ("K", t.Stops[i].KmFromOrigin), ("M", t.Stops[i].MinutesFromStart));
    }

    static void Save(SqlConnection cn, SqlTransaction tx, Ticket t) => Upsert(cn, tx, "Tickets", t.Id,
        ("TripId", t.Trip.Id), ("PassengerId", t.Passenger.Id), ("FromSeq", t.Trip.Stops.IndexOf(t.From)), ("ToSeq", t.Trip.Stops.IndexOf(t.To)),
        ("Class", (int)t.Class), ("Coach", t.Coach), ("Seat", t.Seat), ("Price", t.Price), ("BookedAt", t.BookedAt));

    // ---- helpers ----

    /// <summary>Runs one write in its own transaction. Skipped in demo mode; SQL errors bubble up to the caller.</summary>
    static void Run(Action<SqlConnection, SqlTransaction> work)
    {
        if (!Online) return;
        using var cn = new SqlConnection(ConnStr);
        cn.Open();
        using var tx = cn.BeginTransaction();
        work(cn, tx);
        tx.Commit();
    }

    static void Upsert(SqlConnection cn, SqlTransaction tx, string table, int id, params (string Col, object Val)[] cols)
    {
        string set = string.Join(", ", cols.Select(c => $"{c.Col}=@{c.Col}"));
        string names = string.Join(", ", cols.Select(c => c.Col));
        string vals = string.Join(", ", cols.Select(c => "@" + c.Col));
        Exec(cn, tx, $"UPDATE {table} SET {set} WHERE Id=@Id; IF @@ROWCOUNT = 0 INSERT INTO {table}(Id, {names}) VALUES(@Id, {vals})",
            cols.Append(("Id", id)).ToArray());
    }

    static void Exec(SqlConnection cn, SqlTransaction tx, string sql, params (string Name, object Val)[] ps)
    {
        using var cmd = new SqlCommand(sql, cn, tx);
        foreach (var (name, val) in ps) cmd.Parameters.AddWithValue("@" + name, val);
        cmd.ExecuteNonQuery();
    }

    static void Read(SqlConnection cn, string sql, Action<SqlDataReader> row)
    {
        using var cmd = new SqlCommand(sql, cn);
        using var r = cmd.ExecuteReader();
        while (r.Read()) row(r);
    }
}
