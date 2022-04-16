﻿using GenShin_Launcher_Plus.Service;
using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using GenShin_Launcher_Plus.Service.IService;

namespace GenShin_Launcher_Plus.ViewModels
{
    /// <summary>
    /// UsersPage的ViewModel 
    /// 集成了UsersPage的部分操作实现逻辑
    /// </summary>
    public class UsersPageViewModel : ObservableObject
    {

        public UsersPageViewModel()
        {
            SaveUserDataCommand = new RelayCommand(SaveUserData);
            RemoveThisPageCommand = new RelayCommand(RemoveThisPage);

            _registryService = new RegistryService();
            _userDataService = new UserDataService();
        }

        private IRegistryService _registryService;
        public IRegistryService RegistryService { get => _registryService; }

        private IUserDataService _userDataService;
        public IUserDataService UserDataService { get => _userDataService; }

        public LanguageModel languages { get => App.Current.Language; }

        public string Name { get; set; }

        public ICommand SaveUserDataCommand { get; set; }
        private void SaveUserData()
        {
            //判断YuanShen.exe是否存在，存在则为False，否则为True
            bool isGlobal = !File.Exists(Path.Combine(App.Current.IniModel.GamePath, "YuanShen.exe"));
            //判断isGlobal值，为True时为Cn，否则为Global
            string gamePort = isGlobal ? "Global" : "CN";
            string userdata = RegistryService.GetFromRegistry(Name, gamePort);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "UserData", Name), userdata);
            App.Current.NoticeOverAllBase.UserLists = UserDataService.ReadUserList();
            RemoveThisPage();
        }

        public ICommand RemoveThisPageCommand { get; set; }
        private void RemoveThisPage()
        {
            App.Current.NoticeOverAllBase.MainPagesIndex = 0;
        }
    }
}
