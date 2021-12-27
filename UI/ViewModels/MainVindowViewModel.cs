// Author: Aleksandar Stojimirovic
// E-mail: stojimirovic@yahoo.com
// Mob:    +381 60 087 2516
using System;
using System.Collections.ObjectModel;
using BandwidthNetworkMonitorModel;
using System.Timers;
using System.Windows.Input;

namespace UI.ViewModels
{
    public class MainVindowViewModel : ViewModelBase
    {
        private Model model;
        private ObservableCollection<InterfaceData> dataGridData;
        private Timer tmr;
        private double samplingTime;
        private bool isMonitoring;
        private string isMonitoringBackground;
        private string status;

        private ICommand startMonitoringCommand;
        private ICommand stopMonitoringCommand;

        public MainVindowViewModel()
        {
            samplingTime = 2000;
            IsMonitoring = true;

            model = new Model();
            model.RefreshInterfaces();
            DataGridData = model.GetInterfacesData();
            model.CalculateTotalValues();

            tmr = new Timer();
            tmr.Interval = samplingTime;
            tmr.Elapsed += TimerCallback;
            tmr.Enabled = true;
            tmr.Start();
        }

        public ObservableCollection<InterfaceData> DataGridData
        {
            get { return dataGridData; }
            set
            {
                dataGridData = value;
                OnPropertyChanged("DataGridData");
            }
        }

        public double SamplingTime 
        {
            get
            {
                return samplingTime;
            } 
            set
            {
                samplingTime = value;
                if (IsMonitoring)
                {
                    tmr.Enabled = false;
                    tmr.Interval = samplingTime;
                    tmr.Enabled = true;
                }
            }
        }

        public bool IsMonitoring
        {
            get { return isMonitoring; }
            set
            {
                isMonitoring = value;
                OnPropertyChanged("IsMonitoring");
                if (isMonitoring)
                {
                    IsMonitoringBackground = "Green";
                    Status = "In Progress...";
                }
                else
                {
                    IsMonitoringBackground = "Red";
                    Status = "Stopped!";
                }
            }
        }

        public string IsMonitoringBackground
        {
            get { return isMonitoringBackground; }
            set
            {
                isMonitoringBackground = value;
                OnPropertyChanged("IsMonitoringBackground");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public long TotalBytesReceived
        {
            get { return model.TotalBytesReceived; }
            set { model.TotalBytesReceived = value; }
        }

        public long TotalBytesSend
        {
            get { return model.TotalBytesSend; }
            set { model.TotalBytesSend = value; }
        }

        public ICommand StartMonitoringCommand
        {
            get { return startMonitoringCommand ?? (startMonitoringCommand = new RelayCommand(() => StartMonitoringExecute())); }
        }

        public ICommand StopMonitoringCommand
        {
            get { return stopMonitoringCommand ?? (startMonitoringCommand = new RelayCommand(() => StopMonitoringExecute())); }
        }

        private void StartMonitoringExecute()
        {
            tmr.Enabled = true;
            IsMonitoring = true;
        }

        private void StopMonitoringExecute()
        {
            tmr.Enabled = false;
            IsMonitoring = false;
        }

        private void TimerCallback(Object source, ElapsedEventArgs e)
        {
            DataGridData = model.GetInterfacesData();
            model.CalculateTotalValues();
            OnPropertyChanged("TotalBytesReceived");
            OnPropertyChanged("TotalBytesSend");
        }
    }
}
