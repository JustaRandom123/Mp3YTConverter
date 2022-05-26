using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace Mp3YTConverter
{
    internal class YouTube
    {


        public static YouTubeVideo getVideoInformations(string link)
        {
            var youTube = VideoLibrary.YouTube.Default; // starting point for YouTube actions
            var video = youTube.GetVideo(link); // gets a Video object with info about the video            
            return video;
        }    
        

        public static void AudioConvert(string fileToConvert,string fileToConvertTo,string fileFormat)
        {
            var convert = new NReco.VideoConverter.FFMpegConverter();
          //  convert.ConvertMedia(Application.StartupPath + "\\" + video.FullName, Application.StartupPath + "\\Converted\\" + video.FullName.Replace("mp4", "mp3"), "mp3");
            convert.ConvertMedia(fileToConvert, fileToConvertTo, fileFormat);
        }
     
    }
}
