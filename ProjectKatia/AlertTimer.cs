using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManager;

namespace ProjectKatia
{
      class AlertTimer
    {
        // update the alert window all the time by timer
        private MainWindow mainWindow;
       
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public AlertTimer(MainWindow window)
        {
            mainWindow = window;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,3);
            dispatcherTimer.Start();
        }
        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            mainWindow.UpdateTheAlerts(AlertHandler.UpdateAlerts());
            
        }
    }
}
