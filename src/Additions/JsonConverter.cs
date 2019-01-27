using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _1CProgrammerAssistant.Additions
{
    public class JsonConverter<T> where T : class
    {
        public void Save(T obj, string fileName)
        {
            using (StreamWriter file = File.CreateText(fileName))
            {
                new JsonSerializer().Serialize(file, obj);
            }
        }

        public T Load(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            object obj = new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(fileName)), typeof(T));

            return obj as T;
        }
    }
}
