using System.Collections.Generic;
using PwC.C4.Dfs.Common.Model.Enums;

namespace PwC.C4.Dfs.Common.Model
{
    public static class Const
    {
        public static Dictionary<string, VideoSize> VideoSizeDicByName = new Dictionary<string, VideoSize>()
        {
            {"Small", VideoSize.Small},
            {"Large", VideoSize.Large}
        };

        public static Dictionary<string, ImageSize> ImageSizeDicByName = new Dictionary<string, ImageSize>()
        {
            {"Small",ImageSize.Small},
            {"Middle",ImageSize.Middle },
            {"Large",ImageSize.Large }
        };

        public static Dictionary<VideoSize, string> VideoSizeByEnum = new Dictionary<VideoSize, string>()
        {
            { VideoSize.Small,"s"},
            {VideoSize.Large, "l" }
        };

        public static Dictionary<ImageSize,string> ImageSizeByEnum = new Dictionary<ImageSize, string>()
        {
            { ImageSize.Small,"s"},
            {ImageSize.Middle, "m" },
            {ImageSize.Large, "l" }
        };

        
    }
}
