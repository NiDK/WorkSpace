using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NReco.VideoConverter;

namespace PwC.C4.Dfs.Converter.Config
{
    public static class SettingHelper
    {
        public static ConvertSettings ToConvert(this VideoSetting videoSetting,string enableType)
        {
            var data = new ConvertSettings()
            {
                AppendSilentAudioStream = videoSetting.AppendSilentAudioStream,
                //AudioCodec = videoSetting.AudioCodec,
                //AudioSampleRate = videoSetting.AudioSampleRate,
                CustomOutputArgs = videoSetting.CustomOutputArgs,
                VideoCodec = videoSetting.VideoCodec,
                //VideoFrameCount = videoSetting.VideoFrameCount,
                VideoFrameSize = videoSetting.VideoFrameSize,
                CustomInputArgs = enableType
            };
            return data;
        }

        public static string ToConvert(this ImageSetting imageSetting,string enableType)
        {
            var n = new List<string>()
            {
                "mode=" + imageSetting.Mode,
                "width="+ imageSetting.Width,
                "height="+ imageSetting.Height,
                "quality="+imageSetting.Quality
            };
            return string.Join("&", n) + "$" + enableType;
        }
    }
}