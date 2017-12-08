using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redgum.ServerMonitor.Web.ServerMonitor.Data;
using Redgum.ServerMonitor.Web.ServerMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redgum.ServerMonitor.Web.Tests
{
    [TestClass]
    public class ConvertServerInfo
    {
        [TestMethod]
        public void CreateJSONData()
        {
            var settings = new SystemSettings()
            {
                MonitoredServices = new List<string>() { "hello" }
            };

            var info = new ServerInfo()
            {
                DiskInfo = new List<DiskInfo>()
                 {
                      new DiskInfo(){ FreeSpaceInGB=10, PercentFree=44, Name="blaa", SizeInGB=33, VolumeName="qqqqqq" },
                      new DiskInfo(){ FreeSpaceInGB=100, PercentFree=34, Name="zzzz", SizeInGB=33, VolumeName="aaaaaaaaaaaa" },
                 }
            };

            var r = info.ToServerData(settings).ToList();

            Assert.IsTrue(r.First().Data.Contains("VolumeName"));
            Assert.IsTrue(r.First().Data.Contains("qqqqqq"));
            Assert.IsTrue(r.First().Data.Contains("SizeInGB"));
            Assert.IsTrue(r.First().Data.Contains("FreeSpaceInGB"));
            Assert.IsTrue(r.First().Data.Contains("33.0"));
            Assert.IsTrue(r.First().Data.Contains("33.0"));

        }

        [TestMethod]
        public void CreateAggregateData()
        {
            var nowUtc = new DateTime(2000, 1, 1);

            var settings = new SystemSettings()
            {
                MonitoredServices = new List<string>() { "hello" }
            };

            var info = new ServerInfo()
            {
                DiskInfo = new List<DiskInfo>()
                 {
                      new DiskInfo(){ FreeSpaceInGB=10, PercentFree=4, Name="blaa", SizeInGB=33, VolumeName="qqqqqq" },
                      new DiskInfo(){ FreeSpaceInGB=100, PercentFree=34, Name="zzzz", SizeInGB=33, VolumeName="aaaaaaaaaaaa" },
                 }
            };

            var r = info.ToServerAggregateData(settings, nowUtc).ToList();

            Assert.IsTrue(r.First().Data == "Warning");

        }
    }

}