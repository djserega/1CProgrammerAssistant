using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CProgrammerAssistant
{
    public delegate void ProcessTextInClipboardEvent();
    public class ProcessTextInClipboardEvents : EventArgs
    {
        public static event ProcessTextInClipboardEvent ProcessTextInClipboardEvent;
        public static void EvokeProcess() => ProcessTextInClipboardEvent?.Invoke();
    }
}
