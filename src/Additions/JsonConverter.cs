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
            using (StreamWriter writer = File.CreateText(fileName))
            {
                new JsonSerializer().Serialize(writer, obj);
            }
        }

        public T Load(string fileName)
        {
            object obj = null;

            if (File.Exists(fileName))
            {
                try
                {
                    using (StreamReader reader = File.OpenText(fileName))
                    {
                        obj = new JsonSerializer().Deserialize(new JsonTextReader(reader), typeof(T));
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return obj as T;
        }
    }
}
