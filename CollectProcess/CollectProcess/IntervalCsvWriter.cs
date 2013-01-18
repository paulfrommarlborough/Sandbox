using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Win32;

namespace CollectProcess
{
    class IntervalCsvWriter
    {
        int interval;
        FileStream fs;
        StreamWriter m_streamWriter;
        DateTime intervaltime;
        String datetime_formatted;

       public IntervalCsvWriter()
        {
            string datadir = "";
            try
            {

                RegistryKey key2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\PerfCap\\PM");
                datadir = (string)key2.GetValue("DataDirectory");
                key2.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("registry exception " + ex.ToString());
            }
            string filename = get_filename(datadir);

            try
            {
                fs = File.OpenWrite(datadir + "\\" + filename);
                m_streamWriter = new StreamWriter(fs); 

            }
            catch (Exception e1)
            {
                   Console.WriteLine("open exception " +  filename +  " Exception " + e1.ToString());
            }
                
        }

        public string get_filename(String datadir)
        {

            int count = 1;

            DateTime dt = DateTime.Now;  // format
            String s = dt.ToString("yyyyMMMdd");

            String filename;
            String machinename = System.Environment.MachineName;

            filename = machinename + "_" + s + "_" + count.ToString() + ".csv";
            while (File.Exists(datadir + "\\" + filename))
            {
                count++;
                filename = machinename + "_" + s + "_" + count.ToString() + ".csv";
            }
            return filename;
        }

        public void writeInterval(int it)
        {
            interval = it;

            intervaltime = DateTime.Now;
            datetime_formatted = intervaltime.ToString("dd-MMM-yyyy hh:mm");

//           Console.WriteLine("write interval {0}", it.ToString());
  //          m_streamWriter.WriteLine("Interval");
            return;
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
            fs.Flush();
        }

        public void writeProcess(Process p)
        {
            try
            {
                string outstr = datetime_formatted + "," + p.name + "," + p.pid + "," + p.commandline;
               m_streamWriter.WriteLine(outstr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.Message);
            }
        }

        public void close()
        {
            if (m_streamWriter != null)
                m_streamWriter.Close();
            m_streamWriter = null;

            if (fs != null)
                fs.Close();
            fs = null;
        }
    }
}
