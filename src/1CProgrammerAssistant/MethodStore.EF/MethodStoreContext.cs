using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
            //Database.CreateIfNotExists();
            //Database.ExecuteSqlCommand(
            //    "CREATE TABLE IF NOT EXISTS 'ElementStores' (" +
            //    " 'ID'       INTEGER PRIMARY KEY AUTOINCREMENT," +
            //    " 'Group'    TEXT    NOT NULL," +
            //    " 'Type'     TEXT    NOT NULL," +
            //    " 'Module'   TEXT    NOT NULL," +
            //    " 'Method'   TEXT    NOT NULL," +
            //    " 'Comment'  TEXT    NOT NULL" +
            //    ")");

            Events.UpdateElementStoreEvent.UpdateElementStoreEvents += UpdateElementStores;
            Events.LoadElementsStoreEvent.LoadElementsStoreEvents += GetElementsStores;
            Events.LoadElementStoreEvent.LoadElementStoreEvents += GetElementStores;
            Events.RemoveElementStoreEvent.RemoveElementStoreEvents += RemoveElementStores;
            Events.GetDistinctFieldsEvent.GetElementsEvents += GetDistinctFields;

            Events.DatabaseChangedEvent.InitializeWatcher(
                Environment.CurrentDirectory,
                Database.Connection.DataSource);
        }

        public DbSet<Models.ElementStore> ElementStores { get; set; }

        private static string GetConnectionString()
        {
            string connectionString = SafeResult<string>.SafeAction(() => GetConnectionStringSafeAction());

            if (string.IsNullOrEmpty(connectionString))
                new CreateConnectionStringException();

            return connectionString;
        }

        private static string GetConnectionStringSafeAction()
        {
            string pathConnectionString = Path.Combine(
                Environment.CurrentDirectory,
                "MethodStoreConnectionString.cfg");

            string connectionString = string.Empty;

            FileInfo infoConenctionString = new FileInfo(pathConnectionString);
            if (!infoConenctionString.Exists)
            {
                string defaultConenctionString = "Data Source=  ; Initial Catalog=  ; User Id=  ; Password=  ;";

                using (StreamWriter writer = infoConenctionString.CreateText())
                    writer.Write(defaultConenctionString);

                throw new Exception();
            }
            else
            {
                using (StreamReader reader = infoConenctionString.OpenText())
                    connectionString = reader.ReadToEnd();
            }

            return connectionString;
        }

        #region Additions events

        private bool UpdateElementStores(Models.ElementStore elementStore)
        {
            if (elementStore.ID == 0)
            {
                Models.ElementStore savedElement = ElementStores.Add(elementStore);
                elementStore.ID = savedElement.ID;
            }
            else
            {
                Models.ElementStore findedElement = GetElementStores(elementStore.ID);

                findedElement.Fill(elementStore);
            }

            bool result = SafeResult<bool>.SafeAction(
                () =>
                {
                    SaveChanges();
                    return true;
                });

            return result;
        }

        private List<Models.ElementStore> GetElementsStores()
        {
            List<Models.ElementStore> list = SafeResult<List<Models.ElementStore>>.SafeAction(() => ElementStores.ToList());

            return list;
        }

        private Models.ElementStore GetElementStores(int id)
        {
            Models.ElementStore elementStore = SafeResult<Models.ElementStore>.SafeAction(() => ElementStores.FirstOrDefault(f => f.ID == id));

            return elementStore;
        }

        private void RemoveElementStores(int id)
        {
            Models.ElementStore findedElement = GetElementStores(id);

            ElementStores.Remove(findedElement);

            Safe.SafeAction(() => SaveChanges());
        }

        private IQueryable<string> GetDistinctFields(NamesDistinctField name)
        {
            IQueryable<string> result = null;

            switch (name)
            {
                case NamesDistinctField.Group:
                    result = ElementStores.Select(f => f.Group);
                    break;
                case NamesDistinctField.Type:
                    result = ElementStores.Select(f => f.Type);
                    break;
                case NamesDistinctField.Module:
                    result = ElementStores.Select(f => f.Module);
                    break;
                default:
                    result = (IQueryable<string>)new List<string>();
                    break;
            }

            return result.Distinct().Where(f => !string.IsNullOrEmpty(f));
        }

        #endregion
    }
}
