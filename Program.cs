using System;
using System.IO;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {

        // JSON file path
        string fileNamesPath = "FileNames.json";
        // Target Folder
        string targetDirectory = @"C:\Users\ ... set the target folder path";

        // JSON file read
        string jsonData = File.ReadAllText(fileNamesPath);

        // JSON data to jsonConvert
        List<string> dataList = JsonConvert.DeserializeObject<List<string>>(jsonData);

        // C# data to HashSet
        HashSet<string> hSet = new HashSet<string>(dataList);

        // Display folder info and check file names
        DisplayFolderInfo(targetDirectory, hSet);
    }

    static void DisplayFolderInfo(string directoryPath, HashSet<string> hSet)
    {
        try
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

            if (hSet.Contains(directoryInfo.Name))
            {
                // If the folder name exists in the HashSet, write the folder name and path to the console
                Console.WriteLine("Folder Name: " + directoryInfo.Name);
                Console.WriteLine("Folder Path: " + directoryInfo.FullName);
                Console.WriteLine();

                string currentDirectory = Environment.CurrentDirectory;

                string iconPath = currentDirectory + @"\Ico\folder-" + directoryInfo.Name + ".ico";


                // Create the desktop.ini file inside the folder
                string desktopIniPath = Path.Combine(directoryInfo.FullName, "desktop.ini");
                string iniContent = "[.ShellClassInfo]\r\nIconResource=" + iconPath + ",0";
                File.WriteAllText(desktopIniPath, iniContent);
                File.SetAttributes(desktopIniPath, File.GetAttributes(desktopIniPath) | FileAttributes.System | FileAttributes.Hidden);

                File.SetAttributes(directoryInfo.FullName, File.GetAttributes(directoryInfo.FullName) | FileAttributes.System);



            }

            // Get all subdirectories and recursively call the method for each subdirectory
            foreach (var subdirectory in directoryInfo.GetDirectories())
            {
                DisplayFolderInfo(subdirectory.FullName, hSet);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Handle unauthorized access (e.g., when you don't have permission to access certain folders)
            Console.WriteLine("Unauthorized Access: " + directoryPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}