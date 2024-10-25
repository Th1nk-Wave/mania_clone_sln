using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphics;

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
    }
}
