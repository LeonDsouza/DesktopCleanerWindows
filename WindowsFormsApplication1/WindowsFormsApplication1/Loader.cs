using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Loader
    {
        public static void Load()
        {
            Organizer.Initialize();
            Console.WriteLine("Started");
            if (Organizer.Startup)
                DoOrganize();
            if (!Organizer.Startup)
                DoOrganize();

            Console.WriteLine("ShutDown");
            Tracker.Track();

        }
        private static void DoOrganize()
        {
            String[] filesAndFoldersOnDesktop = Files.GetAllFilesAndFolders();
            Organizer.StartOrganizing(filesAndFoldersOnDesktop);
        }
        public static void StartTrack()
        {
            Tracker.Track();
        }
        public static void CleanUp()
        {
            Organizer.Initialize();
            Console.WriteLine("Started");
            if (Organizer.Startup)
                DoOrganize();
            if (!Organizer.Startup)
                DoOrganize();

            Console.WriteLine("ShutDown");
        }
    }
}
