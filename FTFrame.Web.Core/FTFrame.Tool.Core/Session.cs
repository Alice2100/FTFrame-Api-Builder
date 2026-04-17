using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FTFrame;
using System.Web;
using System.Collections;
using System.Security;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FTFrame.Tool
{
    public class session
    {
        public static void SetString(string key, string val)
        {
            SessionExtensions.SetString(FTFrame.FTHttpContext.Current.Session, key, val);
        }
        public static void SetString(string key, string val, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            Context.Session.SetString(key, val);
        }
        public static string GetString(string key)
        {
            return SessionExtensions.GetString(FTFrame.FTHttpContext.Current.Session, key);
        }
        public static string GetString(string key, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            return Context.Session.GetString(key);
        }
        public static void SetInt32(string key, int val)
        {
            SessionExtensions.SetInt32(FTFrame.FTHttpContext.Current.Session, key, val);
        }
        public static void SetInt32(string key, int val, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            Context.Session.SetInt32(key, val);
        }
        public static int? GetInt32(string key)
        {
            return SessionExtensions.GetInt32(FTFrame.FTHttpContext.Current.Session, key);
        }
        public static int? GetInt32(string key, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            return Context.Session.GetInt32(key);
        }
        public static void Set(string key, byte[] val)
        {
            FTFrame.FTHttpContext.Current.Session.Set(key, val);
        }
        public static void Set(string key, byte[] val, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            Context.Session.Set(key, val);
        }
        public static byte[] Get(string key)
        {
            return FTFrame.FTHttpContext.Current.Session.Get(key);
        }
        public static byte[] Get(string key, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            return Context.Session.Get(key);
        }
        public static void Set<T>(string key, T value)
        {
            FTFrame.FTHttpContext.Current.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(string key)
        {
            var value = FTFrame.FTHttpContext.Current.Session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
        public static void Set<T>(string key, T value, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            Context.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(string key, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            var value = Context.Session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
        public static void Clear()
        {
            FTFrame.FTHttpContext.Current.Session.Clear();
        }
        public static void Clear(Microsoft.AspNetCore.Http.HttpContext Context)
        {
            Context.Session.Clear();
        }
        public static void Remove(string key)
        {
            FTFrame.FTHttpContext.Current.Session.Remove(key);
        }
        public static void Remove(string key, Microsoft.AspNetCore.Http.HttpContext Context)
        {
            Context.Session.Remove(key);
        }
        public static IEnumerable<string> Keys()
        {
            return FTFrame.FTHttpContext.Current.Session.Keys;
        }
        public static IEnumerable<string> Keys(Microsoft.AspNetCore.Http.HttpContext Context)
        {
            return Context.Session.Keys;
        }
    }
}
