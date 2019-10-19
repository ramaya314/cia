
#if __IOS__
using Foundation;
using UIKit;
#endif

using System;  
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Stream = System.IO.Stream;
using System.Threading;

using Microsoft.IO;

using PCLStorage;

using cia.Utils;
using cia.Abstractions;


namespace cia.Conditionals
{
	/// <summary>
	/// This class is useful to include "shared" code in our PCL that is not available to us on PCL implementations
	/// This should also be the only class where we do #if statements to split code between platforms
	/// </summary>
	public class Conditionals : IConditionals
	{

        public async Task<Stream> GetDownloadStream(string url) {
            WebClient client = new WebClient();
            return await client.OpenReadTaskAsync(url);
        }
        
		public void MoveFolder(string from, string to) => Directory.Move(from, to);

		public void SetSkipBackupAttrForIOS(string targetFilePath) {
			#if __IOS__
			NSFileManager.SetSkipBackupAttribute (targetFilePath, true);
			#endif
		}
			
		public float UserFontSize
		{
			get {

				#if __ANDROID__

				//android font sizes scale with the system font size setting
				//so we don't have to modify this at all here therefore return 100%
				return 100;
				#elif __IOS__

				float rawFloat = NSUserDefaults.StandardUserDefaults.FloatForKey ("font_size");

				//turns a [0,1] range into a [80,120] range
				float percentageValue = (rawFloat * 40) + 80;

				return percentageValue;
                #else
                                return 0;
                #endif
            }
		}

		public bool IsDebug {
			get { 

				#if DEBUG
				return true;
				#else
				return false;
				#endif
			}
		}

		public bool IsAndroid
		{
			get
			{ 

				#if __ANDROID__
				return true;
				#else
				return false;
				#endif
			}
		}

		public bool IsIOS {
			get { 
				#if __IOS__
				return true;
				#else
				return false;
				#endif
			}
		}

		public bool IsWindowsPhone{
			get { 
				#if SILVERLIGHT
				return true;
				#elif WINDOWS_PHONE
				return true;
				#else
				return false;
				#endif
			}
		}

		public string OSName
		{
			get{

				#if __ANDROID__
				return "Android";
				#elif __IOS__
				return "iOS";
				# else 
				return "(unknown)";
				#endif
			}
		}


		public string OSVersion
		{
			get{

				#if __ANDROID__
				return Android.OS.Build.VERSION.Release;
				#elif __IOS__
				return UIDevice.CurrentDevice.SystemVersion;
				# else 
				return "(unknown)";
				#endif
			}
		}

		public string DeviceModel
		{
			get{
				#if __ANDROID__
				return Android.OS.Build.Model;
				#elif __IOS__
				return UIDevice.CurrentDevice.Model;
				# else 
				return "(unknown)";
				#endif
			}
		}

		public string DeviceManufacturer
		{
			get{
				#if __ANDROID__
				return Android.OS.Build.Manufacturer;
				#elif __IOS__
				return "Apple";
				# else 
				return "(unknown)";
				#endif
			}
		}

		public int AndroidApi 
		{
			get 
			{
				int ApiLevel = 0;
				#if __ANDROID__
				ApiLevel = (int)Android.OS.Build.VERSION.SdkInt;
				#endif
				return ApiLevel;
			}
		}

