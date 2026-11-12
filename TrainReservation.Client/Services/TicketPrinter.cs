using System.Drawing.Drawing2D; // advanced drawing library (dashed lines and smoothing)
using System.Drawing.Printing; // printing library
using TrainReservation.Client.Models; // data models
using TrainReservation.Client.UI; // colors and theme

namespace TrainReservation.Client.Services; // namespace for the services

/// <summary>Draws a boarding-pass style ticket with GDI+ and shows it in a print preview
/// (print to "Microsoft Print to PDF" to save it as a PDF).</summary>
public static class TicketPrinter // class that draws and prints the ticket
{
    // paper is always white, so the ticket does not follow the app's dark mode
    static readonly Color Ink = Color.FromArgb(17, 24, 39), Grey = Color.FromArgb(107, 114, 128), Line = Color.FromArgb(209, 213, 219); // ink, gray and line colors

    public static void Preview(Ticket ticket, IWin32Window owner) => Preview(new[] { ticket }, owner); // preview one ticket

    /// <summary>One ticket per page.</summary>
    public static void Preview(IReadOnlyList<Ticket> tickets, IWin32Window owner) // preview many tickets (one per page)
    {
        using var doc = new PrintDocument { DocumentName = $"Ticket {string.Join(", ", tickets.Select(t => t.Code))}" }; // the print document, named with the ticket codes
        doc.DefaultPageSettings.Landscape = true; // landscape paper
        int page = 0; // current page number
        doc.BeginPrint += (s, e) => page = 0; // preview and real print each run the pages from the start
        doc.PrintPage += (s, e) => // for each printed page
        {
            Draw(e.Graphics, e.MarginBounds.Location, tickets[page++]); // draw the ticket and go to the next one
            e.HasMorePages = page < tickets.Count; // there are more pages if tickets are left
        };
        using var dlg = new PrintPreviewDialog { Document = doc, Width = 1000, Height = 720, UseAntiAlias = true }; // print preview window
        dlg.PrintPreviewControl.Zoom = 1.0; // zoom 100%
        try // try to open the preview
        {
            dlg.ShowDialog(owner); // show the window
        }
        catch (InvalidPrinterException) // if no printer is installed
        {
            Theme.Warn("No printer is installed. Add 'Microsoft Print to PDF' in Windows settings to preview and save tickets."); // show a warning to the user
        }
    }

    static void Draw(Graphics g, Point at, Ticket t) // draw the ticket on the paper
    {
        g.SmoothingMode = SmoothingMode.AntiAlias; // smooth edges
        var card = new Rectangle(at.X, at.Y, 820, 300); // ticket rectangle
        int stubX = card.Right - 220; // position of the tear-off part on the right

        using (var path = Theme.RoundedRect(card, 16)) // rectangle shape with rounded corners
        using (var border = new Pen(Theme.Primary, 2)) // border pen
        {
            g.FillPath(Brushes.White, path); // white background
            g.SetClip(path); // draw only inside the shape
            using var band = new SolidBrush(Theme.Primary); // top stripe brush
            g.FillRectangle(band, card.X, card.Y, card.Width, 60); // draw the top stripe
            g.ResetClip(); // remove the drawing limit
            g.DrawPath(border, path); // draw the border
        }

        using var white = new SolidBrush(Color.White); // white brush
        using var text = new SolidBrush(Ink); // main text brush
        using var muted = new SolidBrush(Grey); // faded text brush
        using var accent = new SolidBrush(Theme.Accent); // accent color brush
        using var h1 = new Font("Segoe UI", 18F, FontStyle.Bold); // title font
        using var big = new Font("Segoe UI", 22F, FontStyle.Bold); // big font for station codes
        using var label = new Font("Segoe UI", 8.5F); // small font for labels
        using var value = new Font("Segoe UI", 11.5F, FontStyle.Bold); // font for values

        g.DrawString("TRAIN TICKET", h1, white, card.X + 24, card.Y + 14); // write "train ticket"
        g.DrawString(t.Code, h1, accent, stubX + 24, card.Y + 14); // write the ticket code

        // route
        g.DrawString(t.From.Station.Code, big, text, card.X + 24, card.Y + 80); // boarding station code
        g.DrawString(t.To.Station.Code, big, text, card.X + 330, card.Y + 80); // drop-off station code
        using (var dash = new Pen(Line, 2) { DashStyle = DashStyle.Dash }) // dashed line pen
            g.DrawLine(dash, card.X + 120, card.Y + 102, card.X + 310, card.Y + 102); // line between the two stations
        g.DrawString(t.From.Station.Name, label, muted, card.X + 26, card.Y + 122); // boarding station name
        g.DrawString(t.To.Station.Name, label, muted, card.X + 332, card.Y + 122); // drop-off station name

        void Field(string name, string val, int x, int y) // small function that draws a label with a value under it
        {
            g.DrawString(name.ToUpper(), label, muted, x, y); // the label in capital letters
            g.DrawString(val, value, text, x, y + 16); // the value under it
        }

        int fy = card.Y + 160; // position of the data row
        Field("Passenger", t.Passenger.FullName, card.X + 24, fy); // passenger name
        Field("Train", t.Trip.Train.ToString(), card.X + 250, fy); // the train
        Field("Date", t.Departs.ToString("ddd dd MMM yyyy"), card.X + 24, fy + 56); // the date
        Field("Departs", t.Departs.ToString("HH:mm"), card.X + 250, fy + 56); // departure time
        Field("Arrives", t.Arrives.ToString("HH:mm"), card.X + 370, fy + 56); // arrival time

        // tear-off stub
        using (var tear = new Pen(Line, 2) { DashStyle = DashStyle.Dot }) // dotted line pen
            g.DrawLine(tear, stubX, card.Y + 64, stubX, card.Bottom - 4); // the cut line
        Field("Class", t.Class.ToString(), stubX + 24, card.Y + 80); // the class
        Field("Coach / Seat", $"{t.Coach} / {t.Seat}", stubX + 24, card.Y + 130); // coach and seat
        Field("Fare", $"{t.Price:N2} EGP", stubX + 24, card.Y + 180); // price in pounds

        // decorative barcode derived from the ticket code
        var rnd = new Random(t.Id); // fixed random numbers for each ticket
        for (int x = stubX + 24; x < card.Right - 24; x += 3) // loop across every 3 pixels
            if (rnd.Next(3) > 0) g.FillRectangle(Brushes.Black, x, card.Bottom - 56, rnd.Next(1, 3), 40); // draw a black line (looks like a barcode)
    }
}
