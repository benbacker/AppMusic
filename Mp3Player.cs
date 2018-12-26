using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace music
{
    class Mp3Player
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrRuturnString, int uReturnLength, int hwdCal);

        public void Open(string File)
        {
            string Format = @"open ""{0}"" type MPEGVideo alias MediaFile";
            string conmmand = string.Format(Format, File);
            mciSendString(conmmand, null, 0, 0);
        }

        public void Play()
        {
            string conmmand = "play MediaFile";
            mciSendString(conmmand, null, 0, 0);
        }

        public void Stop()
        {
            string conmmand = "stop MediaFile";
            mciSendString(conmmand, null, 0, 0);
        }
    }
}
