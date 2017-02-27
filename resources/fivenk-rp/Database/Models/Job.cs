using System;
using GTANetworkServer.Constant;

namespace fivenk_rp
{
    class Job
    {
        public JobData.Id Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public int BaseSalary { get; set; }
        public Group.Type JobGroup { get; set; }
        public JobData.Id[] NextJobs { get; set; }
        public int ExpToLevel { get; set; }
    }
}
