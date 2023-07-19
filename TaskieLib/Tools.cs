using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace TaskieLib
{
    public class Tools
    {
        // Define the delegate and event for list creation
        public delegate void ListCreated(string name);
        public static event ListCreated ListCreatedEvent;

        // Save a list to a JSON file
        public static void SaveList(string listName, List<ListTask> list)
        {
            try
            {
                string filePath = GetFilePath(listName);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during saving the list
                Console.WriteLine($"Error saving list: {ex.Message}");
            }
        }

        // Get a list of existing lists
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
                // Handle any exceptions that occur during getting the list of lists
                Console.WriteLine($"Error getting lists: {ex.Message}");
                return new string[0]; // Return an empty array in case of an error
            }
        }

        // Read a list from a JSON file
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
                // Handle any exceptions that occur during reading the list
                Console.WriteLine($"Error reading list: {ex.Message}");
            }

            return new List<ListTask>(); // Return an empty list in case of an error
        }

        // Create a list with a given name
        public static void CreateList(string listName)
        {
            try
            {
                string newName = GenerateUniqueListName(listName);
                string filePath = GetFilePath(newName);
                File.Create(filePath).Close();

                // Fire the ListCreated event
                ListCreatedEvent?.Invoke(newName);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during list creation
                Console.WriteLine($"Error creating list: {ex.Message}");
            }
        }

        // Delete a list with a given name
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
                // Handle any exceptions that occur during list deletion
                Console.WriteLine($"Error deleting list: {ex.Message}");
            }
        }

        // Rename a list
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
                // Handle any exceptions that occur during list renaming
                Console.WriteLine($"Error renaming list: {ex.Message}");
            }
        }

        // Generate a unique list name (if the name already exists, append "(new)")
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

        // Get the file path for a given list name
        private static string GetFilePath(string listName)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{listName}.json");
        }

        // Get the content of the JSON file for a given list name
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
