﻿using Covid19Radar.Common;
using Covid19Radar.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Covid19Radar.Services
{
    public class UserDataService
    {
        private readonly HttpDataService httpDataService;

        public UserDataService()
        {
            this.httpDataService = Xamarin.Forms.DependencyService.Resolve<HttpDataService>();
        }

        public async Task<bool> IsExistUserDataAsync()
        {
            if (Application.Current.Properties.ContainsKey("UserData"))
            {
                var userDataJson = Application.Current.Properties["UserData"].ToString();

                UserDataModel userData = Utils.DeserializeFromJson<UserDataModel>(userDataJson);
                var state = await httpDataService.PutUserAsync(userData);

                if (state != null)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<UserDataModel> RegistUserAsync()
        {
            UserDataModel userData = await httpDataService.PostRegisterUserAsync();
            Application.Current.Properties["UserData"] = Utils.SerializeToJson(userData);
            await Application.Current.SavePropertiesAsync();
            return userData;
        }

        public UserDataModel Get()
        {
            if (Application.Current.Properties.ContainsKey("UserData"))
            {
                return Utils.DeserializeFromJson<UserDataModel>(Application.Current.Properties["UserData"].ToString());
            }
            return null;
        }

        public async void SetAsync(UserDataModel userData)
        {
            Application.Current.Properties["UserData"] = Utils.SerializeToJson(userData);
            await Application.Current.SavePropertiesAsync();
        }
    }
}
