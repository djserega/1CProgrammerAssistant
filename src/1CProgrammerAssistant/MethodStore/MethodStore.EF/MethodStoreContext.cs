using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace _1CProgrammerAssistant.MethodStore.EF
{
    public class MethodStoreContext : DbContext
    {
        public Events.UpdateElementStoreEvent ElementStoreEvent;
        public Events.LoadElementsStoreEvent LoadElementsStoreEvent;

        public MethodStoreContext() : base(GetConnectionString())
        {
            Events.UpdateElementStoreEvent.UpdateElementStoreEvents += UpdateElementStores;
            Events.LoadElementsStoreEvent.LoadElementsStoreEvents += GetElementsStores;
            Events.LoadElementStoreEvent.LoadElementStoreEvents += GetElementStores;
            Events.RemoveElementStoreEvent.RemoveElementStoreEvents += RemoveElementStores;
            Events.GetDistinctFieldsEvent.GetElementsEvents += GetDistinctFields;

            //Events.DatabaseChangedEvent.InitializeWatcher(
            //    Environment.CurrentDirectory,
            //    Database.Connection.DataSource);
        }

        public DbSet<Models.ElementStore> ElementStores { get; set; }

        private static string GetConnectionString()
        {
            string connectionString = SafeResult<string>.SafeAction(() => GetConnectionStringSafeAction());

            if (string.IsNullOrEmpty(connectionString))
                throw new CreateConnectionStringException();

            if (!CheckConnectionDataSouce(connectionString))
                throw new PingConnectionException("При проверке подключения к базе данных 'Method store' произошла ошибка.\n" +
                    "Проверьте строку подключения и наличие соединения к серверу.");

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

        private static bool CheckConnectionDataSouce(string connectionString)
        {
            string serverName = GetServerNameFromConnectionString(connectionString);

            bool result = false;
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = ping.Send(serverName, 5);
                    result = reply.Status == IPStatus.Success;
                }
                catch (PingException)
                {
                }

            }
            return result;
        }

        private static string GetServerNameFromConnectionString(string connectionString)
        {
            string serverName = string.Empty;

            string parameterDataSource = "Data Source=";
            int positionDataSource = connectionString.IndexOf(parameterDataSource);
            if (positionDataSource >= 0)
            {
                int positionSeparator = connectionString.IndexOf(';', positionDataSource);
                serverName = connectionString.Substring(
                    positionDataSource + parameterDataSource.Length,
                    positionSeparator - parameterDataSource.Length).Trim();

            }

            return serverName;
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
            List<Models.ElementStore> list = SafeResult<List<Models.ElementStore>>.SafeAction(
                ()
                => ElementStores.OrderByDescending(f => f.ID).ToList());

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

        private IQueryable<string> GetDistinctFields(NamesDistinctField name, string filter = null)
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

            return result.Distinct().Where(f => !string.IsNullOrEmpty(f)).Where(f => filter == null ? true : f.Contains(filter));
        }

        #region Overrides

        public override int SaveChanges()
        {
            int result = base.SaveChanges();

            Events.SaveChangesDatabaseEvent.SaveChanges();

            return result;
        }

        #endregion

        #endregion
    }
}
