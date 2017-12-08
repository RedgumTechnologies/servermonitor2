using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Data
{
    public static class Extensions
    {
        public static Status ToStatus(this string aggregateData)
        {
            if (string.IsNullOrEmpty(aggregateData)) return Status.Unknown;
            return (Status)Enum.Parse(typeof(Status), aggregateData);
        }

        public static int ToNumber(this string aggregateData)
        {
            int.TryParse(aggregateData, out int number);
            return number;
        }

    }
}
