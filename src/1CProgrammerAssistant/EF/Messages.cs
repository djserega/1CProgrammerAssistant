using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _1CProgrammerAssistant.EF
{
    internal static class Messages
    {
        internal static void RemoveElementStore(int id)
        {
            MessageBoxResult result = MessageBox.Show(
                "Действительно удалить элемент?",
                "Удаление элемента",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question,
                MessageBoxResult.Cancel);

            if (result != MessageBoxResult.OK)
                return;

            MethodStore.Events.RemoveElementStoreEvent.Remove(id);
        }
    }
}
