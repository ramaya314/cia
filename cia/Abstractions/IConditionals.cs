using System;
using System.IO;
using System.Threading.Tasks;

using PCLStorage;

using cia.Utils;

namespace cia.Abstractions
{
	public interface IConditionals
	{
		void SetSkipBackupAttrForIOS(string targetFilePath);
		void MoveFolder(string from, string to);
		bool IsDebug { get; }
		bool IsAndroid { get; }
		bool IsIOS { get; }
		bool IsWindowsPhone { get; }
		string OSName {get;}
		string OSVersion { get; }
		string DeviceModel { get; }
		string DeviceManufacturer { get; }
		int AndroidApi { get; }
		float UserFontSize {get; }

		//The next four are here simply to make use of our Microsoft.IO.RecyclableMemoryStream since this can't be added to the PCL
        Task<Stream> GetDownloadStream(string url);
		Task ReadWriteStreamAsync(Stream readStream, Stream writeStream, IProgress<ReadWriteProgress> progessReporter, object CancellationToken = null);
		Task ReadWriteStreamAsync(Stream readStream, Stream writeStream, Action<int> onPercentageUpdate);
		string GetEmbeddedResourceBase64ByteString(string resourceFileName);
		Task<string> ConvertFileToBase64String(IFile file);
    }
}

