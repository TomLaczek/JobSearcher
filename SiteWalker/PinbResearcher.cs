using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace SiteWalker
{
    public class PinbResearcher : Researcher
    {
        public PinbResearcher(List<string> urls, List<string> html, DateTime lastWeek) : base(urls, html, lastWeek)
        {
        }

        public override bool CheckPosts()
        {
            if (Htmls.Count.Equals(0))
            {
                throw new Exception("PINB Error, no any site contents is empty");
            }

            HtmlNodeCollection pinb;
            HtmlNode postDateNode;
            DateTime postDate;
            foreach (var item in Htmls)
            {
                HtmlDoc.LoadHtml(item);
                pinb = HtmlDoc.DocumentNode.SelectNodes("//div[@class='dzial-tresc']/ol/h3");
                postDate = new DateTime();
                
                for (int i = 0; i < pinb.Count; i++)
                {
                    if (pinb[i].InnerText.ToLower().Contains("ogłoszenie o naborze do") ||
                        pinb[i].InnerText.ToLower().Contains("powiatowy inspektorat nadzoru budowlanego") ||
                        pinb[i].InnerText.ToLower().Contains("nabór na stanowisko - ") ||
                        pinb[i].InnerText.ToLower().Contains("ogłoszenie o") ||
                        pinb[i].InnerText.ToLower().Contains("nabór")
                    )
                    {
                        postDateNode = pinb[i].ParentNode.SelectNodes($"//div[@class='dzial-tresc']/ol/h3[{i + 1}]/following::h5").First();
                        Regex rx;
                        MatchCollection matches;
                        try
                        {
                            foreach (var pattern in DatesDict)
                            {
                                rx = new Regex(pattern.Value);
                                matches = rx.Matches(postDateNode.InnerText);
                                if (matches.Count > 0)
                                {
                                    DateTime.TryParse(matches[0].Value, out postDate);
                                    continue;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Missing date in Pinb Post", ex);
                        }

                        if (postDate > LastWeek)
                        {
                            Console.WriteLine("New post during last week found on PINB site");
                            return true;
                        }
                    }

                }
            }
            return false;
        }
    }
}