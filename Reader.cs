using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace Quanlythuvien
{
    class Reader
    {
        // 04 thuộc tính của đọc giả
        public string readerID { get; set; }             //1. Mã đọc giả
        public string readerName { get; set; }             //2. Tên đọc giả
        public string readerMobile { get; set; }             //3. Số điện thoại


        public static bool checkmobilephone(string input)
        {
            long value;
            if (!long.TryParse(input, out value) || input.Length > 11 || input is null)
            {
                Console.WriteLine("Vui lòng nhập giá trị hợp lệ");
                return false;
            }
            else
            {
                return true;
            }
        }

        //revised
        public static bool checkexistedReaderID(string readerID)
        {
            bool isfound = false;
            for (int i = 0; i < MyReader.myrd.Count; i++)
            {
                if (MyReader.myrd[i].readerID == readerID)
                {
                    isfound = true;
                    Console.WriteLine("\nMã đọc giả {0} đã tồn tại", readerID);
                    break;
                }
                else
                {
                    isfound = false;
                }
            }
            return isfound;
        }
        /*public static bool checkexistedReaderID(string ReaderID)
        //{
        //    string filePath = @"../../myReader.txt";
        //    List<string> listreader = new List<string>();
        //    listreader = File.ReadAllLines(filePath).ToList();
        //    bool found_chk = false;

        //    for (int i = 0; i < listreader.Count; i++)
        //    {
        //        //record là mỗi bản ghi trong list , eachRecord là mỗi node của một bản ghi record
        //        string record = listreader[i];
        //        string[] eachRecord = record.Split(',');
        //        //nếu node thứ nhất (có index 0) của record khớp với giá trị đầu vào
        //        if (eachRecord[0] == ReaderID)
        //        {
        //            found_chk = true;
        //            break;
        //        }
        //        else
        //        {
        //            found_chk = false;
        //        }
        //    }
        //    if (found_chk == true)
        //    {
        //        Console.WriteLine("Mã đọc giả đã tồn tại");
        //    }
        //    else
        //    {
        //    }

        //    return found_chk;
        //}*/

        //revised
        public static void addReader()
        {
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            //List<Reader> myreader = new List<Reader>();
            Reader newreader = new Reader();

            string mobilephone = "";

            Console.Write("Nhập Mã Đọc giả: "); newreader.readerID = Console.ReadLine();

            while (checkexistedReaderID(newreader.readerID) || newreader.readerID.Length > 8)
            {
                Console.Write("Vui lòng nhập mã đọc giả khác tối đa 08 ký tự: ");
                newreader.readerID = Console.ReadLine();
            }


            Console.Write("Nhập Tên Đọc giả: "); newreader.readerName = Console.ReadLine();

            while (newreader.readerName.Length > 34)
            {
                Console.Write("Vui lòng nhập tên đọc giả tối đa 34 ký tự "); newreader.readerName = Console.ReadLine();
            }

            Console.Write("Nhập Số điện thoại của Đọc giả: "); mobilephone = Console.ReadLine();

            while (!checkmobilephone(mobilephone))
            {
                mobilephone = Console.ReadLine();
            }
            newreader.readerMobile = mobilephone;

            //thêm một bản ghi thông tin chi tiết đọc giả vào List Object Reader
            //lưu vào một biến string newrecord với dạng $ format cho gọn thay vì phải + từng chuỗi giá trị
            string newrecord = $"{newreader.readerID},{newreader.readerName},{newreader.readerMobile}";

            //mở file txt và lưu các bản ghi hiện có vào một List<string> tên lines
            List<string> lines = System.IO.File.ReadAllLines(@"../../myReader.txt").ToList();
            //thêm bản ghi mới nhất vào lines
            lines.Add(newrecord);
            //và lưu lại vào file myReader.txt
            File.WriteAllLines(@"../../myReader.txt", lines);
            //lưu thông tin đoc giả mới nhất vào danh sách đọc giả MyReader
            MyReader.myrd.Add(newreader);

            //indicate transaction is done, should comment before submission
            Console.WriteLine("\nThêm Đọc giả Đã Hoàn Tất!\n");
            MyReader.inquire_all_reader();
        }

        /* replaced by  MyReader.inquire_all_reader() and  MyReader.inquire_a_reader()
        //public static void listreader()
        //{
        //    Console.OutputEncoding = System.Text.Encoding.UTF8;
        //    string filePath = @"../../myReader.txt";
        //    List<string> listreader = new List<string>();
        //    listreader = File.ReadAllLines(filePath).ToList();

        //    Console.WriteLine("{0,-11}{1,-35}{2,-13}", "Mã Đọc giả", "Tên Đọc giả", "Số Điện thoại");
        //    foreach (string record in listreader)
        //    {
        //        string[] eachRecord = record.Split(',');
        //        Console.WriteLine("{0,-11}{1,-35}{2,-13}", eachRecord[0], eachRecord[1], eachRecord[2]);
        //    }

        //}*/

        //revised
        public static void inqReaderbyName(string readername)
        {
            List<int> indexmatched = new List<int>();

            for (int i = 0; i < MyReader.myrd.Count; i++)
            {
                if (MyReader.myrd[i].readerName.ToUpper().Contains(readername.ToUpper()))
                {
                    indexmatched.Add(i);
                }
                else { }
            }
            //nếu không có kết quả sách nào khớp với từ khoá tìm kiếm, in ra dòng kết quả như sau
            if (indexmatched.Count == 0) { Console.WriteLine("\nKhông tìm thấy đọc giả nào khớp\n"); }
            //nếu có kết quả sách khớp với từ khoá tìm kiếm, lần lượt in thông tin sách ra
            else
            {
                Console.WriteLine("\nDanh sách đọc giả:");
                Console.WriteLine("{0,-11}{1,-35}{2,-13}", "Mã Đọc giả", "Tên Đọc giả", "Số Điện thoại");
                //chạy một vòng lặp từ list các kết quả, đã được lưu vào indexmatched
                for (int x = 0; x < indexmatched.Count; x++)
                {
                    Console.WriteLine("{0,-11}{1,-35}{2,-13}",
                        MyReader.myrd[indexmatched[x]].readerID,
                        MyReader.myrd[indexmatched[x]].readerMobile,
                        MyReader.myrd[indexmatched[x]].readerMobile);
                }
            }
        }

        //revised
        public static Reader inqReaderbyID(string readerID)
        {
            bool apply_chk = false;
            Reader result = new Reader();
            for (int i = 0; i < MyReader.myrd.Count; i++)
            {
                if (MyReader.myrd[i].readerID == readerID)
                {
                    apply_chk = true;
                    //lấy thêm các thông tin còn lại
                    result.readerID = readerID;
                    result.readerName = MyReader.myrd[i].readerName;
                    result.readerMobile = MyReader.myrd[i].readerMobile;
                    break;
                }
                else
                {
                    apply_chk = false;
                }
            }
            if (apply_chk == false)
            {
                Console.WriteLine("\nKhông có mã Đọc giả trên");
                result.readerName = "";
            }

            return result;
        }

        // revised
        public static void editReaderbyID(string readerID)
        {
            string filePath = @"../../myReader.txt";
            List<string> listreader = new List<string>();
            listreader = File.ReadAllLines(filePath).ToList();
            Reader input = Reader.inqReaderbyID(readerID);
            if (input.readerName != "")
            {
                int rd_index = MyReader.find_reader_index(input.readerID);
                Console.WriteLine("Thực hiện việc sửa thông tin đọc giả có mã {0}", readerID);
                Console.Write("Nhập Tên Đọc giả mới: "); MyReader.myrd[rd_index].readerName = Console.ReadLine();
                while (MyReader.myrd[rd_index].readerName.Length > 34)
                {
                    Console.Write("Vui lòng nhập tên đọc giả tối đa 34 ký tự "); MyReader.myrd[rd_index].readerName = Console.ReadLine();
                }
                Console.Write("Nhập Số Điện thoại mới: ");
                string mobilephone = Console.ReadLine();

                while (!checkmobilephone(mobilephone))
                {
                    mobilephone = Console.ReadLine();
                }
                MyReader.myrd[rd_index].readerMobile = mobilephone;
                //lưu kết quả lại vào listreader[i]
                listreader[rd_index] = $"{MyReader.myrd[rd_index].readerID},{MyReader.myrd[rd_index].readerName},{MyReader.myrd[rd_index].readerMobile}";
                File.WriteAllLines(@"../../myReader.txt", listreader);

                Console.WriteLine("\nCập Nhật Thông Tin Đọc giả Đã Hoàn Tất!\n");
                Console.WriteLine("Cập nhật danh sách Đọc giả mới nhất");
                MyReader.inquire_all_reader();
            }
            else
            {
                Console.WriteLine("Không có mã Đọc giả {0}", readerID);
            }
        }

        //revised
        //need to check the relation with library ticket before deleting
        public static void delReaderbyID(string readerID)
        {
            string filePath = @"../../myReader.txt";
            List<string> listreader = new List<string>();
            listreader = File.ReadAllLines(filePath).ToList();

            Reader input = Reader.inqReaderbyID(readerID);
            if (input.readerName != "")
            {
                int rd_index = MyReader.find_reader_index(input.readerID);

                MyReader.myrd.Remove(MyReader.myrd[rd_index]);
                string reader_record = listreader[rd_index];
                listreader.Remove(reader_record);
                File.WriteAllLines(@"../../myReader.txt", listreader);

                Console.WriteLine("Đã xoá Đọc giả có mã {0} ", readerID);
                Console.WriteLine("Cập nhật danh sách Đọc giả mới nhất");
                MyReader.inquire_all_reader();
            }
            else
            {
                Console.WriteLine("Không có mã Đọc giả {0}", readerID);
            }


        }

    }

}
