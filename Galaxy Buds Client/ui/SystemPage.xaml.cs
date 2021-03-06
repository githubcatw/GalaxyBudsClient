﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Galaxy_Buds_Client.message;
using Galaxy_Buds_Client.model.Constants;
using Galaxy_Buds_Client.parser;

namespace Galaxy_Buds_Client.ui
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class SystemPage : BasePage
    {
        private MainWindow _mainWindow;

        private readonly String Waiting = "Waiting for device response...";

        public SystemPage(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            SPPMessageHandler.Instance.GetAllDataResponse += InstanceOnGetAllDataResponse;
            SPPMessageHandler.Instance.BatteryTypeResponse += InstanceOnBatteryTypeResponse;
            SPPMessageHandler.Instance.SerialNumberResponse += InstanceOnSerialNumberResponse;
            SPPMessageHandler.Instance.BuildStringResponse += InstanceOnBuildStringResponse;
            SPPMessageHandler.Instance.ExtendedStatusUpdate += InstanceOnExtendedStatusUpdate;
        }

        private void InstanceOnExtendedStatusUpdate(object sender, ExtendedStatusUpdateParser e)
        {
            Dispatcher.Invoke(() =>
            {
                ProtocolRevision.TextDetail = $"rev{e.Revision}";
            });
        }

        private void InstanceOnBuildStringResponse(object sender, string e)
        {
            Dispatcher.Invoke(() =>
            {
                BuildString.TextDetail = e.Length > 1 ? e.Remove(0,1) : e;
            });
        }

        private void InstanceOnSerialNumberResponse(object sender, DebugSerialNumberParser e)
        {
            Dispatcher.Invoke(() =>
            {
                SerialNumber.TextDetail = $"Left: {e.LeftSerialNumber}, Right: {e.RightSerialNumber}";
            });
        }

        private void InstanceOnBatteryTypeResponse(object sender, BatteryTypeParser e)
        {
            Dispatcher.Invoke(() =>
            {
                BatteryType.TextDetail = $"Left: {e.LeftBatteryType}, Right: {e.RightBatteryType}";
                if (BluetoothService.Instance.ActiveModel == Model.BudsPlus)
                {
                    BatteryType.TextDetail = "Not specified";
                }
            });
        }

        public override void OnPageShown()
        {
            BluetoothService.Instance.SendAsync(SPPMessageBuilder.Info.GetBatteryType());
            BluetoothService.Instance.SendAsync(SPPMessageBuilder.Info.GetSerialNumber());
            BluetoothService.Instance.SendAsync(SPPMessageBuilder.Info.GetBuildString());
            BluetoothService.Instance.SendAsync(SPPMessageBuilder.Info.GetAllData());
            LoadingSpinner.Visibility = Visibility.Visible;
            LoadingSpinner.Start();

            HwVer.TextDetail = Waiting;
            SwVer.TextDetail = Waiting;
            TouchFwVer.TextDetail = Waiting;
            BtAddr.TextDetail = Waiting;
            Proximity.TextDetail = Waiting;
            Thermo.TextDetail = Waiting;
            AdcSoc.TextDetail = Waiting;
            AdcVoltage.TextDetail = Waiting;
            AdcCurrent.TextDetail = Waiting;
            Hall.TextDetail = Waiting;
            SerialNumber.TextDetail = Waiting;
            BatteryType.TextDetail = Waiting;
            BuildString.TextDetail = Waiting;
        }

        public override void OnPageHidden()
        {

        }

        private void InstanceOnGetAllDataResponse(object sender, DebugGetAllDataParser e)
        {
            Dispatcher.Invoke(() =>
            {
                LoadingSpinner.Visibility = Visibility.Hidden;
                LoadingSpinner.Stop();
                HwVer.TextDetail = e.HardwareVersion;
                SwVer.TextDetail = e.SoftwareVersion;
                TouchFwVer.TextDetail = e.TouchSoftwareVersion;
                BtAddr.TextDetail = $"Left: {e.LeftBluetoothAddress}, Right: {e.RightBluetoothAddress}";
                Proximity.TextDetail = $"Left: {e.LeftProximity:N}, Right: {e.RightProximity:N}";
                Thermo.TextDetail = $"Left: {e.LeftThermistor:N} °C, Right: {e.RightThermistor:N} °C";
                AdcSoc.TextDetail = $"Left: {e.LeftAdcSOC:N}%, Right: {e.RightAdcSOC:N}%";
                AdcVoltage.TextDetail = $"Left: {e.LeftAdcVCell:N}V, Right: {e.RightAdcVCell:N}V";
                AdcCurrent.TextDetail = $"Left: {e.LeftAdcCurrent:N}mA, Right: {e.RightAdcCurrent:N}mA";
                Hall.TextDetail = $"Left: {e.LeftHall}, Right: {e.RightHall}";
            });
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.ReturnToHome();
        }

        private void FactoryReset_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindow.GoToPage(MainWindow.Pages.FactoryReset);
        }

        private void RunSelfTest_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindow.GoToPage(MainWindow.Pages.SelfTest);
        }
    }
}
