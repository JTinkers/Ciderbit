#include "pch.h"
#include <iostream>

void printTest()
{
	std::cout << "I'm being ran!";
}

int main()
{
	char y;
	std::cout << "printTest() addr = " << &printTest;
	std::cin >> y;
}
