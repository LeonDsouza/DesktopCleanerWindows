using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WindowsFormsApplication1
{
    public static class Files
    {

        public static String[] GetAllFilesAndFolders()
        {
            //get list of all things in desktop
            List<String> filesList = new List<string>();
            accesssubs(GetDesktopFolder(), ref filesList);
            return filesList.ToArray();
        }
        public static String GetDesktopFolder()
        {
            //get folder paths
            return Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        }


        //access directory and sub-directory
        public static void accesssubs(string targetDirectory, ref List<String> filesList)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                filesList.Add(fileName);

            //get sub directories
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                filesList.Add(subdirectory);
        }
        //used for copying files along with paths
        public static void CopyDirectory(DirectoryInfo diSourceDir, DirectoryInfo diDestDir)
        {
            if (!diDestDir.Exists)
                diDestDir.Create();
            FileInfo[] fiSrcFiles = diSourceDir.GetFiles();
            foreach (FileInfo fiSrcFile in fiSrcFiles)
            {
                fiSrcFile.CopyTo(Path.Combine(diDestDir.FullName, fiSrcFile.Name));
            }
            DirectoryInfo[] diSrcDirectories = diSourceDir.GetDirectories();
            foreach (DirectoryInfo diSrcDirectory in diSrcDirectories)
            {
                CopyDirectory(diSrcDirectory, new DirectoryInfo(Path.Combine(diDestDir.FullName, diSrcDirectory.Name)));
            }
        }

        //compare directories for differences
        public static bool CompareDirectories(DirectoryInfo diSourceDir, DirectoryInfo diDestDir)
        {
            FileInfo[] fiSrcFiles = diSourceDir.GetFiles();
            FileInfo[] fiDstFiles = diDestDir.GetFiles();

            // File number not equal. Return
            if (fiSrcFiles.Length != fiDstFiles.Length)
                return false;

            // Compare all the file's lengths now
            for (int i = 0; i < fiSrcFiles.Length; i++)
                if (fiSrcFiles[i].Length != fiDstFiles[i].Length)
                    return false;

            // Check sub directories for differences in copying
            DirectoryInfo[] diSrcDirectories = diSourceDir.GetDirectories();
            DirectoryInfo[] diDstDirectories = diDestDir.GetDirectories();

            for (int j = 0; j < diSrcDirectories.Length; j++)
                CompareDirectories(diSrcDirectories[j], diDstDirectories[j]);

            return true;
        }
    }
}
