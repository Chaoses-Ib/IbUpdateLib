# IbUpdateLib/C++
## Usage
```cpp
#include <IbUpdate/GitHubUpdater.hpp>
#include <iostream>

int main() {
    try {
        GitHubUpdater updater{ "Chaoses-Ib", "IbEverythingExt", "v0.1" };
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
}
```

## Adding to your CMake project
[vcpkg](https://github.com/microsoft/vcpkg)：
```cmd
vcpkg install curl yaml-cpp
```

[CMakeLists.txt](https://cliutils.gitlab.io/modern-cmake/):
```cmake
# FetchContent_MakeAvailable requires CMake 3.14+
cmake_minimum_required(VERSION 3.14)

project(example)

# fetch IbUpdateLib
include(FetchContent)
FetchContent_Declare(IbUpdate
    GIT_REPOSITORY https://github.com/Chaoses-Ib/IbUpdateLib.git
    # it is recommanded to specify a version instead of using the main branch
    GIT_TAG        main
)
FetchContent_MakeAvailable(IbUpdate)

# here is your project
add_executable(example example.cpp)
target_link_libraries(example PRIVATE IbUpdate)
```

## Building separately
[vcpkg](https://github.com/microsoft/vcpkg):
```cmd
vcpkg install curl yaml-cpp
```

[CMake](https://cliutils.gitlab.io/modern-cmake/):
```cmd
cd IbUpdateLib/C++
mkdir build
cd build
cmake .. -DCMAKE_TOOLCHAIN_FILE="C:\...\vcpkg\scripts\buildsystems\vcpkg.cmake"
cmake --build . --config Release
```

For testing:
```cmd
vcpkg install catch2
```
And add `-DBUILD_TESTING=ON` when calling `cmake ..` .