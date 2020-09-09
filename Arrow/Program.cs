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

namespace Arrow
{
    class Program
    {
        public static string _file = @"c:\Thomas\Desktop\gekko\testing\test.arrow";

        public static void Main(string[] args)
        {
            //https://devblogs.microsoft.com/dotnet/an-introduction-to-dataframe/    
            //Læs også denne interessante tråd: https://github.com/dotnet/runtime/issues/24920

            PrimitiveDataFrameColumn<DateTime> dateTimes = new PrimitiveDataFrameColumn<DateTime>("DateTimes"); // Default length is 0.
            PrimitiveDataFrameColumn<int> ints = new PrimitiveDataFrameColumn<int>("Ints", 3); // Makes a column of length 3. Filled with nulls initially
            StringDataFrameColumn strings = new StringDataFrameColumn("Strings", 3); // Makes a column of length 3. Filled with nulls initially
            //Append 3 values to dateTimes
            dateTimes.Append(DateTime.Parse("2019/01/01"));
            dateTimes.Append(DateTime.Parse("2019/01/01"));
            dateTimes.Append(DateTime.Parse("2019/01/02"));
            DataFrame df = new DataFrame(dateTimes, ints, strings); // This will throw if the columns are of different lengths            
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
            string s = df.ToString();

            Schema sc = null;
            IEnumerable<RecordBatch> batches = df.ToArrowRecordBatches();

            var df7 = new DataFrame(
    new PrimitiveDataFrameColumn<int>("Foo", 10),
    new PrimitiveDataFrameColumn<int>("Bar", Enumerable.Range(1, 10))
);
            
            var bb7 = df7.ToArrowRecordBatches();

            //var batches = new List<RecordBatch>();
            using (var stream = File.OpenWrite(_file))
            using (var writer = new ArrowFileWriter(stream, bb7.First().Schema ))
            {
                foreach (RecordBatch b in bb7)
                {
                    //DataFrame ddf7 = DataFrame.FromArrowRecordBatch(b);
                    writer.WriteRecordBatchAsync(b).Wait();
                }

                writer.WriteEndAsync().Wait();
            }
                        
            //Eric Erhardt from MS has been involved in arrow/C#
            //See this: https://stackoverflow.com/questions/56231247/numpy-pandas-counterpart-in-net-or-netcore/56280314#56280314
            //or better this: https://www.nuget.org/packages/Microsoft.Data.Analysis/
            //github: https://github.com/dotnet/corefxlab/tree/master/src/Microsoft.Data.Analysis
            //there is a proposed dataframe project
            Task t = Main2(args);
            Task<RecordBatch> tt = ReadArrowAsync(_file);
            RecordBatch rb = tt.Result;

            DataFrame df2 = DataFrame.FromArrowRecordBatch(rb);
            IEnumerable<RecordBatch> rb2 = df2.ToArrowRecordBatches();           
            
        }

        public static async Task Main2(string[] args)
        {
            // Use a specific memory pool from which arrays will be allocated (optional)

            var memoryAllocator = new NativeMemoryAllocator(alignment: 64);

            // Build a record batch using the Fluent API

            RecordBatch recordBatch = new RecordBatch.Builder(memoryAllocator)
                .Append("Column A", false, col => col.Int32(array => array.AppendRange(Enumerable.Range(0, 10))))
                .Append("Column B", false, col => col.Float(array => array.AppendRange(Enumerable.Range(0, 10).Select(x => Convert.ToSingle(x * 2)))))
                .Append("Column C", false, col => col.String(array => array.AppendRange(Enumerable.Range(0, 10).Select(x => $"Item {x + 1}"))))
                .Append("Column D", false, col => col.Boolean(array => array.AppendRange(Enumerable.Range(0, 10).Select(x => x % 2 == 0))))
                .Build();
            
            File.Delete(_file);            
            MemoryStream ms = new MemoryStream();            
            ArrowStreamWriter writer = new ArrowStreamWriter(ms, recordBatch.Schema);
            await writer.WriteRecordBatchAsync(recordBatch);
            //Hmmm, it seems the "magic" ARROW1 is not present at the start of the file
            using (FileStream file = new FileStream(_file, FileMode.Create, System.IO.FileAccess.Write)) ms.WriteTo(file);

            

        }

        public static async Task<RecordBatch> ReadArrowAsync(string filename)
        {
            using (var stream = File.OpenRead(filename))
            using (var reader = new ArrowFileReader(stream))
            {
                var recordBatch = await reader.ReadNextRecordBatchAsync();
                //Debug.WriteLine("Read record batch with {0} column(s)", recordBatch.ColumnCount);
                return recordBatch;
            }
        }
    }
}
