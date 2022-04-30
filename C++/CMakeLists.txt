cmake_minimum_required(VERSION 3.14)

project(IbUpdate)

include(FetchContent)
FetchContent_Declare(
    github_api
    GIT_REPOSITORY https://github.com/Kicer86/github_api.git
    GIT_TAG        75cda5b2c1d73ade5d0da64fac1d1afd6acf6d76
)
set(GitHubAPI_CurlBackend ON)
FetchContent_MakeAvailable(github_api)

find_package(yaml-cpp CONFIG REQUIRED)

add_library(IbUpdate
    source/GitHubUpdater.cpp
)
target_include_directories(IbUpdate PUBLIC include)
target_compile_features(IbUpdate PUBLIC cxx_std_17)
target_link_libraries(IbUpdate
    PRIVATE github_api
    PRIVATE yaml-cpp
)

# test
if(CMAKE_PROJECT_NAME STREQUAL PROJECT_NAME)
    option(BUILD_TESTING "Build the testing tree." OFF)
    include(CTest)
    if(BUILD_TESTING)
        find_package(Catch2 REQUIRED)

        add_executable(IbUpdate_test
            test/main.cpp
            test/GitHubUpdater.cpp
        )
        target_link_libraries(IbUpdate_test
            PRIVATE IbUpdate
            PRIVATE Catch2::Catch2
        )
    endif()
endif()