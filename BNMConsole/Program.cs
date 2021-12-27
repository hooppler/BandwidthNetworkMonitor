// Author: Aleksandar Stojimirovic
// E-mail: stojimirovic@yahoo.com
// Mob:    +381 60 087 2516

using System;
using System.Collections.ObjectModel;
using System.Timers;
using BandwidthNetworkMonitorModel;

namespace BNMConsole
{
    class Program
    {
        Model model;
        ObservableCollection<InterfaceData> DataGridData;
        double samplingTime;
        Timer tmr;

        static void Main(string[] args)
        {
            Program prg = new Program();

            prg.samplingTime = 2000;

            prg.model = new Model();
            prg.model.RefreshInterfacesSimple();
            prg.DataGridData = prg.model.GetInterfacesData();
            prg.model.CalculateTotalValues();

            prg.tmr = new Timer();
            prg.tmr.Interval = prg.samplingTime;
            prg.tmr.Elapsed += prg.TimerCallback;
            prg.tmr.Enabled = true;
            prg.tmr.Start();

            prg.Header();
            Console.WriteLine("Initializing...");
            while (Console.Read() != 'q') ;
        }

        private void TimerCallback(Object source, ElapsedEventArgs e)
        {
            DataGridData = model.GetInterfacesData();
            model.CalculateTotalValues();
            Header();
            Console.WriteLine(string.Format("Sampling Time [ms]: {0}", samplingTime));
            Console.WriteLine(string.Format("Total Bytes Received: {0}", model.TotalBytesReceived));
            Console.WriteLine(string.Format("Total Bytes Send: {0}", model.TotalBytesSend));
            
            foreach (InterfaceData data in DataGridData)
            {
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine(string.Format("Name: {0} Status: {1} Description: {2}", data.Name, data.Status, data.Description));
                Console.WriteLine(string.Format("Bytes Received: {0} Bytes Send: {1} Id: {2}", data.BytesReceived, data.BytesSent, data.Id));
            }
        }

        private void Header()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("*******************************");
            Console.WriteLine("*  BANDWIDTH NETWORK MONITOR  *");
            Console.WriteLine("*******************************");
        }
    }
}