		RecyclableMemoryStreamManager MemoryManager { get; } = new RecyclableMemoryStreamManager();

		
        /// <summary>
        /// Writes a stream to another stream asynchronously
        /// </summary>
        /// <param name="readStream">the stream you need to read.</param>
        /// <param name="writeStream">the stream you want to write to.</param>
        /// <param name="progessReporter">Reporter object to use for percentage update reporting.</param>
        public async Task ReadWriteStreamAsync(Stream readStream, Stream writeStream, IProgress<ReadWriteProgress> progessReporter, object CancellationToken = null)
        {
            // handle object CancellationToken, since this can be null
            var cancelToken = CancellationToken != null ? (CancellationToken)CancellationToken : new CancellationToken();

            // check to see if cancelled first
            if (cancelToken.IsCancellationRequested)
                cancelToken.ThrowIfCancellationRequested();

            int Length = 65536;
            byte[] buffer = new byte[Length];

			//After updating Visual Studio to 8.0.2 and Xamarin.Android to 9.2.0.5
			//Streams that come down from System.Net.WebClient.OpenReadTaskAsync()
			//are no longer Seekable and will thus throw an error if we try to
			//query its length. Therefore we must wrap all read streams in a MemoryStreams
			//to be able to update the progress and have this not error out
		
			using(var ms = new RecyclableMemoryStream(MemoryManager))
			{ 
			    await readStream.CopyToAsync(ms);
				readStream.Dispose();

			    ms.Position = 0;

	            long totalBytes = ms.Length;
	            long receivedBytes = 0;

	            int i = 0;
	            int updateFrequency = 10;
	            for (;;)
	            {
	                int bytesRead;

	                bytesRead = await ms.ReadAsync(buffer, 0, buffer.Length, cancelToken);

	                if (bytesRead == 0)
	                {
	                    await Task.Yield();

	                    break;
	                }

	                writeStream.Write(buffer, 0, bytesRead);
	                receivedBytes += bytesRead;

	                if(i++ % updateFrequency == 0) {
		                if (progessReporter != null)
		                {
		                    ReadWriteProgress args = new ReadWriteProgress(receivedBytes, totalBytes);
		                    progessReporter.Report(args);
		                }
	                }

	                if (cancelToken.IsCancellationRequested)
	                    cancelToken.ThrowIfCancellationRequested();
	            }

			}

            writeStream.Dispose();
        }

		
        /// <summary>
        /// Writes a stream to another stream asynchronously
        /// </summary>
        /// <param name="readStream">the stream you need to read.</param>
        /// <param name="writeStream">the stream you want to write to.</param>
        /// <param name="onPercentageUpdate">Action to take on write percentage update.</param>
        public async Task ReadWriteStreamAsync(Stream readStream, Stream writeStream, Action<int> onPercentageUpdate) {

            onPercentageUpdate?.Invoke(0);

            int Length = 65536;
            byte[] buffer = new byte[Length];


			//After updating Visual Studio to 8.0.2 and Xamarin.Android to 9.2.0.5
			//Streams that come down from System.Net.WebClient.OpenReadTaskAsync()
			//are no longer Seekable and will thus throw an error if we try to
			//query its length. Therefore we must wrap all read streams in a MemoryStreams
			//to be able to update the progress and have this not error out
			using (var ms = new RecyclableMemoryStream(MemoryManager))
			{
				await readStream.CopyToAsync(ms);
				readStream.Dispose();
				ms.Position = 0;

				long totalBytes = ms.Length;
				long receivedBytes = 0;

				int i = 0;
				int updateFrequency = 2;
				for (; ; )
				{
					int bytesRead;
					if (i % updateFrequency == 0)
						bytesRead = await ms.ReadAsync(buffer, 0, buffer.Length);
					else
						bytesRead = ms.Read(buffer, 0, buffer.Length);

					if (bytesRead == 0)
					{
						await Task.Yield();

						break;
					}

					writeStream.Write(buffer, 0, bytesRead);
					receivedBytes += bytesRead;

					if (i++ % updateFrequency == 0)
					{
						onPercentageUpdate?.Invoke((int)(((float)receivedBytes / totalBytes) * 100));
					}
				}

				onPercentageUpdate?.Invoke(100);
			}
            writeStream.Dispose();

        }

		

        /// <summary>
        /// Converts a file to base64 string.
        /// </summary>
        /// <returns>The file contents in base64.</returns>
        /// <param name="file">The file to convert.</param>
		public async Task<string> ConvertFileToBase64String(IFile file)
		{
			if (file == null) return null;

			try
			{
				using (RecyclableMemoryStream memoryStream = new RecyclableMemoryStream(MemoryManager))
				{
					(await file.OpenAsync(PCLStorage.FileAccess.Read)).CopyTo(memoryStream);
					byte[] imageBytes = memoryStream.ToArray();

					string base64String = Convert.ToBase64String(imageBytes);
					return base64String;
				}
			}
			catch { return null; }
		}

		
        /// <summary>
        /// Gets the embedded resource base64 byte string.
        /// </summary>
        /// <returns>The embedded resource base64 byte string.</returns>
        /// <param name="resourceFileName">Resource file name.</param>
		public string GetEmbeddedResourceBase64ByteString(string resourceFileName) {
			try{
				Stream stream = cia.Utils.EmbeddedResourceManager.GetEmbeddedResourceStream(resourceFileName);
				using (RecyclableMemoryStream memoryStream = new RecyclableMemoryStream(MemoryManager))
				{
					stream.CopyTo(memoryStream);
					byte[] imageBytes = memoryStream.ToArray();

					string base64String = Convert.ToBase64String(imageBytes);
					return base64String;
				}
			} catch {
				return string.Empty;
			}
		}


	}
}

