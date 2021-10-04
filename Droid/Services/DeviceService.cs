using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Core;
using Firebase.Iid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Droid
{
    public class DeviceService : IDeviceService
    {
        public string DeviceID => GetDeviceId().Result;

        async Task<string> GetDeviceId()
        {
            var instanceIdResult = await FirebaseInstanceId.Instance.GetInstanceId().AsAsync<IInstanceIdResult>();
            var token = instanceIdResult.Token.ToString();
            return token;
        }
    }
}