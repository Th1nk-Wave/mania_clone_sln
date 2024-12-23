﻿using Microsoft.Win32.SafeHandles;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using static ConsoleAPI.ConsoleAPI;
using Graphics;
using static mania_clone.fileDecoder;



// for later reference https://www.codeproject.com/script/Content/ViewAssociatedFile.aspx?rzp=%2FKB%2Fcs%2Fcommandbar%2Fcommandpromptexplorerbar.zip&zep=ZCommon%2FWin32.cs&obid=2366&obtid=2&ovid=1
// https://github.com/spectreconsole/spectre.console

namespace mania_clone
{
    internal class Program
    {
        
        [STAThread]
        private static int Main(string[] args)
        {
            int totalFrames = 6570;
            ushort res = 30;
            Window w = new Window((ushort)(8 * res), (ushort)(6*res),2);

            //w.Box((ushort)(8 * res-6), (ushort)(6 * res-6), (ushort)(8 * res), (ushort)(6 * res), new Color(255, 0, 0));

            Border greenBD = new Border(new Color(0, 255, 0));
            Background WhiteBG = new Background(new Color(255,255,255));
            Frame box1 = new Frame(new UIdim(0, 0f, 0, 0f), new UIdim(5,0f,5,0f), new UIdim(0,0f,0,0f),101);
            box1.Append(WhiteBG);

            Frame box2 = new Frame(new UIdim(0, 1f, 0, 1f), new UIdim(5, 0f, 5, 0f), new UIdim(0, 1f, 0, 1f), 102);
            box2.Append(WhiteBG);

            Background redBG = new Background(new Color(255,0,0));
            
            Frame box = new Frame(new UIdim(0, 0.5f, 0, 0.5f), new UIdim(5, 0f, 5, 0f), new UIdim(0, 0.5f, 0, 0.5f), 100); 
            box.Append(redBG);
            box.Append(greenBD);

            GUI hud = new GUI(w.Width, w.Height);
            hud.Append(box);
            hud.Append(box1);
            hud.Append(box2);

            w.ProcessGUI(hud);
            w.Update_optimise2();
            w.Render_optimise();



            return 0;




            List<UInt32[]> frames = UnpackFrames("D:/stuff thats in better drive/programming/c# programming/mania_clone_sln/mania clone/assets/frameDataCompressed.txt", (ushort)(8 * res), (ushort)(6 * res));
            List<string> baked = w.BakeFramesFixed(frames, (ushort)(8 * res), (ushort)(6 * res),0,0);
            DateTime startTime, endTime;
            startTime = DateTime.UtcNow;

            
            int duration = (totalFrames * 1 / 30)*1000;

            int lastFrame = 0;
            while (true)
            {
                
                endTime = DateTime.UtcNow;
                Double elapsedMillisecs = ((TimeSpan)(endTime - startTime)).TotalMilliseconds;

                if (elapsedMillisecs >= duration)
                {
                    startTime = endTime;
                    elapsedMillisecs = 0;
                }

                int newFrame = (int)((elapsedMillisecs / duration) * totalFrames);
                if (newFrame != lastFrame)
                {
                    //w.FillWith(frames[newFrame]);
                    //w.Update_optimise2();
                    //w.Render_optimise();
                    w.RenderFixedBakedFrame(baked[newFrame]);
                    lastFrame = newFrame;
                }
                
                

            }


            

            





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