using System.Threading.Tasks;
using SQLite;
using System;

using cia.Models;

namespace  cia.DataStores
{
    /// <summary>
    /// Data access layer singleton worker that retrieves all data asynchronously
    /// </summary>
	public class DataAccessAsync : SQLiteAsyncConnection
	{

		static DataAccessAsync _instance;
		public static DataAccessAsync Instance
		{
			get
			{

                //System.Diagnostics.Debug.WriteLine("Getting Async SQL Connection...");
				if (_instance != null) {
					if(!_instance.DatabasePath.Equals(DataAccess.DatabaseFilePath)) {
						var lastInstance = _instance;
						_instance = new DataAccessAsync(DataAccess.DatabaseFilePath);
						//begin disposal of last database
						Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => {

							try
							{
								if (lastInstance != null)
									await lastInstance.CloseAsync();
							} catch(Exception e)
							{
								System.Diagnostics.Debug.WriteLine(e.Message);
								System.Diagnostics.Debug.WriteLine(e.StackTrace);
							}
						});
					}
					return _instance;
				}

				_instance = new DataAccessAsync(DataAccess.DatabaseFilePath);

				return _instance;
			}
		}

        /// <summary>
        /// Cleans up and nulls current connection to effectively reset it
        /// </summary>
		public static async Task ResetInstance()
        {
			try
			{
				if (_instance != null)
	           		await _instance.CloseAsync();

				ResetPool();
				_instance = null;
				DataAccess.KillInstance();


				//What happens when you call SQLiteConnection.Close() is that (along with a number of checks and other things) the SQLiteConnectionHandle 
				//that points to the SQLite database instance is disposed. This is done through a call to SQLiteConnectionHandle.Dispose(), 
				//however this doesn't actually release the pointer until the CLR's Garbage Collector performs some garbage collection. 
				//Since SQLiteConnectionHandle overrides the CriticalHandle.ReleaseHandle() function to call sqlite3_close_interop() (through another function) 
				//this does not close the database.
				//https://stackoverflow.com/questions/8511901/system-data-sqlite-close-not-releasing-database-file
				GC.Collect();
				GC.WaitForPendingFinalizers();
			} catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
				System.Diagnostics.Debug.WriteLine(e.StackTrace);
			}
		}


        /// <summary>
        /// Initializes a new instance of the <see cref="T:.DataStores.DataAccessAsync"/> class.
        /// </summary>
        /// <param name="path">Path.</param>
		public DataAccessAsync(string path) : base(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex)
		{
            Task.Run(async () => await CreateDOMAScheme(this));
		}

        public async static Task CreateDOMAScheme (SQLiteAsyncConnection db) {
            await db.CreateTablesAsync(CreateFlags.ImplicitIndex, DataAccess.SchemaTypes);
        }

	}
}
