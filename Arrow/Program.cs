using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.Arrow;
using Apache.Arrow.Ipc;
using Apache.Arrow.Memory;
using System.IO;
using Microsoft.Data.Analysis;

using Gekko;
using System.Windows.Forms;

namespace Arrow
{
    public class Program
    {
        public static string _fileName = @"c:\Thomas\Desktop\gekko\testing\test.arrow";

        public static void Main(string[] args)
        {
            if (true) Run();
            else UnitTests();
        }

        private static void Run()
        {
            //DataFrame project in .NET
            //See this thread: https://github.com/dotnet/runtime/issues/24920
            //Eric Erhardt from MS has been involved in arrow/C#
            //https://devblogs.microsoft.com/dotnet/an-introduction-to-dataframe/    
            //https://devblogs.microsoft.com/dotnet/net-for-apache-spark-in-memory-dataframe-support/
            //See this: https://stackoverflow.com/questions/56231247/numpy-pandas-counterpart-in-net-or-netcore/56280314#56280314
            //or better this: https://www.nuget.org/packages/Microsoft.Data.Analysis/
            //github: https://github.com/dotnet/corefxlab/tree/master/src/Microsoft.Data.Analysis            
            //regarding python, see also Eviews: http://www.eviews.com/download/whitepapers/pyeviews.pdf
            //regarding R, see also EViews: https://www.eviews.com/download/whitepapers/Using%20R%20with%20EViews.pdf
            //If RAM is supposed to be shared, arrow uses Googles gRPC library
            //Compression with LZ4 should probably be the standard, but how to do this from C#? See https://ursalabs.org/blog/2020-feather-v2/

            //dataframes in R vs Python: https://towardsdatascience.com/python-and-r-for-data-wrangling-examples-for-both-including-speed-up-considerations-f2ec2bb53a86

            string s = null, s0 = null, s1 = null, s2 = null, s3 = null;

            // --------> example
            DataFrame df = null;
            if (false)
            {
                PrimitiveDataFrameColumn<DateTime> dateTimes = new PrimitiveDataFrameColumn<DateTime>("DateTimes"); // Default length is 0.
                PrimitiveDataFrameColumn<int> ints = new PrimitiveDataFrameColumn<int>("Ints", 3); // Makes a column of length 3. Filled with nulls initially
                StringDataFrameColumn strings = new StringDataFrameColumn("Strings", 3); // Makes a column of length 3. Filled with nulls initially
                                                                                         //Append 3 values to dateTimes
                dateTimes.Append(DateTime.Parse("2019/01/01"));
                dateTimes.Append(DateTime.Parse("2019/01/01"));
                dateTimes.Append(DateTime.Parse("2019/01/02"));
                df = new DataFrame(dateTimes, ints, strings); // This will throw if the columns are of different lengths            
                df[0, 1] = 10; // 0 is the rowIndex, and 1 is the columnIndex. This sets the 0th value in the Ints columns to 10
                               // Modify ints and strings columns by indexing
                ints[1] = 100;
                strings[1] = "Foo!";
                df.Info();
                // Add 5 to Ints through the DataFrame
                df.Columns["Ints"].Add(5, inPlace: true);
                DataFrameRow row0 = df.Rows[0];
                for (long i = 0; i < df.Rows.Count; i++)
                {
                    DataFrameRow row = df.Rows[i];
                }
                // Filter rows based on equality
                PrimitiveDataFrameColumn<bool> boolFilter = df.Columns["Strings"].ElementwiseEquals("Bar");
                DataFrame filtered = df.Filter(boolFilter);
            }


            DateTime dt1 = DateTime.Now;

            if (true)
            {

                dt1 = DateTime.Now;
                Globals.unitTestScreenOutput.Clear();
                Gekko.Globals.arrow = true;  //so that messages are not shown
                Gekko.Program.databanks.storage.Add(new Databank("Work"));
                O.Read o0 = new O.Read();
                o0.type = @"read";
                o0.fileName = @"c:\Thomas\Desktop\gekko\testing\jul05";
                o0.opt_first = "yes";
                o0.Exe();
                Databank db = Gekko.Program.databanks.GetFirst();
                s = Globals.unitTestScreenOutput.ToString();
                s0 = "Read gbk took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;

                dt1 = DateTime.Now;
                int t1 = 1998;
                int t2 = 2079;
                int n = t2 - t1 + 1;
                int k = db.storage.Count + 1;

                RecordBatch.Builder recordBatchBuilder = new RecordBatch.Builder(new NativeMemoryAllocator(alignment: 64));
                List<double> data = new List<double>();
                for (int i = 0; i < n; i++)
                {
                    data.Add(double.NaN);
                }

                List<string> dates = new List<string>();
                foreach (GekkoTime t in new GekkoTimeIterator(new GekkoTime(EFreq.A, t1, 1), new GekkoTime(EFreq.A, t2, 1)))
                {
                    dates.Add(t.super.ToString());
                }

                recordBatchBuilder.Append("time", false, col => col.String(array => array.AppendRange(dates)));


                int counter = 0;
                foreach (KeyValuePair<string, IVariable> kvp in db.storage)
                {
                    counter++;
                    PrimitiveDataFrameColumn<double> column = new PrimitiveDataFrameColumn<double>(G.Chop_RemoveFreq(kvp.Key), n);
                    int i = -1;
                    foreach (GekkoTime t in new GekkoTimeIterator(new GekkoTime(EFreq.A, t1, 1), new GekkoTime(EFreq.A, t2, 1)))
                    {
                        i++;
                        Series ts = kvp.Value as Series;
                        data[i] = ts.GetDataSimple(t);
                    }
                    recordBatchBuilder.Append(G.Chop_RemoveFreq(kvp.Key), false, col => col.Double(array => array.AppendRange(data)));
                }
                RecordBatch recordBatch = recordBatchBuilder.Build();
                DataFrame df777 = DataFrame.FromArrowRecordBatch(recordBatch);
                WriteArrow(recordBatch);
                //df777 = df;
                s1 = "Construct arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;
                //df777.Columns["AAA"][3] = 777d;


            }

            if (false)
            {
                dt1 = DateTime.Now;
                Globals.unitTestScreenOutput.Clear();
                Gekko.Globals.arrow = true;  //so that messages are not shown
                Gekko.Program.databanks.storage.Add(new Databank("Work"));
                O.Read o0 = new O.Read();
                o0.type = @"read";
                o0.fileName = @"c:\Thomas\Desktop\gekko\testing\jul05";
                o0.opt_first = "yes";
                o0.Exe();
                Databank db = Gekko.Program.databanks.GetFirst();
                s = Globals.unitTestScreenOutput.ToString();
                s0 = "Read gbk took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;


                dt1 = DateTime.Now;
                int t1 = 1998;
                int t2 = 2079;
                int n = t2 - t1 + 1;
                int k = db.storage.Count + 1;

                List<DataFrameColumn> list = new List<DataFrameColumn>(k);

                StringDataFrameColumn indexColumn = null;

                bool useTimeCol = false;
                if (useTimeCol)
                {
                    //indexColumn = new ArrowStringDataFrameColumn("time");

                    List<string> dates = new List<string>();
                    int i2 = -1;
                    foreach (GekkoTime t in new GekkoTimeIterator(new GekkoTime(EFreq.A, t1, 1), new GekkoTime(EFreq.A, t2, 1)))
                    {
                        i2++;
                        dates.Add(t.super.ToString());
                        //indexColumn.Add<string>(t.super.ToString());
                        //indexColumn[i2] = t.super.ToString();
                        //indexColumn[i2] = t.super.ToString();
                    }
                    //indexColumn = ArrowStringDataFrameColumn.Create("time", dates);
                    //ArrowStringDataFrameColumn.Create<ArrowStringDataFrameColumn>("time", dates);

                    //indexColumn = new ArrowStringDataFrameColumn("lkj");
                    indexColumn = new StringDataFrameColumn("lkj", n);
                    list.Add(indexColumn);
                }

                int counter = 0;
                foreach (KeyValuePair<string, IVariable> kvp in db.storage)
                {
                    counter++;
                    PrimitiveDataFrameColumn<double> column = new PrimitiveDataFrameColumn<double>(G.Chop_RemoveFreq(kvp.Key), n);
                    int i = -1;
                    foreach (GekkoTime t in new GekkoTimeIterator(new GekkoTime(EFreq.A, t1, 1), new GekkoTime(EFreq.A, t2, 1)))
                    {
                        i++;
                        Series ts = kvp.Value as Series;
                        //column.Add<double>(ts.GetDataSimple(t));
                        column[i] = ts.GetDataSimple(t);
                    }
                    list.Add(column);
                }
                DataFrame df777 = new DataFrame(list);
                //df777 = df;
                s1 = "Construct arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;

                RecordBatch recordBatch = null;
                if (true)
                {
                    List<int> xxA = new List<int>() { 1, 2, 3, 4, 5 };
                    List<double> xxB = new List<double>() { 1.1d, 2.1d, 3.1d, 4.1d, 5.1d };
                    List<string> xxC = new List<string>() { "a", "b", "c", "d", "e" };
                    recordBatch = new RecordBatch.Builder(new NativeMemoryAllocator(alignment: 64))
                    .Append("Column A", false, col => col.Int32(array => array.AppendRange(xxA)))
                    .Append("Column B", false, col => col.Double(array => array.AppendRange(xxB)))
                    .Append("Column C", false, col => col.String(array => array.AppendRange(xxC)))
                    .Build();
                    WriteArrow(recordBatch);
                    DataFrame xxx = DataFrame.FromArrowRecordBatch(recordBatch);
                }

                dt1 = DateTime.Now;
                IEnumerable<RecordBatch> rb = df777.ToArrowRecordBatches();
                RecordBatch first = rb.First();

                WriteArrow(first);
                s2 = "Write arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;

            }

            if (true)
            {
                dt1 = DateTime.Now;
                RecordBatch rb = ReadArrow(_fileName);
                DataFrame df2 = DataFrame.FromArrowRecordBatch(rb);
                s3 = "Read arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;
                //IEnumerable<RecordBatch> rb2 = df2.ToArrowRecordBatches();
            }

            Console.WriteLine(s);
            Console.WriteLine(s0);
            Console.WriteLine(s1);
            Console.WriteLine(s2);
            Console.WriteLine(s3);

            int ii = 1;
        }

