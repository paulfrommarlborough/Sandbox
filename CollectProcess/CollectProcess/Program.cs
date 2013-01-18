using System;
using System.Collections.Generic;

using System.Text;
using System.Management;
using System.Xml;
using System.Threading;


namespace CollectProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start_dt = DateTime.Today;                     
            int intervalcount = 0;

            Collector c = new Collector();

            try
            {

                while (true)
                {
                    DateTime intvl_dt = DateTime.Today;

                    if (start_dt != intvl_dt)
                    {
                        Console.WriteLine("Rollover doc");
                        c.RolloverXmlDoc();
                        c.RolloverCsvDoc();
                        start_dt = intvl_dt;
                    }

                    // for every minute, sample every 5 seconds

                    Console.WriteLine("new interval... ");

                    while (intervalcount < 12)
                    {
                        Console.WriteLine("sample... ");
                        c.getProcessData();

                        ++intervalcount;

                        Thread.Sleep(5000);
                    }
                    Console.WriteLine("dump processes for interval");
                    c.removeProcessesUnseen();

                    c.dumpProcessData(); // write to tty
                    c.writeIntervalCsv();
                    c.writeProcessDataCsv();
                    
                    c.writeIntervalXml();
                    c.writeProcessDataXml();
                    c.WriteIntervalXmlEnd();

                    c.markProcessesUnseen();
                    intervalcount = 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Concat(e.Message, e.StackTrace));
            }

            finally
            {
                Console.WriteLine("Finally ");
                c.CloseXml();
            }
        }   // main
    }
}   // class program



     //       dxl.writeInterval(++intervalcount, processlist);
     //       dxl.CloseDoc();
    