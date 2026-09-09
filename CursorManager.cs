using System.Runtime.InteropServices;

namespace Sudoku_Solver;



internal class CursorManager
{

    // VARIABLES



    // the position we last moved the cursor to, used to detect manual takeover
    public static Point expectedCursorPosition = Point.Empty;



    // EXTERNAL METHODS



    [DllImport("user32.dll")]
    static extern bool GetCursorPos(ref Point lpPoint);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
    private const int LeftDown = 0x02;
    private const int LeftUp = 0x04;
    private const int RightDown = 0x08;
    private const int RightUp = 0x10;
    private const int MiddleDown = 0x20;
    private const int MiddleUp = 0x40;



    // PUBLIC METHODS



    // true if the user has moved the mouse since our last click, used to pause the bot
    public static bool cursorMoved()
    {
        if (Cursor.Position.Equals(Point.Empty))
        {
            Console.WriteLine("Unkown cursor position");
            return true;
        }

        if (Cursor.Position.X != expectedCursorPosition.X ||
            Cursor.Position.Y != expectedCursorPosition.Y)
            return true;
        return false;
    }



    // moves the cursor to (x, y) and sends a click of the given button
    public static void click(int x, int y, String button)
    {
        int down = 0;
        int up = 0;

        switch (button)
        {
            case "left": down = LeftDown; up = LeftUp; break;
            case "right": down = RightDown; up = RightUp; break;
            case "middle": down = MiddleDown; up = MiddleUp; break;
        }

        expectedCursorPosition = new Point(x, y);
        Cursor.Position = expectedCursorPosition;
        mouse_event((uint)down | (uint)up, (uint)x, (uint)y, 0, 0);
    }



    public static void click(Point point, String button)
    {
        click(point.X, point.Y, button);
    }



    public static void getPosition(ref Point point)
    {
        GetCursorPos(ref point);
    }



    // waits for the user to hover a point, then scans a box around it for the given color
    public static Point findColorNearPoint(Color color, Point point, int width, int height)
    {
        ScreenReader s = new ScreenReader(point.X - width / 2, point.Y - height / 2, width, height);

        s.captureScreenshot();
        Point position = s.findColor(color);

        // offset from the scan box back to actual screen coordinates
        point.X += position.X - width / 2;
        point.Y += position.Y - height / 2;

        return point;
    }
}