using System;
using System.Threading.Tasks;
using Octokit;
using NuGet.Versioning;

namespace Ib.Update
{
    public class GitHubUpdater
    {
        private string owner, name;
        private string currentTag;
        private bool prerelease;

        public GitHubUpdater(string owner, string name, string currentTag, bool prerelease = false)
        {
            this.owner = owner;
            this.name = name;
            this.currentTag = currentTag;
            this.prerelease = prerelease;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prerelease"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<Release> CheckForNewRelease(bool? prerelease = null)
        {
            bool check_preprelease = prerelease is null ? this.prerelease : prerelease.Value;
            
            // '/' is not allowed
            var client = new GitHubClient(new ProductHeaderValue($"{owner}_{name}"));
            var releases = await client.Repository.Release.GetAll(owner, name);

            foreach (var release in releases)
            {
                if (release.TagName == currentTag)
                {
                    return null;
                }

                if (check_preprelease || !release.Prerelease)
                {
                    return release;
                }
            }
            return null;
        }
    }

    public class GitHubNuGetUpdater
    {
        private string owner, name;
        private NuGetVersion currentVersion;

        public GitHubNuGetUpdater(string owner, string name, NuGetVersion currentVersion)
        {
            this.owner = owner;
            this.name = name;
            this.currentVersion = currentVersion;
        }

        public GitHubNuGetUpdater(string owner, string name, string currentVersion)
            : this(owner, name, NuGetVersion.Parse(currentVersion)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prerelease"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>        
        public async Task<Release> CheckForNewRelease(bool? prerelease = null)
        {
            bool check_preprelease = prerelease is null ? currentVersion.IsPrerelease : prerelease.Value;

            // '/' is not allowed
            var client = new GitHubClient(new ProductHeaderValue($"{owner}_{name}"));
            var releases = await client.Repository.Release.GetAll(owner, name);

            foreach (var release in releases)
            {
                string version = release.TagName;
                if (version.StartsWith("v"))
                {
                    version = version.Substring(1);
                }

                if (NuGetVersion.TryParse(version, out var semver))
                {
                    if (semver <= currentVersion)
                    {
                        return null;
                    }

                    if (check_preprelease || !semver.IsPrerelease)
                    {
                        return release;
                    }
                }
                else
                {
                    // make the error obvious
                    return release;
                }
            }
            return null;
        }
    }
}
