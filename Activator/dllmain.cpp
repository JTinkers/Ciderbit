#include "stdafx.h"

#include <windows.h>

#pragma once

#pragma managed

using namespace System;
using namespace System::Reflection;
using namespace Ciderbit;
using namespace Ciderbit::Libraries;

DWORD WINAPI MainThread(LPVOID param)
{
	AllocConsole();

	Console::WriteLine("#\tActivator has been executed, initializing the component.");

	Component::Initialize();

	FreeLibraryAndExitThread((HMODULE)param, 0);

	return 0;
}

#pragma unmanaged

HMODULE hModule;
BOOL APIENTRY DllMain(HINSTANCE hInstance, DWORD reason, LPVOID reserved) 
{
	switch (reason) 
	{
		case DLL_PROCESS_ATTACH:
			CreateThread(0, 0, MainThread, hModule, 0, 0);
			break;
		case DLL_PROCESS_DETACH:
			FreeLibrary(hModule);
			break;
	}
	return true;
}