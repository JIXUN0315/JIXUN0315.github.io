using StackExchange.Redis;

namespace AdminManagement.Models.ViewModels
{
    public class LineCreateViewModel
    {
        /// <summary>
        /// 通知發送時間
        /// </summary>
        public string? SendTime { get; set; }
        public string? TimePick { get; set; }
        /// <summary>
        /// 訊息內文(手風琴拉開後的顯示)
        /// </summary>
        public string? Text { get; set; }
        public string? Link { get; set; }
        public string? Img { get; set; }


    }
    
    public class Message
    {
        public List<MessageObj> Messages { get; set; }

        public class MessageObj
        {
            public string Type { get; set; }
            public string AltText { get; set; }
            public Template Template { get; set; }

            
        }

        public class Template
        {
            public string Type { get; set; }
            public string Text { get; set; }
            public string ThumbnailImageUrl { get; set; }
            public string ImageAspectRatio { get; set; }
            public string ImageSize { get; set; }
            public string ImageBackgroundColor { get; set; }
            public List<ActionObj> Actions { get; set; }

            public class ActionObj
            {
                public string Type { get; set; }
                public string Label { get; set; }
                public string Uri { get; set; }
            }
        }

        
    }

   

    
}
