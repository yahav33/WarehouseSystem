using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WarehouseManager;

namespace ProjectKatia
{
    class AlertHandler
    {

        public static string UpdateAlerts()
        {
            
            StringBuilder sb = new StringBuilder();
            int index = 1;
            if (Constants.BoxAlerts.Count >= 4) Constants.BoxAlerts.Clear();
            if (Constants.BoxAlerts.Count < 4)
            {
                for (int i = 0; i < Constants.BoxAlerts.Count; i++)
                {
                    sb.AppendLine(index + ": " + Constants.BoxAlerts[i]);
                    index++;
                }
            }
            else {
                for (int i = 0; i < 4; i++)
                {
                    sb.AppendLine(index + ": " + Constants.BoxAlerts[i]);
                    index++;
                }

            }
            
            return sb.ToString();
        }
    }
}
