using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading;


// does collection  need a mutex to lock and a writer thread.

namespace CollectProcess
{
    class Collector
    {

        List<Process> ProcessList;
        IntervalXmlWriter dxl;
        IntervalCsvWriter csv;

        public Collector()
        {
            ProcessList = new List<Process>();
            dxl = new IntervalXmlWriter();
            csv = new IntervalCsvWriter();
        }

        public void getProcessData()
        {
                string wmiQuery = string.Format("select * from Win32_Process");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery);
                ManagementObjectCollection retObjectCollection = searcher.Get();
                         
                foreach (ManagementObject retObject in retObjectCollection)
                {
                    string[] argList = new string[] { string.Empty, string.Empty };
                    int returnVal = 0;
                    try
                    {
                        returnVal = Convert.ToInt32(retObject.InvokeMethod("GetOwner", argList));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception on getowner " + ex.ToString());
                    }
                    String sOwner = "";
                    if (returnVal == 0)
                        sOwner = argList[1] + "\\" + argList[0];

                    long pid = Convert.ToInt64(retObject["ProcessId"]);
                    String sName = Convert.ToString(retObject["Name"]);
                    String sCmd = Convert.ToString(retObject["CommandLine"]);
                
               //    DateTime dt = Convert.ToDateTime(retObject["CreationDate"]);
                    DateTime dt = DateTime.Now;
                    Process fproc = ProcessList.Find(
                                delegate(Process p)
                                {
                                    return p.pid == pid;
                                }
                    );
                    if (fproc == null)
                          ProcessList.Add(new Process(pid, sName, sCmd, sOwner, dt, argList[1], argList[0]));  // new
                    else fproc.alive = true;       // update process to seen this interval
                }                                  // foreach
                searcher = null;
          }
        public void dumpProcessData()
        {
            ProcessList.ForEach(Print);
       }
        public static void Print(Process s)
        {
                Console.WriteLine("Process =  {0} {1}  {2}" , s.pid,s.name,s.commandline);
        }
        public void removeProcessesUnseen()
        {
            ProcessList = ProcessList.Where(a => a.alive != false).ToList(); 

        }
        public void markProcessesUnseen()
        {
            ProcessList.ForEach(markunseen);
        }
        public static void markunseen(Process s)
        {
            s.alive = false;
        }

        //---------------------------------------- csv writer rtns

        public void writeIntervalCsv()
        {
            // get minute - 1 to 1440
            csv.writeInterval(0);
        }

        public void WriteCsv(Process p)
        {
            csv.writeProcess(p);
        }
        public void RolloverCsvDoc()
        {
            csv.close();
            csv = null;
            csv = new IntervalCsvWriter();
        }
        public void writeProcessDataCsv()
        {
            ProcessList.ForEach(WriteCsv);
        }

        //0
        //---------------------------------------- xml writer rtns
        public void writeProcessDataXml()
        {
            ProcessList.ForEach(WriteXml);
        }
        public void writeIntervalXml()
        {
            dxl.writeInterval(0);
        }
        public void WriteIntervalXmlEnd()
        {
            dxl.writeIntervalEnd();
        }
        public void CloseXml()
        {
            dxl.CloseDoc();
        }

        public  void WriteXml(Process p)
        {
            dxl.writeProcess(p);
        }
        public void RolloverXmlDoc()
        {
            dxl.CloseDoc();
            dxl = null;
            dxl = new IntervalXmlWriter();  
        }

    }
}
