using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinDebExtraction
{
    /// <summary>
    /// Класс для распаковки DEB-пакетов
    /// </summary>
    public class WinDebExtractor
    {
        private string _path;
        private string _archiveName;
        private string _libraryPath;
        private string _controlExtension;
        private string _dataExtension;
        /// <summary>
        /// Создание распаковщика DEB-пакета
        /// </summary>
        /// <param name="path">Путь к папке файла</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="libraryPath">Путь к библиотеке 7z.dll</param>
        /// <param name="controlExtension">Расширение вложенного архива control</param>
        /// <param name="dataExtension">Расширение вложенного архива data</param>
        public WinDebExtractor(string path, string fileName, string libraryPath, string controlExtension, string dataExtension)
        {
            _path = path;
            _archiveName = fileName;
            _libraryPath = libraryPath;
            _controlExtension = controlExtension;
            _dataExtension = dataExtension;
            SevenZipExtractor.SetLibraryPath(_libraryPath);
        }
        /// <summary>
        /// Распаковка DEB-пакета
        /// </summary>
        /// <param name="extractPath">Путь для распаковки пакета</param>
        public void Extract(string extractPath=null)
        {
            SevenZipExtractor sze = new SevenZipExtractor(_path + @"\" + _archiveName);
            sze.ExtractArchive(_path);
            ExtractContent(_path, "control", _controlExtension,extractPath);
            ExtractContent(_path, "data", _dataExtension,extractPath);
        }
        /// <summary>
        /// Распаковка вложенных архивов
        /// </summary>
        /// <param name="path">Путь к архиву</param>
        /// <param name="archiveName">Имя вложенного архива</param>
        /// <param name="extension">Расширение вложенного архива</param>
        /// <param name="extractPath">Путь для распаковки</param>
        private static void ExtractContent(string path, string archiveName, string extension,string extractPath=null)
        {
            string contentDirectoryPath = path + @"\" + archiveName;
            string destionationPath = extractPath == null ? contentDirectoryPath : extractPath + @"\" + archiveName;
            SevenZipExtractor primaryExtractor = new SevenZipExtractor(GetPrimaryContentArchivePath(extension, contentDirectoryPath));            
            primaryExtractor.ExtractArchive(contentDirectoryPath);
            SevenZipExtractor secondaryExtractor = new SevenZipExtractor(GetSecondaryContentArchivePath(archiveName, contentDirectoryPath));
            secondaryExtractor.ExtractArchive(destionationPath);
            if (File.Exists(GetPrimaryContentArchivePath(extension, contentDirectoryPath)))
            {
                File.Delete(GetPrimaryContentArchivePath(extension, contentDirectoryPath));
            }
            if (File.Exists(GetSecondaryContentArchivePath(archiveName, contentDirectoryPath)))
            {
                File.Delete(GetSecondaryContentArchivePath(archiveName, contentDirectoryPath));
            }
        }

        /// <summary>
        /// Получение пути к первичному вложеннома архиву
        /// </summary>
        /// <param name="extension">Расширение первичного вложенного архива</param>
        /// <param name="contentDirectoryPath">Путь к папке для разархивирования</param>
        /// <returns>Путь к первичному вложеннома архиву</returns>
        private static string GetPrimaryContentArchivePath(string extension, string contentDirectoryPath)
        {
            return contentDirectoryPath+"." + extension;
        }
        /// <summary>
        /// Получение пути к вторичному вложенному архиву
        /// </summary>
        /// <param name="archiveName">Расширение вторичного вложенного архива</param>
        /// <param name="contentDirectoryPath">Путь к папке для разархивирования</param>
        /// <returns>Путь к вторичному вложенному архиву</returns>
        private static string GetSecondaryContentArchivePath(string archiveName, string contentDirectoryPath)
        {
            return contentDirectoryPath + @"\" + archiveName + ".tar";
        }
    }
}
