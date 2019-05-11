﻿using System;
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
            Events.LoadElementsStoreEvent.LoadElementsStoreEvents += GetElementsStores;
            Events.LoadElementStoreEvent.LoadElementStoreEvents += GetElementStores;
            Events.RemoveElementStoreEvent.RemoveElementStoreEvents += RemoveElementStores;

            Events.DatabaseChangedEvent.InitializeWatcher(
                Environment.CurrentDirectory,
                Database.Connection.DataSource);
        }

        public DbSet<Models.ElementStore> ElementStores { get; set; }

        private static string GetConnectionString()
        {
            string pathDb = "Data Source=./MethodStore.db;Version=3";

            return pathDb;
        }


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
    }
}
