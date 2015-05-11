using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UpdateNeuseInventory.Properties;

namespace UpdateNeuseInventory
{
    internal class Program
    {
        public static String[] NewDriverArray;
        public static String[] OldDriverArray;

        private static void Main(string[] args)
        {
            bool haveFile = false;

            DeleteFromTemp();

            BulkLoad();

            CreateOldInvArray();

            CreateNewInvArray();

            CheckChange();

            // Data Source=sql-shark.cybersharks.net;Initial Catalog=nssnc;User ID=jlegacy;Password=***********
            // Data Source=JOSEPH\SQLEXPRESS;Initial Catalog=nssnc;Integrated Security=True

            UpdateInventory();

            Environment.Exit(0);
        }

        private static void CreateNewInvArray()
        {

            Console.WriteLine("Creating New Array...");

            var myList = new List<string>();

            var ts = new ARSTempDataContext();
            IQueryable<NeuseARSTemp> j =
                (from z in ts.GetTable<NeuseARSTemp>()
                    orderby z.upc
                    select z).Distinct();

            int count = 0;
            string prvUpc = "";
            foreach (NeuseARSTemp z in j)
            {

                myList.Add(z.upc + "@" + z.qty);
            }

            NewDriverArray = myList.ToArray();

            Array.Sort(NewDriverArray);
        }

        private static void CreateOldInvArray()
        {

            Console.WriteLine("Creating Old Array...");


            var myList = new List<string>();

            var ts = new NeuseARSDrvDataContext();
            IQueryable<NeuseARSDrv> j =
                (from z in ts.GetTable<NeuseARSDrv>()
                    select z);
            int count = 0;
            foreach (NeuseARSDrv z in j)
            {
                myList.Add(z.upc + "@" + z.qty);
            }

            OldDriverArray = myList.ToArray();

            Array.Sort(OldDriverArray);
        }

