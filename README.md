# Train Reservation

A Windows Forms desktop app (GUI) for booking train tickets. Passengers search trips, pick seats from a seat map and print their tickets; admins manage stations, trains and trips and view reports.

## Features

- **Login / Register** with two roles: Admin and Passenger
- **Dashboard** with charts and summary numbers
- **Search Trips** between stations, with fares by seat class (First, Second, Economy)
- **Seat selection** on a visual seat map per coach
- **My Tickets**: view your bookings and print them (or save as PDF via *Microsoft Print to PDF*)
- **Admin pages**: Stations, Trains, Trips and Reports

## Requirements

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Run

```bash
dotnet run --project TrainReservation.Client
```

Or open `TrainReservation.slnx` in Visual Studio 2022 and press F5.

## Sample accounts

The app runs on in-memory sample data (`Services/SampleData.cs`), so changes are lost when it closes.

| Role      | Username      | Password   |
|-----------|---------------|------------|
| Admin     | `admin`       | `admin123` |
| Passenger | `abdelrahman` | `123456`   |
| Passenger | `mona`        | `123456`   |
| Passenger | `omar`        | `123456`   |

## Project structure

```
TrainReservation.Client/
├── Forms/      Login, Register, Main window, Seat selection
├── Pages/      Dashboard, Search, My Tickets, Stations, Trains, Trips, Reports
├── Controls/   Custom chart and seat map controls
├── Models/     User, Station, Train, Trip, Ticket...
├── Services/   Sample data and ticket printing
└── UI/         Theme (colors, fonts, message helpers)
```
