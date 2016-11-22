# OpenBR4Net
A wrapper class to use OpenBR (http://openbiometrics.org/) on .NET

VitorVidal, 11/22/2016 

1   - Introduction

1.1 - Projects

1.2 - Requirements

1.3 - OpenBR4Net.App.Console

1.4 - Knowns Bugs

1 - Introduction

  OpenBR is a communal biometrics framework supporting the development of open algorithms and reproducible evaluations.
  More details on http://openbiometrics.org/ , http://openbiometrics.org/publications/klontz2013open.pdf
  
  I spended a lot of time make it works on .Net, it's still "green" and I'm not a good vc++ programer, so I would like help to improve it. 
  
  It has two functions: extract template from image files and compare template files (score between 0 and 1).
  
1.2 - Projects

-OpenBR4Net (solution)

  +OpenBR4Net (VC++ project)
  
  +OpenBR4Net.App.Console (C# console test program)
  
  +OpenBR4NetWrapper (C# wrapper Project)
  
1.2 - Requirements

  You must install OpenBR before use the wrapper. I already provide a install files on "Install\SDK" folder at OpenBR4Net vc++ project. It was builded using VS2012 following the tutorials on http://openbiometrics.org/. Just copy SDK folder contents to an "OpenBR4Net" folder on your executable folder.
  
  To use the wrapper, just create a instance of OpenBR4NetWrapper class as in OpenBR4Net.App.Console project.
  
1.3 - OpenBR4Net.App.Console

  Just an c# console project to test and show the wrapper.
  
  Must provide template files to "verify" method using extension ".gal".
  
1.4 - Knows Bugs

  I'm having issues when compare the same template data. It's returning NaN.
  
  
  
  
  
  

  
  
  
