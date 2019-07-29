using BrandyConsole.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrandyConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            GeneratorProcessor generatorProcessor = new GeneratorProcessor();

            generatorProcessor.Initialize();
            
            //string inputPath = ConfigurationManager.AppSettings["InputFolderPath"];
            //FileSystemWatcher fileWatcher = new FileSystemWatcher(inputPath);

            ////Enable events
            //fileWatcher.EnableRaisingEvents = true;

            ////Add event watcher
            //fileWatcher.Created += FileWatcher_Changed;

            ////var maxThreads = 4;

            ////// Times to as most machines have double the logic processers as cores
            ////ThreadPool.SetMaxThreads(maxThreads, maxThreads * 2);

            //Console.WriteLine("Listening");
            //Console.ReadLine();

        }

        //private static void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        //{
        //    ThreadPool.QueueUserWorkItem((o) => ProcessFile(e));
        //}


    }
}
