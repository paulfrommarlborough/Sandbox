using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace CollectProcess
{

    class IntervalXmlWriter
    {
        int interval;
        XmlWriter writer;


        public  IntervalXmlWriter()
        {
            string datadir="";
            try
            {
               
                RegistryKey key2 =
	Registry.LocalMachine.OpenSubKey("SOFTWARE\\Sample\\CollectProcess");

                datadir = (string)key2.GetValue("DataDirectory");
                key2.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("registry exception " + ex.ToString());
            }
            string filename = get_filename(datadir);
            writer = XmlWriter.Create(datadir + "\\" + filename);
            writer.WriteStartDocument();
            writer.WriteStartElement("Root");

        }

        public string get_filename(String datadir)
        {

            int count = 1;

            DateTime dt = DateTime.Now;  // format
            String s = dt.ToString("yyyyMMMdd");

            String filename;
            String machinename = System.Environment.MachineName;

            filename = machinename + "_" + s + "_" + count.ToString() + ".xml";
            while (File.Exists(datadir + "\\" + filename))
            {
                count++;
                filename = machinename + "_" + s + "_" + count.ToString() + ".xml";

            }
            return filename;
        }

        public void CloseDoc()
        {

            Console.WriteLine("write end intervals");
            writer.WriteEndElement();   // intervals
            
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }


        public void writeProcess(Process p)
        {
            writer.WriteStartElement("Process");
            writer.WriteAttributeString("PID", p.pid.ToString());
            writer.WriteAttributeString("Name", p.name);
            writer.WriteAttributeString("CommandLine", p.commandline);
            writer.WriteAttributeString("Owner", p.owner);
            writer.WriteAttributeString("CreationTime", p.creation_date.ToString());
     //     writer.WriteElementString("PID", p.pid.ToString());
     //     writer.WriteElementString("Name", p.name);
     //     writer.WriteElementString("CommandLine", p.commandline);
     //     writer.WriteElementString("Owner", p.owner);
     //     writer.WriteElementString("CreationTime", p.creation_date.ToString());
            writer.WriteEndElement();  
        }

        public void writeInterval(int it)
        {
            interval = it;

            Console.WriteLine("write interval {0}" , it.ToString());

            writer.WriteStartElement("Interval");
            writer.WriteAttributeString("Count", this.interval.ToString());
            writer.WriteAttributeString("Time", DateTime.Now.ToString());
            writer.WriteStartElement("Processes");
      
            return;
        }
        public void writeIntervalEnd()
        {
            writer.WriteEndElement();   // processes
            writer.WriteEndElement();   // interval
            writer.Flush();
            return;
        }

    }
}
