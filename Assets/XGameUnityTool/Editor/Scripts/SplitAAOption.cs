using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XGame
{
    public enum SplitAAIncludeType
    {
        //包含
        Include,

        //排除
        Exclude,
    }


    /// <summary>
    /// 分离AA Setting
    /// </summary>
    [Serializable]
    public class SplitAAOption
    {
        public static Dictionary<CountyCode, string> CountryNames = new Dictionary<CountyCode, string>()
        {
            { CountyCode.AD, "安道尔 (Andorra)" },
            { CountyCode.AE, "阿联酋 (United Arab Emirates)" },
            { CountyCode.AF, "阿富汗 (Afghanistan)" },
            { CountyCode.AG, "安提瓜和巴布达 (Antigua and Barbuda)" },
            { CountyCode.AI, "安圭拉 (Anguilla)" },
            { CountyCode.AL, "阿尔巴尼亚 (Albania)" },
            { CountyCode.AM, "亚美尼亚 (Armenia)" },
            { CountyCode.AO, "安哥拉 (Angola)" },
            { CountyCode.AQ, "南极洲 (Antarctica)" },
            { CountyCode.AR, "阿根廷 (Argentina)" },
            { CountyCode.AS, "美属萨摩亚 (American Samoa)" },
            { CountyCode.AT, "奥地利 (Austria)" },
            { CountyCode.AU, "澳大利亚 (Australia)" },
            { CountyCode.AW, "阿鲁巴 (Aruba)" },
            { CountyCode.AX, "奥兰 (Åland Islands)" },
            { CountyCode.AZ, "阿塞拜疆 (Azerbaijan)" },
            { CountyCode.BA, "波斯尼亚和黑塞哥维那 (Bosnia and Herzegovina)" },
            { CountyCode.BB, "巴巴多斯 (Barbados)" },
            { CountyCode.BD, "孟加拉 (Bangladesh)" },
            { CountyCode.BE, "比利时 (Belgium)" },
            { CountyCode.BF, "布基纳法索 (Burkina Faso)" },
            { CountyCode.BG, "保加利亚 (Bulgaria)" },
            { CountyCode.BH, "巴林 (Bahrain)" },
            { CountyCode.BI, "布隆迪 (Burundi)" },
            { CountyCode.BJ, "贝宁 (Benin)" },
            { CountyCode.BL, "圣巴泰勒米 (Saint Barthélemy)" },
            { CountyCode.BM, "百慕大 (Bermuda)" },
            { CountyCode.BN, "文莱 (Brunei)" },
            { CountyCode.BO, "玻利维亚 (Bolivia)" },
            { CountyCode.BQ, "加勒比荷兰 (Caribbean Netherlands)" },
            { CountyCode.BR, "巴西 (Brazil)" },
            { CountyCode.BS, "巴哈马 (Bahamas)" },
            { CountyCode.BT, "不丹 (Bhutan)" },
            { CountyCode.BV, "布韦岛 (Bouvet Island)" },
            { CountyCode.BW, "博茨瓦纳 (Botswana)" },
            { CountyCode.BY, "白俄罗斯 (Belarus)" },
            { CountyCode.BZ, "伯利兹 (Belize)" },
            { CountyCode.CA, "加拿大 (Canada)" },
            { CountyCode.CC, "科科斯（基林）群岛 (Cocos (Keeling) Islands)" },
            { CountyCode.CD, "刚果（金） (DR Congo)" },
            { CountyCode.CF, "中非 (Central African Republic)" },
            { CountyCode.CG, "刚果（布） (Republic of the Congo)" },
            { CountyCode.CH, "瑞士 (Switzerland)" },
            { CountyCode.CI, "科特迪瓦 (Côte d'Ivoire)" },
            { CountyCode.CK, "库克群岛 (Cook Islands)" },
            { CountyCode.CL, "智利 (Chile)" },
            { CountyCode.CM, "喀麦隆 (Cameroon)" },
            { CountyCode.CN, "中国 (China)" },
            { CountyCode.CO, "哥伦比亚 (Colombia)" },
            { CountyCode.CR, "哥斯达黎加 (Costa Rica)" },
            { CountyCode.CU, "古巴 (Cuba)" },
            { CountyCode.CV, "佛得角 (Cabo Verde)" },
            { CountyCode.CW, "库拉索 (Curaçao)" },
            { CountyCode.CX, "圣诞岛 (Christmas Island)" },
            { CountyCode.CY, "塞浦路斯 (Cyprus)" },
            { CountyCode.CZ, "捷克 (Czech Republic)" },
            { CountyCode.DE, "德国 (Germany)" },
            { CountyCode.DJ, "吉布提 (Djibouti)" },
            { CountyCode.DK, "丹麦 (Denmark)" },
            { CountyCode.DM, "多米尼克 (Dominica)" },
            { CountyCode.DO, "多米尼加 (Dominican Republic)" },
            { CountyCode.DZ, "阿尔及利亚 (Algeria)" },
            { CountyCode.EC, "厄瓜多尔 (Ecuador)" },
            { CountyCode.EE, "爱沙尼亚 (Estonia)" },
            { CountyCode.EG, "埃及 (Egypt)" },
            { CountyCode.EH, "阿拉伯撒哈拉民主共和国 (Sahrawi Arab Democratic Republic)" },
            { CountyCode.ER, "厄立特里亚 (Eritrea)" },
            { CountyCode.ES, "西班牙 (Spain)" },
            { CountyCode.ET, "埃塞俄比亚 (Ethiopia)" },
            { CountyCode.FI, "芬兰 (Finland)" },
            { CountyCode.FJ, "斐济 (Fiji)" },
            { CountyCode.FK, "福克兰群岛 (Falkland Islands)" },
            { CountyCode.FM, "密克罗尼西亚联邦 (Micronesia)" },
            { CountyCode.FO, "法罗群岛 (Faroe Islands)" },
            { CountyCode.FR, "法国 (France)" },
            { CountyCode.GA, "加蓬 (Gabon)" },
            { CountyCode.GB, "英国 (United Kingdom)" },
            { CountyCode.GD, "格林纳达 (Grenada)" },
            { CountyCode.GE, "格鲁吉亚 (Georgia)" },
            { CountyCode.GF, "法属圭亚那 (French Guiana)" },
            { CountyCode.GG, "根西 (Guernsey)" },
            { CountyCode.GH, "加纳 (Ghana)" },
            { CountyCode.GI, "直布罗陀 (Gibraltar)" },
            { CountyCode.GL, "格陵兰 (Greenland)" },
            { CountyCode.GM, "冈比亚 (Gambia)" },
            { CountyCode.GN, "几内亚 (Guinea)" },
            { CountyCode.GP, "瓜德罗普 (Guadeloupe)" },
            { CountyCode.GQ, "赤道几内亚 (Equatorial Guinea)" },
            { CountyCode.GR, "希腊 (Greece)" },
            { CountyCode.GS, "南乔治亚和南桑威奇群岛 (South Georgia and the South Sandwich Islands)" },
            { CountyCode.GT, "危地马拉 (Guatemala)" },
            { CountyCode.GU, "关岛 (Guam)" },
            { CountyCode.GW, "几内亚比绍 (Guinea-Bissau)" },
            { CountyCode.GY, "圭亚那 (Guyana)" },
            { CountyCode.HK, "香港 (Hong Kong)" },
            { CountyCode.HM, "赫德岛和麦克唐纳群岛 (Heard Island and McDonald Islands)" },
            { CountyCode.HN, "洪都拉斯 (Honduras)" },
            { CountyCode.HR, "克罗地亚 (Croatia)" },
            { CountyCode.HT, "海地 (Haiti)" },
            { CountyCode.HU, "匈牙利 (Hungary)" },
            { CountyCode.ID, "印尼 (Indonesia)" },
            { CountyCode.IE, "爱尔兰 (Ireland)" },
            { CountyCode.IL, "以色列 (Israel)" },
            { CountyCode.IM, "马恩岛 (Isle of Man)" },
            { CountyCode.IN, "印度 (India)" },
            { CountyCode.IO, "英属印度洋领地 (British Indian Ocean Territory)" },
            { CountyCode.IQ, "伊拉克 (Iraq)" },
            { CountyCode.IR, "伊朗 (Iran)" },
            { CountyCode.IS, "冰岛 (Iceland)" },
            { CountyCode.IT, "意大利 (Italy)" },
            { CountyCode.JE, "泽西 (Jersey)" },
            { CountyCode.JM, "牙买加 (Jamaica)" },
            { CountyCode.JO, "约旦 (Jordan)" },
            { CountyCode.JP, "日本 (Japan)" },
            { CountyCode.KE, "肯尼亚 (Kenya)" },
            { CountyCode.KG, "吉尔吉斯斯坦 (Kyrgyzstan)" },
            { CountyCode.KH, "柬埔寨 (Cambodia)" },
            { CountyCode.KI, "基里巴斯 (Kiribati)" },
            { CountyCode.KM, "科摩罗 (Comoros)" },
            { CountyCode.KN, "圣基茨和尼维斯 (Saint Kitts and Nevis)" },
            { CountyCode.KP, "朝鲜 (North Korea)" },
            { CountyCode.KR, "韩国 (South Korea)" },
            { CountyCode.KW, "科威特 (Kuwait)" },
            { CountyCode.KY, "开曼群岛 (Cayman Islands)" },
            { CountyCode.KZ, "哈萨克斯坦 (Kazakhstan)" },
            { CountyCode.LA, "老挝 (Laos)" },
            { CountyCode.LB, "黎巴嫩 (Lebanon)" },
            { CountyCode.LC, "圣卢西亚 (Saint Lucia)" },
            { CountyCode.LI, "列支敦士登 (Liechtenstein)" },
            { CountyCode.LK, "斯里兰卡 (Sri Lanka)" },
            { CountyCode.LR, "利比里亚 (Liberia)" },
            { CountyCode.LS, "莱索托 (Lesotho)" },
            { CountyCode.LT, "立陶宛 (Lithuania)" },
            { CountyCode.LU, "卢森堡 (Luxembourg)" },
            { CountyCode.LV, "拉脱维亚 (Latvia)" },
            { CountyCode.LY, "利比亚 (Libya)" },
            { CountyCode.MA, "摩洛哥 (Morocco)" },
            { CountyCode.MC, "摩纳哥 (Monaco)" },
            { CountyCode.MD, "摩尔多瓦 (Moldova)" },
            { CountyCode.ME, "黑山 (Montenegro)" },
            { CountyCode.MF, "法属圣马丁 (Saint Martin)" },
            { CountyCode.MG, "马达加斯加 (Madagascar)" },
            { CountyCode.MH, "马绍尔群岛 (Marshall Islands)" },
            { CountyCode.MK, "马其顿 (North Macedonia)" },
            { CountyCode.ML, "马里 (Mali)" },
            { CountyCode.MM, "缅甸 (Myanmar)" },
            { CountyCode.MN, "蒙古 (Mongolia)" },
            { CountyCode.MO, "澳门 (Macao)" },
            { CountyCode.MP, "北马里亚纳群岛 (Northern Mariana Islands)" },
            { CountyCode.MQ, "马提尼克 (Martinique)" },
            { CountyCode.MR, "毛里塔尼亚 (Mauritania)" },
            { CountyCode.MS, "蒙特塞拉特 (Montserrat)" },
            { CountyCode.MT, "马耳他 (Malta)" },
            { CountyCode.MU, "毛里求斯 (Mauritius)" },
            { CountyCode.MV, "马尔代夫 (Maldives)" },
            { CountyCode.MW, "马拉维 (Malawi)" },
            { CountyCode.MX, "墨西哥 (Mexico)" },
            { CountyCode.MY, "马来西亚 (Malaysia)" },
            { CountyCode.MZ, "莫桑比克 (Mozambique)" },
            { CountyCode.NA, "纳米比亚 (Namibia)" },
            { CountyCode.NC, "新喀里多尼亚 (New Caledonia)" },
            { CountyCode.NE, "尼日尔 (Niger)" },
            { CountyCode.NF, "诺福克岛 (Norfolk Island)" },
            { CountyCode.NG, "尼日利亚 (Nigeria)" },
            { CountyCode.NI, "尼加拉瓜 (Nicaragua)" },
            { CountyCode.NL, "荷兰 (Netherlands)" },
            { CountyCode.NO, "挪威 (Norway)" },
            { CountyCode.NP, "尼泊尔 (Nepal)" },
            { CountyCode.NR, "瑙鲁 (Nauru)" },
            { CountyCode.NU, "纽埃 (Niue)" },
            { CountyCode.NZ, "新西兰 (New Zealand)" },
            { CountyCode.OM, "阿曼 (Oman)" },
            { CountyCode.PA, "巴拿马 (Panama)" },
            { CountyCode.PE, "秘鲁 (Peru)" },
            { CountyCode.PF, "法属波利尼西亚 (French Polynesia)" },
            { CountyCode.PG, "巴布亚新几内亚 (Papua New Guinea)" },
            { CountyCode.PH, "菲律宾 (Philippines)" },
            { CountyCode.PK, "巴基斯坦 (Pakistan)" },
            { CountyCode.PL, "波兰 (Poland)" },
            { CountyCode.PM, "圣皮埃尔和密克隆 (Saint Pierre and Miquelon)" },
            { CountyCode.PN, "皮特凯恩群岛 (Pitcairn Islands)" },
            { CountyCode.PR, "波多黎各 (Puerto Rico)" },
            { CountyCode.PS, "巴勒斯坦 (Palestine)" },
            { CountyCode.PT, "葡萄牙 (Portugal)" },
            { CountyCode.PW, "帕劳 (Palau)" },
            { CountyCode.PY, "巴拉圭 (Paraguay)" },
            { CountyCode.QA, "卡塔尔 (Qatar)" },
            { CountyCode.RE, "留尼汪 (Réunion)" },
            { CountyCode.RO, "罗马尼亚 (Romania)" },
            { CountyCode.RS, "塞尔维亚 (Serbia)" },
            { CountyCode.RU, "俄罗斯 (Russia)" },
            { CountyCode.RW, "卢旺达 (Rwanda)" },
            { CountyCode.SA, "沙特阿拉伯 (Saudi Arabia)" },
            { CountyCode.SB, "所罗门群岛 (Solomon Islands)" },
            { CountyCode.SC, "塞舌尔 (Seychelles)" },
            { CountyCode.SD, "苏丹 (Sudan)" },
            { CountyCode.SE, "瑞典 (Sweden)" },
            { CountyCode.SG, "新加坡 (Singapore)" },
            { CountyCode.SH, "圣赫勒拿 (Saint Helena)" },
            { CountyCode.SI, "斯洛文尼亚 (Slovenia)" },
            { CountyCode.SJ, "挪威 斯瓦尔巴群岛和扬马延岛 (Svalbard and Jan Mayen)" },
            { CountyCode.SK, "斯洛伐克 (Slovakia)" },
            { CountyCode.SL, "塞拉利昂 (Sierra Leone)" },
            { CountyCode.SM, "圣马力诺 (San Marino)" },
            { CountyCode.SN, "塞内加尔 (Senegal)" },
            { CountyCode.SO, "索马里 (Somalia)" },
            { CountyCode.SR, "苏里南 (Suriname)" },
            { CountyCode.SS, "南苏丹 (South Sudan)" },
            { CountyCode.ST, "圣多美和普林西比 (São Tomé and Príncipe)" },
            { CountyCode.SV, "萨尔瓦多 (El Salvador)" },
            { CountyCode.SX, "荷属圣马丁 (Sint Maarten)" },
            { CountyCode.SY, "叙利亚 (Syria)" },
            { CountyCode.SZ, "斯威士兰 (Eswatini)" },
            { CountyCode.TC, "特克斯和凯科斯群岛 (Turks and Caicos Islands)" },
            { CountyCode.TD, "乍得 (Chad)" },
            { CountyCode.TF, "法属南部领地 (French Southern and Antarctic Lands)" },
            { CountyCode.TG, "多哥 (Togo)" },
            { CountyCode.TH, "泰国 (Thailand)" },
            { CountyCode.TJ, "塔吉克斯坦 (Tajikistan)" },
            { CountyCode.TK, "托克劳 (Tokelau)" },
            { CountyCode.TL, "东帝汶 (Timor-Leste)" },
            { CountyCode.TM, "土库曼斯坦 (Turkmenistan)" },
            { CountyCode.TN, "突尼斯 (Tunisia)" },
            { CountyCode.TO, "汤加 (Tonga)" },
            { CountyCode.TR, "土耳其 (Turkey)" },
            { CountyCode.TT, "特立尼达和多巴哥 (Trinidad and Tobago)" },
            { CountyCode.TV, "图瓦卢 (Tuvalu)" },
            { CountyCode.TW, "台湾 (Taiwan)" },
            { CountyCode.TZ, "坦桑尼亚 (Tanzania)" },
            { CountyCode.UA, "乌克兰 (Ukraine)" },
            { CountyCode.UG, "乌干达 (Uganda)" },
            { CountyCode.UM, "美国本土外小岛屿 (United States Minor Outlying Islands)" },
            { CountyCode.US, "美国 (United States)" },
            { CountyCode.UY, "乌拉圭 (Uruguay)" },
            { CountyCode.UZ, "乌兹别克斯坦 (Uzbekistan)" },
            { CountyCode.VA, "梵蒂冈 (Vatican City)" },
            { CountyCode.VC, "圣文森特和格林纳丁斯 (Saint Vincent and the Grenadines)" },
            { CountyCode.VE, "委内瑞拉 (Venezuela)" },
            { CountyCode.VG, "英属维尔京群岛 (British Virgin Islands)" },
            { CountyCode.VI, "美属维尔京群岛 (United States Virgin Islands)" },
            { CountyCode.VN, "越南 (Vietnam)" },
            { CountyCode.VU, "瓦努阿图 (Vanuatu)" },
            { CountyCode.WF, "瓦利斯和富图纳 (Wallis and Futuna)" },
            { CountyCode.WS, "萨摩亚 (Samoa)" },
            { CountyCode.YE, "也门 (Yemen)" },
            { CountyCode.YT, "马约特 (Mayotte)" },
            { CountyCode.ZA, "南非 (South Africa)" },
            { CountyCode.ZM, "赞比亚 (Zambia)" },
            { CountyCode.ZW, "津巴布韦 (Zimbabwe)" },
        };
        
        private static string GetCountryName(CountyCode code)
        {
            if (CountryNames.ContainsKey(code))
            {
                return CountryNames[code];
            }

            return $"UnknownCountry[{code}]";
        }

        [ValueDropdown("$AssetGroupOptions")]
        //AddressableAssetGroup
        public Object AssetGroup = null;

        [HideInInspector]
        //国家
        public List<CountyCode> Countries = new List<CountyCode>();

        [ValueDropdown("$ModeOptions")]
        //包含，还是排除
        public SplitAAIncludeType Mode = SplitAAIncludeType.Include;

        public SplitAAOption()
        {
        }

        private static Object[] AssetGroupOptions()
        {
            if (AddressableReflection.HasModule() && AddressableReflection.Settings != null)
            {
                var result = new List<Object>();
                var guids = AssetDatabase.FindAssets("t:AddressableAssetGroup");
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    result.Add(AssetDatabase.LoadAssetAtPath<Object>(path));
                }

                return result.ToArray();
            }

            //TODO//反射获取
            return new Object[0];
        }


        private static IEnumerable ModeOptions = new ValueDropdownList<SplitAAIncludeType>()
        {
            { "include", SplitAAIncludeType.Include },
            { "exclude", SplitAAIncludeType.Exclude },
        };


        [Button("$ShowCodeDesc")]
        private void Status()
        {
            SplitAACountryCodeEditorWindow.Open(this);
        }

        public string ShowCodeDesc()
        {
            var sb = new StringBuilder();
            var index = 0;
            foreach (var country in Countries)
            {
                var countryName = GetCountryName(country);
                if (index != 0)
                {
                    sb.Append(",");
                }

                sb.Append(countryName);
                index++;
            }

            var result = sb.ToString();
            if (result == string.Empty)
            {
                return "No country/region selected";
            }

            return result;
        }

        //是否开启
        public bool IsOn(CountyCode county)
        {
            return Countries.Contains(county);
            // var layer = (long)Code;
            // var value = layer & (long)county;
            // return value == (long)county;
        }


        public void Open(CountyCode county, bool sort = true)
        {
            if (!Countries.Contains(county))
            {
                Countries.Add(county);
                if (sort)
                {
                    SortCountry();
                }
            }
            // Code |= county;
        }

        public void Close(CountyCode county, bool sort = true)
        {
            Countries.Remove(county);
            if (sort)
            {
                SortCountry();
            }

            // if ()
            // {
            //     Countries.Add(county);
            //     Countries.Sort();
            // }
            // Code &= ~county;
        }

        private void SortCountry()
        {
            Countries.Sort();
        }


        public void OpenAll()
        {
            var countryNames = SplitAAOption.CountryNames;
            foreach (var country in countryNames)
            {
                Open(country.Key, false);
            }

            SortCountry();
        }

        public void CloseAll()
        {
            Countries.Clear();
        }

        //获取导出的国家代号
        public string[] GetCountriesCodes()
        {
            var result = new List<string>();
            foreach (var country in Countries)
            {
                result.Add(country.ToString());
            }

            return result.ToArray();
        }
    }
}