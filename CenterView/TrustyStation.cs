﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CenterView
{
   public class TrustyStation
    {
        /// <summary>
        /// 获取config.xml文件的信任站点列表
        /// </summary>
        /// <returns></returns>
        private string[] TrustWebsite()
        {
            string[] temp = null;
            XmlTextReader reader = new XmlTextReader("..\\..\\config.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "TrustWebsite")
                    {
                        string sumValue = reader.ReadElementContentAsString().Trim();
                        temp = sumValue.Split(',');
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// 获取授信站点列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetTrustyStations()
        {
            List<string> subkeyNames=new List<string>();
            RegistryKey hkml = Registry.CurrentUser;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.OpenSubKey(@"Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains", true);
         string[]domains = aimdir.GetSubKeyNames();
            //判断有没有域名前缀例如“www”
            for (int i = 0; i < domains.Length; i++)
            {
                RegistryKey temp = aimdir.OpenSubKey(domains[i], false);
                if (temp.GetSubKeyNames().Length == 1)
                {
                    domains[i] = temp.GetSubKeyNames()[0] + "." + domains[i];
                }

            }
            subkeyNames.AddRange(domains);
            //判断可信任站点为ip地址时
            RegistryKey  range = software.OpenSubKey(@"Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Ranges", true);
            //判断是否有IP地址

            string[] ranges = range.GetSubKeyNames();
            for (int i = 0; i < ranges.Length; i++)
            {
                RegistryKey rangeTemp = range.OpenSubKey(ranges[i], false);
                ranges[i] = rangeTemp.GetValue(":Range").ToString();
            }
                subkeyNames.AddRange(ranges);
            return subkeyNames;
          
            
        }
    }
}
