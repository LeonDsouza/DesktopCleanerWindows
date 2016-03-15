using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Windows.Forms;
namespace WindowsFormsApplication1
{
    class Tracker
    {
        public static void Track()
        {

            //#if DEBUG
            //Service1 myService = new Service1();
            //myService.OnDebug();  //call debug function
            String path1 = AppDomain.CurrentDomain.BaseDirectory;
            //File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\log.txt");
            //XmlTextWriter textWriter = new XmlTextWriter("C:\\Users//Leon//Desktop//log.xml", null);
            //textWriter.WriteStartDocument();
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            StringBuilder sb = new StringBuilder();
            float Memory;
            sb.Append("\r\n Running Instance Separator");
            File.AppendAllText(path1 + "log.txt", "\r\n"+sb.ToString());
            File.AppendAllText(path1 + "log.txt", "\r\n");
            sb.Clear();
            sb.Append(Environment.UserName.ToString());
            File.AppendAllText(path1 + "log.txt", "\r\n Logged In As:" + sb.ToString());
            File.AppendAllText(path1 + "log.txt", "\r\n");
            sb.Clear();
            foreach (Process p in Process.GetProcesses("."))  //the dot for exception handling
            {
                try
                {
                    if (!String.IsNullOrEmpty(p.MainWindowTitle))  //if a process has a window name its a foreground app
                    {
                        Console.WriteLine("\r\n");
                        Console.WriteLine("\r\n Window Title:" + p.MainWindowTitle.ToString());
                        Console.WriteLine("\r\n Process Name:" + p.ProcessName.ToString());
                        Console.WriteLine("\r\n Window Handle:" + p.MainWindowHandle.ToString());
                        Console.WriteLine("\r\n Memory Allocation:" + p.PrivateMemorySize64.ToString());//memory occupied
                        Memory = p.PrivateMemorySize64;
                        Memory = Memory / (1024 * 1024);
                        //textWriter.WriteStartElement(p.MainWindowTitle.ToString());
                        //textWriter.WriteEndElement();
                        //textWriter.WriteStartElement(p.ProcessName.ToString());
                        //textWriter.WriteEndElement();
                        //textWriter.WriteStartElement(p.MainWindowHandle.ToString());
                        //textWriter.WriteEndElement();
                        //textWriter.WriteStartElement(p.PrivateMemorySize64.ToString());
                        //textWriter.WriteEndElement();
                        //myService.LogEvent(String.Format("ID:{0} Name:{1} Start Time:{2} ProcessorTime:{3} Threads:{4}", p.Id, p.ProcessName, p.StartTime, p.Threads), EventLogEntryType.Information); //log info
                        sb.Append(DateTime.Now.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n Current Date And Time : " + sb.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n");
                        sb.Clear();
                        sb.Append(p.MainWindowTitle.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\nMain Window Title : " + sb.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n");
                        sb.Clear();
                        sb.Append(p.ProcessName.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\nProcess Name : " + sb.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n");
                        sb.Clear();
                        sb.Append(p.MainWindowHandle.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\nMain Window Handle : " + sb.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n");
                        sb.Clear();
                        sb.Append(p.PrivateMemorySize64.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\nPrivate Memory Size in MB : " + Memory.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n");
                        sb.Clear();
                        sb.Append("\r\n" + (DateTime.Now - p.StartTime).ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n Running Time : " + sb.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n");
                        sb.Clear();
                        sb.Append("<----Log Entry Separator---->");
                        File.AppendAllText(path1 + "log.txt", "\r\n" + sb.ToString());
                        File.AppendAllText(path1 + "log.txt", "\r\n");
                        sb.Clear();
                        
                    }
                }
                catch
                {
                }

            }
            //textWriter.WriteEndDocument();
            //textWriter.Close();
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite); //make the service sleep once its work is done or no processes are there to be scanned
            //#else
            //Application.Exit();
            Environment.Exit(-1);
            /*ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
//#endif*/

        }
    }
}


