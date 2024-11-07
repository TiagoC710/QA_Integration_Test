
using System.Runtime.CompilerServices;

namespace Interview
{
    public class MaintanceFile
    {
        private string mainFolder;
        private string copyFolder;
        public MaintanceFile(string mainFolderLink, string copyFolderLink)
        {
            mainFolder = mainFolderLink;
            copyFolder = copyFolderLink;
        }
        public enum Action
        {
            Create, Delete, Copy, Syncronize, Read
        }
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
        private void CreateFile(string file, string text)
        {
            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
                Console.WriteLine("Source directory does not exist but I will create");
            }
            string fullPath = Path.Combine(mainFolder, file);
            if (!File.Exists(fullPath))
            {
                File.WriteAllText(fullPath , text);
            }

        }
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
                        //files.CopyTo(destFilePath); 
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

        private void ReadFile(string file)
        {
            string fullPath = Path.Combine(mainFolder, file);
            string readFile = File.ReadAllText(fullPath);
            Console.WriteLine("The File "+file + " say: \n"+readFile);
        }


        private static void DeleteAllFiles(string destDir)
        {
             foreach (string file in Directory.GetFiles(destDir))
            {
                File.Delete(file);
            }

            // Apaga todas as subpastas e seu conteúdo
            foreach (string subDir in Directory.GetDirectories(destDir))
            {
                Directory.Delete(subDir, recursive: true);
            }
        }

        private static void SynchFolders(string sourceDir, string destDir)
        {
        
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Diretório de origem não encontrado: {sourceDir}");
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