        public static RecordBatch ReadArrow(string filename)
        {
            using (var stream = File.OpenRead(filename))
            using (var reader = new ArrowFileReader(stream))
            {
                //--->hmm cannot get async version to work, but recordBatches are for splitting very large datasets into batches of rows, 
                //so maybe not that important? But how are we sure all batches are read, if we only call "readNext"?
                //var recordBatch = await reader.ReadNextRecordBatchAsync(); 
                //See under usage here: https://github.com/apache/arrow/tree/master/csharp
                //In the following the non-async version:
                var recordBatch = reader.ReadNextRecordBatch();
                string s = "Read record batch with " + recordBatch.ColumnCount + " {0} column(s)";
                return recordBatch;
            }
        }

        public static async void WriteArrow(RecordBatch recordBatch)
        {
            // Use a specific memory pool from which arrays will be allocated (optional)

            File.Delete(_fileName);
                        
            MemoryStream stream = new MemoryStream();
            ArrowFileWriter writer = new ArrowFileWriter(stream, recordBatch.Schema, leaveOpen: true);
            await writer.WriteRecordBatchAsync(recordBatch);
            await writer.WriteEndAsync();
            using (FileStream fileStream = new FileStream(_fileName, FileMode.Create, System.IO.FileAccess.Write)) stream.WriteTo(fileStream);
            

        }

