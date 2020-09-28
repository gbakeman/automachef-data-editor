using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADE_UnitTesting
{
    [TestClass]
    public class ProfileTesting
    {
        [TestMethod]
        public void TestSingleProfileItem()
        {
            AutomachefDataEditor.ProfileItem singleProfileItem = new AutomachefDataEditor.ProfileItem("resources/76561197960267366/results.automachef");
            Assert.IsNotNull(singleProfileItem.FileObject);
            Assert.IsTrue(singleProfileItem.FileObject.Exists);
        }

        [TestMethod]
        public void TestGetDirContents()
        {
            string[] contents = AutomachefDataEditor.ProfileContents.GetDirectoryContents("resources/76561197960267366");
            Assert.AreEqual<int>(18, contents.Length);
        }
    }
}
