﻿using GenShin_Launcher_Plus.ViewModels;
using GenShin_Launcher_Plus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GenShin_Launcher_Plus.Core
{
    public static class LoadProgramCore
    {
        public static bool ReadLangList()
        {
            if (IniControl.ReadLang == "" || IniControl.ReadLang == null)
            {
                IniControl.ReadLang = "Lang_CN.json";
                LoadLanguageCore(IniControl.ReadLang);
            }
            //获取网络上的语言列表到Temp目录
            FilesControl fc = new();
            if(IniControl.ReadLang!= "Lang_CN.json")
            {
                if (fc.DownloadFile("https://www.dawnfz.com/G.L.P/JsonData/List.json", Path.GetTempPath(), Path.Combine(Path.GetTempPath(), "List.json")))
                {
                    string file = Path.Combine(Path.GetTempPath(), "List.json");
                    string json = File.ReadAllText(file);
                    if (json != null && json != "")
                    {
                        MainBase.langlist = JsonConvert.DeserializeObject<List<LanguageListsModel>>(json);
                    }
                    else
                    {
                        MessageBox.Show("Error: Language pack list is empty, please check the network! !");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to get the language pack list, maybe your network status is not good or the server is wrong, You will try to use the local language pack, press OK to enter, you will not be able to change the language after successful entry");
                }
            }
            else
            {
                if (fc.DownloadFile("https://nenedan.coding.net/p/glp/d/dawnfz.github/git/raw/main/G.L.P/JsonData/List.json", Path.GetTempPath(), Path.Combine(Path.GetTempPath(), "List.json")))
                {
                    string file = Path.Combine(Path.GetTempPath(), "List.json");
                    string json = File.ReadAllText(file);
                    if (json != null && json != "")
                    {
                        MainBase.langlist = JsonConvert.DeserializeObject<List<LanguageListsModel>>(json);
                    }
                    else
                    {
                        MessageBox.Show("错误：语言包列表为空，请检查网络！！");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    MessageBox.Show("语言包列表获取失败，可能是您的网络状态不佳或服务器错误\r\n即将尝试使用本地语言包，按下确定后进入，进入成功后您将无法更改语言");
                }
            }
            //从JSON字符串生成集合对象用于存放语言包列表

            if (!File.Exists($@"Config/{IniControl.ReadLang}"))
            {
                LoadLanguageCore(IniControl.ReadLang);
            }

            //在检查语言包更新前，提前将本地存放的语言包Json序列化到实体类LanguagesModel中
            string oldfile = $@"Config/{IniControl.ReadLang}";
            string oldjson = File.ReadAllText(oldfile);
            if (oldjson != null && oldjson != "")
            {
                MainBase.lang = JsonConvert.DeserializeObject<LanguagesModel>(oldjson);
            }
            else
            {
                LoadLanguageCore(IniControl.ReadLang);
            }
            //判断语言包的版本号，返回一个布尔类型的值给MainWindow对象创建时判断是否执行LoadLanguageCore方法更新语言文件
            foreach (LanguageListsModel ll in MainBase.langlist)
            {
                if (MainBase.lang.Languages == ll.LangName)
                {
                    if (MainBase.lang.LangVersion != ll.LangVersion)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void LoadLanguageCore(string langfile)
        {
            if (IniControl.ReadLang != "Lang_CN.json")
            {
                FilesControl fc = new();
                if (fc.DownloadFile($"https://www.dawnfz.com/G.L.P/JsonData/{langfile}", @"Config", $@"Config/{langfile}"))
                {
                    string file = $@"Config/{langfile}";
                    string json = File.ReadAllText(file);
                    if (json != null && json != "")
                    {
                        MainBase.lang = JsonConvert.DeserializeObject<LanguagesModel>(json);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to complete the initialization of the language pack, you can reopen the program and try again. If this message appears multiple times, please contact the developer!");
                    Environment.Exit(0);
                }
            }
            else
            {
                FilesControl fc = new();
                if (fc.DownloadFile($"https://nenedan.coding.net/p/glp/d/dawnfz.github/git/raw/main/G.L.P/JsonData/{langfile}", @"Config", $@"Config/{langfile}"))
                {
                    string file = $@"Config/{langfile}";
                    string json = File.ReadAllText(file);
                    if (json != null && json != "")
                    {
                        MainBase.lang = JsonConvert.DeserializeObject<LanguagesModel>(json);
                    }
                }
                else
                {
                    MessageBox.Show("无法完成语言包初始化，您可以重新打开本程序重试，若多次出现本消息请联系开发者！");
                    Environment.Exit(0);
                }
            }

        }
        public static void LoadUpdateCore()
        {
            if (IniControl.ReadLang != "Lang_CN.json")
            {
                FilesControl fc = new();
                if (fc.DownloadFile("https://www.dawnfz.com/G.L.P/JsonData/Update_Global.Json", Path.GetTempPath(), Path.Combine(Path.GetTempPath(), "Update_Global.Json")))
                {
                    string file = Path.Combine(Path.GetTempPath(), "Update_Global.Json");
                    string json = File.ReadAllText(file);
                    MainBase.update = JsonConvert.DeserializeObject<UpdateContentModel>(json);
                }
            }
            else
            {
                FilesControl fc = new();
                if (fc.DownloadFile("https://nenedan.coding.net/p/glp/d/dawnfz.github/git/raw/main/G.L.P/JsonData/Update_CN.Json", Path.GetTempPath(), Path.Combine(Path.GetTempPath(), "Update_CN.Json")))
                {
                    string file = Path.Combine(Path.GetTempPath(), "Update_CN.Json");
                    string json = File.ReadAllText(file);
                    MainBase.update = JsonConvert.DeserializeObject<UpdateContentModel>(json);
                }
            }
        }
    }
}
