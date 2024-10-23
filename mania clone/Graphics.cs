using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static ConsoleAPI.ConsoleAPI;

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
        private char[] CharBuffer;

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

        public Window(UInt16 Width, UInt16 Height)
        {
            _Width = Width;
            _Height = Height;

            ColorBuffer = new UInt32[Width * Height];
            CharBuffer = new char[Width * Height * 2];

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
            SetCurrentFont(Hwindow, "Consolas", 1);


        }

        public void SetPixel(UInt32 x, UInt32 y, Color col)
        {
            ColorBuffer[x + y*_Width] = col.ToUint32();
        }
        public Color GetPixel(UInt32 x, UInt32 y)
        {
            return Color.FromUint32(ColorBuffer[x + y*_Width]);
        }

        public void Render()
        {
            foreach (UInt32 uintCol in ColorBuffer)
            {
                byte r = (byte)(uintCol >> 8);
                byte g = (byte)(uintCol >> 16);
                byte b = (byte)(uintCol >> 24);
            }
        }
    }
}
