# IbUpdateLib/.NET
[![NuGet](http://img.shields.io/nuget/v/Ib.Update.svg)](https://www.nuget.org/packages/Ib.Update)

Usage:
```csharp
using System;
using Ib.Update;

try
{
    var updater = new GitHubUpdater("Chaoses-Ib", "IbEverythingExt", "v0.1");
    var release = await updater.CheckForNewRelease();
    if (release is null)
    {
        Console.WriteLine("No new release");
        return;
    }

    Console.WriteLine($"{release.TagName}, published at {release.PublishedAt}");
}
catch (Octokit.ApiException e)
{
    Console.WriteLine(e.Message);
}
```