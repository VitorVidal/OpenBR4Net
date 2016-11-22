#include "OpenBR4Net.h"

OpenBR4Net::OpenBR4Net(int value):value(value)
{
	initialized=false;
}
OpenBR4Net::OpenBR4Net()
{

}
OpenBR4Net::~OpenBR4Net()
{
}

void OpenBR4Net::initialize(char* bibfile)
{
	int argc = 0;
	char* argv = "";

	//QCoreApplication(argc, &argv);
	if(initialized == false)
		br::Context::initialize(argc, &argv,bibfile,false);	
	initialized=true;

	/*char cCurrentPath[FILENAME_MAX];
	GetCurrentDir(cCurrentPath, sizeof(cCurrentPath));
	cCurrentPath[sizeof(cCurrentPath) - 1] = '\0';
	printf ("The current working directory is %s", cCurrentPath);*/
}
void OpenBR4Net::finalize()
{
	br::Context::finalize();
	initialized=false;
}
bool OpenBR4Net::getInitialized()
{
	return initialized;
}
int OpenBR4Net::getTemplate(char* file,char* templateFilename)
{
	try
	{
		if(!fileExist(templateFilename))
		{
			//firstEye = tmpl1.file.get<QPoint>("Affine_0");
			//secondEye = tmpl1.file.get<QPoint>("Affine_1");				

			//QSharedPointer<br::Transform> transformATT = br::Transform::fromAlgorithm("FaceRecognitionATT");

			br::Template tmpl1(file);			
			//br::Template tmpl2(file);
			
			QSharedPointer<br::Transform> transformATT = br::Transform::fromAlgorithm("FaceRecognitionATT");

			tmpl1>>*transformATT;
					
			
			br::Gallery *gallery = br::Gallery::make(templateFilename);
			gallery->write(tmpl1);

			delete gallery;
		    //br::Format::write(templateFilename, tmpl2);
			return 1;
		}
		else
			return -1;
	}
	catch(std::exception& e)
	{
		std::cerr << "exception caught: " << e.what() << '\n';
		return -1;
	}


}
bool OpenBR4Net::fileExist(const char *fileName)
{
	std::ifstream infile(fileName);
	return infile.good();
}
void OpenBR4Net::getFirstEye(char* templateFilename,int* x,int* y)
{

	try
	{
		/*if(!firstEye.isNull())
		*x=firstEye.x();*y=firstEye.y();*/

		if(fileExist(templateFilename))
		{
			br::Gallery *qA = br::Gallery::make(templateFilename);
			br::TemplateList tqA = qA->read(); // returns a TemplateList of every template stored in the gallery
			br::Template queryA = tqA.at(0);
			delete qA;
			firstEye = queryA.file.get<QPoint>("First_Eye");

			if(!firstEye.isNull())
				*x=firstEye.x();*y=firstEye.y();
		}
	}
	catch(std::exception& e)
	{
		std::cerr << "exception caught: " << e.what() << '\n';		
	}


}
void OpenBR4Net::getSecondEye(char* templateFilename,int* x,int* y)
{
	/*if(!secondEye.isNull())
		*x=secondEye.x();*y=secondEye.y();*/

	try
	{
		if(fileExist(templateFilename))
		{
			br::Gallery *qA = br::Gallery::make(templateFilename);
			br::TemplateList tqA = qA->read(); // returns a TemplateList of every template stored in the gallery
			br::Template queryA = tqA.at(0);
			delete qA;
			secondEye = queryA.file.get<QPoint>("Second_Eye");
						
			if(!secondEye.isNull())
				*x=secondEye.x();*y=secondEye.y();
		}
	}
	catch(std::exception& e)
	{
		std::cerr << "exception caught: " << e.what() << '\n';		
	}
}

void OpenBR4Net::verify(char* query,char* target,float* score)
{
	try
	{
		if(initialized)
		{
			QSharedPointer<br::Distance> distance = br::Distance::fromAlgorithm("FaceRecognitionATT");
			//br::Template queryA = br::Format::read(query);
			//br::Template targetA = br::Format::read(target);
			
			br::Gallery *qA = br::Gallery::make(query);
			br::TemplateList tqA = qA->read(); // returns a TemplateList of every template stored in the gallery
			br::Template queryA = tqA.at(0);

			br::Gallery *tA = br::Gallery::make(target);
			br::TemplateList ttA = tA->read(); // returns a TemplateList of every template stored in the gallery
			br::Template targetA = ttA.at(0);

			delete qA;
			delete tA;

			float comparisonA = distance->compare(queryA,targetA);
			
			*score=comparisonA;

			
		}
	}
	catch(std::exception& e)
	{
		std::cerr << "exception caught: " << e.what() << '\n';		
	}


}
