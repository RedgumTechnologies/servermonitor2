using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Redgum.ServerMonitor.Web.ServerMonitor.Data.TestData
{
    static class Extensions
    {

        public static string GetTestData(string fname)
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"Redgum.ServerMonitor.Web.ServerMonitor.Data.TestData.{fname}");

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }


        public static IEnumerable<string> GetLines(this string s)
        {
            return s.Split('\n').Where(ss => !string.IsNullOrEmpty(ss.Trim()));
        }

        public static string GetTabIndexString(this string s, int i)
        {
            return s.Split('\t')[i].Trim();
        }

        public static int GetTabIndexInt(this string s, int i)
        {
            return int.Parse(s.GetTabIndexString(i));
        }
        public static Guid GetTabIndexGuid(this string s, int i)
        {
            return Guid.Parse(s.GetTabIndexString(i));
        }
        public static DateTime GetTabIndexDate(this string s, int i)
        {
            return DateTime.Parse(s.GetTabIndexString(i));
        }


    }

    public class MonitorContextInitialiser
    {
        MonitorDbContext _ctx;
        public MonitorContextInitialiser(MonitorDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Initialize()
        {
            _ctx.Database.EnsureCreated();

            // Look for any existing data.
            if (_ctx.Servers.Any())
            {
                return;   // DB has been seeded
            }

            var testDataServers = Extensions.GetTestData("Servers.txt")
                .GetLines()
                .Select(s => new
                {
                    Id = s.GetTabIndexInt(0),
                    Server = new ServerDataModel()
                    {
                        //                    Id = s.GetTabIndexInt(0),
                        Identifier = s.GetTabIndexGuid(1),
                        Name = s.GetTabIndexString(2),
                        Domain = s.GetTabIndexString(3),
                        UpdatedUtc = s.GetTabIndexDate(4)
                    },
                }).ToList();

            var testServersById = testDataServers.ToDictionary(k => k.Id, v => v.Server);

            _ctx.Servers.AddRange(testDataServers.Select(s=>s.Server));

            _ctx.SaveChanges();

            _ctx.ServerData.AddRange(Extensions.GetTestData("ServerData.txt")
                .GetLines()
                .Select(s => new ServerDataDataModel()
                {
                    //Id = s.GetTabIndexInt(0),
                    Identifier = s.GetTabIndexGuid(1),
                    //Server = _ctx.Servers.Single(ss => ss.Id == s.GetTabIndexInt(2)),
                    DataKey = s.GetTabIndexString(3),
                    Data = s.GetTabIndexString(4),
                    Server = testServersById[s.GetTabIndexInt(2)],
                })
                .ToList());

            _ctx.ServerDataAgregates.AddRange(Extensions.GetTestData("ServerDataAgregate.txt")
                .GetLines()
                .Select(s => new ServerDataAgregateDataModel()
                {
                    //Id = s.GetTabIndexInt(0),
                    Identifier = s.GetTabIndexGuid(1),
                    //Server = _ctx.Servers.Single(ss => ss.Id == s.GetTabIndexInt(2)),
                    DataKey = s.GetTabIndexString(3),
                    Data = s.GetTabIndexString(4),
                    Server = testServersById[s.GetTabIndexInt(2)],
                })
                .ToList());

            _ctx.Settings.AddRange(Extensions.GetTestData("Settings.txt")
                .GetLines()
                .Select(s => new SettingsDataModel()
                {
                    //Id = s.GetTabIndexInt(0),
                    Identifier = s.GetTabIndexGuid(1),
                    DataKey = s.GetTabIndexString(2),
                    Data = s.GetTabIndexString(3),
                })
                .ToList());

            _ctx.SaveChanges();

        }
    }
}

