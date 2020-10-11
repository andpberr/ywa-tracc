using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;


namespace Google.Apis.YouTube.Samples
{
    /// <summary>
    /// YouTube Data API v3 sample: search by keyword.
    /// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
    /// See https://developers.google.com/api-client-library/dotnet/get_started
    ///
    /// Set ApiKey to the API key value from the APIs & auth > Registered apps tab of
    ///   https://cloud.google.com/console
    /// Please ensure that you have enabled the YouTube Data API for your project.
    /// </summary>
    public static class YouTubeCaller
    {
        public static void FetchVideos(ywa_tracc.Data.ApplicationDbContext context, Boolean fetchAll=false)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = AWSSecrets.Helpers.GetYTAPIKey(),
                ApplicationName = "Search"
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.ChannelId = "UCFKE7WVJfvaHW5q283SxchA"; // YWA channel ID
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            searchListRequest.MaxResults = 50;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = searchListRequest.Execute();

            List<ywa_tracc.Models.Video> vids = new List<ywa_tracc.Models.Video>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.

            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        var v = new ywa_tracc.Models.Video()
                        {
                            ID = searchResult.Id.VideoId,
                            Name = searchResult.Snippet.Title,
                            ThumbnailURL = searchResult.Snippet.Thumbnails.High.Url,
                            PostedDate = Convert.ToDateTime(searchResult.Snippet.PublishedAt)
                        };
                        var existentVid = (from vid in context.Video
                                           where vid.ID == v.ID
                                           select vid).FirstOrDefault();
                        if (existentVid == null)
                        {
                            context.Add(v);
                        }
                        break;
                }
            }

            if (fetchAll)
            {
                while (searchListResponse.NextPageToken != null)
                            {
                                searchListRequest = youtubeService.Search.List("snippet");
                                searchListRequest.ChannelId = "UCFKE7WVJfvaHW5q283SxchA"; // YWA channel ID
                                searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
                                searchListRequest.PageToken = searchListResponse.NextPageToken;
                                searchListRequest.MaxResults = 50;

                                // Call the search.list method to retrieve results matching the specified query term.
                                searchListResponse = searchListRequest.Execute();

                                vids = new List<ywa_tracc.Models.Video>();

                                // Add each result to the appropriate list, and then display the lists of
                                // matching videos, channels, and playlists.

                                foreach (var searchResult in searchListResponse.Items)
                                {
                                    switch (searchResult.Id.Kind)
                                    {
                                        case "youtube#video":
                                            var v = new ywa_tracc.Models.Video()
                                            {
                                                ID = searchResult.Id.VideoId,
                                                Name = searchResult.Snippet.Title,
                                                ThumbnailURL = searchResult.Snippet.Thumbnails.High.Url,
                                                PostedDate = Convert.ToDateTime(searchResult.Snippet.PublishedAt)
                                            };
                                            var existentVid = (from vid in context.Video
                                                               where vid.ID == v.ID
                                                               select vid).FirstOrDefault();
                                            if (existentVid == null)
                                            {
                                                context.Add(v);
                                            }
                                            break;
                                    }
                                }

                            }
            }

            context.SaveChanges();
        }


        public static void FillDurationIfNotExists(ywa_tracc.Data.ApplicationDbContext context, string videoId)
        {
            var video = (from vid in context.Video
                         where vid.ID == videoId
                         select vid).FirstOrDefault();
            
            if (video.Duration == 0)
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = AWSSecrets.Helpers.GetYTAPIKey(),
                    ApplicationName = "Video"
                });

                var videoListRequest = youtubeService.Videos.List("ContentDetails");
                videoListRequest.Id = videoId; 

                // Call the video.list method to retrieve result matching the specified query term.
                var videoListResponse = videoListRequest.Execute();

                string durationISO8601 = videoListResponse.Items.FirstOrDefault().ContentDetails.Duration;

                // Google API uses ISO 8601 for durations
                // Format is PT<num_minutes>M<num_seconds>S
                // The substring/indexOf calculation extracts the number of minutes and parses it into an int
                int duration = int.Parse(durationISO8601.Substring(2, durationISO8601.IndexOf("M") - 2));

                video.Duration = duration;
                context.SaveChanges();
            }
        }
    }
}
