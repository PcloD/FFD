CMAKE_MINIMUM_REQUIRED(VERSION 2.8)
PROJECT(FFD)

set(CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake)
if(APPLE)
    find_library(COCOA_LIBRARY Cocoa)
    mark_as_advanced(COCOA_LIBRARY)
    set(PLATFORM "Mac")
    set(MAC ON)
elseif(${CMAKE_SYSTEM_NAME} STREQUAL "Linux")
    set(PLATFORM "Linux")
    set(LINUX ON)
endif()

include(AddPlugin)
include(ISPC)


# Tests
option(BUILD_TESTS "Tests" OFF)


set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -fPIC")
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -fpermissive -fPIC -std=c++11 -Wno-deprecated")

add_subdirectory(MeshUtils)
add_subdirectory(FFD)

# Tests
if(BUILD_TESTS)
    add_subdirectory(Test)
endif()

