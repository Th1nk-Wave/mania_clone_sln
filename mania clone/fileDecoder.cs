using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics;
using static Utility.Utils;

namespace mania_clone
{
    public static class fileDecoder
    {
        public static List<UInt32[]> GetRawFrames(string FilePath)
        {
            List<UInt32[]> AllFrames = new List<uint[]>();
            StreamReader reader = new StreamReader(FilePath,Encoding.ASCII);
            
            
            string[] frames = reader.ReadToEnd().Split('|');
            int frameIndex = -1;
            foreach (string frame in frames)
            {
                frameIndex++;
                UInt32[] SanitisedFrame = new UInt32[frame.Replace("#","").Length];
                int y = -1;
                foreach (string Line in frame.Split('#'))
                {
                    y++;
                    for (int x = 0;  x < Line.Length; x++)
                    {
                        char num = Line[x];
                        
                        int value = int.Parse(num.ToString()) * 255 / 8;
                        SanitisedFrame[x + y*Line.Length] = new Color((byte)Math.Min(value,255), (byte)Math.Min(value, 255), (byte)Math.Min(value, 255)).ToUint32();
                    }
                }
                
                AllFrames.Add(SanitisedFrame);
            }
            return AllFrames;
        }
        public static List<UInt32[]> GetRawFramesCompressed(string FilePath, UInt16 FrameWidth, UInt16 FrameHeight, UInt64 totalFrames)
        {
            List<UInt32[]> AllFrames = new List<uint[]>();
            FileStream stream = new FileStream(FilePath,FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            UInt32[] frame = new UInt32[FrameWidth * FrameHeight];
            UInt32[] gigachunk = new UInt32[(UInt64)FrameWidth * (UInt64)FrameHeight * totalFrames + 10];

            UInt64 indexInFrame = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte color = reader.ReadByte();
                UInt16 repeated = reader.ReadUInt16();
                

                UInt32[] colorSegment = new UInt32[repeated]; Populate(colorSegment, new Color(color, color, color).ToUint32());
                colorSegment.CopyTo(gigachunk, (int)indexInFrame);
                indexInFrame += repeated;
                Console.WriteLine($"proccessed: {indexInFrame} indexes");
            }
            return AllFrames;
        }
    }
}
