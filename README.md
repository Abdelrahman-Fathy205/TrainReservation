# Train Reservation

A Windows Forms desktop app (GUI) for booking train tickets. Passengers search trips, pick seats from a seat map and print their tickets; admins manage stations, trains and trips and view reports.

## Features

- **Login / Register** with two roles: Admin and Passenger
- **Dashboard** with summary numbers and, for passengers, a "your next trip" card with a print button
- **Dark mode**: the button at the bottom of the sidebar (or Ctrl+D); the choice is remembered
- **Keyboard**: Ctrl+1, Ctrl+2... switch pages; Enter searches or books; arrow keys pick a seat; on My Tickets, Enter prints and Delete cancels
- **Search Trips** between stations, with fares by seat class (First, Second, Economy)
- **Seat selection** on a visual seat map per coach
- **My Tickets**: view, print (or save as PDF via *Microsoft Print to PDF*) and cancel upcoming bookings
- **Admin pages**: Stations, Trains, Trips and Reports (a train can't be scheduled on two overlapping trips)

## Requirements

- Windows
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Run

```bash
dotnet run --project TrainReservation.Client
```

Or open `TrainReservation.slnx` in Visual Studio 2022 and press F5.

## Sample accounts

Demo data (`Services/SampleData.cs`) is generated on first run. When the app closes, everything is saved to `%LOCALAPPDATA%\TrainReservation\data.json` and loaded on the next start. Delete that file to reset to fresh demo data (the demo schedule covers 14 days before to 6 days after the first run). Passwords are stored as salted PBKDF2 hashes.

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
├── Services/   Sample data, JSON save/load, password hashing, ticket printing
└── UI/         Theme (colors, fonts, message helpers)
```
