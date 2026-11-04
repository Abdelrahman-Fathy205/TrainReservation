namespace TrainReservation.Client.Models;

public enum SeatClass { First, Second, Economy }
public enum UserRole { Admin, Passenger }

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    // shortcut: plain-text password in sample data only; store a hash once the server + DB exist
    public string Password { get; set; } = "";
    public UserRole Role { get; set; }
}

public class Station
{
    public int Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string City { get; set; } = "";
    public override string ToString() => Name;
}

public class Train
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public string Name { get; set; } = "";
    public int FirstCoaches { get; set; }
    public int SecondCoaches { get; set; }
    public int EconomyCoaches { get; set; }
    public int SeatsPerCoach { get; set; }

    public int Capacity => (FirstCoaches + SecondCoaches + EconomyCoaches) * SeatsPerCoach;

    public int CoachCount(SeatClass c) => c switch
    {
        SeatClass.First => FirstCoaches,
        SeatClass.Second => SecondCoaches,
        _ => EconomyCoaches
    };

    public override string ToString() => $"{Number} - {Name}";
}

/// <summary>A station on a trip, with distance and time measured from the first station.</summary>
public class TripStop
{
    public Station Station { get; set; }
    public int KmFromOrigin { get; set; }
    public int MinutesFromStart { get; set; }
}

public class Trip
{
    public int Id { get; set; }
    public Train Train { get; set; }
    public DateTime Departure { get; set; }
    public List<TripStop> Stops { get; set; } = new();

    public DateTime Arrival => Departure.AddMinutes(Stops[^1].MinutesFromStart);
    public string Route => $"{Stops[0].Station.Name} → {Stops[^1].Station.Name}";
    public TripStop StopAt(Station s) => Stops.FirstOrDefault(x => x.Station == s);
    public DateTime TimeAt(TripStop s) => Departure.AddMinutes(s.MinutesFromStart);
}

public class Ticket
{
    public int Id { get; set; }
    public Trip Trip { get; set; }
    public User Passenger { get; set; }
    public TripStop From { get; set; }
    public TripStop To { get; set; }
    public SeatClass Class { get; set; }
    public int Coach { get; set; }
    public int Seat { get; set; }
    public decimal Price { get; set; }
    public DateTime BookedAt { get; set; }

    public string Code => $"TK{Id:D5}";
    public DateTime Departs => Trip.TimeAt(From);
    public DateTime Arrives => Trip.TimeAt(To);
}

public static class Fares
{
    public static decimal RatePerKm(SeatClass c) => c switch
    {
        SeatClass.First => 1.50m,
        SeatClass.Second => 1.00m,
        _ => 0.60m
    };

    public static decimal Calc(TripStop from, TripStop to, SeatClass c) =>
        Math.Round((to.KmFromOrigin - from.KmFromOrigin) * RatePerKm(c), 2);
}
