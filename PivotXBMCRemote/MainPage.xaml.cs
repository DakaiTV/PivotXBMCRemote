﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace PivotXBMCRemote
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 全局WebClient
        private WebClient client = new WebClient();

        // 视频播放器的编号
        private int Player = 1;
                
        // 应用独立空间配置
        private IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        private SendRemoteKey remote = new SendRemoteKey();
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            try
            {
                // 更新配置页面的表单
                string host = (string)userSettings["host"];
                textBoxHost.Text = host;
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                MessageBox.Show("无法获取XBMC媒体中心的服务器，请先配置XBMC媒体中心的服务器地址。");
                userSettings.Add("host", "");
            }
        }

        // 向上按钮事件
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Input.Up", "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Up\", \"id\": 1}");           
        }

        // 向下按钮事件
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Input.Down", "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Down\", \"id\": 1}");
        }

        // 向左按钮事件
        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Input.Left", "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Left\", \"id\": 1}");
        }

        // 方向向右按钮事件
        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Input.Right", "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Right\", \"id\": 1}");
        }

        // 确认按钮事件
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Input.Select", "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Select\", \"id\": 1}");
        }

        // 返回按钮事件
        private void ButtonHistory_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Input.Back", "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Back\", \"id\": 1}");
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Input.Home", "{\"jsonrpc\": \"2.0\", \"method\": \"Input.Home\", \"id\": 1}");
        }

        private void ButtonMute_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Application.SetMute", "{\"jsonrpc\": \"2.0\", \"method\": \"Application.SetMute\", \"params\": { \"mute\": \"toggle\" }, \"id\": 1}");
        }

        private void ButtonVolumeDown_Click(object sender, RoutedEventArgs e)
        {
            // 获得当前音量
            remote.SendKey("VolumeDown", "{\"jsonrpc\": \"2.0\", \"method\": \"Application.GetProperties\", \"params\": {\"properties\": [\"volume\"]}, \"id\": 1}");
        }

        private void ButtonVolumeUp_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("VolumeUp", "{\"jsonrpc\": \"2.0\", \"method\": \"Application.GetProperties\", \"params\": { \"properties\": [ \"volume\" ] }, \"id\": 1}");
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            // remote.SendKey("Player.PlayPause", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.PlayPause\", \"params\": { \"playerid\": 0 }, \"id\": 1}");
            remote.SendKey("Player.PlayPause", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.PlayPause\", \"params\": { \"playerid\": "+ Player.ToString() +" }, \"id\": 1}");
            // remote.SendKey("Player.PlayPause", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.PlayPause\", \"params\": { \"playerid\": 2 }, \"id\": 1}");
            // 获取播放器列表
            // {"jsonrpc": "2.0", "method": "Playlist.GetPlaylists", "id": 1}
            // {"jsonrpc": "2.0", "method": "Player.PlayPause", "params": { "playerid": ' + player + ' }, "id": 1}
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Player.Stop", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.Stop\", \"params\": { \"playerid\": "+ Player.ToString() +" }, \"id\": 1}");
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Player.SetSpeed", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.SetSpeed\", \"params\": { \"playerid\": " + Player.ToString() + ", \"speed\": \"decrement\" }, \"id\": 1}");
        }

        private void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Player.SetSpeed", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.SetSpeed\", \"params\": { \"playerid\": "+ Player.ToString() +", \"speed\": \"increment\" }, \"id\": 1}");
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Player.GoPrevious", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.GoPrevious\", \"params\": { \"playerid\": " + Player.ToString() + " }, \"id\": 1}");
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Player.GoNext", "{\"jsonrpc\": \"2.0\", \"method\": \"Player.GoNext\", \"params\": { \"playerid\": " + Player.ToString() + " }, \"id\": 1}");
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userSettings.Contains("host") == true)
                {
                    userSettings.Remove("host");
                }
                userSettings.Add("host", textBoxHost.Text);
                userSettings.Save();
                MessageBox.Show("配置保存成功。");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonPowerOff_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("Application.Quit", "{\"jsonrpc\": \"2.0\", \"method\": \"Application.Quit\", \"id\": 1}");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            remote.SendKey("System.Shutdown", "{\"jsonrpc\": \"2.0\", \"method\": \"System.Shutdown\", \"id\": 1}");
        }
    }
}