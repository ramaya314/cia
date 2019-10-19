using System;
using System.Reflection;
using Stream = System.IO.Stream;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

using PCLStorage;

using Xamarin.Forms;

using cia.Abstractions;

namespace cia.Utils
{
    public static class EmbeddedResourceManager
    {

        static Assembly assembly = typeof(EmbeddedResourceManager).GetTypeInfo().Assembly;

        private static readonly string EmbeddedResourcePrefix = "cia.";
            

        public static Stream GetSharedFileStreamForFile(string filePath)
        {
            //m'stream 
            Stream myStream = assembly.GetManifestResourceStream(filePath);
            if (myStream == null)
                myStream = GetEmbeddedResourceStream(filePath);

            return myStream;
        }


        public async static Task CopyRequiredAssetsToFolder(string folder, Action onFileInstall)
        {
            await CopyAssetGoupToFolder("RequiredAssets", folder, onFileInstall);
        }

        /// <summary>
        /// Copies a folder on the embedded manifest to a target location
        /// </summary>
        /// <param name="assetGroup">The dot notation path of the folder on the embedded resource manifest.</param>
        /// <param name="folder">The path of the folder where the resources should be copied to.</param>
        public async static Task CopyAssetGoupToFolder(string assetGroup, string folder)
        {
            await CopyAssetGoupToFolder(assetGroup, folder, null);
        }

        /// <summary>
        /// Copies a folder on the embedded manifest to a target location
        /// </summary>
        /// <param name="assetGroup">The dot notation path of the folder on the embedded resource manifest.</param>
        /// <param name="folder">The path of the folder where the resources should be copied to.</param>
        /// <param name="onFileInstall">An action to execute every time a file is succesfully copied.</param>
		public async static Task CopyAssetGoupToFolder(string assetGroup, string folder, Action onFileInstall)
        {
            string streamingAssetTopFolder = assetGroup + ".";
            string rootEmbeddedPrefix = EmbeddedResourcePrefix + streamingAssetTopFolder;


            string[] names = assembly.GetManifestResourceNames();
            foreach (string name in names)
            {
                if (!name.StartsWith(rootEmbeddedPrefix, StringComparison.CurrentCulture))
                    continue;

                string cleanPath = name.Substring(rootEmbeddedPrefix.Length);

                string workingPart = cleanPath;

                string targetFolder = folder;
                string targetName = string.Empty;
                string targetExtension = string.Empty;
                string targetFile = string.Empty;

                int workingIndex = -1;

                workingIndex = workingPart.LastIndexOf(".", StringComparison.CurrentCulture);
                if (workingIndex >= 0)
                {
                    targetExtension = workingPart.Substring(workingIndex);
                    workingPart = workingPart.Substring(0, workingIndex);
                }

                workingIndex = workingPart.LastIndexOf(".", StringComparison.CurrentCulture);
                if (workingIndex >= 0)
                {
                    targetName = workingPart.Substring(workingPart.LastIndexOf(".", StringComparison.CurrentCulture) + 1);
                    workingPart = workingPart.Substring(0, workingPart.LastIndexOf(".", StringComparison.CurrentCulture));

                    targetFolder = Path.Combine(folder, workingPart.Replace(".", "/"));
                }
                else
                {
                    targetName = workingPart;

                    targetFolder = folder;
                }

                targetFile = targetName + targetExtension;

                //IFolder iFolder = await FileSystem.Current.LocalStorage.CreateFolderAsync(targetFolder, CreationCollisionOption.OpenIfExists);
                //IFile iFile = await iFolder.CreateFileAsync(targetFile, CreationCollisionOption.ReplaceExisting);

                string filePath = targetFolder + "/" + targetFile;
                //if (IsAndroid && !filePath.StartsWith("file://", StringComparison.CurrentCulture))
                //	filePath = "file://" + filePath;

                string targetResourcePath = streamingAssetTopFolder + cleanPath;
                await CopyFileToLocation(targetResourcePath, filePath, true);

                if (onFileInstall != null)
                    onFileInstall();

                //conditionals.SetSkipBackupAttrForIOS(filePath);

                //Debug.WriteLine("created file: " + filePath);
            }

        }


        /// <summary> 
        /// Attempts to find and return the given resource from within the specified assembly. 
        /// </summary> 
        /// <returns>The embedded resource stream.</returns> 
        /// <param name="resourceFileName">Resource file name.</param> 
        public static Stream GetEmbeddedResourceStream(string resourceFileName)
        {
            string[] resourceNamesFiltered = new string[0];
            string[] resourceNames = assembly.GetManifestResourceNames();

            int filteredCount = 0;
            for (int i = 0; i < resourceNames.Length; i++)
            {
                var item = (string)resourceNames.GetValue(i);
                if (item.EndsWith(resourceFileName, StringComparison.CurrentCulture))
                {
                    filteredCount++;
                }
            }

            if (filteredCount > 0)
            {
                int index = 0;
                resourceNamesFiltered = new string[filteredCount];
                for (int i = 0; i < resourceNames.Length; i++)
                {
                    var item = (string)resourceNames.GetValue(i);
                    if (item.EndsWith(resourceFileName, StringComparison.CurrentCulture))
                    {
                        resourceNamesFiltered[index] = item;
                        index++;
                    }
                }
            }

            if (resourceNamesFiltered.Length <= 0)
            {
                throw new Exception(string.Format("Resource ending with {0} not found.", resourceFileName));
            }

            if (resourceNamesFiltered.Length > 1)
            {
                throw new Exception(string.Format("Multiple resources ending with {0} found: {1}{2}", resourceFileName, Environment.NewLine, string.Join(Environment.NewLine, resourceNamesFiltered)));
            }

            return assembly.GetManifestResourceStream(resourceNamesFiltered[0]);
        }

