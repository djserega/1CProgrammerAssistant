using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace _1CProgrammerAssistant.MethodStore.EF
{
    public class MethodStoreContext : DbContext
    {
        public Events.UpdateElementStoreEvent ElementStoreEvent;
        public Events.LoadElementsStoreEvent LoadElementsStoreEvent;

        public MethodStoreContext() : base(GetConnectionString())
        {
            Database.CreateIfNotExists();
            Database.ExecuteSqlCommand(
                "CREATE TABLE IF NOT EXISTS 'ElementStores' (" +
                " 'ID'       INTEGER PRIMARY KEY AUTOINCREMENT," +
                " 'Group'    TEXT    NOT NULL," +
                " 'Type'     TEXT    NOT NULL," +
                " 'Module'   TEXT    NOT NULL," +
                " 'Method'   TEXT    NOT NULL," +
                " 'Comment'  TEXT    NOT NULL" +
                ")");

            Events.UpdateElementStoreEvent.UpdateElementStoreEvents += UpdateElementStores;
            Events.LoadElementsStoreEvent.LoadElementsStoreEvents += GetElementStores;
        }

        public DbSet<Models.ElementStore> ElementStores { get; set; }

        private static string GetConnectionString()
        {
            string pathDb = "Data Source=./MethodStore.db;Version=3";

            return pathDb;
        }

        private void UpdateElementStores(Models.ElementStore elementStore)
        {
            Models.ElementStore savedElement = ElementStores.Add(elementStore);
            elementStore.ID = savedElement.ID;

            Safe.SafeAction(() => SaveChanges());
        }

        public List<Models.ElementStore> GetElementStores()
        {
            List<Models.ElementStore> list = SafeResult<List<Models.ElementStore>>.SafeAction(() => ElementStores.ToList());

            return list;
        }
    }
}
