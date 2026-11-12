namespace TrainReservation.Client.Models; // namespace for the data models

public enum SeatClass { First, Second, Economy } // seat classes: first, second, economy
public enum UserRole { Admin, Passenger } // user type: admin or passenger

public class User // class that represents a user
{
    public int Id { get; set; } // unique user id
    public string Username { get; set; } = ""; // login name
    public string FullName { get; set; } = ""; // full name
    public string Email { get; set; } = ""; // email address
    public string Phone { get; set; } = ""; // phone number
    /// <summary>PBKDF2 hash from Services.Passwords; never the password itself.</summary>
    public string PasswordHash { get; set; } = ""; // the hashed password (not the real password)
    public UserRole Role { get; set; } // user role (admin or passenger)
}

public class Station // class that represents a station
{
    public int Id { get; set; } // station id
    public string Code { get; set; } = ""; // short station code
    public string Name { get; set; } = ""; // station name
    public string City { get; set; } = ""; // the city of the station
    public override string ToString() => Name; // show the station name when it is shown as text
}

public class Train // class that represents a train
{
    public int Id { get; set; } // train id in the system
    public string Number { get; set; } = ""; // official train number
    public string Name { get; set; } = ""; // train name
    public int FirstCoaches { get; set; } // number of first class coaches
    public int SecondCoaches { get; set; } // number of second class coaches
    public int EconomyCoaches { get; set; } // number of economy coaches
    public int SeatsPerCoach { get; set; } // number of seats in each coach

    public int Capacity => (FirstCoaches + SecondCoaches + EconomyCoaches) * SeatsPerCoach; // total capacity = coaches × seats

    public int CoachCount(SeatClass c) => c switch // returns the number of coaches for a class
    {
        SeatClass.First => FirstCoaches, // if first, return first class coaches
        SeatClass.Second => SecondCoaches, // if second, return second class coaches
        _ => EconomyCoaches // otherwise return economy coaches
    };

    public override string ToString() => $"{Number} - {Name}"; // show the train as "number - name"
}

/// <summary>A station on a trip, with distance and time measured from the first station.</summary>
public class TripStop // a stop on a trip
{
    public Station Station { get; set; } // the station itself
    public int KmFromOrigin { get; set; } // distance in km from the first station
    public int MinutesFromStart { get; set; } // minutes from the start of the trip
}

public class Trip // class that represents a trip
{
    public int Id { get; set; } // trip id
    public Train Train { get; set; } // the train used on the trip
    public DateTime Departure { get; set; } // departure time
    public List<TripStop> Stops { get; set; } = new(); // list of stations where it stops

    public DateTime Arrival => Departure.AddMinutes(Stops[^1].MinutesFromStart); // arrival time = departure + minutes of the last stop
    public string Route => $"{Stops[0].Station.Name} → {Stops[^1].Station.Name}"; // route: from the first station to the last station
    public TripStop StopAt(Station s) => Stops.FirstOrDefault(x => x.Station == s); // finds the trip stop at a given station
    public DateTime TimeAt(TripStop s) => Departure.AddMinutes(s.MinutesFromStart); // the time the train reaches this stop
}

public class Ticket // class that represents a ticket
{
    public int Id { get; set; } // ticket id
    public Trip Trip { get; set; } // the trip of the ticket
    public User Passenger { get; set; } // the passenger who owns the ticket
    public TripStop From { get; set; } // boarding station
    public TripStop To { get; set; } // drop-off station
    public SeatClass Class { get; set; } // seat class
    public int Coach { get; set; } // coach number
    public int Seat { get; set; } // seat number
    public decimal Price { get; set; } // ticket price
    public DateTime BookedAt { get; set; } // booking time

    public string Code => $"TK{Id:D5}"; // ticket code, for example TK00012
    public DateTime Departs => Trip.TimeAt(From); // departure time from the boarding station
    public DateTime Arrives => Trip.TimeAt(To); // arrival time at the drop-off station
}

public static class Fares // class that calculates prices
{
    public static decimal RatePerKm(SeatClass c) => c switch // price per km for each class
    {
        SeatClass.First => 1.50m, // first: 1.5 per km
        SeatClass.Second => 1.00m, // second: 1 per km
        _ => 0.60m // economy: 0.6 per km
    };

    public static decimal Calc(TripStop from, TripStop to, SeatClass c) => // calculate the ticket price
        Math.Round((to.KmFromOrigin - from.KmFromOrigin) * RatePerKm(c), 2); // distance × price per km, rounded to 2 decimals
}
