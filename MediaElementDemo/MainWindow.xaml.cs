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

namespace MediaElementDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /*
        private void SkipBlockedTimes()
        {
            // if the mediaElement is between forbidden times, skip past it
            for (int i =0; i < timesCombo.Items.Count; i++)
            {
                string line = timesCombo.Items[i].ToString();
                string[] words = line.Split(" ");
                double cut = Double.Parse(words[0]);
                double restart = Double.Parse(words[1]);
                //double cut = TimesStringToDouble(words[0]);
                //double restart - TimeStringToDouble(words[1]);
                if (MediaPlayer.Position.TotalSeconds >= cut 
                    && MediaPlayer.Position.TotalSeconds < restart)
                {
                    MediaPlayer.Position = TimeSpan.FromSeconds(restart);
                }
            }
        }
        */
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
                Media.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Media.CanPause)
                Media.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (Media.Source != null)
                Media.Stop();
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            Media.IsMuted = !Media.IsMuted;
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
    }
}
