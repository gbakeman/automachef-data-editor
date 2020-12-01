using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace AutomachefDataEditor
{
    /// <summary>
    /// Defines an Automachef profile with associated data.
    /// </summary>
    class AutomachefProfile
    {
        private const string profileFilename = "results.automachef";

        public string ID { get; set; }
        public string Location { get; set; } //The location on the filesystem of this profile.
        public int Version { get; set; }
        public String EncryptedData { get; set; }
        public ProfileContents contents { get; }

        /// <summary>
        /// Constructs a new AutomachefProfile from the given profile data.
        /// </summary>
        /// <param name="profilePath">The path to the profile folder (with the numeric UID).</param>
        public AutomachefProfile(string profilePath)
        {
            int finalSeparator = profilePath.LastIndexOf('\\');
            Location = profilePath.Substring(0, finalSeparator);
            ID = profilePath.Substring(finalSeparator + 1);
            FileInfo profileFile = new FileInfo(profilePath + "\\" + profileFilename);
            FileStream fs = profileFile.Open(FileMode.Open, FileAccess.Read);

            using (StreamReader reader = new StreamReader(fs))
            {
                string readIn = reader.ReadLine();
                if (!readIn.Substring(0, 4).Equals("Ver:"))
                    return;
                Version = Int32.Parse(readIn[4].ToString());
                fs.Position = 0;
                reader.DiscardBufferedData();
                EncryptedData = reader.ReadToEnd();
            }

            fs.Dispose();

            contents = new ProfileContents(profilePath);
        }
    }

    /// <summary>
    /// Represents a file found within a profile (a level, blueprint, or the profile its self, etc.)
    /// Generally encrypted, but can also be plain text.
    /// </summary>
    public class ProfileItem
    {
        public FileInfo FileObject { get; set; }
        //public bool IsEncrypted { get; }

        public ProfileItem(string filePath) => FileObject = new FileInfo(filePath);
    }

    /// <summary>
    /// Maintains a list of files within the profile, meant to be displayed in a ListView.
    /// </summary>
    public class ProfileContents
    {
        public ObservableCollection<ProfileItem> ProfileItems { get; }

        public ProfileContents(string profilePath)
        {
            ProfileItems = new ObservableCollection<ProfileItem>();

            foreach (string file in GetDirectoryContents(profilePath))
            {
                ProfileItems.Add(new ProfileItem(file));
            }
        }

        public static List<string> GetDirectoryContents(string path, List<string> files = null)
        {
            if (files == null)
                files = new List<string>();

            foreach (string file in Directory.GetFiles(path))
            {
                files.Add(file);
            }

            foreach (string dir in Directory.GetDirectories(path))
            {
                GetDirectoryContents(dir, files);
            }

            return files;
        }
    }
}
