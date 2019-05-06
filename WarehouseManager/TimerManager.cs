using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WarehouseManager
{
   
    public class TimerManager
    {
        Timer timer1;
        BoxManager BoxManagerObj;
        public TimerManager(BoxManager manager)
        {
            BoxManagerObj = manager;
            timer1 = new Timer(new TimerCallback(TickTimer), null, 1000, 10000);
        }
        
        void TickTimer(object state)
        {
            // activate the func to check the boxes time 
            if (BoxManagerObj.CheckOverDatedTime())
                ExportObject.SaveAndExport();
        }
        
    }
}
