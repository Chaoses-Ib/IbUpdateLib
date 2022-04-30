#include <catch.hpp>
#include <IbUpdate/GitHubUpdater.hpp>
#include <iostream>

TEST_CASE("GitHubUpdater") {
    auto common_test = [](std::string currentTag, bool prerelease = false) {
        try {
            GitHubUpdater updater{ "Chaoses-Ib", "IbEverythingExt", currentTag, prerelease };
            YAML::Node release = updater.check_for_new_release();
            if (release.IsNull()) {
                std::cout << "No new release\n";
                return;
            }

            std::cout << release["tag_name"].as<std::string>()
                << ", published at " << release["published_at"].as<std::string>()
                << '\n';
        }
        catch (std::runtime_error& e) {
            std::cout << e.what() << '\n';
        }
    };

    SECTION("test") {
        common_test("v0.1");
    }

    SECTION("test prerelease") {
        common_test("v0.1", true);
    }
}