using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpProject
{
    public class DataManager
    {
        public string getFileNameByFolder(int[] ymd, string folderName, out bool isokay)
        {
            isokay = false;
            string dir = folderName + "\\";
            string zero1 = ".";
            string zero2 = ".";
            if (ymd[1] < 10)
                zero1 = ".0";
            if (ymd[2] < 10)
                zero2 = ".0";
            dir += "kemp-abh-sensor-" + ymd[0] + zero1 + ymd[1] + zero2 +  ymd[2] + ".csv";
            FileInfo fi = new FileInfo(dir);
            if (fi.Exists)
            {
                isokay = true;
            }
            else
                isokay = false;
            return dir;
        }

        public string getFileName(string fileName, out bool isokay)
        {
            string dir = fileName;
            FileInfo fi = new FileInfo(dir);
            if (fi.Exists)
            {
                isokay = true;
            }
            else
                isokay = false;
            return dir;
        }

        public string[] timeSplit(string time) // list[0] = hour ,list[1] = minute ,list[2] = second
        {
            string[] list = time.Split(':');
            if (list[0].Contains("오전"))
            {
                list[0] = list[0].Substring(list[0].LastIndexOf(' ') + 1);
            }
            else if (list[0].Contains("오후"))
            {
                list[0] = (int.Parse(list[0].Substring(list[0].LastIndexOf(' ') + 1)) + 12).ToString();
            }

            return list;
        }

        public List<Data> getData(string dir)
        {
            List<Data> datalist = new List<Data>();
            DataReader reader = new DataReader();
            datalist = reader.readDataFile(dir);
            return datalist;          
        }
    }
}
