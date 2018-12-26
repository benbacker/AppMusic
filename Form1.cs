using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace music
{
    public partial class Form1 : Form
    {
        #region Valune
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        private Mp3Player mp3player = new Mp3Player();
        #endregion
        public Form1()
        {
            InitializeComponent();
        }
        String[] fileNames; //Tên File
        String[] filePaths; //Đường dẫn File
        #region play
        int i = 1;
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (i % 2 != 0)
            {
                Player.Ctlcontrols.pause();
                //Player.URL = filePaths[listBox1.SelectedIndex];
                //mp3player.Play();
                pictureBox1.Enabled = false;
                btnPlay.Image = Properties.Resources.icons8_circled_play_filled_96;
                i++;
            }
            else
            {
                Player.Ctlcontrols.play();
                btnPlay.Image = Properties.Resources.play;
                pictureBox1.Enabled = true;
                i++;
            }
        }
        #endregion
        #region Selectedindex
        private void ListMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp3player.Open(filePaths[ListMusic.SelectedIndex]);
        }
        #endregion
        #region Volume
        public int GetApplicationVolume()
        {
            uint vol = (uint)0;
            waveOutGetVolume(IntPtr.Zero,out vol);
            return ((int)vol & 0x0000ffff) / (ushort.MaxValue / 100);
        }

        private void bunifuSlider2_ValueChanged(object sender, EventArgs e)
        {
            Player.settings.volume = bunifuSlider2.Value;
            //int NewVolume = ((ushort.MaxValue / 10) * bunifuSlider2.Value);
            //uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            //waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }
        #endregion
        #region ListMusic
        private void ListMusic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Player.URL= filePaths[ListMusic.SelectedIndex];
            btnPlay.Image = Properties.Resources.play;
            pictureBox1.Enabled = true;
            TimeMusic.Start();
        }
        #endregion
        #region Load
        private void Form1_Load(object sender, EventArgs e)
        {
            bunifuSlider2.MaximumValue = 0;
            bunifuSlider2.MaximumValue = 100;
            bunifuSlider2.Value = GetApplicationVolume();
            timer1.Interval = 1;
            timer1.Start();
        }
        #endregion
        #region Event click      
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Mp3 Files|*.mp3";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileNames = openFileDialog1.SafeFileNames;
                filePaths = openFileDialog1.FileNames;
                foreach (String fileName in fileNames)
                {
                    ListMusic.Items.Add(fileName);
                }
            }
            //using (OpenFileDialog ofd = new OpenFileDialog())
            //{
            //    ofd.Filter = "Mp3 Files|*.mp3";
            //    if(ofd.ShowDialog()==DialogResult.OK)
            //    {
            //        mp3player.Open(ofd.FileName);
            //    }
            //}
        }

        private void btnPremo_Click_1(object sender, EventArgs e)
        {
            if(ListMusic.SelectedIndex !=0)
            {
                ListMusic.SetSelected(ListMusic.SelectedIndex - 1, true);
                Player.URL = filePaths[ListMusic.SelectedIndex];
                pictureBox1.Enabled = true;
            }
        }
        int a = 1;
        private void btnVolume_Click(object sender, EventArgs e)
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
            if (a % 2 != 0)
            {
                btnVolume.Image = Properties.Resources.volumemin;
                a++;
            }
            else
            {
                btnVolume.Image = Properties.Resources.volumemax;
                a++;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ListMusic.SelectedIndex != ListMusic.Items.Count -1)
            {
                ListMusic.SetSelected(ListMusic.SelectedIndex + 1, true);
                Player.URL = filePaths[ListMusic.SelectedIndex];
                pictureBox1.Enabled = true;
                btnPlay.Image = Properties.Resources.play;
            }
        }

        private void btnPlaySong_Click(object sender, EventArgs e)
        {
            Player.URL = filePaths[ListMusic.SelectedIndex];
            pictureBox1.Enabled = true;
            btnPlay.Image = Properties.Resources.play;
            TimeMusic.Start();
        }
        #endregion
        #region EventTimer    
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox5.Location = new Point(pictureBox5.Location.X - 1, pictureBox5.Location.Y);
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (pictureBox5.Location.X == 244)
                {
                    pictureBox5.Location = new Point(1650, 870);
                }
            }
            else
            {
                if (pictureBox5.Location.X == 244)
                {
                    pictureBox5.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                    pictureBox5.Location = new Point(917, 444);
                }
            }
        }
        private void TimeMusic_Tick(object sender, EventArgs e)
        {
            if (labelStart.Text != "" && Convert.ToInt32(labelStart.Text.Replace(":", "")) == Convert.ToInt32(LabelEnd.Text.Replace(":", "")) - 2)
            {
                Random random = new Random();
                ListMusic.SetSelected(random.Next(ListMusic.Items.Count), true);
            }
            labelStart.Text = Player.Ctlcontrols.currentPositionString;
            LabelEnd.Text = Player.Ctlcontrols.currentPositionString;
        }
        #endregion

    }
}
