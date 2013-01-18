using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollectProcess
{
    class Process
    {
        public String owner;
        public String domain;
        public String user;
        public String name;
        public String commandline;
        public long pid;
        public DateTime creation_date;
        public bool alive;

        public Process(long id, string name, string cmdline, string owner, DateTime dt, string domain, string user)
        {
            this.pid = id;
            this.owner = owner;
            this.domain = domain;
            this.user = user;
            this.name = name;
            this.commandline = cmdline;
            alive = true;
            this.creation_date = dt;
        }
        public void print()
        {
            Console.WriteLine("{0},{1},{2},{3} ", this.pid, this.name, this.commandline, this.owner);
            return;
        }
    }
}