        /// <summary>
        /// Matches resourceFileName to whatever resource has an identifier that ends in it and will return the very first match.
        /// Usually used to determine if a uniquely named file is present and where to find it
        /// </summary>
        /// <returns>The full manifest resource name matched to the resource ending in resourceFileName.</returns>
        /// <param name="resourceFileName">Resource file name.</param>
        public static string GetFirstMatchingEmbeddedResourceIdentifier(string resourceFileName)
        {
            if (string.IsNullOrEmpty(resourceFileName))
                return null;

            string[] resourceNames = assembly.GetManifestResourceNames();

            for (int i = 0; i < resourceNames.Length; i++)
            {
                var item = (string)resourceNames.GetValue(i);
                if (item.EndsWith(resourceFileName, StringComparison.CurrentCulture))
                    return item;
            }

            return null;
        }


        /// <summary>
        /// Gets the embedded resource base64 byte string.
        /// </summary>
        /// <returns>The embedded resource base64 byte string.</returns>
        /// <param name="resourceFileName">Resource file name.</param>
        public static string GetEmbeddedResourceBase64ByteString(string resourceFileName)
            => conditionals.GetEmbeddedResourceBase64ByteString(resourceFileName);

        /// <summary>
        /// Converts a file to base64 string.
        /// </summary>
        /// <returns>The file contents in base64.</returns>
        /// <param name="filePath">The path of the file to convert.</param>
		public static async Task<string> ConvertFileToBase64String(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;
            return await ConvertFileToBase64String(await FileSystem.Current.GetFileFromPathAsync(filePath));
        }

        /// <summary>
        /// Converts a file to base64 string.
        /// </summary>
        /// <returns>The file contents in base64.</returns>
        /// <param name="file">The file to convert.</param>
        public static async Task<string> ConvertFileToBase64String(IFile file)
            => await conditionals.ConvertFileToBase64String(file);

