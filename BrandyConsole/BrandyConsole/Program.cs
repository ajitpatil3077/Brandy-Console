using BrandyConsole.BusinessLogic;

namespace BrandyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create instance of generator processor and initialize for execution.
            GeneratorProcessor generatorProcessor = new GeneratorProcessor();
            generatorProcessor.Initialize();
        }
    }
}
