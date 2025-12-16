cmake_minimum_required(VERSION 3.10)
project(WindowController)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Create shared library (DLL)
add_library(WindowController SHARED WindowController.cpp)

# Link Windows libraries
target_link_libraries(WindowController user32)

# Output to Unity Plugins folder
set_target_properties(WindowController PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../Assets/Plugins/x86_64"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../Assets/Plugins/x86_64"
)
