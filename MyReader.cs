using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Quanlythuvien
{
    class MyReader
    {
        public static List<Reader> myrd { get; set; }
      
        //init a list of library reader from txt file
        public static void init_lib_reader()
        {
            List<Reader> init = new List<Reader>();
            string filePath = @"../../myReader.txt";
            List<string> lst_reader = new List<string>();
            lst_reader = File.ReadAllLines(filePath).ToList();
            foreach (string record in lst_reader)
            {
                string[] reader_att = record.Split(',');

                Reader a = new Reader() { };
                a.readerID = reader_att[0];
                a.readerName = reader_att[1];
                a.readerMobile = reader_att[2];
                
                init.Add(a);
            }
            MyReader.myrd = init;

        }

        public static void inquire_all_reader()
        {
            Console.WriteLine("{0,-11}{1,-35}{2,-13}", "Ma Doc Gia", "Ten Doc Gia", "So Dien Thoai");
            for (int i = 0; i < MyReader.myrd.Count; i++)
            {
                inquire_a_reader(i);
            }
        }

        public static void inquire_a_reader(int index)
        {
            int i = index;
            Console.WriteLine("{0,-11}{1,-35}{2,-13}",
               MyReader.myrd[i].readerID,
               MyReader.myrd[i].readerName,
               MyReader.myrd[i].readerMobile);
        }

        public static int find_reader_index(string readerID)
        {
            int index = 0;//init
            for (int i = 0; i < MyReader.myrd.Count; i++)
            {
                if (MyReader.myrd[i].readerID == readerID)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
