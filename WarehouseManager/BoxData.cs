using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManager
{
    class BoxData : IComparable<BoxData>
    {
        public double Y { get; set; }
        //we keep a ref to the node in the link to faster access
        public LinkQueue<TimeData>.Node refNode { get; private set; }
        public int Count { get; private set; }

        public BoxData(double y,int count,LinkQueue<TimeData>.Node timeNodeRef)
        {
            Y = y;
            Count = count;
            refNode = timeNodeRef;
        }

        public BoxData(double y)
        {
            Y = y;
        }

        public int CompareTo(BoxData other)
        {
            return Y.CompareTo(other.Y);
        }

        internal void AddCount(int count)
        {
            Count += count;
        }
    }
}
