﻿using BattleTech;
using BattleTech.Data;
using BattleTech.Framework;
using Harmony;
using HBS.Collections;
using Newtonsoft.Json;
using PersistentMapAPI;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace PersistentMapClient {

    public class SaveFields {

    }

    public class Helper {
        public static Settings LoadSettings() {
            try {
                using (StreamReader r = new StreamReader($"{ PersistentMapClient.ModDirectory}/settings.json")) {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            catch (Exception ex) {
                Logger.LogError(ex);
                return null;
            }
        }

        public static StarMap GetStarMap() {
            try {
                string URL = Fields.settings.ServerURL + "warServices/StarMap";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "GET";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StarMap map;
                using (Stream responseStream = response.GetResponseStream()) {
                    StreamReader reader = new StreamReader(responseStream);
                    string mapstring = reader.ReadToEnd();
                    map = JsonConvert.DeserializeObject<StarMap>(mapstring);
                }
                return map;
            }
            catch (Exception e) {
                Logger.LogError(e);
                return null;
            }
        }

        public static void PostMissionResult(PersistentMapAPI.MissionResult mresult) {
            try {
                string URL = Fields.settings.ServerURL + "warServices/Mission";
                string result;
                using (var client = new WebClient()) {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    result = client.UploadString(URL, "POST", JsonConvert.SerializeObject(mresult));
                }
                Logger.LogLine(result);
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }
}