        public static void WriteArrow(IEnumerable<RecordBatch> batches, string fileName)
        {
            //DataFrame ddf7 = DataFrame.FromArrowRecordBatch(b);
            File.Delete(fileName);
            //using (var stream = File.OpenWrite(fileName))
            
            using (var stream = File.OpenWrite(fileName))
            using (var writer = new ArrowStreamWriter(stream, batches.First().Schema))
            {
                foreach (RecordBatch b in batches)
                {
                    writer.WriteRecordBatchAsync(b);
                    //writer.WriteRecordBatchAsync(b);
                }
                writer.WriteEndAsync();
                //writer.WriteEndAsync();

            }

        }

        public static void UnitTests()
        {
            //reset;
            //time 2020 2022;
            //x1 = 1, 2, 3;
            //x2 = series(1);
            //x2[a] = 2, 3, 4;
            //x2[b] = 3, 4, 5;
            //x3 = series(2);
            //x3[i1, j1] = 4, 5, 6;
            //x3[i1, j2] = 5, 6, 7;
            //option freq q;
            //time 2020q1 2020q3;
            //x4 = 6, 7, 8;
            //x5 = series(1);
            //x5[a] = 8, 9, 10;
            //x5[b] = 9, 10, 11;
            //x6 = series(2);
            //x6[i1, j1] = 10, 11, 12;
            //x6[i1, j2] = 11, 12, 13;


        }

    }
}
