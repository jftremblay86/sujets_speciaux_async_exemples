﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _01_web_calls
{
    public static class DemoMethods
    {
        private static List<string> PrepData()
        {
            var output = new List<string>();

            output.Add("https://www.lapresse.ca");
            output.Add("https://www.reddit.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.amazon.ca");
            output.Add("https://www.stackoverflow.com");
            output.Add("https://www.facebook.com");
            output.Add("https://www.google.ca");
            output.Add("https://www.hotmail.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://fr.wikipedia.org");

            return output;
        }

        public static List<WebsiteDataModel> RunDownloadSync()
        {
            var websites = PrepData();
            var output = new List<WebsiteDataModel>();

            foreach (string site in websites)
            {
                WebsiteDataModel ws = DownloadWebsite(site);
                output.Add(ws);
            }

            return output;
        }

        public static async Task<List<WebsiteDataModel>> RunDownloadAsync(IProgress<ProgressReportModel> progress)
        {
            var websites = PrepData();
            var output = new List<WebsiteDataModel>(); // Liste des sites à retourner
            var report = new ProgressReportModel(); // Rapport de progression

            foreach (string site in websites)
            {
                WebsiteDataModel ws = await DownloadWebsiteAsync(site);
                output.Add(ws); // On ajoute le site terminé à la liste

                report.SitesDownloaded = output; // On assigne la liste au rapport
                report.PercentageComplete = (output.Count * 100) / websites.Count; // Calcul du %

                progress.Report(report); // On déclenche l'événement de rapportage
            }
            return output;
        }

        public static async Task<List<WebsiteDataModel>> RunDownloadParallelAsync()
        {
            var websites = PrepData();
            var tasks = new List<Task<WebsiteDataModel>>();

            foreach (string site in websites)
            {
                tasks.Add(DownloadWebsiteAsync(site));
            }

            var results = await Task.WhenAll(tasks);

            return new List<WebsiteDataModel>(results);
        }

        private static WebsiteDataModel DownloadWebsite(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();
            output.WebsiteUrl = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);

            return output;
        }

        private static async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

            return output;
        }

        
    }
}
