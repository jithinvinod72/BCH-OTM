using BCMCH.OTM.API.Shared.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMCH.OTM.External
{
    public interface IOTMDataClient
    {
        Task<T> GetAsync<T>(string uri);
        Task<T> PostAsync<T>(string relativeUrl, object payload);
        Task<Authentication> AuthenticateUser(string token);


    }
}
