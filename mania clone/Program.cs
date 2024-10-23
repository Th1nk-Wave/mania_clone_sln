using Microsoft.Win32.SafeHandles;
using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using static ConsoleAPI.ConsoleAPI;
using Graphics;



// for later reference https://www.codeproject.com/script/Content/ViewAssociatedFile.aspx?rzp=%2FKB%2Fcs%2Fcommandbar%2Fcommandpromptexplorerbar.zip&zep=ZCommon%2FWin32.cs&obid=2366&obtid=2&ovid=1
// https://github.com/spectreconsole/spectre.console

namespace mania_clone
{
    internal class Program
    {
        
        [STAThread]
        private static int Main(string[] args)
        {

            Window w = new Window(100,100);





            return 0;
            IntPtr buffer1 = CreateConsoleScreenBuffer(
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                CONSOLE_TEXTMODE_BUFFER,
                0
            );
            IntPtr buffer2 = CreateConsoleScreenBuffer(
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                CONSOLE_TEXTMODE_BUFFER,
                0
            );

            IntPtr hStdout1;

            hStdout1 = GetStdHandle(STD_OUTPUT_HANDLE);
            SetVirtual(hStdout1);
            SetCurrentFont(hStdout1, "Consolas", 1);

            int sizeX = Console.WindowWidth;
            int sizeY = Console.WindowHeight;

            int r = 255;
            int g = 100;
            int b = 100;

            string temp = "";
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeX; y++)
                {
                    temp += $"\x1b[48;2;{r};{g};{b}m ";
                }
            }

            Console.Write(temp);




            return 0;
            SMALL_RECT SReadRect = new SMALL_RECT();
            SMALL_RECT SWriteRect = new SMALL_RECT();
            CHAR_INFO[] buffer = new CHAR_INFO[160];
            COORD bufferSize = new COORD();
            COORD bufferCoord = new COORD();
            bool fsuccess;



            IntPtr hStdout = GetStdHandle(STD_OUTPUT_HANDLE);
            IntPtr hNewScreenBuffer = CreateConsoleScreenBuffer(
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                CONSOLE_TEXTMODE_BUFFER,
                0
            );
            if (hStdout == INVALID_HANDLE_VALUE || hNewScreenBuffer == INVALID_HANDLE_VALUE)
            {
                Console.WriteLine("CreateConsoleScreenBuffer failed");
                return 1;
            }

            if (! SetConsoleActiveScreenBuffer(hNewScreenBuffer))
            {
                Console.WriteLine("SetConsoleActiveScreenBuffer failed");
                return 1;
            }

            SReadRect.Top = 0;
            SWriteRect.Left = 0;
            SReadRect.Bottom = 1;
            SWriteRect.Right = 79;

            bufferSize.Y = 2;
            bufferSize.X = 80;

            bufferCoord.X = 0;
            bufferCoord.Y = 0;

            fsuccess = ReadConsoleOutput(
                hStdout,
                buffer,
                bufferSize,
                bufferCoord,
                ref SWriteRect
            );
            if (!fsuccess)
            {
                Console.WriteLine("ReadConsoleOutPut Failed");
            }

            SWriteRect.Top = 10;
            SWriteRect.Left = 0;
            SWriteRect.Bottom = 11;
            SWriteRect.Right = 79;

            fsuccess = WriteConsoleOutput(
                hStdout,//hNewScreenBuffer,
                buffer,
                bufferSize,
                bufferCoord,
                ref SWriteRect
            );
            if (!fsuccess)
            {
                Console.WriteLine("WriteConsoleOutPut Failed");
                return 1;
            }
            if (! SetConsoleActiveScreenBuffer(hStdout)) {
                Console.WriteLine("failed to set screen buffer");
            }

            Console.WriteLine("Hello, World!");
            return ( 0 );
            
        }
    }
}