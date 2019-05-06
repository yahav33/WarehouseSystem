using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManager
{
    public class TimeData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime dateTime { get;  set; }
        public DateTime lastDateOnShelf;

        public TimeData(double x,double y,int days = 22)
        {
            X = x;
            Y = y;
            UpdateTime();
            lastDateOnShelf = DateTime.Now.AddDays(days);
        }
        public TimeData(double x, double y,DateTime time,DateTime lastTime)
        {
            X = x;
            Y = y;
            dateTime = time;
            lastDateOnShelf = lastTime;
        }
        public void UpdateTime()
        {
            dateTime = DateTime.Now;
        }
    }
}
