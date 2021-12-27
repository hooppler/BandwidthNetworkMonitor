// Author: Aleksandar Stojimirovic
// E-mail: stojimirovic@yahoo.com
// Mob:    +381 60 087 2516

using System;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace BandwidthNetworkMonitorModel
{
    public class Model
    {
        private ObservableCollection<InterfaceData> interfacesData;
        private NetworkInterface[] netInterfaces;
        private static Object obj = new Object();
        private long totalBytesReceived;
        private long totalBytesSend;

        public Model()
        {
        }

        public ObservableCollection<InterfaceData> InterfacesData
        {
            get { return interfacesData; }
            set { interfacesData = value; }
        }

        public long TotalBytesReceived
        {
            get { return totalBytesReceived; }
            set { totalBytesReceived = value; }
        }

        public long TotalBytesSend
        {
            get { return totalBytesSend; }
            set { totalBytesSend = value; }
        }

        public void RefreshInterfaces()
        {
            netInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            interfacesData = new ObservableCollection<InterfaceData>();
            BindingOperations.EnableCollectionSynchronization(interfacesData, obj);
            foreach (NetworkInterface netInterface in netInterfaces)
            {
                interfacesData.Add(new InterfaceData());
            }
        }

        public void RefreshInterfacesSimple()
        {
            netInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            interfacesData = new ObservableCollection<InterfaceData>();
            foreach (NetworkInterface netInterface in netInterfaces)
            {
                interfacesData.Add(new InterfaceData());
            }
        }

        public void CalculateTotalValues()
        {
            TotalBytesReceived = 0;
            TotalBytesSend = 0;
            foreach (InterfaceData interfaceData in interfacesData)
            {
                TotalBytesReceived += interfaceData.BytesReceived;
                TotalBytesSend += interfaceData.BytesSent;
            }
        }

        public ObservableCollection<InterfaceData> GetInterfacesData()
        {

            for (int i = 0; i < interfacesData.Count; i++)
            {
                interfacesData[i] = GetInterfaceData(i);
                interfacesData[i].No = (i + 1).ToString();
            }

            return interfacesData;
        }

        private InterfaceData GetInterfaceData(int i)
        {
            InterfaceData intData = new InterfaceData();

            intData.Id = netInterfaces[i].Id;
            intData.Name = netInterfaces[i].Name;
            intData.Description = netInterfaces[i].Description;

            intData.Status = netInterfaces[i].OperationalStatus.ToString();
            intData.BytesReceived = netInterfaces[i].GetIPv4Statistics().BytesReceived;
            //intData.ChkBytesReceived = true;
            intData.BytesSent = netInterfaces[i].GetIPv4Statistics().BytesSent;
            //intData.ChkBytesSent = true;

            return intData;
        }
    }
}
