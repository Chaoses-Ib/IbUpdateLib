#include <string>
#include <optional>
#include <yaml-cpp/yaml.h>

class GitHubUpdater {
public:
    GitHubUpdater(std::string owner, std::string name, std::string current_tag, bool prerelease = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prerelease"></param>
    /// <returns>See https://docs.github.com/en/rest/releases/releases#list-releases--code-samples</returns>
    /// <exception cref="std::runtime_error"></exception>
    YAML::Node check_for_new_release(std::optional<bool> prerelease={});

private:
    std::string owner, name;
    std::string current_tag;
    bool prerelease;
};