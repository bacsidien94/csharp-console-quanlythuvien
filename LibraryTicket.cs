using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlythuvien
{
    class LibraryTicket
    {

        // 09 thuộc tính của phiếu mượn sách
        public string readerID { get; set; }           //1. Mã đọc giả     
        public string bookID { get; set; }            //2. Mã sách
        public string isEN { get; set; }              //3. Loại sách - để xác định rate mượn sách  từ GeneralCode
        public string status { get; set; }            //4. Trạng thái phiếu mượn sách default là 1 đang mượn : 1-đang mượn/ 2-đã trả 
        public string fromDate { get; set; }          //5. Ngày mượn DD-MM-YYYY
        public string toDate { get; set; }            //6. Ngày trả default là null DD-MM-YYYY
        
        //revise overDays to int literally, why string T_T
        public string overDays { get; set; }          //7.Số ngày quá hạn default là 0 : được tính từ (6)-(5) -7 (số ngày free day từ GeneralCode) 
        public double standardRate { get; set; }      //8.Mức đóng phạt dựa trên loại sách - lấy từ GeneralCode default là 10000
        public double amount { get; set; }            //9.Số tiền phạt = (10)x(11)

        public static bool checkDatetime(string input)
        {
            //string current = DateTime.Now.ToString();
            //DateTime.Parse(input).ToShortDateString();
            DateTime value;
            if (!DateTime.TryParse(input, out value))
            {
                Console.Write("Vui long nhap gia tri hop le. ");
                return false;
            }
            else
            {
                return true;
            }
        }

        //public static void print_header_part()
        //{
        //    Console.WriteLine("\n{0,-30}{1,-30}{2,-18}", "Tên Đọc Giả", "Tên Sách", "Ngày Mượn");
        //}

        //hiển thị tên đọc giả / tên sách trong khi lưu mã đọc giả / mã sách / so ngay muon sach qua han / tien phat tuong ung
        public static void print_header()
        {
            Console.WriteLine("\n{0,-30}{1,-30}{2,-19}{3,-18}{4,-18}{5,-17}{6,-13}{7,-13}", 
                "Ten Doc Gia",
                "Ten Sach",
                "Loai Sach",
                "Ngay Muon",
                "Ngay Tra",
                "Qua Han (ngay)",
                "Don Gia",
                "Tien Phat");
        }

        //register a new library ticket with reader info, book info and start date (issue date)
        //revised
        public static void addTicket()
        {
            LibraryTicket newticket = new LibraryTicket();
            string readerID = "", bookID = "";
            Reader input_rd = new Reader();
            Book input_bk = new Book();
            MyReader.inquire_all_reader();
            Console.Write("Hay nhap Ma Doc Gia tuong ung: ");
                    
            while (input_rd.readerName == "")
            {
                readerID = Console.ReadLine();
                input_rd = Reader.inqReaderbyID(readerID);                
            }
            newticket.readerID = input_rd.readerID;
            
            //thông tin sách
            Book.print_header();
            MyLibrary.inquire_all_book();

            Console.Write("Hay nhap Ma Sach tuong ung: ");
            while (input_bk.bookName == "")
            {
                bookID = Console.ReadLine();
                input_bk = Book.inqBookbyID(bookID);                             
            }
            newticket.bookID = input_bk.bookName;
            newticket.isEN = input_bk.bookISBN;
            newticket.status = "1";//init

            Console.Write("Nhap ngay bat dau muon (vi du: 10/22/2018): ");
            string inputtedDate = Console.ReadLine();
            while (!checkDatetime(inputtedDate))
            {
                inputtedDate = Console.ReadLine();
            }
            newticket.fromDate = DateTime.Parse(inputtedDate).ToShortDateString().ToString();
            newticket.toDate = "";//init
            
            // Cấu hình thiết lập thông tin quá hạn mượn sách
            // Mỗi quyển sách được mượn tối đa 7 ngày (kể cả Thứ Bảy, Chủ Nhật):
            // Sách tiếng Việt sẽ bị phạt 10000d đồng/ngày trễ hạn
            // Sách Ngoại văn sẽ bị phạt 20000d đồng/ngày trễ hạn

            GeneralCode iniTicket = new GeneralCode(7, 10000d, 20000d);
            if (newticket.isEN == "1")
            {
                newticket.standardRate = iniTicket.StandardRateEN;
            }
            else
            {
                newticket.standardRate = iniTicket.StandardRateVN;
            }

            newticket.overDays = "0";//init
            newticket.amount = 0;//init

            //lưu thông tin đồng thời vào txt file và MyTicket 
            string newrecord = $"{newticket.readerID},{newticket.bookID},{newticket.isEN},{newticket.status},{newticket.fromDate},{newticket.toDate},{newticket.overDays},{newticket.standardRate},{newticket.amount}";
            List<string> lines = System.IO.File.ReadAllLines(@"../../myTicket.txt").ToList();
            lines.Add(newrecord);
            File.WriteAllLines(@"../../myTicket.txt", lines);
            MyTicket.mytk.Add(newticket);

            Console.WriteLine("\nTao Phieu Muon Sach thanh cong!\n");
            //in ket qua phieu muon sach
            MyTicket.inquire_a_ticket(MyTicket.mytk.Count-1);


        }

        //hàm này để lấy thông tin danh sách phiếu mượn sách LibraryTicket
        //tính ngày quá hạn cho đến thời điểm hiện tại bằng hàm overdayslistLibraryTicket
        //cập nhật lại thông tin cho phiếu mượn sách tương ứng
        //revised
        public static void updateLibraryTicket(string currentdate, string freeday)
        {
            string filePath = @"../../myTicket.txt";
            List<string> listTicket = new List<string>();
            listTicket = File.ReadAllLines(filePath).ToList();

           // LibraryTicket input = 
            for (int i = 0; i < MyTicket.mytk.Count; i++)
            {            
                //only apply to issued library ticket only, not apply to return library ticket
                //update to MyTicket.mk data directly
                if (MyTicket.mytk[i].status=="1")
                {
                    MyTicket.mytk[i].toDate = currentdate;//toDate  
                    MyTicket.mytk[i].overDays = calculateOverdays(MyTicket.mytk[i].fromDate, freeday.ToString());    //overDays
                    if (int.Parse(MyTicket.mytk[i].overDays) != 0)
                    {
                        //calculate amount
                        MyTicket.mytk[i].amount = MyTicket.mytk[i].standardRate * double.Parse(MyTicket.mytk[i].overDays);  //amount
                    }
                }
                //lưu kết quả lại vào listTicket[i]
                listTicket[i] = $"{MyTicket.mytk[i].readerID},{MyTicket.mytk[i].bookID},{MyTicket.mytk[i].isEN},{MyTicket.mytk[i].status},{MyTicket.mytk[i].fromDate},{MyTicket.mytk[i].toDate},{MyTicket.mytk[i].overDays},{MyTicket.mytk[i].standardRate},{MyTicket.mytk[i].amount}";
            }
            // update lại list ticket
            // ghi lại vào file txt từ List<string> tên listTicket
            File.WriteAllLines(@"../../myTicket.txt", listTicket);
        }

        //need revise
        public static void returnTicket(string ticketID)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            GeneralCode iniTicket = new GeneralCode(7, 10000d, 20000d);
            int freedays = iniTicket.FreeRentingDay;
            string currentDate = DateTime.Now.ToShortDateString();
            updateLibraryTicket(currentDate, freedays.ToString());
            string confirm = "0"; // dùng để xác định người dùng có chấp nhận trả sách hay không
            string filePath = @"../../myTicket.txt";
            List<string> myTicketlist = new List<string>();
            myTicketlist = File.ReadAllLines(filePath).ToList();
            bool edit_chk = false;
            for (int i = 0; i < myTicketlist.Count; i++)
            {
                //record là mỗi bản ghi trong list , eachNode là mỗi node của một bản ghi record
                string record = myTicketlist[i];
                string[] eachNode = record.Split(',');
                //nếu node thứ nhất (có index 0) của record khớp với giá trị đầu vào

                //chỉ cho phép trả sách nếu người dùng nhập đúng mã phiếu mượn sách và trạng thái phiếu mượn là 1 - đang mượn
                if (eachNode[0] == ticketID && eachNode[6] == "1")
                {

                    Console.WriteLine("Thong tin Phieu Muon Sach:\n");
                    Console.WriteLine("Ten Doc Gia: {0}", eachNode[2]);
                    Console.WriteLine("Ten Sach: {0}", eachNode[4]);
                    Console.WriteLine("Ngay Muon: {0}", eachNode[7]);
                    Console.WriteLine("Ngay Tra: {0}", currentDate);
                    if (int.Parse(eachNode[9]) > 0)
                    {
                        Console.WriteLine("Phieu muon sach qua han: {0} ngay", eachNode[9]);
                        string amount = double.Parse(eachNode[11]).ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
                        Console.WriteLine("De tra sach, vui long thanh toan so tien {0} \n", amount);
                    }
                    Console.Write("Xac nhan hoan tat: (1) Dong y - (0) Huy bo: "); confirm = Console.ReadLine();
                    if (confirm == "1")
                    {
                        Console.WriteLine("Tra sach hoan tat");
                        eachNode[6] = "2";
                    }
                    else
                    {
                        Console.WriteLine("Huy bo tac vu");
                    }
                    //lưu kết quả lại vào myTicketlist[i]
                    myTicketlist[i] = $"{eachNode[0]},{eachNode[1]},{eachNode[2]},{eachNode[3]},{eachNode[4]},{eachNode[5]},{eachNode[6]},{eachNode[7]},{eachNode[8]},{eachNode[9]},{eachNode[10]},{eachNode[11]}";
                    edit_chk = true;

                    break;
                }
                else
                {
                    edit_chk = false;
                }
            }

            if (edit_chk == false)
            {
                Console.WriteLine("Khong co thong tin Phieu Muon Sach tren");
                return;
            }
            else
            {
                // update lại list reader sau khi sửa
                // ghi lại vào file txt từ List<string> tên myTicketlist
                File.WriteAllLines(@"../../myTicket.txt", myTicketlist);
                // Console.WriteLine("Cập nhật danh sách phiếu mượn sách mới nhất");
                // LibraryTicket.inquireTicketlist();

            }
        }


        // tính ngày quá hạn từ ngày mượn cho tới ngày hiện tại, cập nhật ngày quá hạn
        public static string calculateOverdays(string fromDate, string freeday)
        {
            int overDays = 0;
            string sfromDate = DateTime.Parse(fromDate).ToShortDateString();
            string scurrentDate = DateTime.Now.ToShortDateString();
            DateTime currentDate = DateTime.Parse(scurrentDate);
            // TimeSpan difference = toDate - fromDate;
            TimeSpan difference = currentDate - DateTime.Parse(sfromDate);
            string days = difference.TotalDays.ToString();// số ngày mượn sách đến thời điểm hiện tại
            // test
            // Console.WriteLine("số ngày mượn sách là {0}", days); 
            if (int.Parse(days) > int.Parse(freeday))
            {
                overDays = int.Parse(days) - int.Parse(freeday);
            }
            //test
            // Console.WriteLine("số ngày mượn sách quá hạn là {0}", overDays.ToString());
            return overDays.ToString();
        }

        //revised
        public static void overdayslistLibraryTicket()
        {
            bool found_chk = false;
            LibraryTicket.print_header();
            for (int i = 0; i < MyTicket.mytk.Count; i++)
            {
                if (int.Parse(MyTicket.mytk[i].status) == 1 && int.Parse(MyTicket.mytk[i].overDays) != 0)
                {
                    // Reader.inqReaderbyID(MyTicket.mytk[i].readerID),
                    // Book.inqBookbyID(MyTicket.mytk[i].bookID),
                    Console.WriteLine("{0,-30}{1,-30}{2,-19}{3,-18}{4,-18}{5,-17}{6,-13}{7,-13}",
                                   Reader.inqReaderbyID(MyTicket.mytk[i].readerID).readerName,
                                   Book.inqBookbyID(MyTicket.mytk[i].bookID).bookName,
                                   Book.convertENVN(MyTicket.mytk[i].isEN),
                                   MyTicket.mytk[i].fromDate,
                                   MyTicket.mytk[i].toDate,
                                   MyTicket.mytk[i].overDays,
                                   Book.converPricetag(MyTicket.mytk[i].standardRate),
                                   Book.converPricetag(MyTicket.mytk[i].amount)
                                   );
                    found_chk = true;
                }
            }
            if (found_chk == false) {
                Console.WriteLine("\n Khong co thong tin Phieu Muon Sach qua han.");
            }
        }
    }
}
