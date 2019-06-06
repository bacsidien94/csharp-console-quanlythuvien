using System;


namespace Quanlythuvien
{
    class Program
    {
        static void Main(string[] args)
        {
            //cho phép hiển thị UTF-8 string (Tiếng Việt) trong Console Application
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            Console.SetWindowSize(180, 30);

            //khoi gan thong tin thu vien: sach / doc gia / phieu muon sach tu file txt file
            MyLibrary.init_library();
            MyReader.init_lib_reader();
            MyTicket.init_lib_ticket();

            //testing new function

            // chương trình bắt đầu 
            menuScreen();
        }
        //hàm in màn hình chương trình menu cho console - nhan gia tri dau vao
        static void menuScreen()
        {
            string selection = string.Empty;
            int value = 99;
            Console.WriteLine("---CHUONG TRINH QUAN LY THU VIEN---\n\n"
                         + "Danh sach cac chuc nang:\n"
                         + "\t1. Them sach\n"
                         + "\t2. Xoa sach\n"
                         + "\t3. Sua sach\n"
                         + "\t4. Tim kiem sach\n"
                         + "\t5. Them doc gia\n"
                         + "\t6. Xoa doc gia\n"
                         + "\t7. Sua doc gia\n"
                         + "\t8. Tim kiem doc gia\n"
                         + "\t9. Lap phieu muon sach\n"
                         + "\t10.Lap phieu tra sach\n"
                         + "\t11.Liet ke danh sach muon sach tre han\n"
                         + "\t12.Danh muc sach co trong thu vien\n"
                         + "\t0. Thoat\n");
            do
            {
                // kiểm tra dữ liệu đầu vào
                // nếu thoả thì cho phép truyền giá trị này vào hàm tương ứng (1-11)
                // nên gán lại giá trị selection trong mỗi lần chạy vòng lặp, nếu để bên ngoài, vòng lặp sẽ chạy infinite sẽ không đạt yêu cầu
                Console.Write("\nVui long chon chuc nang can thuc hien: ");
                selection = Console.ReadLine();
                if (selection == "0")
                { // nếu là 0 thì kết thúc chương trình
                    Console.Write("\nKet thuc chuong trinh");
                    Console.ReadLine();
                }
                else if (!int.TryParse(selection, out value))
                {// nếu không phải số thì yêu cầu nhập lại
                    Console.Write("\nVui long chon chuc nang can thuc hien: ");
                    Console.ReadLine();
                }
                else if (int.Parse(selection) < 0 || int.Parse(selection) > 12)
                {// nếu là số nhưng không có chức năng cũng yêu cầu nhập lại
                    Console.Write("\nVui long chon chuc nang can thuc hien: ");
                    Console.ReadLine();
                }
                else
                {
                    begins(int.Parse(selection));
                }
                //  menuScreen();
            } while (selection != "0");
        }
        //hàm gọi 11 chức năng của console
        static void begins(int slt)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            switch (slt)
            {
                case 1:
                    {
                        Console.WriteLine("\nChuc nang them sach");
                        Book.addBook();
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("\nChuc nang xoa sach");                                            
                        MyLibrary.inquire_all_book();
                        Console.Write("\nNhap Ma sach tuong ung voi Ten Sach can xoa: ");
                        
                        string inputvar = Console.ReadLine();
                        Book.delBookbyID(inputvar);
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("\nChuc nang sua sach");
                        MyLibrary.inquire_all_book();
                        Console.Write("\nNhap Ma Sach tuong ung voi ten sach can sua: ");
                        string inputvar = Console.ReadLine();
                        Book.editBookbyID(inputvar);
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("\nChuc nang tim kiem Sach");
                        MyLibrary.inquire_all_book();
                        Console.Write("\nNhap Ten Sach can tim: ");
                        string inputvar = Console.ReadLine();
                        Book.inqBookbyName(inputvar);
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("\nChuc nang them Doc Gia");
                        Reader.addReader();
                        break;
                    }
                case 6:
                    {
                        Console.WriteLine("\nChuc nang xoa Doc Gia");
                        MyReader.inquire_all_reader();
                        Console.Write("Nhap Ma Doc Gia can xoa: ");
                        string inputvar = Console.ReadLine();
                        Reader.delReaderbyID(inputvar);
                        break;
                    }
                case 7:
                    {
                        Console.WriteLine("\nChuc nang sua Doc Gia");
                        MyReader.inquire_all_reader();
                        Console.Write("Nhap Ma Doc Gia can sua: ");
                        string inputvar = Console.ReadLine();
                        Reader.editReaderbyID(inputvar);
                        break;
                    }
                case 8:
                    {
                        Console.WriteLine("\nChuc nang tim kiem Doc Gia");
                        Console.Write("Nhap Ten Doc Gia can tim: ");
                        string inputvar = Console.ReadLine();
                        Reader.inqReaderbyName(inputvar);
                        break;
                    }
                case 9:
                    {
                        Console.WriteLine("\nChuc nang lap Phieu Muon Sach");
                        LibraryTicket.addTicket();
                        break;
                    }
                case 10:
                    {
                        Console.WriteLine("\nChuc nang lap Phieu Tra Sach");
                        Console.Write("Nhap Ma Phieu Muon Sach can tra: ");
                        string inputvar = Console.ReadLine();
                        LibraryTicket.returnTicket(inputvar);
                        break;
                    }
                case 11:
                    {
                        Console.WriteLine("\nChuc nang liet ke danh sach muon sach tre han");
                        LibraryTicket.overdayslistLibraryTicket();
                        break;
                    }
                case 12:
                    {
                        Console.WriteLine("\nDanh muc sach co trong thu vien");
                        MyLibrary.inquire_all_book();
                        break;
                    }
                    // default: break;
            }
        }

    }

}

