using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DsLib;

namespace WarehouseManager
{
    class BoxButton : IComparable<BoxButton>
    {
        public double X { get; set; }
        public BST<BoxData> Tree;

        public BoxButton(double x,double Y,int count,LinkQueue<TimeData>.Node tmpNodeRef)
        {
            X = x ;
            Tree = new BST<BoxData>();
            Tree.Add(new BoxData(Y, count,tmpNodeRef));
            
        }

        public BoxButton(double x)
        {
            X = x;
            Tree = null;
        }
        
        public int CompareTo(BoxButton other)
        {
            return X.CompareTo(other.X);
        }
    }
}
