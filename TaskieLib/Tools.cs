using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;

namespace TaskieLib
{
    public class Tools
    {

        public delegate void ListCreated(string name);
        public static event ListCreated ListCreatedEvent;
        public static void SaveList(string ListName, List<ListTask> list)
        {
            if (!File.Exists($"{ListName}.json"))
            {
                ListCreatedEvent.Invoke(ListName);
            }
            File.WriteAllText($"{Windows.Storage.ApplicationData.Current.LocalFolder.Path}\\{ListName}.json", JsonConvert.SerializeObject(list));
        }


        public static string[] GetLists()
        {
            string localFolderPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            DirectoryInfo info = new DirectoryInfo(localFolderPath);
            FileInfo[] files = info.GetFiles("*.json");
            List<string> lists = new List<string>();
            foreach (FileInfo file in files)
            {
                lists.Add(file.Name.Replace(".json", ""));
            }
            return lists.ToArray();
        }


        public static List<ListTask> ReadList(string ListName)
        {
            string taskfilecontent = GetTaskFileContent(ListName);
            if (taskfilecontent != null)
            {
                return JsonConvert.DeserializeObject<List<ListTask>>(taskfilecontent);
            }
            else
            {
                return new List<ListTask>();
            }
        }
        private static string GetTaskFileContent(string filename)
        {
            string filePath = $"{Windows.Storage.ApplicationData.Current.LocalFolder.Path}\\{filename}.json";
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                File.Create(filePath).Close();
                return null;
            }
        }

    }
}

