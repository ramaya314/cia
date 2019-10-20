using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using SQLite;

using PCLStorage;

using cia.Models;
using cia.Utils;

namespace cia.DataStores
{

    /// <summary>
    /// Data access layer singleton worker that retrieves all data from the configuration database
    /// </summary>
	public class DataAccess : SQLiteConnection
	{

		public object locker = new object ();

		//for old sql library. not needed right now but we might need it in the future
		//public static ISQLitePlatform targetPlatform = null;

		//NOTE: if we ever implement a multi-level interface stacking then 
		//we should implement this as a stack of db names and just pop and push accordingly
		private static string db_name = null;

		public static string DB_NAME {
			get{
				if (db_name == null) {
					db_name = "carbon.db";
					return db_name;
				} else
					return db_name;
			}
			set {

				//reset the connection if we provide a different database.
				if (value != db_name)
                    KillInstance();

				//if we are reseting
				if (value == null) {
					db_name = null;
				} else {
					db_name = "Database." + value;
				}

			}
		}


		private static DataAccess _instance;
		public static DataAccess Instance
		{
			get 
			{
				if (_instance != null)
					return _instance;

				_instance = new DataAccess (DatabaseFilePath);

				return _instance;
			}
		}

		public DataAccess (string path) : base (path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex)
		{
            CreateScheme(this);
		}


        static Type[] _schemaTypes = {
            typeof(Item),
            typeof(ShoppingCart),
            typeof(ShoppingCartItem),
            typeof(Alternative)
        };

        public static Type[] SchemaTypes => _schemaTypes;


        public static void CreateScheme (SQLiteConnection db) {
            db.CreateTables(CreateFlags.ImplicitIndex, SchemaTypes);
        }


		public async static Task<bool> DoesExist()
		{
			IFile dbFile = await FileSystem.Current.GetFileFromPathAsync (DatabaseFilePath);
			return dbFile != null;
		}

		public static string DatabasesFolder
		{
			get
			{
				return Path.Combine(FileSystem.Current.LocalStorage.Path, "databases");
			}
		}
        

        public static void KillInstance() {
            _instance?.Close();
            _instance = null;
			
			//What happens when you call SQLiteConnection.Close() is that (along with a number of checks and other things) the SQLiteConnectionHandle 
			//that points to the SQLite database instance is disposed. This is done through a call to SQLiteConnectionHandle.Dispose(), 
			//however this doesn't actually release the pointer until the CLR's Garbage Collector performs some garbage collection. 
			//Since SQLiteConnectionHandle overrides the CriticalHandle.ReleaseHandle() function to call sqlite3_close_interop() (through another function) 
			//this does not close the database.
			//https://stackoverflow.com/questions/8511901/system-data-sqlite-close-not-releasing-database-file
			GC.Collect();
			GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Gets the database file path.
        /// </summary>
        /// <value>The database file path.</value>
		public static string DatabaseFilePath {
			get { 
                return Path.Combine(DatabasesFolder, DB_NAME);
			}
		}

        /// <summary>
        /// Deletes the folder where all the default databases should be located
        /// </summary>
		public async static Task DestroyAllDatabases()
		{
			IFolder databaseFolder = await FileSystem.Current.GetFolderFromPathAsync(DatabasesFolder);
			if (databaseFolder != null)
			{
				await databaseFolder.DeleteAsync();
			}
		}

		public async static Task InitDatabase()
		{
			await EmbeddedResourceManager.CopyFileToLocation(DB_NAME, DatabaseFilePath, false);
		}
        
	}
}
