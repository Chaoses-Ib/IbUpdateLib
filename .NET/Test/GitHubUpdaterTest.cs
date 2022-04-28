using Microsoft.VisualStudio.TestTools.UnitTesting;
using Update;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class GitHubUpdaterTest
    {
        public TestContext TestContext { get; set; }

        private async Task CommonTest(string currentTag, bool prerelease = false)
        {
            try
            {
                var updater = new GitHubUpdater("Chaoses-Ib", "IbEverythingExt", currentTag, prerelease);
                var release = await updater.CheckForNewRelease();
                if (release is null)
                {
                    TestContext.WriteLine("No new release");
                    return;
                }

                TestContext.WriteLine($"{release.TagName}, published at {release.PublishedAt}");
            }
            catch (Octokit.ApiException e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        [TestMethod]
        public async Task Test()
        {
            await CommonTest("v0.1");
        }

        [TestMethod]
        public async Task TestPrerelease()
        {
            await CommonTest("v0.1", true);
        }
    }

    [TestClass]
    public class GitHubNuGetUpdaterTest
    {
        public TestContext TestContext { get; set; }

        private async Task CommonTest(string currentVersion)
        {
            try
            {
                var updater = new GitHubNuGetUpdater("Chaoses-Ib", "IbEverythingExt", currentVersion);
                var release = await updater.CheckForNewRelease();
                if (release is null)
                {
                    TestContext.WriteLine("No new release");
                    return;
                }

                TestContext.WriteLine($"{release.TagName}, published at {release.PublishedAt}");
            }
            catch (Octokit.ApiException e)
            {
                TestContext.WriteLine(e.Message);
            }
        }

        [TestMethod]
        public async Task Test()
        {
            await CommonTest("0.1");
        }

        [TestMethod]
        public async Task TestPrerelease()
        {
            await CommonTest("0.1-alpha");
        }
    }
}