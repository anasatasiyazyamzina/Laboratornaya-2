using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Lab2cresh
{

    /// <summary>
    /// Конструктор абстрактного класса Source
    /// <param name="NameOfPublication">Название публикации</param>
    /// <param name="LastNameOfAuthor">Фамилия автора</param>
    /// <param name="Getinformation">метод, позволяющий выводить информацию об издании</param>
    /// </summary>
    [XmlRootAttribute("Source", Namespace = "Lab2cresh", IsNullable = false)]
    public abstract class Source
    {
        [XmlElement(ElementName = "NameofPublic")]
        public string NameOfPublication;
        public string LastNameOfAuthor;
        public Source(string nameofpublic, string lastname)
        {
            NameOfPublication = nameofpublic;
            LastNameOfAuthor = lastname;
        }
        public abstract void GetInformation();
    }
    /// <summary>
    /// Конструктор класса Book
    /// <param name="YearOfPublication">Год публикации</param>
    /// <param name="NameOfEdition">Издание</param>
    /// <param name="Getinformation">Метод, позволяющий выводить информацию об издании</param>
    /// </summary>
    class Book : Source
    {
        public string YearOfPublication;
        public string NameOfEdition;
        
        public Book(string nameofpublic, string lastname, string yearofpublic, string nameofedition) : base(nameofpublic, lastname)
        {
            YearOfPublication = yearofpublic;
            NameOfEdition = nameofedition;
        }
        public override void GetInformation()
        {
            Console.WriteLine("Information : {0},{1},{2},{3}", NameOfPublication, LastNameOfAuthor, YearOfPublication, NameOfEdition);
            //TRACING OUR METHODS
            Trace.WriteLine("Information : " + NameOfPublication + ' ' + LastNameOfAuthor + ' ' + YearOfPublication + ' ' + NameOfEdition);
        }
    }
    /// <summary>
    /// Конструктор класса Paper
    /// <param name="NameOfMagazine">Название журнала</param>
    /// <param name="YearOfPublicat">Год публикации</param>
    /// <param name="Number">Номер журнала</param>
    /// <param name="Getinformation">Метод, позволяющий выводить информацию об издании</param>
    /// </summary>
    class Paper : Source
    {
        [XmlElement(ElementName = "NameofMag")]
        public string NameOfMagazine;
        [XmlElement(ElementName = "Number")]
        public string Number;
        [XmlElement(ElementName = "YearofPub")]
        public string YearOfPublicat;

        
        public Paper(string nameofpublic, string lastname, string yearofpublic, string nameofmagazine, string number) : base(nameofpublic, lastname)
        {
            NameOfMagazine = nameofmagazine;
            Number = number;
            YearOfPublicat = yearofpublic;
        }
        public override void GetInformation()
        {
            Console.WriteLine("Information:{0},{1},{2},{3},{4}", NameOfPublication, LastNameOfAuthor, NameOfMagazine, Number, YearOfPublicat);
            //TRACING OUR METHODS
            Trace.WriteLine("Information : " + NameOfPublication + ' ' + LastNameOfAuthor + ' ' + NameOfMagazine + ' ' + Number + ' ' + YearOfPublicat);
        }
    }
    /// <summary>
    /// Конструктор класса EResources
    /// <param name="Link">Ссылка</param>
    /// <param name="Annotation">Описание</param>
    /// <param name="Getinformation">Метод, позволяющий выводить информацию об издании</param>
    /// </summary>
    
    class EResources : Source
    {
        public string Link;
        public string Annotation;
        public EResources(string nameofpublic, string lastname, string link, string annotation) : base(nameofpublic, lastname)
        {

            Link = link;
            Annotation = annotation;
        }
        public override void GetInformation()
        {
            Console.WriteLine("Information:{0},{1},{2},{3}", NameOfPublication, LastNameOfAuthor, Link, Annotation);
            //TRACING OUR METHODS
            Trace.WriteLine("Information : " + NameOfPublication + ' ' + LastNameOfAuthor + ' ' + Link + ' ' + Annotation);
        }
    }
    /// <summary>
    /// Конструктор класса Catalog
    /// <param name="list">Каталог изданий</param>
    /// <param name="AddEdition">Метод, добавляющий информацию об изданиях</param>
    /// <param name="Getinformation">Метод, позволяющий выводить информацию об издании</param>
    /// </summary>
    public class Catalog
    {
        
        [XmlElement(ElementName = "BookList")]
        public List<Source> list = new List<Source>();

        public void AddEdition(Source edit)
        {
            list.Add(edit);
        }
        public void FindEdition(string lastname)
        {
            Trace.WriteLine("Founded Information : " + lastname);
            foreach (var p in list.FindAll(p => p.LastNameOfAuthor == lastname))
            {
                //TRACING OUR METHODS
                Trace.WriteLineIf(p.LastNameOfAuthor == lastname, "Editions are Equal");
                p.GetInformation();
            }

        }
    }
    public class Program
    {
        /// <summary>
        /// Этот метод читает заданную сторку из файла
        /// </summary>
        /// <param name="splitted">Массив с разделенными данным</param>
        /// <returns>Каталог Изданий</returns>
        /// <remarks>Так как чтение библиотеки происходит из input.txt, нужно разделить строку на массив данных и потом добавлять их в каталог</remarks>
        private static Catalog Text_inputed()
        {
             Catalog c = new Catalog();
            try
            {
                string s;
                StreamReader f = new StreamReader("input.txt");
                while ((s = f.ReadLine()) != null)
                {
                    string[] splitted = s.Split(',');
                    if (splitted[0] == "Book") c.AddEdition(new Book(splitted[0], splitted[1], splitted[2], splitted[3]));
                    if (splitted[0] == "Article") c.AddEdition(new Paper(splitted[0], splitted[1], splitted[2], splitted[3], splitted[4]));
                    if (splitted[0] == "Internet") c.AddEdition(new EResources(splitted[0], splitted[1], splitted[2], splitted[3]));
                }
                f.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return c;
        }
        /// <summary>
        /// Точка входа для приложения.
        /// </summary>
        /// <param name="args"> Список аргументов командной строки</param>
        /// <param name="с">Переменная для хранения каталога</param>    
        static void Main(string[] args)
        {

            Test.test("po.xml");
            Catalog c = Text_inputed();
            /*foreach (var p in c.list)
            {
                p.GetInformation();
            }*/
            c.FindEdition("Test");
            Console.ReadLine();
        }
    }
    class Test
    {
        public static void test(string filename)
        {
            // Create the XmlSerializer.
            XmlSerializer s = new XmlSerializer(typeof(List<Source>));

            // To write the file, a TextWriter is required.
            TextWriter writer = new StreamWriter(filename);

            /* Create an instance of the group to serialize, and set
               its properties. */
            Catalog c = new Catalog();
            //c.AddEdition(new Paper("asd", "sd", "df,", "sds", "sd"));
                
            // Serialize the object, and close the TextWriter.      
            s.Serialize(writer, c.list);
            writer.Close();
        }
    }
}