        /// <summary>
        /// Gets the rext content of the embedded resource file.
        /// </summary>
        /// <returns>The embedded resource text content.</returns>
        /// <param name="resourceFileName">Resource file name.</param>
		public static string GetEmbeddedResourceTextContent(string resourceFileName)
        {
            try
            {
                Stream stream = GetEmbeddedResourceStream(resourceFileName);
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Copies an embedded file from the resource manifest to the root folder of the local storage
        /// </summary>
        /// <param name="strFileName">The file name on the resource manifest</param>.
        /// <param name="overwrite">If set to <c>true</c> overwrite.</param>
        /// <returns>The file object for the copied file.</returns>
		public async static Task<IFile> CopyEmbededFileToLocalResources(string strFileName)
        {
            return await CopyEmbededFileToLocalResources(strFileName, false);
        }

        /// <summary>
        /// Copies an embedded file from the resource manifest to the root folder of the local storage
        /// </summary>
        /// <param name="strFileName">The file name on the resource manifest</param>.
        /// <param name="overwrite">If set to <c>true</c> overwrite.</param>
        /// <returns>The file object for the copied file.</returns>
		public async static Task<IFile> CopyEmbededFileToLocalResources(string strFileName, bool overwrite)
        {
            string strDestinationFilePath = Path.Combine(FileSystem.Current.LocalStorage.Path, strFileName);
            return await CopyFileToLocation(strFileName, strDestinationFilePath, overwrite);
        }

        /// <summary>
        /// Copies an embedded file from the resource manifest to the given path
        /// </summary>
        /// <param name="strFileName">The file name on the resource manifest</param>.
        /// <param name="strDestinationFilePath">The full path of where the resource should be extracted to.</param>
        /// <param name="overwrite">If set to <c>true</c> overwrite.</param>
        /// <returns>The file object for the copied file.</returns>
		public async static Task<IFile> CopyFileToLocation(string strFileName, string strDestinationFilePath, bool overwrite)
        {
            return await CopyFileToLocation(strFileName, string.Empty, strDestinationFilePath, overwrite);
        }

        /// <summary>
        /// Copies an embedded file from the resource manifest to the given path
        /// </summary>
        /// <param name="strFileName">The file name on the resource manifest</param>.
        /// <param name="strPointerFoler">the folder name on the resource manifest.</param>
        /// <param name="strDestinationFilePath">The full path of where the resource should be extracted to.</param>
        /// <param name="overwrite">If set to <c>true</c> overwrite.</param>
        /// <returns>The file object for the copied file.</returns>
		public async static Task<IFile> CopyFileToLocation(string strFileName, string strPointerFoler, string strDestinationFilePath, bool overwrite)
        {

            //if the file already exists, then don't bother
            IFile targetFile = await FileSystem.Current.GetFileFromPathAsync(strDestinationFilePath);
            if (targetFile != null && !overwrite)
                return targetFile;

            if (strDestinationFilePath.StartsWith(FileSystem.Current.LocalStorage.Path, StringComparison.CurrentCulture))
            {
                strDestinationFilePath = strDestinationFilePath.Replace(FileSystem.Current.LocalStorage.Path, string.Empty);
                if(strDestinationFilePath.StartsWith("/"))
                {
                    strDestinationFilePath = strDestinationFilePath.Substring(1);
                }
            }

            //Android will fail file creation if the folder is not already present
            string strContaintingFolder = strDestinationFilePath.Substring(0, strDestinationFilePath.LastIndexOf("/", StringComparison.CurrentCulture));
            await FileSystem.Current.LocalStorage.CreateFolderAsync(strContaintingFolder, CreationCollisionOption.OpenIfExists);

            IFile file = await FileSystem.Current.LocalStorage.CreateFileAsync(strDestinationFilePath, CreationCollisionOption.ReplaceExisting);
            try
            {
                using (StreamReader reader = new StreamReader(GetSharedFileStreamForFile(EmbeddedResourcePrefix + strPointerFoler + strFileName)))
                {
                    Stream readStream = reader.BaseStream;
                    Stream writeStream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite);
                    ReadWriteStream(readStream, writeStream);
                }

                conditionals.SetSkipBackupAttrForIOS(strDestinationFilePath);
                return file;
            }
            catch (Exception e)
            {
                await file.DeleteAsync();
                throw e;
            }
        }

        /// <summary>
        /// Copies a file to a destination folder
        /// </summary>
        /// <param name="file">The file that should be copied.</param>
        /// <param name="destinationFolder">The destination folder where the file should be copied.</param>
        /// <param name="cancellationToken">A Cancellation token.</param>
        public static async Task CopyFileTo(this IFile file, IFolder destinationFolder, CancellationToken cancellationToken = default(CancellationToken))
        {
            var destinationFile =
                await destinationFolder.CreateFileAsync(file.Name, CreationCollisionOption.ReplaceExisting, cancellationToken);

            using (var outFileStream = await destinationFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite, cancellationToken))
            using (var sourceStream = await file.OpenAsync(PCLStorage.FileAccess.Read, cancellationToken))
            {
                await sourceStream.CopyToAsync(outFileStream, 81920, cancellationToken);
            }
        }

        /// <summary>
        /// Writes a stream to another stream
        /// </summary>
        /// <param name="readStream">the stream you need to read.</param>
        /// <param name="writeStream">the stream you want to write to.</param>
		public static void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Dispose();
            writeStream.Dispose();
        }


        /// <summary>
        /// Writes a stream to another stream asynchronously
        /// </summary>
        /// <param name="readStream">the stream you need to read.</param>
        /// <param name="writeStream">the stream you want to write to.</param>
        /// <param name="progessReporter">Reporter object to use for percentage update reporting.</param>
        public static async Task ReadWriteStreamAsync(Stream readStream, Stream writeStream, IProgress<ReadWriteProgress> progessReporter, object CancellationToken = null)
            => await conditionals.ReadWriteStreamAsync(readStream, writeStream, progessReporter);

        /// <summary>
        /// Writes a stream to another stream asynchronously
        /// </summary>
        /// <param name="readStream">the stream you need to read.</param>
        /// <param name="writeStream">the stream you want to write to.</param>
        /// <param name="onPercentageUpdate">Action to take on write percentage update.</param>
        public static async Task ReadWriteStreamAsync(Stream readStream, Stream writeStream, Action<int> onPercentageUpdate)
            => await conditionals.ReadWriteStreamAsync(readStream, writeStream, onPercentageUpdate);

        static bool skipRoleFilteringForContent;
        public static bool SkipRoleFilteringForContent
        {
            get { return skipRoleFilteringForContent; }
            set { skipRoleFilteringForContent = value; }
        }

        public static IConditionals conditionals { get; set; }
        public static string OSName { get { return conditionals?.OSName; } }
        public static string OSVersion { get { return conditionals?.OSVersion; } }
        public static string DeviceModel { get { return conditionals?.DeviceModel; } }
        public static string DeviceManufacturer { get { return conditionals?.DeviceManufacturer; } }
        public static bool IsDebug { get { return conditionals?.IsDebug ?? true; } }
        public static bool IsAndroid { get { return conditionals?.IsAndroid ?? Device.RuntimePlatform == Device.Android; } }
        public static bool IsIOS { get { return conditionals?.IsIOS ?? Device.RuntimePlatform == Device.iOS; } }
        public static bool IsWindowsPhone { get { return conditionals?.IsWindowsPhone ?? false; } }
        public static int AndroidApi { get { return conditionals?.AndroidApi ?? 24; } }
        public static float UserFontSize { get { return conditionals?.UserFontSize ?? 12; } }

    }
}
