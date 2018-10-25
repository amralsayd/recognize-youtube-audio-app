using NAudio.Wave;
using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeTool.Business
{
    public class AudioLogic
    {
        public void TrimWavFile(string inPath,string inExt, string outPath,string outExt, int cutFromStart, int cutFromEnd)
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.ConvertMedia(inPath, inExt, outPath, outExt, new ConvertSettings()
            {
                Seek = cutFromStart,
                MaxDuration = cutFromEnd,
            });
        }
    }
}
