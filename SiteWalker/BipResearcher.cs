using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiteWalker
{
    public class BipResearcher : Researcher
    {
        public BipResearcher(List<string> urls, List<string> html, DateTime lastWeek) : base(urls, html, lastWeek)
        {
        }

        public override bool CheckPosts() 
        {
            if (Htmls.Count.Equals(0))
            {
                throw new Exception("BIP Error, no any site contents is empty");
            }

            HtmlNodeCollection bip;
            HtmlNode postDateNode;
            DateTime postDate;
            foreach (var item in Htmls)
            {
                postDate = new DateTime();
                HtmlDoc.LoadHtml(item);
                var noOfferBip = HtmlDoc.DocumentNode.SelectSingleNode("//div[@id='content_page']/p[text()='Nie znaleziono zawartości.']");
                if(noOfferBip == null)
                {
                    continue;
                }
                bip = HtmlDoc.DocumentNode.SelectNodes("//div[@id='content_page']/div");

                for (int i = 0; i < bip.Count; i++)
                {
                    if (bip[i].InnerText.ToLower().Contains("ogłoszenie o naborze do") ||
                        bip[i].InnerText.ToLower().Contains("powiatowy inspektorat nadzoru budowlanego") ||
                        bip[i].InnerText.ToLower().Contains("nabór na stanowisko - ") ||
                        bip[i].InnerText.ToLower().Contains("ogłoszenie o") ||
                        bip[i].InnerText.ToLower().Contains("nabór")
                    )
                    {
                        postDateNode = bip[i].ParentNode.SelectNodes($"//div[@class='dzial-tresc']/ol/h3[{i + 1}]/following::h5").First();
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
                            Console.WriteLine("New post during last week found on BIP site");
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
