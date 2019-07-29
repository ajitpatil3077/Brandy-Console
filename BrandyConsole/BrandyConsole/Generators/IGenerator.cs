using BrandyConsole.DTO;
using System.Collections.Generic;

namespace BrandyConsole.Generators
{
    public interface IGenerator
    {
        void SetInputFile(string filePath);

        void SetData();

        List<GeneratorDTO> Calculate();
    }
}
