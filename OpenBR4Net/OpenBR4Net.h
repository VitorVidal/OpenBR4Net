#pragma once
#include <msvc2012/openbr/openbr_plugin.h>
#include <fstream>
#include <direct.h>
#include <iostream>
#include <exception>      // std::exception
#define GetCurrentDir _getcwd

class __declspec(dllexport) OpenBR4Net
{
private:
	QPoint firstEye;
	QPoint secondEye;
	bool initialized;
	bool fileExist(const char *fileName);	
public:
	int value;
	OpenBR4Net(int value);
	OpenBR4Net(void);
	~OpenBR4Net(void);
	void initialize(char* bibfile);
	void finalize();
	bool getInitialized();
	int getTemplate(char* file,char* templateFilename);
	void getFirstEye(char* templateFilename,int* x,int* y);
	void getSecondEye(char* templateFilename,int* x,int* y);
	void verify(char* query,char* target,float* score);
};

