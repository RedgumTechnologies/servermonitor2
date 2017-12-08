//using Redgum.ServerMonitor.Web.ServerMonitor.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Redgum.ServerMonitor.Web.ServerMonitor.Data
//{
//    public static class ServerQueries
//    {
//        //public IQueryable<ServerDataModel> GetByName(this IQueryable<ServerDataModel> servers,string serverName)
//        //{
//        //    return servers.Where()
//        //}


//        public static IEnumerable<ServerDataDataModel> ToDataModels(this IEnumerable<RawData> data)
//        {
//            return data.Select(d => new ServerDataDataModel()
//            {
//                 Identifier=Guid.NewGuid(),
//                 DataKey=d.DataKey,
//                 Data=d.Data
//            });
//        }

//    }
//}