        public static void DeleteFromTemp()
        {
            var con = new SqlConnection(Settings.Default.nssncConnectionString);
            con.Open();

            string sql = @"DELETE FROM NeuseARSTemp";
            var cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void CheckChange()
        {

            Console.WriteLine("Checking Changes...");

            //Loop new Array  
            //First Delete any Old Data Not in new Driver//
            var dr = new NeuseARSDrvDataContext();
            String[] splitString;
            foreach (string value in OldDriverArray)
            {
                int result = Array.BinarySearch(NewDriverArray, value);
                if (result < 0)
                {
                    var db = new NeuseARSDrv();
                    splitString = (value.Split('@'));
                    db.upc = splitString[0];
                    db.qty = Convert.ToDecimal(splitString[1]);
                    db.prc = "N";
                    dr.NeuseARSDrvs.Attach(db);
                    dr.NeuseARSDrvs.DeleteOnSubmit(db);
                }
            }

            dr.SubmitChanges();

            //Second Add any New Changes to Old Driver//
            dr = new NeuseARSDrvDataContext();
            int count = 0;
            foreach (string value in NewDriverArray)
            {
                int result = Array.BinarySearch(OldDriverArray, value);
                if (result < 0)
                {
                    ++count;
                    var db = new NeuseARSDrv();
                    splitString = (value.Split('@'));
                    db.upc = splitString[0];
                    db.qty = Convert.ToDecimal(splitString[1]);
                    db.prc = "Y";
                    dr.NeuseARSDrvs.InsertOnSubmit(db);
                }
            }

            dr.SubmitChanges();
        }

        public static void BulkLoad()
        {

            Console.WriteLine("Bulk Loading...");

            var arsQtyData = new DataTable("NeuseARSTemp");

            // Create Column 1: SaleDate
            var upcColumn = new DataColumn();
            upcColumn.DataType = Type.GetType("System.String");
            upcColumn.ColumnName = "upc";

            // Create Column 2: ProductName
            var qtyColumn = new DataColumn();
            qtyColumn.DataType = Type.GetType("System.Int32");
            qtyColumn.ColumnName = "qty";

            // Add the columns to the ProductSalesData DataTable
            arsQtyData.Columns.Add(upcColumn);
            arsQtyData.Columns.Add(qtyColumn);

            DataRow arsQtyRow = null;

            // Let's populate the datatable with our stats.
            // You can add as many rows as you want here!
            int count = 0;
            var reader = new StreamReader(File.OpenRead(Settings.Default.tempFileName));
            while (!reader.EndOfStream)
            {
                count++;
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                arsQtyRow = arsQtyData.NewRow();
                if (values[0].CompareTo("upc") != 0 && values[0].CompareTo("\"\"") != 0)
                {
                    arsQtyRow["upc"] = Regex.Replace(values[0], @"[^\d]", "");
                    arsQtyRow["qty"] = Convert.ToDouble(values[1]);
                    if (arsQtyRow != null) arsQtyData.Rows.Add(arsQtyRow);
                }
            }

            // Create a new row

            // Add the row to the ProductSalesData DataTable

            // Copy the DataTable to SQL Server using SqlBulkCopy
            using (var dbConnection = new SqlConnection(Settings.Default.nssncConnectionString))
            {
                dbConnection.Open();
                using (var s = new SqlBulkCopy(dbConnection))
                {

                    // Set the timeout.
                    s.BulkCopyTimeout = Settings.Default.timeOut;

                    s.DestinationTableName = arsQtyData.TableName;

                    foreach (object column in arsQtyData.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());

                    s.WriteToServer(arsQtyData);
                }
            }
        }

        public static void UpdateInventory()
        {

            Console.WriteLine("Updating Inventory...");

            double count = 0;
            double count2 = 0;
            double count3 = 0;
            double notUsed = 0;
            var searchCharacters1 = new StringBuilder();
            var searchCharacters2 = new StringBuilder();
            var searchCharacters3 = new StringBuilder();

            //test

            try
            {
                var ts = new NeuseARSDrvDataContext();
                IQueryable<NeuseARSDrv> j =
                    from z in ts.GetTable<NeuseARSDrv>()
                    where z.prc.Equals("Y")
                    select z;
                foreach (NeuseARSDrv z in j)
                {
                    count2++;

                    Console.SetCursorPosition(0, 4); //move cursor
                    Console.Write("CSV Item: " + count2 + ":" + z.upc + "    ");

                    searchCharacters1.Clear();
                    searchCharacters1.Append(z.upc.TrimStart().TrimStart('0'));

                    searchCharacters2.Clear();
                    searchCharacters2.Append("0" + z.upc.TrimStart('0'));

                    searchCharacters3.Clear();
                    searchCharacters3.Append("00" + z.upc.TrimStart('0'));

                    var cs = new ProductsDataContext();
                    IQueryable<product> q =
                        from a in cs.GetTable<product>()
                        where
                            (a.pID.StartsWith(searchCharacters1.ToString()) ||
                             a.pID.StartsWith(searchCharacters2.ToString())
                             || a.pID.StartsWith(searchCharacters3.ToString()))
                        select a;

                    z.prc = "N";

                    ts.SubmitChanges();

                    foreach (product a in q)
                    {
                        ++count3;
                        BuildData(a, z);
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
            }

            ZeroInventory();
        }

        private static void ZeroInventory()
        {
            ProductsDataContext cs;
            int count = 0;
            cs = new ProductsDataContext();
            IQueryable<product> z =
                from a in cs.GetTable<product>()
                where a.pCustom2.Equals("p")
                select a;

            foreach (product a in z)
            {
                BuildData2(a);
                try
                {
                   
                    count = count + 1;
                    Console.SetCursorPosition(0, 5); //move cursor
                    Console.Write("Final Update Record: " + count);
                }
                catch (Exception e)
                {
                    Console.Write(e.ToString());
                }
            }

            cs.SubmitChanges();
        }

        private static void BuildData(product myProduct, NeuseARSDrv z)
        {
            myProduct.pCustom1 = Convert.ToString(Convert.ToInt32(z.qty));
            if (z.qty > 0)
            {
                myProduct.pInStock = Convert.ToInt32(myProduct.pInStock + Convert.ToInt32(z.qty));
            }
            myProduct.pCustom2 = "p";
        }

        private static void BuildData2(product myProduct)
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
    }
}