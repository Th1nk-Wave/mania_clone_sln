using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ConsoleAPI.ConsoleAPI;
using static Utility.Utils;

namespace Graphics
{
    public struct Color
    {
        byte r, g, b;
        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public static Color FromUint32(UInt32 uint32)
        {
            return new Color((byte)(uint32 >> 8), (byte)(uint32 >> 16), (byte)(uint32 >> 24));
        }
        public UInt32 ToUint32()
        {
            return ((UInt32)this.r << 8) | ((UInt32)this.g << 16) | ((UInt32)this.b << 24);
        }
    }
    public class Window
    {
        private UInt32[] ColorBuffer;
        private UInt32[] CharColorBuffer;
        private char[] CharBuffer;
        private Boolean[] LineUpdates;
        private Boolean[] NeedRender;
        private string[] RenderStrings;

        private UInt16 _Width = 100;
        private UInt16 _Height = 100;

        private IntPtr Hwindow;
        private IntPtr FrontBuffer;
        private IntPtr BackBuffer;

        public UInt16 Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        public UInt16 Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        public Window(UInt16 Width, UInt16 Height, Int16 FontSize)
        {
            _Width = Width;
            _Height = Height;

            ColorBuffer = new UInt32[Width * Height]; Populate(ColorBuffer, 0u);
            CharColorBuffer = new UInt32[Width * Height * 2]; Populate(CharColorBuffer, 0u);
            CharBuffer = new char[Width * Height * 2]; Populate(CharBuffer, ' ');
            LineUpdates = new Boolean[Height]; Populate(LineUpdates, true);
            NeedRender = new Boolean[Height]; Populate(NeedRender, true);
            RenderStrings = new string[Height]; Populate(RenderStrings, "render string not calculated for this line");

            //get std handle
            Hwindow = GetStdHandle(STD_OUTPUT_HANDLE);
            //create screen buffers
            FrontBuffer = CreateConsoleScreenBuffer(
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                CONSOLE_TEXTMODE_BUFFER,
                0
            );
            BackBuffer = CreateConsoleScreenBuffer(
                GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                0,
                CONSOLE_TEXTMODE_BUFFER,
                0
            );

            //set virtual
            SetVirtual(Hwindow);

            //set font to tiny (for per-pixel console writing)
            SetCurrentFont(Hwindow, "Consolas", FontSize);
            SetWindowSize(Hwindow, Width, Height);
            


        }

        public void SetPixel(UInt16 x, UInt16 y, Color col)
        {
            ColorBuffer[x + y*_Width] = col.ToUint32();
            LineUpdates[y] = true;
            NeedRender[y] = true;
        }
        public Color GetPixel(UInt16 x, UInt16 y)
        {
            return Color.FromUint32(ColorBuffer[x + y*_Width]);
        }

        public void Fill(Color col)
        {
            Populate(ColorBuffer,col.ToUint32());
        }

        public void Update()
        {
            
            for (UInt16 ypos = 0; ypos < _Height; ypos++)
            {
                UInt32 oldrgb = 0u;
                UInt32 oldTextrgb = 0u;
                if (!LineUpdates[ypos]) { continue; }

                string lineString = "";
                for (UInt16 xpos = 0; xpos < _Width; xpos++)
                {
                    string charString = "";
                    UInt32 currentTextrgb = 0u;

                    UInt32 textrgb1 = CharColorBuffer[xpos*2 + ypos * _Width*2];
                    char char1 = CharBuffer[xpos*2 + ypos * _Width*2];
                    if ((textrgb1!=0u) && (textrgb1 != oldTextrgb))
                    {
                        charString += $"\x1b[38;2;{(byte)(textrgb1 >> 8)};{(byte)(textrgb1 >> 16)};{(byte)(textrgb1 >> 24)}m";
                        currentTextrgb = textrgb1;
                    } else
                    {
                        currentTextrgb = oldTextrgb;
                    }
                    charString += char1;

                    UInt32 textrgb2 = CharColorBuffer[(xpos * 2) + 1 + ypos * _Width*2];
                    char char2 = CharBuffer[(xpos * 2) + 1 + ypos * _Width*2];
                    if ((textrgb2 != 0u) && (textrgb1 != textrgb2))
                    {
                        charString += $"\x1b[38;2;{(byte)(textrgb2 >> 8)};{(byte)(textrgb2 >> 16)};{(byte)(textrgb2 >> 24)}m";
                        currentTextrgb = textrgb2;
                    }
                    charString += char2;

                    UInt32 rgb = ColorBuffer[xpos + ypos * _Width];
                    if (rgb == oldrgb)
                    {
                        lineString += charString;
                    } else
                    {
                        lineString += $"\x1b[48;2;{(byte)(rgb >> 8)};{(byte)(rgb >> 16)};{(byte)(rgb >> 24)}m{charString}";
                    }
                    oldrgb = rgb;
                    oldTextrgb = currentTextrgb;
                }
                RenderStrings[ypos] = lineString;
                LineUpdates[ypos] = false;


            }
        }

        public void Render()
        {
            for (UInt16 ypos = 0; ypos < _Height; ypos++)
            {
                if (NeedRender[ypos])
                {
                    Console.Write($"\x1b[{ypos + 1};0H{RenderStrings[ypos]}");
                    NeedRender[ypos] = false;
                }
            }
            Console.Write($"\x1b[{_Height + 1};0H");
        }
    }
}
