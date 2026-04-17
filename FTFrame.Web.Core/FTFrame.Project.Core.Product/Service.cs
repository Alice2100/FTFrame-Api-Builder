using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FTFrame.Tool;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FTFrame.Project.Core.Service
{
    /// <summary>
    /// Backend scheduled tasks,Config int job.json
    /// </summary>
    public class JobService : BackgroundService
    {
        private List<JobConfig> jobConfig = null;
        private Dictionary<string, DateTime> jobLast = new Dictionary<string, DateTime>();
        private bool jobRunning = false;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // log.Debug("JobService Timer");
                    if (jobRunning)
                    {
                        log.Debug("Job is Running");
                    }
                    else
                    {
                        if (jobConfig == null)
                        {
                            log.Debug("Build JobConfig");
                            var filename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "job.json";
                            log.Debug("filename:" + filename);
                            jobConfig = new List<JobConfig>();
                            if (File.Exists(filename))
                            {
                                log.Debug("File Exists");
                                string json = null;
                                using (StreamReader sr = new StreamReader(filename))
                                {
                                    json = sr.ReadToEnd();
                                }
                                if (!string.IsNullOrWhiteSpace(json))
                                {
                                    var jList = JArray.Parse(json);
                                    foreach (var jt in jList)
                                    {
                                        if (jt.Type.ToString() == "Object")
                                        {
                                            var jo = jt as JObject;
                                            JobConfig jConfig = new JobConfig();
                                            jConfig.Interval = jo["interval"].ToString();
                                            jConfig.Trigger = jo["trigger"].ToString();
                                            jConfig.Jobs = new List<JobDetail>();
                                            JArray jobs = jo["job"] as JArray;
                                            foreach (JObject job in jobs)
                                            {
                                                jConfig.Jobs.Add(new JobDetail()
                                                {
                                                    Type = job["type"].ToString(),
                                                    Value = job["value"].ToString()
                                                });
                                            }
                                            jobConfig.Add(jConfig);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                log.Debug("File Not Exists");
                            }
                        }
                        foreach (var config in jobConfig)
                        {
                            var dt = DateTime.Now;
                            var key = config.Interval + "|" + config.Trigger;
                            switch (config.Interval)
                            {
                                case "hour":
                                    if (!jobLast.ContainsKey(key))
                                    {
                                        if (dt > DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + config.Trigger)) jobLast.Add(key, DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + config.Trigger));
                                        else jobLast.Add(key, DateTime.Parse(dt.AddHours(-1).Year + "-" + dt.AddHours(-1).Month + "-" + dt.AddHours(-1).Day + " " + dt.AddHours(-1).Hour + ":" + config.Trigger));
                                    }
                                    if (dt >= DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + config.Trigger) && jobLast[key].ToString("yyyy-MM-dd HH") != dt.ToString("yyyy-MM-dd HH"))
                                    {
                                        jobLast[key] = dt;
                                        jobRunning = true;
                                        JobRun(config.Jobs);
                                    }
                                    break;
                                case "day":
                                    if (!jobLast.ContainsKey(key))
                                    {
                                        if (dt > DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + config.Trigger)) jobLast.Add(key, DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + config.Trigger));
                                        else jobLast.Add(key, DateTime.Parse(dt.AddDays(-1).Year + "-" + dt.AddDays(-1).Month + "-" + dt.AddDays(-1).Day + " " + config.Trigger));
                                    }
                                    if (dt >= DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + config.Trigger) && jobLast[key].ToString("yyyy-MM-dd") != dt.ToString("yyyy-MM-dd"))
                                    {
                                        jobLast[key] = dt;
                                        jobRunning = true;
                                        JobRun(config.Jobs);
                                    }
                                    break;
                                case "month":
                                    if (!jobLast.ContainsKey(key))
                                    {
                                        if (dt > DateTime.Parse(dt.Year + "-" + dt.Month + "-" + config.Trigger)) jobLast.Add(key, DateTime.Parse(dt.Year + "-" + dt.Month + "-" + config.Trigger));
                                        else jobLast.Add(key, DateTime.Parse(dt.AddMonths(-1).Year + "-" + dt.AddMonths(-1).Month + "-" + config.Trigger));
                                    }
                                    if (dt >= DateTime.Parse(dt.Year + "-" + dt.Month + "-" + config.Trigger) && jobLast[key].ToString("yyyy-MM") != dt.ToString("yyyy-MM"))
                                    {
                                        jobLast[key] = dt;
                                        jobRunning = true;
                                        JobRun(config.Jobs);
                                    }
                                    break;
                            }
                        }
                    }
                    jobRunning = false;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    jobRunning = false;
                }
                await Task.Delay(60000, stoppingToken);//Wait for 60 seconds
            }
        }
        private void JobRun(List<JobDetail> jobs)
        {
            foreach (var job in jobs)
            {
                if (job.Type == "WebApi")
                {
                    log.Debug("WebApi " + job.Value, "JobRun");
                    Utils.Net.HttpPost(job.Value, "{}", "application/json;charset=utf-8", "token", str.GetEncode(str.GetDateTime()));
                    log.Debug("WebApi End", "JobRun");
                }
                else if (job.Type == "Code")
                {
                    log.Debug("Code " + job.Value + " Start", "JobRun");
                    if (job.Value.StartsWith("@code("))
                    {
                        Code.Get(job.Value, null, null);
                    }
                    log.Debug("Code " + job.Value + " End", "JobRun");
                }
                else if (job.Type == "Function")
                {
                    log.Debug("Function " + job.Value, "JobRun");
                    var data = job.Value.Split(',');
                    var assembly = Assembly.Load(data[0]);
                    Type type = assembly.GetType(data[1]);
                    object instance = assembly.CreateInstance(data[1]);
                    type.GetMethod(data[2]).Invoke(instance, null);
                    log.Debug("Function End", "JobRun");
                }
            }
        }
        private class JobConfig
        {
            public string Interval { get; set; }
            public string Trigger { get; set; }
            public List<JobDetail> Jobs { get; set; }
        }
        private class JobDetail
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
