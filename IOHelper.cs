using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutomachefDataEditor
{
    class IOHelper
    {

    }

    /// <summary>
    /// Defines an Automachef profile with associated data.
    /// </summary>
    class AutomachefProfile
    {
        private const string profileFilename = "results.automachef";

        public string ID { get; set; }
        public int Version { get; set; }
        public String EncryptedData { get; set; }

        /// <summary>
        /// Constructs a new AutomachefProfile from the given profile data.
        /// </summary>
        /// <param name="profilePath">The path to the profile folder (with the numeric UID).</param>
        public AutomachefProfile(string profilePath)
        {
            ID = profilePath.Substring(profilePath.LastIndexOf('\\') + 1);
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
        }
    }
}
