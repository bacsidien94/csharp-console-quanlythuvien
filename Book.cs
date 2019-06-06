using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Quanlythuvien
{
    class Book
    {
        /* class sách  thể có 08 attributes
        mã sách, tên sách, tác giả, nhà xuất bản, giá sách 
        Có hai loại sách: sách tiếng Việt và sách Ngoại văn (có thêm ISBN)
        trạng thái để xác định sách có thể cho mượn được ko 
        mặc định số lượng mỗi loại sách là 1
        */
        // 09 thuộc tính của sách 
        public string bookID { get; set; }            //1. Mã sách
        public string bookName { get; set; }          //2. Tên sách
        public string bookAuthor { get; set; }        //3. Tác giả 
        public string bookPublisher { get; set; }     //4. Nhà xuất bản     
        public double bookPriceTag { get; set; }      //5. Giá sách
        public string isEN { get; set; }              //6. Loại sách
        public string bookISBN { get; set; }          //7. ISBN 

       // public bool isOccupied { get; set; }          //8. check availability next release

        //hàm điều chỉnh loại ngôn ngữ sách
        public static string convertENVN(string input)
        {
            string convert;
            if (input == "1")
            {
                convert = "Sach Ngoai Van";
            }
            else { convert = "Sach Tieng Viet"; }
            //  var convert = int.Parse(input) == 1 ? "Sách Ngoại Văn" : "Sách Tiếng Việt"; //error cannot solve T_T
            return convert;
        }

        //not in use
        /*
        public static string convertStatus(bool input)
        {
            string convert;
            if (input == false)
            {
                convert = "OK";
            }
            else { convert = "N/A"; }
            //  var convert = int.Parse(input) == 1 ? "OK" : "N/A"; //error cannot solve T_T
            return convert;
        }
        */
        public static void print_header()
        {
            Console.WriteLine("\n{0,-8}{1,-30}{2,-30}{3,-20}{4,-13}{5,-18}{6,-18}\n",
                "Ma Sach",
                "Ten Sach",
                "Ten Tac Gia",
                "NXB",
                "Gia",
                "Ngon Ngu",
                "ISBN");

        }
        //hàm validate giá tiền sách
        public static bool convertPrice(string input)
        {
            double temp;
            if (!double.TryParse(input, out temp) && input != "0")
            {
                Console.WriteLine("Vui long nhap Gia Tien bang chu so toi da 13 ky tu");
                return false;
            }
            else { return true; }

        }

        public static string converPricetag(double input)
        {
            //var pricetag = input.ToString("C0");
            //lỗi hiển thị ký tự đ
            var pricetag = input.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
        
            return pricetag;
        }

        //hàm truy vấn sách theo ID sách để kiểm tra mã sách đã tồn tại hay chưa
        //revised
        public static bool checkexistedbookID(string bookID)
        {
            bool isfound = false;

            for (int i = 0; i < MyLibrary.mylib.Count; i++)
            {
                if (MyLibrary.mylib[i].bookID == bookID)
                {
                    isfound = true;
                    Console.WriteLine("\nMa Sach {0} da ton tai", bookID);
                    break;
                }
                else
                {
                    isfound = false;
                }
            }
            return isfound;
        }


        //hàm thêm thông tin sách
        public static void addBook()
        {
            Book newbook = new Book();
            string inputprice;

            Console.Write("Nhap Ma Sach: "); newbook.bookID = Console.ReadLine();
            //validation
            while (checkexistedbookID(newbook.bookID) || newbook.bookID.Length > 8)
            {
                //Console.Write("Vui lòng nhập mã sách tối đa 08 ký tự hoặc mã sách đã tồn tại ");
                Console.Write("Vui long nhap Ma Sach khac toi da 08 ky tu: ");
                newbook.bookID = Console.ReadLine();
            }

            Console.Write("Nhap Ten Sach: "); newbook.bookName = Console.ReadLine();
            //validation
            while (newbook.bookName.Length > 30)
            {
                Console.Write("Vui long nhap Ten Sach toi da 30 ky tu: "); newbook.bookName = Console.ReadLine();
            }

            Console.Write("Nhap Ten Tac Gia: "); newbook.bookAuthor = Console.ReadLine();
            //validation
            while (newbook.bookAuthor.Length > 30)
            {
                Console.Write("Vui long nhap Ten Tac Gia toi da 30 ky tu: ");
                newbook.bookAuthor = Console.ReadLine();
            }

            Console.Write("Nhap Nha Xuat Ban: "); newbook.bookPublisher = Console.ReadLine();
            // validation
            while (newbook.bookPublisher.Length > 20)
            {
                Console.Write("Vui long nhap Nha Xuat Ban toi da 20 ky t"); newbook.bookPublisher = Console.ReadLine();
            }

            Console.Write("Nhap Gia Sach: "); inputprice = Console.ReadLine();
            //validation
            // kiểm tra giá trị nhập phải là giá trị số
            while (!convertPrice(inputprice) || inputprice.Length > 13)
            {
                //nếu không phải là số thì yêu cầu nhập lại giá trị hợp lệ
                inputprice = Console.ReadLine();
            }
            newbook.bookPriceTag = double.Parse(inputprice);

            Console.WriteLine("Nhap Loai Sach: ");
            Console.Write("Sach Tieng Viet - nhap (0) | Sach Ngoai Van - nhap (1): "); newbook.isEN = Console.ReadLine();
            // kiểm tra giá trị nhập phải là 0 hoặc 1
            //validation
            while (newbook.isEN != "0" && newbook.isEN != "1")
            {
                Console.WriteLine("Khong hop le");
                Console.Write("Sach Tieng Viet - nhap (0) | Sach Ngoai Van - nhap (1): "); newbook.isEN = Console.ReadLine();
            }
            //Nếu là sách Tiếng Việt sẽ không có ISBN - Nếu là sách Ngoại Văn, yêu cầu nhập ISBN
            if (newbook.isEN == "1")
            {
                Console.Write("Nhap Ma ISBN: "); newbook.bookISBN = Console.ReadLine();
                while (newbook.bookISBN.Length > 13)
                {
                    Console.Write("Vui long nhap ma ISBN toi da 13 ky tu: "); newbook.bookISBN = Console.ReadLine();
                }
            }
            else { newbook.bookISBN = "NA"; } //gia tri mac dinh la NA - not available

            //newbook.isOccupied = false; //init
            //thêm một bản ghi thông tin chi tiết một cuốn sách hoàn chỉnh vào thư viện sách mylib 
            //lưu vào một biến string newrecord với dạng $ format thay thế cho gọn thay vì phải + từng chuỗi giá trị
            string newrecord = $"{newbook.bookID},{newbook.bookName},{newbook.bookAuthor},{newbook.bookPublisher},{newbook.bookPriceTag},{newbook.isEN},{newbook.bookISBN}";//,{newbook.isOccupied}";

            //mở file txt và lưu các bản ghi hiện có vào một List<string> tên lines
            List<string> allrecords = System.IO.File.ReadAllLines(@"../../myLibrary.txt").ToList();
            //thêm bản ghi mới nhất vào lines
            allrecords.Add(newrecord);
            //tới đây biến lines sẽ
            //bao gồm các bản ghi thể hiện các sách đã có trong thư viện
            //và một bản ghi mới của sách ta mới tạo ra
            //lưu thông tin hiện lại vào file myLibrary.txt
            File.WriteAllLines(@"../../myLibrary.txt", allrecords);
            //lưu thông tin sách mới vào thư viện MyLibrary
            MyLibrary.mylib.Add(newbook);

            Console.WriteLine("\nThem sach da hoan tat!\n");
            Book.print_header();
            MyLibrary.inquire_a_book(MyLibrary.mylib.Count - 1);

        }

        
        //hàm này thực hiện chức năng in danh sách các sách hiện có trong file        
        //hàm này được chạy sau khi người dùng hoàn tất một tác vụ vd nhu thêm xoá sửa sách
        //có ý nghĩa vừa kiểm tra lại kết quả vừa làm, vừa xem danh sách thư viện mới nhất
        //đã được thay thế bằng hàm MyLibrary.inquire_a_book();
        
            /*
        public static void listbook()
        {
            string filePath = @"../../myLibrary.txt";
            List<string> listbook = new List<string>();
            listbook = File.ReadAllLines(filePath).ToList();
            print_header();
            foreach (string record in listbook)
            {
                string[] eachRecord = record.Split(',');
                Console.WriteLine("{0,-8}{1,-30}{2,-30}{3,-20}{4,-13}{5,-18}{6,-18}", eachRecord[0], eachRecord[1], eachRecord[2], eachRecord[3], Book.converPricetag(double.Parse(eachRecord[4])), Book.convertENVN(eachRecord[5]), eachRecord[6]);
            }
        } 
        */

        /*hàm truy vấn sách theo tên sách
        revised*/
        public static void inqBookbyName(string inputhere)
        {
            List<int> indexmatched = new List<int>();

            for (int i = 0; i < MyLibrary.mylib.Count; i++)
            {
                if (MyLibrary.mylib[i].bookName.ToUpper().Contains(inputhere.ToUpper()))
                {
                    indexmatched.Add(i);
                }
                else { }
            }
            //nếu không có kết quả sách nào khớp với từ khoá tìm kiếm, in ra dòng kết quả như sau
            if (indexmatched.Count == 0) { Console.WriteLine("\nKhong co thong tin sach tren\n"); }
            //nếu có kết quả sách khớp với từ khoá tìm kiếm, lần lượt in thông tin sách ra
            else
            {
                //nếu có kết quả thì in ra thông tin sách có thứ tự như sau
                Console.WriteLine("\nDanh sach ket qua:");
                Book.print_header();
                //chạy một vòng lặp, lấy các thông tin sách theo list các index khớp với điều kiện và đã được lưu vào indexmatched
                for (int x = 0; x < indexmatched.Count; x++)
                {
                    Console.WriteLine("{0,-8}{1,-30}{2,-30}{3,-20}{4,-13}{5,-18}{6,-18}",
                       MyLibrary.mylib[indexmatched[x]].bookID,
                       MyLibrary.mylib[indexmatched[x]].bookName,
                       MyLibrary.mylib[indexmatched[x]].bookAuthor,
                       MyLibrary.mylib[indexmatched[x]].bookPublisher,
                       Book.converPricetag(MyLibrary.mylib[indexmatched[x]].bookPriceTag),
                       Book.convertENVN(MyLibrary.mylib[indexmatched[x]].isEN),
                       MyLibrary.mylib[indexmatched[x]].bookISBN);
                }
            }
        }

        /*hàm truy vấn sách theo ID sách
        kq trả về là một class object
        revised*/
        public static Book inqBookbyID(string bookID)
        {
            bool apply_chk = false;
            Book result = new Book();
            for (int i = 0; i < MyLibrary.mylib.Count; i++)
            {
                if (MyLibrary.mylib[i].bookID == bookID)
                {
                    apply_chk = true;
                    //lấy thêm các thông tin còn lại                   
                    result.bookID = bookID;
                    result.bookName = MyLibrary.mylib[i].bookName;
                    result.bookAuthor = MyLibrary.mylib[i].bookAuthor;
                    result.bookPublisher = MyLibrary.mylib[i].bookPublisher;
                    result.bookPriceTag = MyLibrary.mylib[i].bookPriceTag;
                    result.isEN = MyLibrary.mylib[i].isEN;
                    result.bookISBN = MyLibrary.mylib[i].bookISBN;
                    break;
                }
                else
                {
                    apply_chk = false;
                }
            }
            if (apply_chk == false)
            {
                Console.WriteLine("\nKhong co Ma Sach tren");
                result.bookName = "";
            }

            return result;
        }

        //hàm cho phép chỉnh sửa thông tin sách theo mã sách 
        //revised
        public static void editBookbyID(string bookID)
        {
            string filePath = @"../../myLibrary.txt";
            List<string> listbook = new List<string>();
            listbook = File.ReadAllLines(filePath).ToList();

            Book input = Book.inqBookbyID(bookID);
            if (input.bookName != "")
            {
                //get lib index of the candidate book for modifying
                int lib_index = MyLibrary.find_lib_index(input.bookID);

                Console.WriteLine("Thuc hien viec sua thong tin sach co Ma Sach {0}", bookID);

                Console.Write("Nhap Ten Sach: "); MyLibrary.mylib[lib_index].bookName = Console.ReadLine();
                while (MyLibrary.mylib[lib_index].bookName.Length > 30)
                {
                    Console.Write("Vui long nhap Ten Sach toi da 30 ky tu: ");
                    MyLibrary.mylib[lib_index].bookName = Console.ReadLine();
                }

                Console.Write("Nhap Ten Tac Gia: "); MyLibrary.mylib[lib_index].bookAuthor = Console.ReadLine();
                while (MyLibrary.mylib[lib_index].bookAuthor.Length > 30)
                {
                    Console.Write("Vui long nhap ten tac gia toi da 30 ky tu: "); MyLibrary.mylib[lib_index].bookAuthor = Console.ReadLine();
                }

                Console.Write("Nhap Nha Xuat Ban: "); MyLibrary.mylib[lib_index].bookPublisher = Console.ReadLine();
                while (MyLibrary.mylib[lib_index].bookPublisher.Length > 20)
                {
                    Console.Write("Vui long nhap Nha Xuat Ban toi da 20 ky tu: "); MyLibrary.mylib[lib_index].bookPublisher = Console.ReadLine();
                }

                string inputprice;
                Console.Write("Nhap Gia Sach: "); inputprice = Console.ReadLine();
                while (!convertPrice(inputprice) || inputprice.Length > 13)
                {
                    inputprice = Console.ReadLine();
                }
                MyLibrary.mylib[lib_index].bookPriceTag = double.Parse(inputprice);

                listbook[lib_index] = $"{MyLibrary.mylib[lib_index].bookID},{MyLibrary.mylib[lib_index].bookName},{MyLibrary.mylib[lib_index].bookAuthor},{MyLibrary.mylib[lib_index].bookPublisher},{MyLibrary.mylib[lib_index].bookPriceTag},{MyLibrary.mylib[lib_index].isEN},{MyLibrary.mylib[lib_index].bookISBN}";
                File.WriteAllLines(@"../../myLibrary.txt", listbook);

                Console.WriteLine("\nCap nhat thong tin sach da hoan tat!\n");
                Console.WriteLine("Cap nhat Thu Vien Sach moi nhat\n");
                Book.print_header();
                MyLibrary.inquire_all_book();
            }
            else
            {
                Console.WriteLine("Khong co thong sach voi Ma Sach {0}", bookID);
            }
        }

        //hàm cho phép chỉnh xoá thông tin sách theo mã sách
        //revised
        //need to check the relation with library ticket before deleting
        public static void delBookbyID(string bookID)
        {
            string filePath = @"../../myLibrary.txt";
            List<string> listbook = new List<string>();
            listbook = File.ReadAllLines(filePath).ToList();

            Book input = Book.inqBookbyID(bookID);
            if (input.bookName != "")
            {
                //get lib index of the candidate book for modifying
                int lib_index = MyLibrary.find_lib_index(input.bookID);

                MyLibrary.mylib.Remove(MyLibrary.mylib[lib_index]);
                string book_record = listbook[lib_index];
                listbook.Remove(book_record);
                File.WriteAllLines(@"../../myLibrary.txt", listbook);

                Console.WriteLine("Da xoa thong tin sach voi Ma Sach {0} ", input.bookID);
                Console.WriteLine("Cap nhat Thu Vien Sach moi nhat\n");
                Book.print_header();
                MyLibrary.inquire_all_book();
            }
            else { Console.WriteLine("Khong co thong tin sach voi Ma Sach {0}", bookID); }
        }
    }

}
