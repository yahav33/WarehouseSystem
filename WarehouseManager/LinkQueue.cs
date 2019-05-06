using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManager
{
    public class LinkQueue<T>
    {
        public Node Start { get; private set; }
        public Node End { get; private set; }
       
        // take from the start
        public bool DeQueue(out T item)
        {
            item = default(T);
            if (IsEmpty()) return false;
            item = Start.data;
            Start = Start.next;
            if (Start != null)
            {
                Start.Previous = null;
                
            }
            else End = Start;
           
            return true;
        }
        
        //just pick not removed!
        public T Pick()
        {
            return Start.data;
        }

        //check the line if empty
        public bool IsEmpty()
        {
            if (Start == null) return true;
            return false;
        }

        //add to the end
        public void EnQueue(T item)
        {
            Node newNode = new Node(item);
            if (Start == null)
            {
                Start = End = newNode;
            }
            else
            {
                End.next = newNode;
                newNode.Previous = End;
                End = newNode;
            }
           
        }

        // take from the middle with a ref,straight access 
        //using int because each case we handling different
        public int PollFromTheMiddle(Node TimeTmpData)
        {
            if (IsEmpty()) return -2;
            if (TimeTmpData == End) return -1;
            if (TimeTmpData == Start) return 0;
            (TimeTmpData.Previous).next = TimeTmpData.next;
            (TimeTmpData.next).Previous = TimeTmpData.Previous;
           
            return 1;
        }

       

        public class Node
        {
            public T data;
            public Node next;
            public Node Previous;
            public Node(T data)
            {
                this.data = data;
            }
        }
    }
}
