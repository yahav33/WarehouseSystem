using DsLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WarehouseManager
{
     public class BoxManager
    {
        
         internal static BST<BoxButton> BoxCollection = new BST<BoxButton>();//create the main tree
         private static LinkQueue<TimeData> TimeDataQueue = new LinkQueue<TimeData>();
         private List<ExportObject> JsonObjects = new List<ExportObject>();
        
        public string GetTreeBuild()
        {
            return File.ReadAllText("boxDatabase.json");
            
        }
        #region Add to the tree
        public string Add(double x, double y, int count,bool import,ExportObject obj = null)
        {
            
            BoxButton bTmp = new BoxButton(x);
            BoxButton foundBtn;
            if (BoxCollection.Find(out foundBtn, bTmp))
            {
                BoxData dTmp = new BoxData(y);
                BoxData foundData;
                if (foundBtn.Tree.Find(out foundData, dTmp))
                {
                    if (!(count + foundData.Count > Constants.MaxCountCapacity))
                    {
                        foundData.AddCount(count);
                        return count + " was added successfully";
                    }
                    else
                    {
                        string alert; 
                        int amount = Constants.MaxCountCapacity - foundData.Count;
                        foundData.AddCount(amount);
                        alert = $"we added only {amount} to the boxes, and return {Math.Abs(count - amount)} to the supplier";
                        Constants.BoxAlerts.Add(alert);
                       
                        return $"You tried to add more than the capacity thats allowed!\n" +
                            $"the amount that was added is {amount}";
                    }
                    
                }
                else
                {
                    if (!import)// if this is real time adding
                        TimeDataQueue.EnQueue(new TimeData(x, y));
                   else //  if i import from json no change in the time stamp
                        TimeDataQueue.EnQueue(new TimeData(x, y, obj.Time, obj.LastDateTime));
                    foundBtn.Tree.Add(new BoxData(y, count,TimeDataQueue.End));
                    return "New box hight was added!";
                }
            }
            else
            { 
                if(!import)
                    TimeDataQueue.EnQueue(new TimeData(x, y));
                else
                    TimeDataQueue.EnQueue(new TimeData(x, y, obj.Time,obj.LastDateTime));
                BoxCollection.Add(new BoxButton(x, y, count,TimeDataQueue.End));
                return "New box was added!";
            }
            

        }
        #endregion

        #region Purchase
        public double Purchase(double x, double y)
        {
            BoxButton chosenOne;
            BoxButton tBoxButton = new BoxButton(x);
            BoxCollection.FindExactOrClose(out chosenOne, tBoxButton);
            if (chosenOne != null)
            {
                BoxData dTmp = new BoxData(y);
                BoxData foundData;
                chosenOne.Tree.Find(out foundData, dTmp);
                if (foundData == null)
                    return 0; // No Suitable hight was found
                else
                {
                    foundData.AddCount(-1);
                    int status = TimeDataQueue.PollFromTheMiddle(foundData.refNode);
                    TimeStampChange(status,foundData.refNode,foundData);
                    if (CheckIfCountIsUnderTheMin(foundData))
                    {
                        string alert = $"Purchase was made!\nThe count was under the min amount of the box ({chosenOne.X}, {foundData.Y}) now you have : {foundData.Count} boxes left";
                        if(foundData.Count != 0) Constants.BoxAlerts.Add(alert);
                        
                        
                    }
                    CheckIfItWasLastItem(foundData,chosenOne);
                    ExportObject.SaveAndExport();// after all removing we save to json
                    return chosenOne.X; // Purchase was made                      
                }
            }
            else
            {
                return -1; // No BoxButtom was found
            }


        }
        #endregion

        #region Time Stamp Change
        private void TimeStampChange(int status,LinkQueue<TimeData>.Node refTmp,BoxData tmpBoxData)
        {
            // func that update the time stamp
            switch (status)
            {
                //case -2 empty(wont get here)
                //case 1 Pooled form the middle
                case -1:
                    refTmp.data.UpdateTime(); // End State, Update time only
                    break;
                case 0://if the node is start
                    TimeData tmp = null;
                    TimeDataQueue.DeQueue(out tmp);
                    if (tmpBoxData.Count > 0)
                    {
                        tmp.UpdateTime();
                        TimeDataQueue.EnQueue(tmp);
                    }
                    break;
            }
        }
        #endregion

        #region CheckIfItWasLastItem
        private void CheckIfItWasLastItem(BoxData foundData,BoxButton chosenOne)
        {
            if (foundData.Count <= 0)
            {
                string capacityalert = $"The hight : {foundData.Y} of box {chosenOne.X} ,has been removed!, please restock!";
                
                chosenOne.Tree.RemoveFromTree(foundData);
                if(chosenOne.Tree.IsEmpty())
                {
                    BoxCollection.RemoveFromTree(chosenOne);
                    // remove all the leaf if the inside tree(Y) is empty
                    capacityalert = $"The Bottom of the box : {chosenOne.X}, has been removed! please restock!";
                }
                Constants.BoxAlerts.Add(capacityalert);
            }
        }
        #endregion

        #region Check Over Dated Time
        public bool CheckOverDatedTime()
        {
            //run by timer to check valid of the boxes
            TimeData tmpTimeData;
            try
            {
                if (DateTime.Now >= TimeDataQueue.Start.data.lastDateOnShelf)
                {
                    // get in if the start of the queue is not valid any more
                    double x = TimeDataQueue.Start.data.X;
                    double y = TimeDataQueue.Start.data.Y;
                    // then we need the obj itself to remove him from the stock
                    if (TimeDataQueue.DeQueue(out tmpTimeData))
                    {
                        RemoveBox(x, y);
                        CheckOverDatedTime();
                        return true;
                    }
                    return false;
                }

            }
            catch (Exception)
            {// we remove all the tree 
                return false;
            }
            return false;
        }
        #endregion

        #region Check Valid Count - 2 func
        private bool CheckIfCountIsUnderTheMin(BoxData foundData)
        {
            return foundData.Count <= Constants.MinCountCapacity;
        }
        private bool CheckCountValid(int count)
        {
            return count <= Constants.MaxCountCapacity;
        }
        #endregion

        #region GetBoxInfo
        public string GetBox(double x,double y)
        {
            BoxButton bTmp = new BoxButton(x);
            BoxButton foundBtn;
            if (BoxCollection.Find(out foundBtn, bTmp))
            {
                BoxData dTmp = new BoxData(y);
                BoxData foundData;
                if (foundBtn.Tree.Find(out foundData, dTmp))
                {
                    return $"This box has {foundBtn.X} as X(Button) and {foundData.Y} as Y(Hight) and {foundData.Count} Pieces, " +
                        $"and the date of adding is {foundData.refNode.data.dateTime}";
                }
                else
                {
                    return "No box was found";
                }
            }
            else
            {
                return "No box was found";
            }

        }

        #endregion

        #region Remove Box Time expired
        public bool RemoveBox(double x, double y)
        {
            // removing func
            BoxButton bTmp = new BoxButton(x);
            BoxButton foundBtn;
            if (BoxCollection.Find(out foundBtn, bTmp))
            {
                BoxData dTmp = new BoxData(y);
                BoxData foundData;
                if (foundBtn.Tree.Find(out foundData, dTmp))
                {
                    foundBtn.Tree.RemoveFromTree(foundData);
                    string alert = $"we removed box with hight {foundData.Y} of this button {foundBtn.X}";
                    if (foundBtn.Tree.IsEmpty())
                    {
                        BoxCollection.RemoveFromTree(foundBtn);
                        alert = $"we removed the box with {x} button, and {y} hight because the box got old!";
                    }
                    Constants.BoxAlerts.Add(alert);
                    return true;

                }
                else
                {
                    return false; 
                }
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region On Close
        public void OnClose()
        {
            //Export current objects to json
            ExportObject.SaveAndExport();
        }
        #endregion

        public void addToTheQueue(TimeData data)
        {
            TimeDataQueue.EnQueue(data);
        }


    }
}
