using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace TaskieLib
{
    public class Tools
    {
        public delegate void ListCreated(string name);
        public static event ListCreated ListCreatedEvent;
        public static void SaveList(string listName, List<ListTask> list)
        {
            try
            {
                string filePath = GetFilePath(listName);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving list: {ex.Message}");
            }
        }
        public static string[] GetLists()
        {
            try
            {
                string localFolderPath = ApplicationData.Current.LocalFolder.Path;
                DirectoryInfo info = new DirectoryInfo(localFolderPath);
                FileInfo[] files = info.GetFiles("*.json");
                List<string> lists = new List<string>();
                foreach (FileInfo file in files)
                {
                    lists.Add(file.Name.Replace(".json", ""));
                }
                return lists.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting lists: {ex.Message}");
                return new string[0];
            }
        }

        public static List<ListTask> ReadList(string listName)
        {
            try
            {
                string taskFileContent = GetTaskFileContent(listName);
                if (taskFileContent != null)
                {
                    return JsonConvert.DeserializeObject<List<ListTask>>(taskFileContent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading list: {ex.Message}");
            }

            return new List<ListTask>();
        }
        public static void CreateList(string listName)
        {
            try
            {
                string newName = GenerateUniqueListName(listName);
                string filePath = GetFilePath(newName);
                File.Create(filePath).Close();
                ListCreatedEvent?.Invoke(newName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating list: {ex.Message}");
            }
        }
        public static void DeleteList(string listName)
        {
            try
            {
                string filePath = GetFilePath(listName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting list: {ex.Message}");
            }
        }
        public static void RenameList(string oldListName, string newListName)
        {
            try
            {
                string oldFilePath = GetFilePath(oldListName);
                string newFilePath = GetFilePath(newListName);

                if (File.Exists(oldFilePath))
                {
                    File.Move(oldFilePath, newFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error renaming list: {ex.Message}");
            }
        }

        private static string GenerateUniqueListName(string listName)
        {
            string uniqueName = listName;
            int count = 2;
            while (File.Exists(GetFilePath(uniqueName)))
            {
                uniqueName = $"{listName} (new {count++})";
            }
            return uniqueName;
        }
        private static string GetFilePath(string listName)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{listName}.json");
        }

        private static string GetTaskFileContent(string listName)
        {
            string filePath = GetFilePath(listName);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }
    }
}
