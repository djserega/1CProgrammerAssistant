using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _1CProgrammerAssistant
{
    internal static class Permission
    {
        private static string _fullPathApplication;
        private static readonly string _nameApplication;

        static Permission()
        {
            _fullPathApplication = Assembly.GetExecutingAssembly().Location;
            _nameApplication = Path.GetFileNameWithoutExtension(new FileInfo(_fullPathApplication).Name);
        }

        internal static bool GetStatusAutostart()
        {
            try
            {
                return StatusAutostart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось получить статус автозапуска.\nПричина: {ex.Message}");
                return false;
            }
        }

        internal static bool SetRemoveAutostart(bool status)
        {
            try
            {
                if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    RunWithAdministrator();
                    return false;
                };

                if (status)
                    SetAutostart();
                else
                    RemoveAutostart();

                return true;
            }
            catch (Exception ex)
            {
                if (status)
                    MessageBox.Show($"Не удалось подключить автозапуск.\nПричина: {ex.Message}");
                else
                    MessageBox.Show($"Не удалось отключить автозапуск.\nПричина: {ex.Message}");

                return false;
            }
        }

        #region Private method

        private static void SetAutostart()
        {
            using (RegistryKey key = GetRegistryKey(true))
            {
                key.SetValue(_nameApplication, _fullPathApplication);
            };
        }

        private static void RemoveAutostart()
        {
            using (RegistryKey key = GetRegistryKey(true))
            {
                key.DeleteValue(_nameApplication);
            };
        }

        private static bool StatusAutostart()
        {
            using (RegistryKey key = GetRegistryKey())
            {
                object status = key.GetValue(_nameApplication);
                return status != null;
            };

        }

        private static RegistryKey GetRegistryKey(bool writable = false)
        {
            return Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", writable);
        }

        private static void RunWithAdministrator()
        {
            try
            {
                Process.Start(new ProcessStartInfo(_fullPathApplication, "/\"run from administrator\"")
                {
                    Verb = "runas"
                });
                Application.Current.Shutdown();
            }
            catch (Win32Exception) { }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка повышения прав.");
            }
        }

        #endregion
    }
}
