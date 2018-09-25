#pragma once

#ifdef _WIN32
    #define ffdAPI extern "C" __declspec(dllexport)
#else
    #define ffdAPI extern "C" 
#endif

#include "MeshUtils/MeshUtils.h"
using namespace mu;
