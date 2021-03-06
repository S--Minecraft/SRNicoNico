﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SRNicoNico.Models.NicoNicoViewer {

    //ハイパーリンクを解析してハイパーリンク化する
    public class HyperLinkParser {

        //URLをハイパーリンク化する
        public static string Parse(string desc) {

            if(Properties.Settings.Default.EnableUrlLink) {

                //普通のURLだったら
                Regex url = new Regex(@"(.....)(https?://[\w/:%#\$&\?\(\)~\.=\+\-]+)");
                desc = url.Replace(desc, new MatchEvaluator((match) => {


                    //aタグが書いてあったらそのまま返す
                    if(match.Groups[1].Value.Length != 0 && (match.Groups[1].Value == "/" || match.Groups[1].Value == ">" || match.Groups[1].Value == "ref=\"")) {

                        return match.Value;
                    }

                    return match.Groups[1].Value + "<a href=\"" + match.Groups[2].Value + "\">" + match.Groups[2].Value + "</a>";
                }));
            }

            if(Properties.Settings.Default.EnableTwitterLink) {

                //TwitterのIDだった場合
                Regex twitter = new Regex(@"@[A-z|0-9|_]{1,15}");
                desc = twitter.Replace(desc, new MatchEvaluator((match) => {

                    return "<a href=\"https://twitter.com/" + match.Value.Substring(1) + "\">" + match.Value + "</a>";
                }));
            }

            return desc;
        }
    }
}
