using System;
using System.IO;
using LiteDB;

namespace Core
{
    public static class StoreService
    {
        static ILiteDatabase _instance;
        static object __lock = new object();
        static string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "database.db");

        public static void Start()
        {
            lock (__lock)
            {
                Dispose();
                _instance = new LiteDatabase(new ConnectionString(Path) { Password = "NovassatSystems" });

            }
        }
        public static ILiteDatabase Instance
        {
            get
            {
                lock (__lock)
                {
                    if (_instance == null)                    
                        _instance = new LiteDatabase(Path);                   

                    return _instance;
                }
            }
        }

        public static void Dispose()
        {
            _instance?.Dispose();
            _instance = null;
        }

        public static void DeleteDataBase()
        {
            Dispose();
            File.Delete(Path);
        }
    }
}