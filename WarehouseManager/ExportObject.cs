using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DsLib;

namespace WarehouseManager
{
    // create json object
    public class ExportObject
    {
        public double _X { get; set; }      
        public double _Y { get; set; }
        public int Count { get; set; }
        public DateTime Time{ get; set; }
        public DateTime LastDateTime { get; set; }

        private static List<ExportObject> Objects = new List<ExportObject>();
        
        public static void SaveAndExport()
        {
            Objects.Clear();
            BoxManager.BoxCollection.ScanInOrder(x => x.Tree.ScanInOrder(y =>
            {
                Objects.Add(new ExportObject {_X = y.refNode.data.X, _Y = y.Y,Count = y.Count,Time = y.refNode.data.dateTime,LastDateTime = y.refNode.data.lastDateOnShelf });
            }));
            string JsonTxt = JsonConvert.SerializeObject(Objects,Formatting.Indented);
            File.WriteAllText("boxDatabase.json", JsonTxt);
        }
        public static void ReadFromJson()
        {
            string JsonConv = File.ReadAllText("boxDatabase.json");
            List<ExportObject> exportObjects = JsonConvert.DeserializeObject<List<ExportObject>>(JsonConv);
            BoxManager boxManager = new BoxManager();
            if (exportObjects == null) return;
            //sort the json and insert them by order
            List<ExportObject> ObjectsAfterSort = exportObjects.OrderBy(obj => obj.Time).ToList();
            ObjectsAfterSort.ForEach(x => boxManager.Add(x._X, x._Y, x.Count, true, x));
            
           
        }
        

    }
}
