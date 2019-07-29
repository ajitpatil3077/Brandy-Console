using BrandyConsole.DTO;
using BrandyConsole.Generators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace BrandyConsole.BusinessLogic
{
    /// <summary>
    /// This class is responsible for reading the input file from the input file mentioned in App.config and create output file as mentioned in app.config
    /// </summary>
    public class GeneratorProcessor
    {
        #region private variables
        private List<IGenerator> generatorList = null;
        #endregion

        #region public methods
        
        /// <summary>
        /// This method sets the file paths and Registers the generator with processor for generating output file.
        /// </summary>
        public void Initialize()
        {
            // Set file paths
            string referenceDataFilePath = GetReferenceDataFilePath();
            string inputFilePath = GetInputFilePath();
            generatorList = new List<IGenerator>();

            // Register available Generators to process and generate output file.
            RegisterGenerator(new CoalGenerator(inputFilePath, referenceDataFilePath));
            RegisterGenerator(new GasGenerator(inputFilePath, referenceDataFilePath));
            RegisterGenerator(new WindGenerator(inputFilePath, referenceDataFilePath));

            // Litens to input if new file is available and computes it.
            ListenInputFolder();
        }

        /// <summary>
        /// This method calls all registered generator and computes information and creates output file, each time when new file is placed in input folder.
        /// </summary>
        /// <param name="e"></param>
        public void Execute(FileSystemEventArgs e)
        {
            try
            {
                XmlDocument xmlDoc;
                XmlNode totalsNode, maxEmissionGeneratorsNode, actualHeatRatesNode;

                CreateOutputFileXMLNode(out xmlDoc, out totalsNode, out maxEmissionGeneratorsNode, out actualHeatRatesNode);
                foreach (IGenerator generator in generatorList)
                {
                    generator.SetInputFile(e.FullPath);
                    List<GeneratorDTO> generatorData = generator.Calculate();
                    AddNodeToOutput(generatorData, xmlDoc, totalsNode, maxEmissionGeneratorsNode, actualHeatRatesNode);
                }

                string outputPath = ConfigurationManager.AppSettings[ApplicationConstant.OUTPUT_FILEPATH];
                xmlDoc.Save(outputPath);

                Console.WriteLine("Converted to XML");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Invalid Input File : {0}.", ex.Message));
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// FileSystemWatcher object is created and enables handler for file created event.
        /// </summary>
        private void ListenInputFolder()
        {
            FileSystemWatcher fileWatcher = new FileSystemWatcher(GetInputFolderPath());

            //Enable events
            fileWatcher.EnableRaisingEvents = true;

            //Add event watcher
            fileWatcher.Created += FileWatcher_Changed;

            Console.WriteLine("Listening");
            Console.ReadLine();
        }

        /// <summary>
        /// This event is fired when file is added to input folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((o) => Execute(e));
        }

        /// <summary>
        /// Creates the Output file schema.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="totalsNode"></param>
        /// <param name="maxEmissionGeneratorsNode"></param>
        /// <param name="actualHeatRatesNode"></param>
        private static void CreateOutputFileXMLNode(out XmlDocument xmlDoc, out XmlNode totalsNode, out XmlNode maxEmissionGeneratorsNode, out XmlNode actualHeatRatesNode)
        {
            xmlDoc = new XmlDocument();
            XmlNode generationOutputRootNode = xmlDoc.CreateElement(ApplicationConstant.GENERATION_OUTPUT);
            xmlDoc.AppendChild(generationOutputRootNode);

            totalsNode = xmlDoc.CreateElement(ApplicationConstant.TOTALS);
            generationOutputRootNode.AppendChild(totalsNode);

            maxEmissionGeneratorsNode = xmlDoc.CreateElement(ApplicationConstant.MAX_EMISSION_GENERATORS);
            generationOutputRootNode.AppendChild(maxEmissionGeneratorsNode);

            actualHeatRatesNode = xmlDoc.CreateElement(ApplicationConstant.ACTUAL_HEAT_RATES);
            generationOutputRootNode.AppendChild(actualHeatRatesNode);
        }

        /// <summary>
        /// Adds computed values of registered generator into the output file.
        /// </summary>
        /// <param name="generatorData"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="totalsNode"></param>
        /// <param name="maxEmissionGeneratorsNode"></param>
        /// <param name="actualHeatRatesNode"></param>
        private void AddNodeToOutput(List<GeneratorDTO> generatorData, XmlDocument xmlDoc, XmlNode totalsNode, XmlNode maxEmissionGeneratorsNode, XmlNode actualHeatRatesNode)
        {
            foreach (GeneratorDTO generatorDTO in generatorData)
            {
                XmlNode generatorNode = xmlDoc.CreateElement(ApplicationConstant.GENERATOR);

                XmlNode nameNode = xmlDoc.CreateElement(ApplicationConstant.NAME);
                nameNode.InnerText = generatorDTO.Name;

                XmlNode totalNode = xmlDoc.CreateElement(ApplicationConstant.TOTAL);
                totalNode.InnerText = generatorDTO.DailyGenerationValue.ToString();

                generatorNode.AppendChild(nameNode);
                generatorNode.AppendChild(totalNode);

                totalsNode.AppendChild(generatorNode);

                XmlNode nameactualHeatRatesNode = xmlDoc.CreateElement(ApplicationConstant.NAME);
                nameactualHeatRatesNode.InnerText = generatorDTO.Name;

                if (generatorDTO.ActualHeatRate > 0)
                {
                    XmlNode heatRateNode = xmlDoc.CreateElement(ApplicationConstant.HEAT_RATE);
                    heatRateNode.InnerText = generatorDTO.ActualHeatRate.ToString();

                    actualHeatRatesNode.AppendChild(nameactualHeatRatesNode);
                    actualHeatRatesNode.AppendChild(heatRateNode);
                }

                foreach (DayDTO dayDTO in generatorDTO.Generation)
                {
                    if (dayDTO.DailyEmissionsValue > 0)
                    {
                        XmlNode dayNode = xmlDoc.CreateElement(ApplicationConstant.DAY);

                        XmlNode nameDayNode = xmlDoc.CreateElement(ApplicationConstant.NAME);
                        nameDayNode.InnerText = generatorDTO.Name;

                        XmlNode dateDayNode = xmlDoc.CreateElement(ApplicationConstant.DATE);
                        dateDayNode.InnerText = dayDTO.Date.ToString(ApplicationConstant.DATE_FORMAT);

                        XmlNode emissionDayNode = xmlDoc.CreateElement(ApplicationConstant.EMISSION);
                        emissionDayNode.InnerText = dayDTO.DailyEmissionsValue.ToString();

                        dayNode.AppendChild(nameDayNode);
                        dayNode.AppendChild(dateDayNode);
                        dayNode.AppendChild(emissionDayNode);

                        maxEmissionGeneratorsNode.AppendChild(dayNode);
                    }
                }
            }
        }

        /// <summary>
        /// Registers all the available Generator to process information.
        /// </summary>
        /// <param name="generator"></param>
        private void RegisterGenerator(IGenerator generator)
        {
            generatorList.Add(generator);
        }

        /// <summary>
        /// Retreives Input file path
        /// </summary>
        /// <returns></returns>
        private string GetInputFilePath()
        {
            return ConfigurationManager.AppSettings[ApplicationConstant.INPUT_FILEPATH];
        }

        /// <summary>
        /// Retreives Input folder path
        /// </summary>
        /// <returns></returns>
        private string GetInputFolderPath()
        {
            return ConfigurationManager.AppSettings[ApplicationConstant.INPUT_FOLDERPATH];
        }

        /// <summary>
        /// Retreives Reference Data file path
        /// </summary>
        /// <returns></returns>
        private string GetReferenceDataFilePath()
        {
            return ConfigurationManager.AppSettings[ApplicationConstant.REFERENCE_DATA_FILEPATH];
        } 
        #endregion
    }
}
