
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace WindowsFormsApplication1
{
    public static class Organizer
    {
        // stores extension-destination mapping details
        public static Dictionary<String, String> Mapping
            = new Dictionary<String, String>();

        // ignore files
        public static List<String> Ignore
            = new List<string>();

        // dump directory
        public static String OrganizedFolder = string.Empty;

        // clean at start or shutdown
        public static bool Startup = true;
        //public void  static Business();
        //max copies of a file
        public static int Cap = 99;
        public static void Initialize()
        {
            StringCollection AllowedEx = Properties.Settings.Default.ExtensionMap;

            if (AllowedEx != null)
            {
                if (AllowedEx.Count > 0)
                {
                    String[] k = new String[2];
                    foreach (String ext in AllowedEx)
                    {
                        k = ext.Split(',');
                        if (k.Length == 2)
                            if (!Mapping.ContainsKey(k[0]))
                                Mapping.Add(k[0], k[1]);
                    }
                }
            }

            //see the exclusion list
            StringCollection Exclude = Properties.Settings.Default.Exclude;

            if (Exclude != null)
            {
                if (Exclude.Count > 0)
                {
                    foreach (String fileOrFolderToExclude in Exclude)
                        Ignore.Add(fileOrFolderToExclude);
                }
            }


            // get destination directory
            OrganizedFolder = Properties.Settings.Default.OrganizedFolder;

            // decide when to run the cleaning
            if (Properties.Settings.Default.Schedule.ToLower() == "shutdown")
                Startup = false;

            // get value of max capacity
            Cap = Properties.Settings.Default.Cap;

            // check for folder existence if not then create them
            CreateInitialFolders();
        }


        public static void StartOrganizing(String[] objectsOnDesktop)
        {
            String moveFile = String.Empty;
            String newFileName = string.Empty;
            String newFolderName = string.Empty;
            int attempt = 0;

            foreach (String objectOnDesktop in objectsOnDesktop)
            {
                try
                {
                    FileInfo fi = new FileInfo(objectOnDesktop);

                    if (fi.Attributes != FileAttributes.Directory)
                    {
                        // if file in exclusion list LEAVE IT ALONE!!!!
                        if (Ignore.Contains(fi.Name.ToLower()))
                            continue;

                        // determine move directory
                        moveFile = Mapping.ContainsKey(fi.Extension.Remove(0, 1)) ? Mapping[fi.Extension.Remove(0, 1).ToLower()] : "Dump";

                        // check if directory<->extension mapping directory exists
                        DirectoryInfo di = new DirectoryInfo(OrganizedFolder + "\\" + moveFile);

                        // if no then create
                        if (!di.Exists)
                            di.Create();

                        // Log the attempt to move the file
                        Console.WriteLine("Attempting to move " + fi.FullName + " to " + di.FullName);

                        newFileName = fi.Name;
                        // if file exists..give copy no.
                        FileInfo checkDest = new FileInfo(OrganizedFolder + "\\" + moveFile + "\\" + newFileName);
                        if (checkDest.Exists)
                        {
                            newFileName = GetUniqueFileName(newFileName);

                            attempt = 1;
                            bool fileExists = true;

                            // if attempts to move <=99
                            while (fileExists && (attempt <= Cap))
                            {
                                FileInfo checkDestAgain = new FileInfo(OrganizedFolder + "\\" + moveFile + "\\" + newFileName);
                                if (checkDestAgain.Exists)
                                    fileExists = true;
                                else
                                    fileExists = false;

                                if (fileExists)
                                {
                                    newFileName = GetUniqueFileName(newFileName);
                                    attempt++;
                                }
                            }
                        }

                        if (attempt != Cap)
                        {
                            Console.WriteLine("Attempt = " + attempt.ToString() + " : Cap = " + Cap.ToString());

                            // Avoid overwrite,create copy
                            fi.CopyTo(OrganizedFolder + "\\" + moveFile + "\\" + newFileName, false);

                            FileInfo newFi = new FileInfo(OrganizedFolder + "\\" + moveFile + "\\" + newFileName);
                            //check if copy and original are of same size
                            if (newFi.Length == fi.Length)
                            {
                                //yes then delete the original
                                fi.Delete();
                            }

                        }
                        else
                        {
                            Console.WriteLine("Attempt Limit Exceeded");
                        }
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(fi.FullName);
                        newFolderName = fi.Name;



                        // if folder exists..append no.
                        DirectoryInfo newDi = new DirectoryInfo(OrganizedFolder + "\\Folders\\" + newFolderName);
                        if (newDi.Exists)
                        {
                            newFolderName = GetUniqueFileName(newFolderName);

                            attempt = 1;
                            bool folderExists = true;
                            while (folderExists && (attempt <= Cap))
                            {
                                newDi = new DirectoryInfo(OrganizedFolder + "\\Folders\\" + newFolderName);

                                if (newDi.Exists)
                                {
                                    folderExists = true;
                                    newFolderName = GetUniqueFileName(newFolderName);
                                }
                                else
                                    folderExists = false;

                                if (folderExists)
                                    attempt++;
                            }
                        }

                        // Attempts <=99
                        if (attempt != Cap)
                        {
                            if (Directory.GetDirectoryRoot(fi.FullName) == Directory.GetDirectoryRoot(OrganizedFolder + "\\Folders\\"))
                            {
                                //copy entire folder
                                Directory.Move(fi.FullName, OrganizedFolder + "\\Folders\\" + newFolderName);
                            }
                            else
                            {


                                // moved and original file size check
                                if (Files.CompareDirectories(di, new DirectoryInfo(OrganizedFolder + "\\Folders\\" + newFolderName)))
                                {
                                    Directory.Delete(di.FullName, true);
                                }
                            }


                        }
                        else
                        {
                            Console.WriteLine("Limit exceeded");
                        }
                    }

                }
                catch (Exception ex)
                {
                    // 
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("Fail");
                }
            }
        }

        public static void CreateInitialFolders()
        {
            try
            {
                // if dump folder doesnt exist then create
                DirectoryInfo diCheckMain = new DirectoryInfo(OrganizedFolder);
                if (!diCheckMain.Exists)
                    diCheckMain.Create();

                //check dump and sub folders where copies will be moved
                DirectoryInfo diCheckFolder = new DirectoryInfo(OrganizedFolder + "\\Folders");
                if (!diCheckFolder.Exists)
                    diCheckFolder.Create();
            }
            catch (Exception ex)
            {

            }
        }


        private static String GetUniqueFileName(String CurrentFileName)
        {
            String current = String.Empty;
            String CurrentFileNameTemp = String.Empty;

            try
            {
                // No file name means folder isnt present/access denied
                if (String.IsNullOrEmpty(CurrentFileName))
                    return String.Empty;

                // make a local temp copy without extension
                if (CurrentFileName.Contains("."))
                    //excluding extension using Last
                    CurrentFileNameTemp = CurrentFileName.Substring(0, CurrentFileName.LastIndexOf('.'));
                else
                    CurrentFileNameTemp = CurrentFileName;


                if (CurrentFileNameTemp.Length > 4)
                {
                    String last4Chars = CurrentFileNameTemp.Substring(CurrentFileNameTemp.Length - 4);
                    // check for copies like (no.)\0=4 characters
                    if (last4Chars.StartsWith("(") && last4Chars.EndsWith(")"))
                    {
                        last4Chars = last4Chars.Remove(0, 1);
                        last4Chars = last4Chars.Remove(2, 1);
                        int newNum = 1;
                        if (Int32.TryParse(last4Chars, out newNum))
                            newNum = newNum + 1;

                        current = newNum.ToString();
                        if (current.Length < 2)
                            current = "0" + current;
                    }
                }


            }
            catch (Exception ex)
            {

            }

            return CurrentFileNameTemp;
        }
    }
}
