using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using TrainReservation.Client.Models;
using TrainReservation.Client.UI;

namespace TrainReservation.Client.Services;

/// <summary>Draws a boarding-pass style ticket with GDI+ and shows it in a print preview
/// (print to "Microsoft Print to PDF" to save it as a PDF).</summary>
public static class TicketPrinter
{
    public static void Preview(Ticket ticket, IWin32Window owner)
    {
        using var doc = new PrintDocument { DocumentName = $"Ticket {ticket.Code}" };
        doc.DefaultPageSettings.Landscape = true;
        doc.PrintPage += (s, e) => Draw(e.Graphics, e.MarginBounds.Location, ticket);
        using var dlg = new PrintPreviewDialog { Document = doc, Width = 1000, Height = 720, UseAntiAlias = true };
        dlg.PrintPreviewControl.Zoom = 1.0;
        try
        {
            dlg.ShowDialog(owner);
        }
        catch (InvalidPrinterException)
        {
            Theme.Warn("No printer is installed. Add 'Microsoft Print to PDF' in Windows settings to preview and save tickets.");
        }
    }

    static void Draw(Graphics g, Point at, Ticket t)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var card = new Rectangle(at.X, at.Y, 820, 300);
        int stubX = card.Right - 220;

        using (var path = Theme.RoundedRect(card, 16))
        using (var border = new Pen(Theme.Primary, 2))
        {
            g.FillPath(Brushes.White, path);
            g.SetClip(path);
            using var band = new SolidBrush(Theme.Primary);
            g.FillRectangle(band, card.X, card.Y, card.Width, 60);
            g.ResetClip();
            g.DrawPath(border, path);
        }

        using var white = new SolidBrush(Color.White);
        using var text = new SolidBrush(Theme.Text);
        using var muted = new SolidBrush(Theme.Muted);
        using var accent = new SolidBrush(Theme.Accent);
        using var h1 = new Font("Segoe UI", 18F, FontStyle.Bold);
        using var big = new Font("Segoe UI", 22F, FontStyle.Bold);
        using var label = new Font("Segoe UI", 8.5F);
        using var value = new Font("Segoe UI", 11.5F, FontStyle.Bold);

        g.DrawString("TRAIN TICKET", h1, white, card.X + 24, card.Y + 14);
        g.DrawString(t.Code, h1, accent, stubX + 24, card.Y + 14);

        // route
        g.DrawString(t.From.Station.Code, big, text, card.X + 24, card.Y + 80);
        g.DrawString(t.To.Station.Code, big, text, card.X + 330, card.Y + 80);
        using (var dash = new Pen(Theme.Border, 2) { DashStyle = DashStyle.Dash })
            g.DrawLine(dash, card.X + 120, card.Y + 102, card.X + 310, card.Y + 102);
        g.DrawString(t.From.Station.Name, label, muted, card.X + 26, card.Y + 122);
        g.DrawString(t.To.Station.Name, label, muted, card.X + 332, card.Y + 122);

        void Field(string name, string val, int x, int y)
        {
            g.DrawString(name.ToUpper(), label, muted, x, y);
            g.DrawString(val, value, text, x, y + 16);
        }

        int fy = card.Y + 160;
        Field("Passenger", t.Passenger.FullName, card.X + 24, fy);
        Field("Train", t.Trip.Train.ToString(), card.X + 250, fy);
        Field("Date", t.Departs.ToString("ddd dd MMM yyyy"), card.X + 24, fy + 56);
        Field("Departs", t.Departs.ToString("HH:mm"), card.X + 250, fy + 56);
        Field("Arrives", t.Arrives.ToString("HH:mm"), card.X + 370, fy + 56);

        // tear-off stub
        using (var tear = new Pen(Theme.Border, 2) { DashStyle = DashStyle.Dot })
            g.DrawLine(tear, stubX, card.Y + 64, stubX, card.Bottom - 4);
        Field("Class", t.Class.ToString(), stubX + 24, card.Y + 80);
        Field("Coach / Seat", $"{t.Coach} / {t.Seat}", stubX + 24, card.Y + 130);
        Field("Fare", $"{t.Price:N2} EGP", stubX + 24, card.Y + 180);

        // decorative barcode derived from the ticket code
        var rnd = new Random(t.Id);
        for (int x = stubX + 24; x < card.Right - 24; x += 3)
            if (rnd.Next(3) > 0) g.FillRectangle(Brushes.Black, x, card.Bottom - 56, rnd.Next(1, 3), 40);
    }
}
