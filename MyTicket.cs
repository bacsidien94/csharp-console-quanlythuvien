using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Quanlythuvien
{
    class MyTicket
    {
        public static List<LibraryTicket> mytk { get; set; }

        //init a list of library tickets from txt file
        public static void init_lib_ticket()
        {
            List<LibraryTicket> init = new List<LibraryTicket>();
            string filePath = @"../../myTicket.txt";
            List<string> lst_lib_ticket = new List<string>();
            lst_lib_ticket = File.ReadAllLines(filePath).ToList();
            foreach (string record in lst_lib_ticket)
            {
                // print to test, comment before submitting
                // Console.WriteLine(record);
                string[] lib_ticket_att = record.Split(',');

                LibraryTicket a = new LibraryTicket() { };
                a.readerID = lib_ticket_att[0];
                a.bookID = lib_ticket_att[1];
                a.isEN = lib_ticket_att[2];
                a.status = lib_ticket_att[3];
                a.fromDate = lib_ticket_att[4];
                a.toDate = lib_ticket_att[5];
                a.overDays = lib_ticket_att[6];
                a.standardRate = double.Parse(lib_ticket_att[7]);
                a.amount = double.Parse(lib_ticket_att[8]);
                init.Add(a);
            }
            MyTicket.mytk = init;
            // print to test, comment before submitting
            // Console.WriteLine("Total {0} Ticket(s).\n", MyTicket.mytk.Count); 
        }

        //in thông tin đã được lưu thực tế trong txt file
        public static void inquire_all_lib_ticket()
        {
            LibraryTicket.print_header();
            for (int i = 0; i < MyTicket.mytk.Count; i++)
            {
                Console.WriteLine("{0,-8}{1,-8}{2,-12}{3,-12}{4,-16}{5,-16}{6,-15}{7,-8}{8,-8}",
                MyTicket.mytk[i].readerID,
                MyTicket.mytk[i].bookID,
                MyTicket.mytk[i].isEN,
                MyTicket.mytk[i].status,
                MyTicket.mytk[i].fromDate,
                MyTicket.mytk[i].toDate,
                MyTicket.mytk[i].overDays,
                MyTicket.mytk[i].standardRate.ToString(),
                MyTicket.mytk[i].amount.ToString()
                );
            }
        }
        
        //in thông tin in ra kết quả sau khi tạo phiếu mượn
        public static void inquire_a_ticket(int index)
        {
            string reader_name = string.Empty;
            string book_name = string.Empty;

            reader_name = Reader.inqReaderbyID(MyTicket.mytk[index].readerID).readerName;
            book_name = Book.inqBookbyID(MyTicket.mytk[index].bookID).bookName;

            Console.WriteLine("Thong tin Phieu Muon Sach:\n");
            Console.WriteLine("Ten Doc Gia", reader_name);
            Console.WriteLine("Ten Sach", book_name);
            Console.WriteLine("Muon Tu", MyTicket.mytk[index].fromDate);
        }
      
        public static int find_tk_index(string bookID,string readerID)
        {
            int index = 0;//init
            for (int i = 0; i < MyTicket.mytk.Count; i++)
            {
                if (MyTicket.mytk[i].bookID == bookID && MyTicket.mytk[i].readerID == readerID)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
