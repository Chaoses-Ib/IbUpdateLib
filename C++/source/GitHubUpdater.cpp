#include <IbUpdate/GitHubUpdater.hpp>
#include <github_api/github_api_curl.hpp>
#include <github_api/request.hpp>

GitHubUpdater::GitHubUpdater(std::string owner, std::string name, std::string current_tag, bool prerelease)
    : owner(owner), name(name), current_tag(current_tag), prerelease(prerelease) {}

YAML::Node GitHubUpdater::check_for_new_release(std::optional<bool> prerelease) {
    bool check_preprelease = prerelease.has_value() ? *prerelease : this->prerelease;

    GitHub::CurlBackend::Api backend;
    GitHub::Request client(backend.connect());
    std::string response = client.getReleases(owner, name);
    if (response.empty())
        throw std::runtime_error{ "GitHubUpdater: Empty response" };

    YAML::Node releases = YAML::Load(response);
    if (!releases.IsSequence())
        throw std::runtime_error{ "GitHubUpdater: Resource not found" };

    for (auto& release : releases) {
        if (release["tag_name"].as<std::string>() == current_tag)
            return {};

        if (check_preprelease || !release["prerelease"].as<bool>())
            return release;
    }
    return {};
}