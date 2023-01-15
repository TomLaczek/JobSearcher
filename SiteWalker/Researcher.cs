using Caller;
using HtmlAgilityPack;

namespace SiteWalker
{
    public abstract class Researcher
    {
        private readonly UrlCaller _caller;
        public List<string> Urls { get; set; }
        public List<string> Htmls { get; set; }
        public HtmlDocument HtmlDoc { get; set; }
        public DateTime LastWeek { get; set; }
        public Dictionary<string, string> DatesDict = new Dictionary<string, string>() {
            {".", @"\d{ 1,2}.\d{ 2}.\d{4}"},
            {"/", @"\d{1,2}\/\d{2}\/\d{4}"},
            {"-", @"\d{1,2}-\d{2}-\d{4}"} 
        };


        public Researcher(List<string> urls, List<string> htmls, DateTime lastWeek)
        {
            _caller = new UrlCaller();
            Urls = urls;
            Htmls = htmls;
            HtmlDoc = new HtmlDocument();
            LastWeek = lastWeek;
        }

        public async Task GetContents()
        {
            foreach (var item in Urls)
            {
                Htmls.Add(await _caller.GetResponseString(item));
            }
        }

        public abstract bool CheckPosts();
    }
}
