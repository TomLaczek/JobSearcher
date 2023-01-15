using SiteWalker;

namespace JobSearcher.App
{
    public class Crawler
    {
        private readonly ConfigurationHelper _configParams;
        private bool BipResult = false;
        private bool PinbResult = false;
        private readonly DateTime LastWeek;
        private readonly List<string> BipContents;
        private readonly List<string> PinbContents;

        public Crawler(ConfigurationHelper configParams)
        {
            _configParams = configParams;
            LastWeek = DateTime.Now.AddDays(-7);
            BipContents = new List<string>();
            PinbContents = new List<string>();
        }

        public async Task CrawlThroughSites()
        {
            #region Bip
            var bip = new BipResearcher(_configParams.BipUrls, BipContents, LastWeek);
            await bip.GetContents();
            BipResult = bip.CheckPosts();
            if (BipResult)
            {
                Console.WriteLine("New job offer match on Bip site.");
            }
            #endregion


            #region PINB
            var pinb = new PinbResearcher(_configParams.PinbUrls, PinbContents, LastWeek);
            await pinb.GetContents();
            PinbResult = pinb.CheckPosts();
            if (PinbResult)
            {
                Console.WriteLine("New job offer match on PINB site.");
            }
            #endregion
        }
    }
}