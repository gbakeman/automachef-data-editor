using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            //TODO: Handle Browse... action.
            Dispatcher.BeginInvoke(new Action(LoadProfileInfo));
        }

        private void DoDecrypt(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
           {
               txt_decryptedProfile.Text = Encryption.Decrypt(selectedProfile.EncryptedData, selectedProfile.ID);
           }));
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

            //TODO: Implement browse function
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
            lstBx_profileContents.Items.Clear();

            selectedProfile = new AutomachefProfile((string) cmbBox_profile.SelectedItem);
            lbl_profVersion.Content = selectedProfile.Version;
            txt_encryptedData.Text = selectedProfile.EncryptedData;

            //Populate list of profile items
            foreach (ProfileItem item in selectedProfile.contents.ProfileItems)
            {
                lstBx_profileContents.Items.Add(item);
            }
        }
    }
}
