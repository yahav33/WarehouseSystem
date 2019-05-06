using System;
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
using WarehouseManager;

namespace ProjectKatia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BoxManager boxManager;
        AlertTimer timeManager;
        public MainWindow()
        {
            InitializeComponent();
            boxManager = new BoxManager();
            ExportObject.ReadFromJson();
            timeManager = new AlertTimer(this);
           TimerManager timerManager = new TimerManager(boxManager);
           
        }
        #region AddFunc
        private void AddToTheWareHouse(object sender, RoutedEventArgs e)
        {
            if (Validation.checkIfValidDouble(buttonAdd.Text))
            {
                if (Validation.checkIfValidDouble(highAdd.Text))
                {
                    if (Validation.checkIfValidInt(countAdd.Text))
                    {
                        addBlock.Text = boxManager.Add(Convert.ToDouble(buttonAdd.Text), Convert.ToDouble(highAdd.Text),
                        Convert.ToInt32(countAdd.Text),false);
                    }
                    else addBlock.Text = "Please Insert a Valid Amount Number";
                }
                else addBlock.Text = "Please Insert a Valid Hight Number";
            }
            else addBlock.Text = "Please Insert a Valid Button Number";       
        }
        #endregion

        #region Bring Box Info
        private void BringBoxInfo(object sender, RoutedEventArgs e)
        {
            
            if (Validation.checkIfValidDouble(buttonInfo.Text))
            {
                if (Validation.checkIfValidDouble(highInfo.Text))
                {
                    boxInfoBlock.Text = boxManager.GetBox(Convert.ToDouble(buttonInfo.Text), Convert.ToDouble(highInfo.Text));
                }
                else boxInfoBlock.Text = "Please Insert a Valid Hight Number";
            }
            else boxInfoBlock.Text = "Please Insert a Valid Button Number";
        }
        #endregion

        #region Purchase
        private void Purchase(object sender, RoutedEventArgs e)
        {
           
            double res;
            if (Validation.checkIfValidDouble(buttonPurch.Text))
            {
                if (Validation.checkIfValidDouble(highPurch.Text))
                {
                    res = boxManager.Purchase(Convert.ToDouble(buttonPurch.Text), Convert.ToDouble(highPurch.Text));
                    if (res == (-1))
                    {
                        purchaseBlock.Text = "No BoxButton was found";
                    }
                    else if (res == 0)
                    {
                        purchaseBlock.Text = "No Suitable Hight was found";
                    }
                    else
                    {
                        purchaseBlock.Text = $"Purchase Was Made! you Got this button : {res} and this hight : {highPurch.Text}";
                    }
                }
                else purchaseBlock.Text = "Please Insert a Valid Hight Number";
            }
            else purchaseBlock.Text = "Please Insert a Valid Button Number";
        }
        #endregion

        #region Close Event
        void Close(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += window_Closing;
        }

        void window_Closing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            
        }

        #endregion

        public void UpdateTheAlerts(string alert)
        {
            alertsBlock.Text = alert; 
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            boxManager.OnClose();
        }
    }
}
