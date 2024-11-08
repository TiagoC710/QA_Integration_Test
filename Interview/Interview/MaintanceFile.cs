﻿
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Interview
{
    public class MaintanceFile
    {
        private string mainFolder;
        private string copyFolder;
        /// <summary>
        /// Initializes a new instance of the MaintanceFile class.
        /// </summary>
        /// <param name="mainFolderLink">The path to the main folder for file operations.</param>
        /// <param name="copyFolderLink">The path to the copy folder for synchronization.</param>
        public MaintanceFile(string mainFolderLink, string copyFolderLink)
        {
            mainFolder = mainFolderLink;
            copyFolder = copyFolderLink;
        }
        
        /// <summary>
        /// Defines possible actions to perform on a file.
        /// </summary>
        public enum Action
        {
            Create, Delete, Copy, Syncronize, Read
        }

        // <summary>
        /// Performs the specified action on a file.
        /// </summary>
        /// <param name="actionEnum">The action to be performed on the file.</param>
        /// <param name="file">The name of the file on which the action is performed (optional).</param>
        /// <param name="text">The content to write to the file if creating (optional).</param>
        public void ActionOnFile(Action actionEnum, string file = "", string text = "")
        {
            switch (actionEnum)
            {
                case Action.Create:
                    CreateFile(file, text);
                    Console.WriteLine("File Create");
                    break;
                case Action.Delete:
                    DeleteFile(mainFolder, file);
                    break;
                case Action.Syncronize:
                    DeleteAllFiles(copyFolder);
                    SynchFolders(mainFolder, copyFolder);
                    Console.WriteLine("Folders Synch");
                    break;
                case Action.Copy:
                    CopyFile(mainFolder, file);
                    Console.WriteLine("File Copied");
                    break;
                case Action.Read:
                    ReadFile(file);
                    break;
            }
        }

        /// <summary>
        /// Creates a file with specified content if it does not already exist.
        /// </summary>
        /// <param name="file">The name of the file to create.</param>
        /// <param name="text">The text content to write to the file.</param>
        private void CreateFile(string file, string text)
        {
            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
                Console.WriteLine("Source directory does not exist! Will be created.");
            }
            if(file != ""){
                string fullPath = Path.Combine(mainFolder, file);
                if (!File.Exists(fullPath))
                {
                    File.WriteAllText(fullPath , text);
                }
            }
            else{
                Console.WriteLine("Please add name to your file.");
            }
           

        }

        /// <summary>
        /// Deletes a specified file from the main folder and its subdirectories.
        /// </summary>
        /// <param name="mainFolder">The main directory containing the file.</param>
        /// <param name="file">The name of the file to delete.</param>
        private void DeleteFile( string mainFolder , string file)
        {
             
             
            foreach (string files in Directory.GetFiles(mainFolder))
            {
                DirectoryInfo dir = new DirectoryInfo(mainFolder);
                string fullPath = Path.Combine(mainFolder, file);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                        Console.WriteLine("File has been deleted.");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                        return;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine($"Insufficient permissions to delete the file: {ex.Message}");
                        return;
                    }                                                                   
                }
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    string destSubDirPath = Path.Combine(mainFolder, subdir.Name);
                    DeleteFile( destSubDirPath, file); 
                }
            }

            
        }


        /// <summary>
        /// Copies a specified file from the main folder to the copy folder.
        /// </summary>
        /// <param name="mainFolder">The source directory containing the file.</param>
        /// <param name="file">The name of the file to copy.</param>
        private void CopyFile(string mainFolder , string file)
        {
            foreach (string files in Directory.GetFiles(mainFolder))
            {
                DirectoryInfo dir = new DirectoryInfo(mainFolder);
                string fullPath = Path.Combine(mainFolder, file);
                string destFilePath = Path.Combine(copyFolder, file);
                if (File.Exists(fullPath) && !File.Exists(destFilePath))
                {
                    try
                    {
                        File.Copy(fullPath, destFilePath);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
                        return;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine($"Insufficient permissions to delete the file: {ex.Message}");
                        return;
                    }                                                                   
                }
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    string destSubDirPath = Path.Combine(mainFolder, subdir.Name);
                    CopyFile( destSubDirPath, file); 
                }
            }

        }

        /// <summary>
        /// Reads and displays the contents of a specified file.
        /// </summary>
        /// <param name="file">The name of the file to read.</param>
        private void ReadFile(string file)
        {
            string fullPath = Path.Combine(mainFolder, file);
            string readFile = File.ReadAllText(fullPath);
            Console.WriteLine("The File "+file + " say: \n"+readFile);
        }

        /// <summary>
        /// Deletes all files and directories in a specified directory.
        /// </summary>
        /// <param name="destDir">The directory from which to delete all contents.</param>
        private static void DeleteAllFiles(string destDir)
        {
             foreach (string file in Directory.GetFiles(destDir))
            {
                File.Delete(file);
            }

            foreach (string subDir in Directory.GetDirectories(destDir))
            {
                Directory.Delete(subDir, recursive: true);
            }
        }
        
        /// <summary>
        /// Synchronizes two folders by copying all files and subdirectories from the source directory to the destination.
        /// </summary>
        /// <param name="sourceDir">The source directory to synchronize from.</param>
        /// <param name="destDir">The destination directory to synchronize to.</param>
        private static void SynchFolders(string sourceDir, string destDir)
        {
            using (var md5 = MD5.Create())
            {
                DirectoryInfo dir = new DirectoryInfo(sourceDir);
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException($"Source directory does not found: {sourceDir}");
                }

                Directory.CreateDirectory(destDir);

                foreach (FileInfo file in dir.GetFiles())
                {
                    string destFilePath = Path.Combine(destDir, file.Name);
                    file.CopyTo(destFilePath); 
                }

                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    string destSubDirPath = Path.Combine(destDir, subdir.Name);
                    SynchFolders(subdir.FullName, destSubDirPath); 
                }
            }
          
        }
    }
}
