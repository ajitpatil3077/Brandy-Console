using BrandyConsole.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Xml.Linq;

namespace BrandyConsole.Generators
{
    /// <summary>
    /// This a base class for all the geneartor classes having common implementation.
    /// </summary>
    public abstract class GeneratorBase : IGenerator
    {
        #region protected variables
        protected string generatorName = null;
        protected string filePath = null;
        protected string refDataFilePath = null;
        protected ReferenceDataDTO referenceDataDTO = null;
        protected List<GeneratorDTO> generatorDTOList = null;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="refDataFilePath"></param>
        public GeneratorBase(string filePath, string refDataFilePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new Exception(ApplicationConstant.INPUT_FILE_NOT_FOUND);

            if (string.IsNullOrWhiteSpace(refDataFilePath))
                throw new Exception(ApplicationConstant.REFERENCE_DATA_FILE_NOT_FOUND);

            this.filePath = filePath;
            this.refDataFilePath = refDataFilePath;

            Initialize();
        }

        #endregion

        #region abstract methods
        /// <summary>
        /// abstract method to set generator name by derived generator 
        /// </summary>
        /// <param name="generatorName"></param>
        public abstract void SetGeneratorName(ref string generatorName);

        /// <summary>
        /// abstract method to be implementation by derived generator for calculating based upon generator specific rules.
        /// </summary>
        /// <returns></returns>
        public abstract List<GeneratorDTO> Calculate(); 
        #endregion

        #region virtual methods
        public virtual void SetData()
        {
            SetReferenceData();
            SetGeneratorData();
        }
        #endregion

        #region public methods
        public void SetInputFile(string inputFilePath)
        {
            filePath = inputFilePath;
            ReInitialize();
        } 
        #endregion

        #region private methods
        /// <summary>
        /// Initialization of generator and reference data.
        /// </summary>
        private void Initialize()
        {
            SetGeneratorName(ref generatorName);
            SetData();
        }

        /// <summary>
        /// Reintialization of generator data only, excluding reference data.
        /// </summary>
        private void ReInitialize()
        {
            generatorDTOList = null;
            SetGeneratorData();
        }

        /// <summary>
        /// This method sets the reference data DTO by reading reference data xml file.
        /// </summary>
        private void SetReferenceData()
        {
            XDocument xdoc = XDocument.Load(refDataFilePath);

            //Set Value Factor
            var selectFactorValue = from fResult in xdoc.Descendants(ApplicationConstant.VALUE_FACTOR)
                                    select fResult;
            XElement nodeValue = selectFactorValue.ElementAt(0);

            if (null == referenceDataDTO)
                referenceDataDTO = new ReferenceDataDTO();

            referenceDataDTO.ValueFactorHigh = double.Parse(nodeValue.Element(ApplicationConstant.HIGH).Value);
            referenceDataDTO.ValueFactorMedium = double.Parse(nodeValue.Element(ApplicationConstant.MEDIUM).Value);
            referenceDataDTO.ValueFactorLow = double.Parse(nodeValue.Element(ApplicationConstant.LOW).Value);

            //Parse EmissionFactor
            var selectEmissionValue = from fResult in xdoc.Descendants(ApplicationConstant.EMISSIONS_FACTOR)
                                      select fResult;
            nodeValue = selectEmissionValue.ElementAt(0);

            referenceDataDTO.EmissionFactorHigh = double.Parse(nodeValue.Element(ApplicationConstant.HIGH).Value);
            referenceDataDTO.EmissionFactorMedium = double.Parse(nodeValue.Element(ApplicationConstant.MEDIUM).Value);
            referenceDataDTO.EmissionFactorLow = double.Parse(nodeValue.Element(ApplicationConstant.LOW).Value);
        }

        /// <summary>
        /// This method sets the generator DTO by reading input data xml file.
        /// </summary>
        private void SetGeneratorData()
        {
            XDocument xdoc = XDocument.Load(filePath);

            Console.WriteLine(generatorName);
            var selectGenerator = from x in xdoc.Descendants(generatorName) select x;

            if (null == generatorDTOList)
                generatorDTOList = new List<GeneratorDTO>();

            foreach (var generator in selectGenerator)
            {
                var selectGeneration = from genResult in generator.Descendants(ApplicationConstant.DAY) select genResult;
                GeneratorDTO generatorDTO = new GeneratorDTO();
                generatorDTO.Name = generator.Element(ApplicationConstant.NAME).Value;

                if (generator.Element(ApplicationConstant.LOCATION) != null)
                    generatorDTO.Location = generator.Element(ApplicationConstant.LOCATION).Value;

                if (generator.Element(ApplicationConstant.TOTAL_HEAT_INPUT) != null)
                    generatorDTO.TotalHeatInput = double.Parse(generator.Element(ApplicationConstant.TOTAL_HEAT_INPUT).Value);

                if (generator.Element(ApplicationConstant.ACTUAL_NET_GENERATION) != null)
                    generatorDTO.ActualNetGeneration = double.Parse(generator.Element(ApplicationConstant.ACTUAL_NET_GENERATION).Value);

                if (generator.Element(ApplicationConstant.EMISSIONS_RATING) != null)
                    generatorDTO.EmissionsRating = double.Parse(generator.Element(ApplicationConstant.EMISSIONS_RATING).Value);

                if (null == generatorDTO.Generation)
                    generatorDTO.Generation = new List<DayDTO>();

                foreach (var generation in selectGeneration)
                {
                    DayDTO dayDTO = new DayDTO();
                    dayDTO.Date = DateTime.Parse(generation.Element(ApplicationConstant.DATE).Value).ToUniversalTime();
                    dayDTO.Energy = double.Parse(generation.Element(ApplicationConstant.ENERGY).Value);
                    dayDTO.Price = double.Parse(generation.Element(ApplicationConstant.PRICE).Value);

                    generatorDTO.Generation.Add(dayDTO);
                }

                generatorDTOList.Add(generatorDTO);
            }
        } 
        #endregion
    }
}

