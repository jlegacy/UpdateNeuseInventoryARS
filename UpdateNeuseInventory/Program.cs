using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQtoCSV;
using UpdateNeuseInventory.Properties;

namespace UpdateNeuseInventory
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool haveFile = false;

            // Data Source=sql-shark.cybersharks.net;Initial Catalog=nssnc;User ID=jlegacy;Password=***********
            // Data Source=JOSEPH\SQLEXPRESS;Initial Catalog=nssnc;Integrated Security=True

            ReadCsv();

            Environment.Exit(0);

        }

        public static void ReadCsv()
        {
            double count = 0;
            double count2 = 0;
            double count3 = 0;
            double notUsed = 0;
            var searchCharacters = new StringBuilder();

            //test

        /*    try
            {
                var inputFileDescription = new CsvFileDescription
                {
                    SeparatorChar = ',',
                    FirstLineHasColumnNames = true,
                };

                var cc = new CsvContext();

                IEnumerable<SectionCsv> sections =
                    cc.Read<SectionCsv>(Settings.Default.tempFileName, inputFileDescription);

                IEnumerable<SectionCsv> sectionsByName =
                    from p in sections
                    where p.qty > 0
                    select p;

                foreach (SectionCsv item in sectionsByName)
                {
                    count2++;

                    Console.SetCursorPosition(0, 4); //move cursor
                    Console.Write("CSV Item: " + count2 + ":" + item.upc + "    ");

                    try
                    {
                        notUsed = Convert.ToDouble(item.upc);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    searchCharacters.Clear();
                    searchCharacters.Append(item.upc.TrimStart().TrimStart('0'));

                    var cs = new ProductsDataContext();
                    IQueryable<product> q =
                        from a in cs.GetTable<product>()
                        where (a.pID.Contains(searchCharacters.ToString()))
                        select a;

                    foreach (product a in q)
                    {
                        ++count3;
                        buildData(a, item);
                        try
                        {
                            cs.SubmitChanges();
                            count = count + 1;
                            Console.SetCursorPosition(0, 0); //move cursor
                            Console.Write("Update Record: " + count);
                        }
                        catch (Exception e)
                        {
                            Console.Write(e.ToString());
                        }
                    }
                } 

               
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            } */

            ZeroInventory();
        }

        private static void ZeroInventory()
        {
            ProductsDataContext cs;
            int count = 0;
            cs = new ProductsDataContext();
            IQueryable<product> z =
                from a in cs.GetTable<product>()
                select a;

            foreach (product a in z)
            {
                buildData2(a);
                try
                {
                    cs.SubmitChanges();
                    count = count + 1;
                    Console.SetCursorPosition(0, 5); //move cursor
                    Console.Write("Final Update Record: " + count);
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
            }
         
        }

        private static void buildData(product myProduct, SectionCsv item)
        {
            myProduct.pCustom1 = Convert.ToString(item.qty);
            myProduct.pCustom2 = "p";
        }

        private static void buildData2(product myProduct)
        {
            if ((myProduct.pCustom2 == null))
            {
                myProduct.pCustom2 = "";
                myProduct.pCustom1 = "0";
            }
            else
            {
                if ((myProduct.pCustom2.CompareTo("p") == 0))
                {
                    myProduct.pCustom2 = "";
                }
                else
                {
                    myProduct.pCustom1 = "0";
                    myProduct.pCustom2 = "";  
                }
                
            }
        }

        public class Result
        {
            public string description;
            public string status;
        }

        internal class SectionCsv
        {
            [CsvColumn(Name = "upc", FieldIndex = 1)]
            public string upc { get; set; }

            [CsvColumn(Name = "qoh", FieldIndex = 2)]
            public double qty { get; set; }
        }
    }
}