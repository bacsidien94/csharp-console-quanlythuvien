using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace Quanlythuvien
{
    class MyLibrary
    {
        public static List<Book> mylib { get; set; }

        //init a library of books from txt file
        public static void init_library()
        {
            List<Book> init = new List<Book>();
            string filePath = @"../../myLibrary.txt";
            List<string> listbook = new List<string>();
            listbook = File.ReadAllLines(filePath).ToList();
            foreach (string record in listbook)
            {
                // print to test, comment before submitting
                // Console.WriteLine(record);
                string[] book_att = record.Split(',');

                Book a = new Book() { };
                a.bookID = book_att[0];
                a.bookName = book_att[1];
                a.bookAuthor = book_att[2];
                a.bookPublisher = book_att[3];
                a.bookPriceTag = double.Parse(book_att[4]);
                //a.bookISBN = book_att[5];
                a.isEN = book_att[5];
                a.bookISBN = book_att[6];
                init.Add(a);
            }
            MyLibrary.mylib = init;
            // print to test, comment before submitting
            // Console.WriteLine("Total {0} book(s).\n", MyLibrary.mylib.Count); 
        }

        // print last all books 
        public static void inquire_all_book()
        {
            Book.print_header();
            for (int i = 0; i < MyLibrary.mylib.Count; i++)
            {
                inquire_a_book(i);
            }
        }

        public static void inquire_a_book(int index)
        {
            int i = index;
            
            Console.WriteLine("{0,-8}{1,-30}{2,-30}{3,-20}{4,-13}{5,-18}{6,-18}",
                MyLibrary.mylib[i].bookID,
                MyLibrary.mylib[i].bookName,
                MyLibrary.mylib[i].bookAuthor,
                MyLibrary.mylib[i].bookPublisher,
                Book.converPricetag(MyLibrary.mylib[i].bookPriceTag),
                Book.convertENVN(MyLibrary.mylib[i].isEN),
                MyLibrary.mylib[i].bookISBN);          
        }

        public static int find_lib_index(string bookID)
        {
            int index = 0;//init
            for (int i = 0; i < MyLibrary.mylib.Count; i++)
            {
                if (MyLibrary.mylib[i].bookID == bookID)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
