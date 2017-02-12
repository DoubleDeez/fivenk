using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkServer.Constant;

namespace fivenk_rp
{
    class JobData
    {
        public enum Id
        {
            // Only add to the END of the list
            None = -1,
            TheOfficials_MeterMaid,
            TheOfficials_MallCop,
            TheOfficials_SecurityGuard,
            TheOfficials_TrafficCop,
            TheHicks_MethCook,
            TheThugs_Pusher,
            TheTriad_BlueLantern,
        }

        private static readonly Dictionary<Id, Job> JobIdMap = new Dictionary<Id, Job>
        {
            { Id.None, null },
            { Id.TheOfficials_MeterMaid, GetMeterMaid() },
            { Id.TheOfficials_MallCop, GetMallCop() },
            { Id.TheOfficials_SecurityGuard, null },
            { Id.TheOfficials_TrafficCop, null },
            { Id.TheHicks_MethCook, GetMethCook() },
            { Id.TheThugs_Pusher, GetPusher() },
            { Id.TheTriad_BlueLantern, GetBlueLantern() },
        };

        public static Id GetFirstJobIdForGroup(Group.Type GroupType)
        {
            switch (GroupType)
            {
                default:
                    return Id.None;
                case Group.Type.TheOfficials:
                    return Id.TheOfficials_MeterMaid;
                case Group.Type.TheHicks:
                    return Id.TheHicks_MethCook;
                case Group.Type.TheThugs:
                    return Id.TheThugs_Pusher;
                case Group.Type.TheTriad:
                    return Id.TheTriad_BlueLantern;
            }
        }

        public static Job GetJob(Id JobId)
        {
            if(!JobIdMap.ContainsKey(JobId))
            {
                return null;
            }
            return JobIdMap[JobId];
        }

        public static string GetJobTitle(Id JobId)
        {
            Job job = GetJob(JobId);
            if (job == null) return "None";
            return job.JobTitle;
        }

        private static Job MeterMaid;
        public static Job GetMeterMaid()
        {
            if (MeterMaid != null) return MeterMaid;
            MeterMaid = new Job
            {
                Id = Id.TheOfficials_MeterMaid,
                JobTitle = "Meter Maid",
                JobDescription = "",
                BaseSalary = 200,
                JobGroup = Group.Type.TheOfficials,
                NextJobs = new Id[] { Id.TheOfficials_MallCop },
                ExpToLevel = 100,
            };
            return MeterMaid;
        }

        private static Job MallCop;
        public static Job GetMallCop()
        {
            if (MallCop != null) return MallCop;
            MallCop = new Job
            {
                Id = Id.TheOfficials_MallCop,
                JobTitle = "Mall Cop",
                JobDescription = "",
                BaseSalary = 400,
                JobGroup = Group.Type.TheOfficials,
                NextJobs = new Id[] { Id.TheOfficials_SecurityGuard },
                ExpToLevel = 300,
            };
            return MallCop;
        }

        private static Job MethCook;
        public static Job GetMethCook()
        {
            if (MethCook != null) return MethCook;
            MethCook = new Job
            {
                Id = Id.TheHicks_MethCook,
                JobTitle = "Meth Cook",
                JobDescription = "",
                BaseSalary = 200,
                JobGroup = Group.Type.TheHicks,
                NextJobs = new Id[] { Id.None },
                ExpToLevel = 100,
            };
            return MethCook;
        }

        private static Job Pusher;
        public static Job GetPusher()
        {
            if (Pusher != null) return Pusher;
            Pusher = new Job
            {
                Id = Id.TheThugs_Pusher,
                JobTitle = "Pusher",
                JobDescription = "",
                BaseSalary = 200,
                JobGroup = Group.Type.TheThugs,
                NextJobs = new Id[] { Id.None },
                ExpToLevel = 100,
            };
            return Pusher;
        }

        private static Job BlueLantern;
        public static Job GetBlueLantern()
        {
            if (BlueLantern != null) return BlueLantern;
            BlueLantern = new Job
            {
                Id = Id.TheTriad_BlueLantern,
                JobTitle = "Blue Lantern",
                JobDescription = "",
                BaseSalary = 200,
                JobGroup = Group.Type.TheTriad,
                NextJobs = new Id[] { Id.None },
                ExpToLevel = 100,
            };
            return BlueLantern;
        }
    }
}
