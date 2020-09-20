using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace AutomachefDataEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Dispatcher.BeginInvoke(new Action(LoadProfiles));
        }

        private void ProfileSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(LoadProfileInfo));
        }

        /// <summary>
        /// Background thread to get the available profiles.
        /// </summary>
        private void LoadProfiles()
        {
            string rootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            rootFolderPath += @"\AppData\LocalLow\HermesInteractive\Automachef\Saves";
            if (Directory.Exists(rootFolderPath))
            {
                foreach (String profile in Directory.EnumerateDirectories(rootFolderPath))
                {
                    cmbBox_profile.Items.Add(profile);
                }
            }

            cmbBox_profile.Items.Add("Browse...");
            cmbBox_profile.SelectedIndex = 0;
            return;
        }

        AutomachefProfile selectedProfile;
        /// <summary>
        /// Gets information about the requested profile.
        /// </summary>
        private void LoadProfileInfo()
        {
            selectedProfile = new AutomachefProfile((string) cmbBox_profile.SelectedItem);
            lbl_profVersion.Content = selectedProfile.Version;
        }
    }
}
