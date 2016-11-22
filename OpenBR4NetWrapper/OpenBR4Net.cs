using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenBR4NetWrapper
{
  

    public class Template
    {
        public class Feature
        {
            public string Name { get; private set; }
            public float X { get; private set; }
            public float Y { get; private set; }
            public Feature(string name,float x, float y)
            {
                Name = name; X = x; Y = y;
            }

        }
        public byte[] bytes { get; private set; }
        public Feature F1 { get; private set; }
        public Feature F2 { get; private set; }
        public Template(byte[] _bytes, Feature f1,Feature f2)
        {
            bytes = _bytes; F1 = f1; F2 = f2;
        }
    }

    public unsafe class OpenBR4Net : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private unsafe struct __OpenBR4Net
        {
            public IntPtr* _vtable;
            public int value;
        }

        private string _extension = ".gal";
        private __OpenBR4Net* _cpp;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);
                
        [DllImport("OpenBR4Net.dll", EntryPoint = "??0OpenBR4Net@@QEAA@H@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int _OpenBR4Net_Constructor(__OpenBR4Net* ths, int value);
        [DllImport("OpenBR4Net.dll", EntryPoint = "??1OpenBR4Net@@QEAA@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int _OpenBR4Net_Destructor(__OpenBR4Net* ths);
        [DllImport("OpenBR4Net.dll", EntryPoint = "?initialize@OpenBR4Net@@QEAAXPEAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern void _initialize(__OpenBR4Net* ths,string bibfile);
        [DllImport("OpenBR4Net.dll", EntryPoint = "?finalize@OpenBR4Net@@QEAAXXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void _finalize(__OpenBR4Net* ths);
        [DllImport("OpenBR4Net.dll", EntryPoint = "?getTemplate@OpenBR4Net@@QEAAHPEAD0@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int _getTemplate(__OpenBR4Net* ths, string file,string templateFilename);
        [DllImport("OpenBR4Net.dll", EntryPoint = "?getFirstEye@OpenBR4Net@@QEAAXPEADPEAH1@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int _getFirstEye(__OpenBR4Net* ths, string file, ref int x,ref int y);
        [DllImport("OpenBR4Net.dll", EntryPoint = "?getSecondEye@OpenBR4Net@@QEAAXPEADPEAH1@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int _getSecondEye(__OpenBR4Net* ths, string file, ref int x,ref int y);
        [DllImport("OpenBR4Net.dll", EntryPoint = "?getInitialized@OpenBR4Net@@QEAA_NXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern bool _isInitialized(__OpenBR4Net* ths);
        [DllImport("OpenBR4Net.dll", EntryPoint = "?verify@OpenBR4Net@@QEAAXPEAD0PEAM@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int _verify(__OpenBR4Net* ths, string query, string target,ref float score);

        public OpenBR4Net()
        {
            //Allocate storage for object
            _cpp = (__OpenBR4Net*)Marshal.AllocHGlobal(sizeof(__OpenBR4Net));
            //Call constructor
            if (Environment.CurrentDirectory.Contains("OpenBR4Net"))
            {
                SetDllDirectory(Environment.CurrentDirectory + "\\OpenBR4Net");
                //System.IO.Directory.SetCurrentDirectory(Environment.CurrentDirectory + "\\OpenBR4Net");
            }

            //if (Environment.GetEnvironmentVariable("QTDIR") == null)
                // If it doesn't exist, create it.
                //Environment.SetEnvironmentVariable("envName", Environment.CurrentDirectory + "\\OpenBR4Net");

            _OpenBR4Net_Constructor(_cpp, 1);
        }

        public void Dispose()
        {
            //call destructor
            _OpenBR4Net_Destructor(_cpp);
            //release memory
            Marshal.FreeHGlobal((IntPtr)_cpp);
            _cpp = null;
        }

        public void initialize(string sdkFolder)
        {

            if (!System.IO.File.Exists(sdkFolder + @"\share\openbr\openbr.bib"))
                if (!System.IO.File.Exists(sdkFolder + @"\OpenBR4Net\share\openbr\openbr.bib"))
                    throw new ArgumentException("Invalid SDK path ...");
                else
                    sdkFolder = sdkFolder + "\\OpenBR4Net";

            _initialize(_cpp, sdkFolder);
        }

        public bool isInitialized()
        {
            return _isInitialized(_cpp);
        }

        public void finalize()
        {
            _finalize(_cpp);
        }

        public Template getTemplate(string file)
        {
            string templateFilename = string.Empty;
            byte[] bytes = null;
            try
            {
                templateFilename = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + _extension;
                if (_getTemplate(_cpp, file, templateFilename) > 0 &&
                    System.IO.File.Exists(templateFilename))
                {
                    int a, b, x, y;
                    a = b = x = y = 0;
                    _getFirstEye(_cpp, templateFilename, ref a, ref b);
                    _getSecondEye(_cpp, templateFilename, ref x, ref y);                    

                    bytes = System.IO.File.ReadAllBytes(templateFilename);
                    try{System.IO.File.Delete(templateFilename);}catch(Exception){}
                    return new Template(bytes,
                        new Template.Feature("FirstEye", a, b),
                        new Template.Feature("SecondEye", x, y));
                }
                else
                    throw new Exception("Can't get template");
            }
            catch (Exception ex)
            {
                throw ex;
            }                       
                       
        }

        public void verify(byte[] query, byte[] target, ref float score)
        {
            string tmpQueryFile = string.Empty;
            string tmpTargetFile = string.Empty;

            tmpQueryFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + _extension;
            tmpTargetFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + _extension;

            System.IO.File.WriteAllBytes(tmpQueryFile, query);
            System.IO.File.WriteAllBytes(tmpTargetFile, target);

            this.verify(tmpQueryFile, tmpTargetFile, ref score);

            try
            {
                System.IO.File.Delete(tmpQueryFile);
                System.IO.File.Delete(tmpTargetFile);
            }
            catch (Exception ex)
            {
            
            }

        }

        public void verify(string query, string target, ref float score)
        {
            try
            {
                _verify(_cpp, query, target, ref score);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
