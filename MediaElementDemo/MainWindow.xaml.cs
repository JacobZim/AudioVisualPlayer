using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.IO;

namespace MediaElementDemo
{
    public partial class MainWindow : Window
    {
        bool mIsPlaying = false;
        //private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;
        private string[] lines;
        private bool sidebar_enabled = true;
        private GridLength buffer;
        //private bool single_flag = true;
        public MainWindow()
        {
            InitializeComponent();
            buffer = SideBar.Width;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        
        private void SkipBlockedTimes()
        {
            // if the mediaElement is between forbidden times, skip past it
            for (int i =0; i < timesCombo.Items.Count; i++)
            {
                string line = timesCombo.Items[i].ToString();
                string[] words = line.Split(' ');
                double cut = Double.Parse(words[0]);
                double restart = Double.Parse(words[1]);
                //double cut = TimesStringToDouble(words[0]);
                //double restart - TimeStringToDouble(words[1]);
                if (Media.Position.TotalSeconds >= cut 
                    && Media.Position.TotalSeconds < restart
                    && check_skip_times.IsChecked == true)
                {
                    Media.Position = TimeSpan.FromSeconds(restart);
                }
            }
        }

        private void LoadSkipped_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choose .txt file";
            if (dialog.ShowDialog() == true)
            {
                timesCombo.Items.Clear();
                lines = File.ReadAllLines(dialog.FileName, Encoding.UTF8);
                for (int i = 0; i <= lines.Count() - 1; i++)
                {
                    timesCombo.Items.Add(lines[i]);
                }
            }
        }

        private void SideBar_Click(object sender, RoutedEventArgs e)
        {
            sidebar_enabled = !sidebar_enabled;
            if (sidebar_enabled) SideBar.Width = buffer;
            else SideBar.Width = ZeroColumn.Width;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Choose Media";
            if (dialog.ShowDialog() == true)
            {
                Media.Source = new Uri(dialog.FileName);
                MediaName.Text = dialog.FileName;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (Media.Source != null)
                if (!mIsPlaying)
                {
                    Media.Play();
                    mIsPlaying = true;
                    play_pause_text.Text = "Pause";
                } else
                {
                    Media.Pause();
                    mIsPlaying = false;
                    play_pause_text.Text = "Play";
                }
                    
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Media.CanPause)
                Media.Pause();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            Media.Position = TimeSpan.Zero;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (Media.Source != null)
                Media.Stop();
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            Media.IsMuted = !Media.IsMuted;
            if (Media.IsMuted) MuteButton.Content = "UnMute";
            else MuteButton.Content = "Mute";
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {
            Media.Position = Media.Position.Add(TimeSpan.FromSeconds(-10));
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            Media.Position = Media.Position.Add(TimeSpan.FromSeconds(10));
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Media.Volume = VolumeSlider.Value;
        }

        private void Balance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Media.Balance = BalanceSlider.Value;
        }

        private void Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Media.SpeedRatio = SpeedSlider.Value;
        }
        private void Progress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void Progress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            Media.Position = TimeSpan.FromSeconds(VideoSlider.Value);
        }
        private void Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            time_display.Text = TimeSpan.FromSeconds(VideoSlider.Value).ToString(@"hh\:mm\:ss");
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((Media.Source != null) && Media.NaturalDuration.HasTimeSpan && (!userIsDraggingSlider))
            {
                VideoSlider.Minimum = 0;
                VideoSlider.Maximum = Media.NaturalDuration.TimeSpan.TotalSeconds;
                VideoSlider.Value = Media.Position.TotalSeconds;
                SkipBlockedTimes();
            }
        }
    }
}
