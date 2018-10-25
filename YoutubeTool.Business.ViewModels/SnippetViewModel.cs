using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeTool.Bases;

namespace YoutubeTool.Business.ViewModels
{
    public class SnippetViewModel
    {
        public string id { set; get; }
        public string title { set; get; }
        public YoutubeTool.Bases.Enums.YoutubeSnippetKind kind { set; get; }
    }
}
