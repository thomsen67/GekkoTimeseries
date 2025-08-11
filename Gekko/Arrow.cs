/*
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2025, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Apache.Arrow;
using Apache.Arrow.Ipc;
using Apache.Arrow.Memory;
using System.IO;
using Microsoft.Data.Analysis;

namespace Gekko
{
    /// <summary>
    /// This class is under construction...
    /// </summary>
    public class Arrow
    {
        public static string _fileName = @"c:\Thomas\Desktop\gekko\testing\test.arrow";

        public static void Run()
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

            //older commentaries:
            //Feather kan læse / skrive hurtigt med lille ram-forbrug hvis, især hvis brug af pandas.Categorial(slags enums)
            //Men parquet er mere stabilt, tæt knyttet til Hadoop. Feather er nyere
            //Parquet understøttes også af tensorflow.Sammenligning: se https://towardsdatascience.com/the-best-format-to-save-pandas-data-414dca023e0d
            //Konklusion: feather er godt mht. næsten alle benchmarks
            //https://github.com/elastacloud/parquet-dotnet  finde til .NET Core
            //https://github.com/kevin-montrose/FeatherDotNet  finde til .NET Core
            //Hadoop og Spark er de store inden for big data. Spark er nyere og kører in-memory og integrer Arrow.
            //Fremtiden er Spark, meget bedre gearet mod Tensorflow, Python, etc.Efter Spark kommer Flink, og de overvejer
            //Arrow som internt format. Flink er bedre til streaming, f.eks.aktiekurser, hvor < millisekunder er vigtigt.
            //Men Spark prøver vist også at blive bedre til dette.Spark bedre integreret med Python / R.
            //Snowflake kunne være god.
            //Use using PLINQ to do pivoting. That should be future - safe.
            //Parquet.NET 3.3.9 har brug for flg.fra nuget netstandard.library, system.buffers, system.memory, system.reflection.emit.lightweight
            //De sidste 3 kommer vist med automatisk, mens den første skulle installeres manuelt. Men så kørte det.

            /* Some old parquet stuff, maybe it can be useful?
             *
             * using Parquet.Data;             *
             * if (false)
            {
                var idColumn = new DataColumn(new DataField<int>("id"), new int[] { 1, 2 });

                var cityColumn = new DataColumn(new DataField<string>("city"), new string[] { "London", "Derby" });

                // create file schema
                var schema = new Schema(idColumn.Field, cityColumn.Field);

                using (Stream fileStream = System.IO.File.OpenWrite(@"c:\Thomas\Desktop\gekko\testing\grane\p.parquet"))
                {
                    using (var parquetWriter = new Parquet.ParquetWriter(schema, fileStream))
                    {
                        // create a new row group in the file
                        using (Parquet.ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup())
                        {
                            groupWriter.WriteColumn(idColumn);
                            groupWriter.WriteColumn(cityColumn);
                        }
                    }
                }
            }



            var idColumn = new DataColumn(new DataField<int>("id"), new int[] { 1, 2 });

            var cityColumn = new DataColumn(new DataField<string>("city"), new string[] { "London", "Derby" });


            using (Stream fileStream = System.IO.File.OpenRead(@"c:\Thomas\Desktop\gekko\testing\grane\p.parquet"))
            {
                // open parquet file reader
                using (var parquetReader = new Parquet.ParquetReader(fileStream))
                {
                    // get file schema (available straight after opening parquet reader)
                    // however, get only data fields as only they contain data values
                    DataField[] dataFields = parquetReader.Schema.GetDataFields();

                    // enumerate through row groups in this file
                    for (int i = 0; i < parquetReader.RowGroupCount; i++)
                    {
                        // create row group reader
                        using (Parquet.ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
                        {
                            // read all columns inside each row group (you have an option to read only
                            // required columns if you need to.
                            DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                            // get first column, for instance
                            DataColumn firstColumn = columns[0];

                            // .Data member contains a typed array of column data you can cast to the type of the column
                            Array data = firstColumn.Data;
                            int[] ids = (int[])data;
                        }
                    }
                }
            }

             *
             * */

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
                //Globals.unitTestScreenOutput.Clear();
                //Gekko.Globals.arrow = true;  //so that messages are not shown
                Gekko.Program.databanks.storage.Add(new Databank("Work"));
                O.Read o0 = new O.Read();
                o0.type = @"read";
                o0.fileName = @"c:\Thomas\Desktop\gekko\testing\jul05";
                o0.opt_first = "yes";
                o0.Exe();
                Databank db = Gekko.Program.databanks.GetFirst();
                //s = Globals.unitTestScreenOutput.ToString();
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
                WriteArrow(recordBatch, _fileName);
                //df777 = df;
                s1 = "Construct arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;
                G.Writeln(s1);
                //df777.Columns["AAA"][3] = 777d;
            }

            if (false)
            {
                dt1 = DateTime.Now;
                //Globals.unitTestScreenOutput.Clear();
                //Gekko.Globals.arrow = true;  //so that messages are not shown
                Gekko.Program.databanks.storage.Add(new Databank("Work"));
                O.Read o0 = new O.Read();
                o0.type = @"read";
                o0.fileName = @"c:\Thomas\Desktop\gekko\testing\jul05";
                o0.opt_first = "yes";
                o0.Exe();
                Databank db = Gekko.Program.databanks.GetFirst();
                //s = Globals.unitTestScreenOutput.ToString();
                s0 = "Read gbk took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;
                G.Writeln(s0);


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
                G.Writeln(s1);

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
                    WriteArrow(recordBatch, _fileName);
                    DataFrame xxx = DataFrame.FromArrowRecordBatch(recordBatch);
                }

                dt1 = DateTime.Now;
                IEnumerable<RecordBatch> rb = df777.ToArrowRecordBatches();
                RecordBatch first = rb.First();

                WriteArrow(first, _fileName);
                s2 = "Write arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;
                G.Writeln(s2);

            }

            if (true)
            {
                dt1 = DateTime.Now;
                RecordBatch rb = ReadArrow(_fileName);
                DataFrame df2 = DataFrame.FromArrowRecordBatch(rb);
                s3 = "Read arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;
                G.Writeln(s3);
                //IEnumerable<RecordBatch> rb2 = df2.ToArrowRecordBatches();
            }

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
                //string s = "Read record batch with " + recordBatch.ColumnCount + " {0} column(s)";
                return recordBatch;
            }
        }

        public static void WriteArrowDatabank(List<Tuple<string, IVariable>> list2, GekkoTime t1, GekkoTime t2, string pathAndFilename)
        {
            //Note that date formats are not yes supported when a .NET dataframe wraps around an arrow.
            //therefore we postpone the use of dates.
            //We can use null for string and NaN for double, and they return with same values, nice!
            //cf. https://github.com/dotnet/corefxlab/blob/master/src/Microsoft.Data.Analysis/DataFrame.Arrow.cs

            EArrowType arrowType = EArrowType.Ver1_0;
            if (Globals.runningOnTTComputer) arrowType = EArrowType.Ver1_0__with_time_int_and_underscore_name;

            DateTime dt = DateTime.Now;

            bool allPeriods = t1.IsNull() && t2.IsNull();
            AllFreqsHelper allFreqs = null;
            if (!allPeriods) allFreqs = G.ConvertDateFreqsToAllFreqs(t1, t2);

            //Databank db1 = Gekko.Program.databanks.GetFirst();
            int n = GekkoTime.Observations(t1, t2);
            int k = list2.Count + 1;

            int npers = 0;
            int ndims = 0;

            List<string> names = new List<string>();
            List<string> freqs = new List<string>();
            List<int> dimNumber = new List<int>();
            List<List<string>> dims = new List<List<string>>();
            List<List<int>> pers = new List<List<int>>();
            List<double> datas = new List<double>();

            List<int> dates = new List<int>();  //days since 01-01-1970 (unix time)
            List<string> longNames = new List<string>();  //like x__i__j__q

            int rowCounter = 0;
            bool hasSubSeries = false;
            int seriesCounter = 0;

            //We first sort the series names, and if it is an array-series, we also sort the sub-series names
            //It is a little bit inefficient that we first sort and then look up by Dictionary, but the inefficienty
            //is probably not critical, and fixing this would mean construction of special sortable objects (not worth the pain)

            //List<string> namesWithFreq = new List<string>();
            foreach (Tuple<string, IVariable> tup in list2)
            {
                if (tup.Item2.Type() != EVariableType.Series) continue;  //skip
                //namesWithFreq.Add(kvp1.Key);
                Series ts = tup.Item2 as Series;
                npers = Math.Max(npers, G.FreqType(ts));  //after loop, nfreqs can be 1, 2 or 3
                int dimensions = 0;
                if (ts.IsArraySubSeries() && ts.mmi.parent != null) dimensions = ts.mmi.parent.dimensions;
                ndims = Math.Max(ndims, dimensions);  //after loop, ndims can be 0 or larger
            }
            //namesWithFreq.Sort(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < npers; i++) pers.Add(new List<int>());
            for (int i = 0; i < ndims; i++) dims.Add(new List<string>());
            
            foreach (Tuple<string, IVariable> tup in list2)
            {
                seriesCounter++;
                                
                string freq = G.Chop_GetFreq(tup.Item1);
                Series ts = tup.Item2 as Series;
                MultidimItem mmi = ts.mmi;
                string varnameWithoutFreqAndIndex = G.Chop_GetName(tup.Item1);                

                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;
                if (allPeriods)
                {
                    gt1 = ts.GetRealDataPeriodFirst();
                    gt2 = ts.GetRealDataPeriodLast();
                }
                else
                {
                    G.PickFromAllFreqs(allFreqs, ts.freq, out gt1, out gt2);
                }

                foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                {
                    rowCounter++;

                    string longVarnameWithoutFreq = varnameWithoutFreqAndIndex;

                    if (arrowType == EArrowType.Ver1_0__with_time_int_and_underscore_name)
                    {
                        dates.Add(GekkoTime.FromDateTimeToUnixDays(GekkoTime.FromGekkoTimeToDateTime(t, O.GetDateChoices.FlexibleStart)));
                    }

                    names.Add(varnameWithoutFreqAndIndex);
                    freqs.Add(freq);

                    int super = 0;
                    int sub1 = 0;
                    int sub2 = 0;
                    if (G.FreqType(ts) == 1)
                    {
                        super = t.super;
                    }
                    else if (G.FreqType(ts) == 2)
                    {
                        super = t.super;
                        sub1 = t.sub;
                    }
                    else if (G.FreqType(ts) == 3)
                    {
                        super = t.super;
                        sub1 = t.sub;
                        sub2 = t.subsub;
                    }

                    for (int i2 = 0; i2 < npers; i2++)  //nfreqs can be 1, 2, or 3, then i2 runs up to 0, 1, or 2.
                    {
                        if (i2 == 0) pers[i2].Add(super);
                        else if (i2 == 1) pers[i2].Add(sub1);
                        else if (i2 == 2) pers[i2].Add(sub2);
                        else throw new GekkoException();  //should not be possible
                    }

                    if (!ts.IsArraySubSeries())
                    {
                        dimNumber.Add(0);
                    }
                    else
                    {
                        dimNumber.Add(ts.mmi.storage.Length);
                        hasSubSeries = true;
                    }

                    //TODOTODOTODO
                    //TODOTODOTODO
                    //TODOTODOTODO
                    //TODOTODOTODO
                    //TODOTODOTODO move non-data stuff out of time loop!!!
                    //TODOTODOTODO
                    //TODOTODOTODO
                    //TODOTODOTODO
                    //TODOTODOTODO

                    for (int i2 = 0; i2 < ndims; i2++)  //ndims is 0 if only normal timeseries present
                    {
                        if (ts.IsArraySubSeries()) //is an array-subseries
                        {
                            if (i2 < mmi.storage.Length)
                            {
                                dims[i2].Add(mmi.storage[i2]);
                                if (arrowType == EArrowType.Ver1_0__with_time_int_and_underscore_name)
                                {
                                    longVarnameWithoutFreq += "__" + mmi.storage[i2];
                                }
                            }
                            else
                            {
                                dims[i2].Add(null);
                            }
                        }
                        else
                        {
                            dims[i2].Add(null);
                        }
                    }
                    datas.Add(ts.GetDataSimple(t));

                    if (arrowType == EArrowType.Ver1_0__with_time_int_and_underscore_name)
                    {
                        longNames.Add(longVarnameWithoutFreq);
                    }
                }
            }

            RecordBatch.Builder recordBatchBuilder = new RecordBatch.Builder(new NativeMemoryAllocator(alignment: 64));
            bool nullable = true;
            recordBatchBuilder.Append("name", nullable, col => col.String(array => array.AppendRange(names.ToArray())));
            recordBatchBuilder.Append("freq", nullable, col => col.String(array => array.AppendRange(freqs.ToArray())));
            recordBatchBuilder.Append("dims", nullable, col => col.Int32(array => array.AppendRange(dimNumber.ToArray())));
            for (int i = 0; i < dims.Count; i++)
            {
                recordBatchBuilder.Append("dim" + (i + 1), nullable, col => col.String(array => array.AppendRange(dims[i].ToArray())));
            }
            for (int i = 0;i<pers.Count;i++)
            {
                recordBatchBuilder.Append("per" + (i + 1), nullable, col => col.Int32(array => array.AppendRange(pers[i].ToArray())));
            }
            recordBatchBuilder.Append("value", nullable, col => col.Double(array => array.AppendRange(datas)));

            if (arrowType == EArrowType.Ver1_0__with_time_int_and_underscore_name)
            {
                recordBatchBuilder.Append("longname", nullable, col => col.String(array => array.AppendRange(longNames.ToArray())));
                recordBatchBuilder.Append("date", nullable, col => col.Int32(array => array.AppendRange(dates.ToArray())));
            }

            RecordBatch recordBatch1 = recordBatchBuilder.Build();

            //DataFrame df1 = DataFrame.FromArrowRecordBatch(recordBatch1);

            Arrow.WriteArrow(recordBatch1, pathAndFilename);

            if (false)
            {
                RecordBatch recordBatch2 = Arrow.ReadArrow(Globals.ttPath2 + @"\regres\Databanks\jul05.arrow");
                DataFrame df2 = DataFrame.FromArrowRecordBatch(recordBatch2);
                Databank db2 = new Databank(null);
            }

            string s = null; if (hasSubSeries) s = " (including array-subseries)";
            G.Writeln2("Wrote " + seriesCounter + " series" + s + " to arrow file with " + rowCounter + " rows in " + G.Seconds(dt));

        }

        public static async void WriteArrow(RecordBatch recordBatch, string fileName)
        {
            // Use a specific memory pool from which arrays will be allocated (optional)
            File.Delete(fileName);
            MemoryStream stream = new MemoryStream();
            ArrowFileWriter writer = new ArrowFileWriter(stream, recordBatch.Schema, leaveOpen: true);
            await writer.WriteRecordBatchAsync(recordBatch);
            await writer.WriteEndAsync();
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(fileStream);
            }
        }

        public static void WriteArrow(IEnumerable<RecordBatch> batches, string fileName)
        {
            File.Delete(fileName);
            using (var stream = File.OpenWrite(fileName))
            using (var writer = new ArrowStreamWriter(stream, batches.First().Schema))
            {
                foreach (RecordBatch b in batches)
                {
                    writer.WriteRecordBatchAsync(b);
                }
                writer.WriteEndAsync();
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
