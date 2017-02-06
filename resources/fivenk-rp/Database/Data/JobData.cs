﻿using System;
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
        }

        public static Id GetFirstJobIdForGroup(Group.Type GroupType)
        {
            switch (GroupType)
            {
                default:
                case Group.Type.TheHicks:
                case Group.Type.TheThugs:
                case Group.Type.TheTriad:
                    return Id.None;
                case Group.Type.TheOfficials:
                    return Id.TheOfficials_MeterMaid;
            }
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
    }
}
