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

        private int version;
        MemoryStream encryptedData;
        public int Version { get => version; set => version = value; }
        public MemoryStream EncryptedData { get => encryptedData; set => encryptedData = value; }

        /// <summary>
        /// Constructs a new AutomachefProfile from the given profile data.
        /// </summary>
        /// <param name="profilePath">The path to the profile folder (with the numeric UID).</param>
        public AutomachefProfile(string profilePath)
        {
            FileInfo profileFile = new FileInfo(profilePath + "\\" + profileFilename);

            using (StreamReader reader = new StreamReader(profileFile.Open(FileMode.Open, FileAccess.Read)))
            {
                string readIn = reader.ReadLine();
                if (!readIn.Substring(0, 4).Equals("Ver:"))
                    return;
                version = Int32.Parse(readIn[4].ToString());
            }
        }
    }
}
