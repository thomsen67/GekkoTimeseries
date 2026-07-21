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
using Apache.Arrow.Types;
using Apache.Arrow.Ipc;
using Apache.Arrow.Memory;
using System.IO;
using Microsoft.Data.Analysis;
using System.Threading.Tasks;
using Parquet;
using Parquet.Data;
using Parquet.Schema;
using System.Globalization;


namespace Gekko
{
    public class ParquetHelper
    {
        /// <summary>
        /// Before, it was .GetAwaiter().GetResult(), which fails because of parallelism. This seems to fix that.
        /// </summary>
        /// <param name="col"></param>
        public static void WriteCol(ParquetRowGroupWriter group, DataColumn col)
        {
            group.WriteColumnAsync(col).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public static void WriteParquetPeriodStart(GekkoTime gt1, out DateTime? pq_date_starts, out string pq_period_starts)
        {
            pq_date_starts = null;
            pq_period_starts = null;
            if (!gt1.IsNull())
            {
                try { pq_date_starts = GekkoTime.FromGekkoTimeToDateTime(gt1, O.GetDateChoices.FlexibleStart); } catch { } //Non-utc, but if using .ToUniversalTime(), it messes up the hours
                pq_period_starts = DateStringFormat(gt1);
            }
        }

        public static void WriteParquetPeriodEnd(GekkoTime gt2, out DateTime? pq_date_ends, out string pq_period_ends)
        {
            pq_date_ends = null;
            pq_period_ends = null;
            if (!gt2.IsNull())
            {
                try { pq_date_ends = GekkoTime.FromGekkoTimeToDateTime(gt2, O.GetDateChoices.FlexibleStart); } catch { } //Non-utc, but if using .ToUniversalTime(), it messes up the hours                    
                pq_period_ends = DateStringFormat(gt2);
            }
        }

        public static string DateStringFormat(GekkoTime gt1)
        {
            //Output
            return gt1.ToString();
        }

        public static GekkoTime DateStringFormat(string s)
        {
            //Input
            return GekkoTime.FromStringToGekkoTime(s, true);
        }
    }
    
    public class ArrowDataRow
    {
        public string Name { get; set; }
        public string Freq { get; set; }
        public int DimNumber { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Freq: {Freq}, Dims: {DimNumber}, Value: {Value}";
        }
    }

    public class SimpleRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public double Value { get; set; }
    }

    /// <summary>
    /// This class is under construction...
    /// </summary>
    public class Arrow
    {        
        public static string _fileName = @"c:\Thomas\Desktop\gekko\testing\test1.arrow";

        public static DateTime UtcDateTime(int year, int month, int day)
        {
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc); //Using DateTimeKind.Local will fail after a roundtrip (1 hour change). If called with (1970, 1, 1, ...), it *seems* there are 8-byte zero values for the dates in the parquet file.
        }

        public static void ReadParquetDatabank(Databank databank, Program.ReadInfo readInfo, string filePath, List<string> errors, string bankName2)
        {
            //
            // NOTE: regarding dates/periods, only the string period for each observation is used, together with the DateTime stamp.
            //       The 4 start/end dates/periods are not used, and the DateTime date is not used.
            //

            string filterBankName = bankName2;
            if (G.NullOrBlanks(bankName2)) filterBankName = null;

            DateTime dt1 = DateTime.Now;
            int yearMin = int.MaxValue;
            int yearMax = int.MinValue;
            GekkoDictionary<string, bool> nameCounter = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, bool> bankCounter = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            int dateWarnings1 = 0;
            int dateWarnings2 = 0;
            int dataCounter = 0;

            string fileVersion = null;
            string fileTimestamp = null;
            string fileInfo1 = null;

            // -------------------------------                        
            // -------------------------------     
            string[] ids1 = null;
            string[] ids2 = null;
            // --
            string[] banks = null;
            string[] names = null;
            string[] freqs = null;
            int?[] dims = null;
            string[][] dimis = null;
            string[] labels = null;
            string[] sources = null;
            string[] units = null;
            bool?[] is_timelesss = null;           
            DateTime?[] date_starts = null; //Not used
            DateTime?[] date_ends = null; //Not used
            string[] period_starts = null; //Not used
            string[] period_ends = null; //Not used
            DateTime?[] stamps = null;
            DateTime?[] dates = null; //Not used
            string[] periods = null;
            double?[] values = null;

            if (!File.Exists(filePath))
            {                
                Error("Could not find file '" + filePath + "'", errors);
            }

            using (Stream fileStream = File.OpenRead(filePath))
            using (ParquetReader reader = ParquetReader.CreateAsync(fileStream).GetAwaiter().GetResult())
            {
                try
                {
                    Dictionary<string, string> metadata = reader.CustomMetadata;
                    metadata.TryGetValue("parquet.design.version", out fileVersion);
                    metadata.TryGetValue("export.timestamp", out fileTimestamp);
                    metadata.TryGetValue("table.label", out fileInfo1);
                }
                catch { }

                if (true)
                {

                    using (ParquetRowGroupReader group = reader.OpenRowGroupReader(0))
                    {
                        try { ids1 = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "id")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'id'", errors); }
                        try { banks = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "bank")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'bank'", errors); }
                        try { names = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "name")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'name'", errors); }
                        try { freqs = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "freq")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'freq'", errors); }
                        try { dims = ((int?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "dims")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'dims'", errors); }
                        int dimMax = 0;
                        foreach (int i in dims)
                        {
                            if (i < 0) Error("Rowgroup 0: Dims element with value " + i, errors);
                            dimMax = Math.Max(dimMax, i);
                        }
                        dimis = new string[dimMax][];
                        for (int i = 1; i <= dimMax; i++)
                        {
                            try { dimis[i - 1] = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "dim" + i)).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column '" + "dim" + i + "'", errors); }
                        }
                        try { labels = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "label")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'label'", errors); }
                        try { sources = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "source")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'source'", errors); }
                        try { units = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "unit")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'unit'", errors); }
                        try { is_timelesss = ((bool?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "is_timeless")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'is_timeless'", errors); }
                        try { date_starts = ((DateTime?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date_start")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'date_start'", errors); }
                        try { date_ends = ((DateTime?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date_end")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'date_end'", errors); }
                        try { period_starts = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period_start")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'period_start'", errors); }
                        try { period_ends = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period_end")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'period_end'", errors); }
                        try { stamps = ((DateTime?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "stamp")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'stamp'", errors); }
                    }

                    using (ParquetRowGroupReader group = reader.OpenRowGroupReader(1))
                    {
                        try { ids2 = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "id")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'id'", errors); }
                        try { dates = ((DateTime?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'date'", errors); }
                        try { periods = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'period'", errors); }
                        try { values = ((double?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "value")).GetAwaiter().GetResult()).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'value'", errors); }
                    }
                }
            }
            

            if (ids1 == null || ids1.Length == 0) Error("Rowgroup 1: Number of rows is = 0", errors);
            if (ids2 == null || ids2.Length == 0) Error("Rowgroup 2: Number of rows is = 0", errors);

            foreach (string s in banks)
            {
                if (!bankCounter.ContainsKey(s)) bankCounter.Add(s, false);                
            }            

            if (filterBankName == null)
            {
                if (bankCounter.Count > 1) Error("Rowgroup 0: Some values in the column 'bank' are different: use <bankname=...> option to select. Banks are: " + string.Join(", ", bankCounter.Keys.OrderBy(key => key).Select(key => $"'{key}'")), errors);
            }
            else
            {
                if (!bankCounter.ContainsKey(filterBankName))
                {
                    Error("Rowgroup 0: No values in the column 'bank' are identical to '" + filterBankName + "'. Possibilities are: " + string.Join(", ", bankCounter.Keys.OrderBy(key => key).Select(key => $"'{key}'")), errors);
                }
            }            

            string[] namePrettys = new string[ids1.Length];

            Dictionary<string, int> vars = new Dictionary<string, int>();
            for (int i = 0; i < ids1.Length; i++)
            {
                try
                {
                    string id = ids1[i];
                    string id2 = names[i] + "!" + freqs[i].ToLower();  //We do not want abc.A as series name.
                    string index = null;
                    string s = null;
                    if (dims[i] > 0)
                    {                        
                        for (int ii = 0; ii < dims[i]; ii++)
                        {
                            if (ii > 0) s += ",";
                            s += dimis[ii][i];
                        }                        
                    }
                    if (s != null) id2 += "[" + s + "]";                    
                    namePrettys[i] = id2;
                    if (id != (banks[i] + ":" + id2).ToLower())
                    {
                        string s2 = null;
                        if (dims[i] > 0)
                        {
                            s2 = ", " + s.Replace(",", ", ");
                        }
                        Error("Rowgroup 0: The id '" + id + "' is not compatible with bank, name, freq, dims and dim1..dimN (" + banks[i] + ", " + names[i] + ", " + freqs[i] + ", " + dims[i] + s2 + ")", errors);
                    }
                    if (vars.ContainsKey(id)) Error("Rowgroup 0: The id '" + id + "' appears > 1 time, which is not allowed.", errors);
                    vars.Add(id, i);
                    string nameTemp = names[i];
                    if (!nameCounter.ContainsKey(nameTemp)) nameCounter.Add(nameTemp, false);
                }
                catch { Error("Rowgroup 0 row " + i + ": Problem with one of these columns: bank, name, freq, dims, dim1..dimN (check for null value)", errors); }
            }

            string bank = null;
            string name = null;
            string freq = null;
            int? dim = null;
            string[] dimsi = null;            
            string label = null;
            string source = null;
            string unit = null;
            bool? is_timeless = null;
            DateTime? date_start = null;
            DateTime? date_end = null;
            string period_start = null;
            string period_end = null;
            DateTime? stamp = null;
            // -------------------------
            Series ts = null;
            string currentSeries = null;
            for (int i2 = 0; i2 < ids2.Length; i2++)
            {
                int i1 = -12345;
                try { i1 = vars[ids2[i2]]; } catch { Error("Rowgroup 1 row " + i2 + ": Cannot find id in rowgroup 0", errors); }
                if (filterBankName != null && !G.Equal(filterBankName, banks[i1])) continue; //Do not load data for this "bank". Do not use bank instead of banks[i1].

                string namePretty = namePrettys[i1];
                // ---
                DateTime? date = dates[i2]; //Not used
                string period = periods[i2];
                double? value = values[i2];

                if (currentSeries == null || currentSeries != namePretty)
                {
                    bank = banks[i1];                    
                    name = names[i1];
                    freq = freqs[i1];
                    dim = dims[i1];
                    dimsi = null;
                    if (dim > 0)
                    {
                        dimsi = new string[(int)dim];
                        for (int ii = 0; ii < dim; ii++)
                        {
                            dimsi[ii] = dimis[ii][i1];
                        }
                    }
                    label = labels[i1];
                    source = sources[i1];
                    unit = units[i1];
                    is_timeless = is_timelesss[i1];
                    date_start = date_starts[i1];  //Not used
                    date_end = date_ends[i1]; //Not used
                    period_start = period_starts[i1]; //Not used
                    period_end = period_ends[i1]; //Not used
                    stamp = stamps[i1];
                    // ----
                    currentSeries = namePretty;
                    ts = databank.GetIVariableMayCreateSeries(namePretty) as Series;                    
                    if (ts.meta == null) ts.meta = new SeriesMetaInformation();
                    ts.meta.label = label;
                    ts.meta.source = source;
                    ts.meta.units = unit;
                    if (stamp != null) ts.meta.stamp = ((DateTime)stamp).ToString("dd-MM-yyyy");  //This is the old Gekko format that we continue here

                    //
                    // NOTE: If label/source/unit is originally from an .ArraySuper, these will
                    //       now be duplicated into each sub-series. We live with that.
                    //
                }

                GekkoTime gt = GekkoTime.tNull;

                double d = double.NaN;
                if (value != null)
                {
                    d = (double)value;                 
                }
                if (is_timeless == true)
                {
                    if (period != null) Error("Rowgroup 1 row " + i2 + ": For a timeless series, the 'period' value is expected to be null", errors);
                    ts.type = ESeriesType.Timeless;
                    ts.data.SetDataarray_ONLY_INTERNAL_USE(new double[1]);
                    ts.data.GetDataArray_ONLY_INTERNAL_USE()[0] = d;
                    dataCounter++;
                }
                else
                {
                    if (period == null)
                    {
                        if (value != null)
                        {
                            Error("Rowgroup 1 row " + i2 + ": For a non-timeless series with a value, the 'period' value is expected to be <> null", errors);
                        }
                        if (date != null) //Ok if both period, date and value are == null
                        {
                            dateWarnings2++;
                        }
                    }
                    else
                    {
                        if (value == null) Error("Rowgroup 1 row " + i2 + ": Null value for period", errors);
                    }

                    if (period == null && value == null)
                    {
                        //Do nothing: this is a series with no data
                    }
                    else
                    {
                        try
                        {
                            //Allows K for Q, and U for W. Because numbers are parsed, 2020M01D01 or 2020U05 etc. are ok.
                            //Aborts if error
                            //Does not allow 98 for 1998.
                            gt = GekkoTime.FromStringToGekkoTime(period, true, true, false);
                        }
                        catch { Error("Rowgroup 1 row " + i2 + ": Could not parse period '" + period + "'", errors); }
                        
                        ts.SetData(gt, d); dataCounter++;

                        yearMin = Math.Min(yearMin, gt.super);
                        yearMax = Math.Max(yearMax, gt.super);
                    }
                }
            }
            if (dateWarnings1 > 0) G.Warning("w44.1", dateWarnings1 + " rows in rowgroup 1 with non-matching 'date' and 'period' values.");
            if (dateWarnings2 > 0) G.Warning("w44.1", dateWarnings1 + " rows in rowgroup 1 with 'date' non-null and 'period' null.");
            if (dataCounter == 0) G.Warning("w44.1", "No actual .parquet data was read.");

            readInfo.startPerInFile = yearMin;
            readInfo.endPerInFile = yearMax;
            readInfo.nanCounter = 0;

            readInfo.variables = nameCounter.Count;
            readInfo.time = (DateTime.Now - dt1).TotalMilliseconds;

            readInfo.startPerResultingBank = readInfo.startPerInFile;
            readInfo.endPerResultingBank = readInfo.endPerInFile;            

            DateTime parsedDate;            
            bool success = DateTime.TryParseExact(fileTimestamp, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedDate);
            if (success) readInfo.date = parsedDate.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

            readInfo.databankVersion = "(vers: " + fileVersion + ")";

            readInfo.info1 = fileInfo1;
        }

        private static void Error(string s, List<string> errors)
        {
            errors.Add(s);
            throw new GekkoException();
        }        

        public static RecordBatch ReadArrowOld(string filename)
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

        public static async Task<List<ArrowDataRow>> ReadArrow(string fileName)
        {
            // Check if the file exists
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Error: File not found at {fileName}");
                return new List<ArrowDataRow>();
            }

            using FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using ArrowFileReader reader = new ArrowFileReader(fileStream);
            
            var dataList = new List<ArrowDataRow>();

            int recordBatchCount = await reader.RecordBatchCountAsync();

            // Iterate through all record batches in the file (your file has one)
            for (int i = 0; i < recordBatchCount; i++)
            {
                // Read the record batch asynchronously
                RecordBatch recordBatch = await reader.ReadNextRecordBatchAsync();

                if (recordBatch == null) continue;

                //var arrays = recordBatch.Arrays.ToList();

                // Cast the arrays to their specific types for easy extraction
                var nameArray = recordBatch.Arrays.ElementAt(0) as Apache.Arrow.StringArray;
                var freqArray = recordBatch.Arrays.ElementAt(1) as Apache.Arrow.StringArray;
                var dimsArray = recordBatch.Arrays.ElementAt(2) as Apache.Arrow.Int32Array;
                //var valueArray = recordBatch.Arrays.ElementAt(3) as Apache.Arrow.DoubleArray;

                // Ensure all arrays were successfully cast
                if (nameArray == null || freqArray == null || dimsArray == null)
                {
                    new Error("Failed to cast one or more arrays to expected types.");
                }

                // Iterate over the rows and extract data
                for (int j = 0; j < recordBatch.Length; j++)
                {
                    dataList.Add(new ArrowDataRow
                    {
                        Name = nameArray.GetString(j),
                        Freq = freqArray.GetString(j),
                        DimNumber = dimsArray.GetValue(j) ?? 0, // Handle potential null (though your write code implies non-null)
                        //Value = valueArray.GetValue(j) ?? double.NaN // Handle potential null
                    });
                }
            }

            return dataList;
        }

        public static void WriteArrowDatabank(List<Tuple<string, IVariable>> list2, GekkoTime t1, GekkoTime t2, string pathAndFilename)
        {
            // TODO
            // TODO
            // TODO  Handle timeless series
            // TODO
            // TODO

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

            //Note that date formats are not yet supported when a .NET dataframe wraps around an arrow.
            //therefore we postpone the use of dates.
            //We can use null for string and NaN for double, and they return with same values, nice!
            //cf. https://github.com/dotnet/corefxlab/blob/master/src/Microsoft.Data.Analysis/DataFrame.Arrow.cs

            EArrowType arrowType = EArrowType.Ver1_0;
            if (Globals.runningOnTTComputer) arrowType = EArrowType.Ver1_0__with_time_int_and_underscore_name;

            DateTime dt = DateTime.Now;

            bool allPeriods = t1.IsNull() && t2.IsNull();
            AllFreqsHelper allFreqs = null;
            if (!allPeriods) allFreqs = G.ConvertDateFreqsToAllFreqs(t1, t2);

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
                MultidimElement mmi = ts.mmi;
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

            Arrow.WriteArrow(recordBatch1, pathAndFilename); //Doesn't the call need await??

            string s = null; if (hasSubSeries) s = " (including array-subseries)";
            new Writeln("Wrote " + seriesCounter + " series" + s + " to arrow file with " + rowCounter + " rows in " + G.Seconds(dt));

        }


        /// <summary>
        /// Writes a .parquet file with data, possibly with mixed frequencies, multiple dimensions, and from multiple databanks (timeless series are possible, too).
        /// </summary>        
        public static void WriteParquetDatabank(List<Tuple<string, IVariable>> listSorted, GekkoTime t1, GekkoTime t2, string pathAndFilename, string hdg, string bankName2)
        {
            //Note: the input list is already sorted by name

            string filterBankName = bankName2;
            if (G.NullOrBlanks(bankName2)) filterBankName = null;

            string gekkoParquetVersion = "1.0";

            DateTime dt = DateTime.Now;

            bool allPeriods = t1.IsNull() && t2.IsNull();
            AllFreqsHelper allFreqs = null;
            if (!allPeriods) allFreqs = G.ConvertDateFreqsToAllFreqs(t1, t2);

            int ndims = 0;
            foreach (Tuple<string, IVariable> tup in listSorted)
            {
                if (tup.Item2.Type() != EVariableType.Series) continue;  //skip             
                Series ts = tup.Item2 as Series;
                int dimensions = 0;
                if (ts.IsArraySubSeries()) dimensions = ts.mmi.storage.Length;
                ndims = Math.Max(ndims, dimensions);  //after loop, ndims can be 0 or larger
            }

            List<string> ids1 = new List<string>();
            List<string> ids2 = new List<string>();
            // ============================================================================
            List<string> banks = new List<string>();
            List<string> names = new List<string>();
            List<string> freqs = new List<string>();
            List<string> labels = new List<string>();
            List<string> sources = new List<string>();
            List<string> units = new List<string>();
            List<int?> dims = new List<int?>();
            List<List<string>> dimss = new List<List<string>>();
            for (int i = 0; i < ndims; i++) dimss.Add(new List<string>());
            List<bool?> timelesss = new List<bool?>();
            List<DateTime?> date_starts = new List<DateTime?>();
            List<DateTime?> date_ends = new List<DateTime?>();
            List<string> period_starts = new List<string>();
            List<string> period_ends = new List<string>();
            List<DateTime?> stamps = new List<DateTime?>();
            // ---            
            List<DateTime?> dates = new List<DateTime?>();
            List<string> periods = new List<string>();
            List<double?> values = new List<double?>();

            // ================================================

            ParquetSchema schema = WriteParquetDatabankSchema(ndims);
            Dictionary<string, string> metadata = WriteParquetDatabankMetadata(ndims, hdg, gekkoParquetVersion);
                        
            bool hasSubSeries = false;            

            string bank = null;
            if (filterBankName == null) bank = Path.GetFileNameWithoutExtension(pathAndFilename);
            else bank = filterBankName;

            foreach (Tuple<string, IVariable> tup in listSorted)
            {
                if (tup.Item2.Type() != EVariableType.Series) continue;
                Series ts = tup.Item2 as Series;
                bool isTimeless = ts.type == ESeriesType.Timeless;
                string fullName = G.Chop_AddBank(tup.Item1, bank).Replace(" ", "").ToLower();
                string freq = G.Chop_GetFreq(tup.Item1);
                string varnameWithoutFreqAndIndex = G.Chop_GetName(tup.Item1);
                string varnameWithoutIndex = G.Chop_GetNameAndFreq(tup.Item1);

                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;

                if (isTimeless)
                {
                    gt1 = GekkoTime.tNull;
                    gt2 = GekkoTime.tNull;
                }
                else
                {
                    if (allPeriods)
                    {
                        gt1 = ts.GetRealDataPeriodFirst();
                        gt2 = ts.GetRealDataPeriodLast();
                    }
                    else
                    {
                        G.PickFromAllFreqs(allFreqs, ts.freq, out gt1, out gt2);
                    }
                }

                ids1.Add(fullName);
                banks.Add(bank);
                names.Add(varnameWithoutFreqAndIndex);
                freqs.Add(freq);

                if (!ts.IsArraySubSeries())
                {
                    dims.Add(0);
                }
                else
                {
                    dims.Add(ts.mmi.storage.Length);
                    hasSubSeries = true;
                }

                for (int i2 = 0; i2 < ndims; i2++)  //ndims is 0 if only normal timeseries present
                {
                    if (ts.IsArraySubSeries()) //is an array-subseries
                    {
                        if (i2 < ts.mmi.storage.Length)
                        {
                            dimss[i2].Add(ts.mmi.storage[i2]);
                        }
                        else
                        {
                            dimss[i2].Add(null);
                        }
                    }
                    else
                    {
                        dimss[i2].Add(null);
                    }
                }

                //
                // A .frm file varlist, or varlist.dat must be loaded before writing with DOC<varlist>.
                //                
                labels.Add(ts.MetaGetLabel());
                sources.Add(ts.MetaGetSource());
                units.Add(ts.MetaGetUnits());
                timelesss.Add(isTimeless);

                // -------------------------------------------------------------------------------------------------------
                // Note about UTC. Regarding the DateTime object, it only contains ticks + a flag regarding UTC or local.
                // It seems that Parquet.NET ignores any 'UTC or not' flag anyway.
                // Polars and parquet datetimes are unix utc ('naive'), only pandas allows to state a timezone, but that
                // is inefficient for calculations. Better to keep the datetimes as utc, and only convert them when
                // humans are *viewing* the datatimes.
                // -------------------------------------------------------------------------------------------------------

                DateTime? pq_timestamp = WriteParquetDatabankGetTimestamp(ts.MetaGetStamp());
                stamps.Add(pq_timestamp);

                DateTime? pq_date_starts; string pq_period_starts;
                ParquetHelper.WriteParquetPeriodStart(gt1, out pq_date_starts, out pq_period_starts);
                date_starts.Add(pq_date_starts); period_starts.Add(pq_period_starts);

                DateTime? pq_date_ends; string pq_period_ends;
                ParquetHelper.WriteParquetPeriodEnd(gt2, out pq_date_ends, out pq_period_ends);
                date_ends.Add(pq_date_ends); period_ends.Add(pq_period_ends);

                if (gt1.IsNull() || gt2.IsNull())
                {                    
                    ids2.Add(fullName);
                    dates.Add(null);
                    periods.Add(null);
                    if (isTimeless)
                    {
                        values.Add(ts.GetTimelessData());
                    }
                    else
                    {
                        values.Add(null);
                    }
                }
                else
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                    {                        
                        ids2.Add(fullName);
                        dates.Add(GekkoTime.FromGekkoTimeToDateTime(t, O.GetDateChoices.FlexibleStart));
                        periods.Add(ParquetHelper.DateStringFormat(t));
                        values.Add(ts.GetDataSimple(t));
                    }
                }
            }

            WriteParquetDatabankFile(ids1, ids2, banks, names, freqs, ndims, dims, dimss, labels, sources, units, timelesss, date_starts, date_ends, period_starts, period_ends, stamps, dates, periods, values, pathAndFilename, schema, metadata);

            string s = null; if (hasSubSeries) s = " (including array-subseries)";
            new Writeln("Wrote " + ids1.Count + " series" + s + " to parquet file with " + (ids1.Count + ids2.Count) + " rows in two rowgroups in " + G.Seconds(dt));
        }
        
        public static void WriteParquetPlot(PlotTable plotTable, List<O.Prt.Element> containerExplode, EFreq highestFreq, string pathAndFilename)
        {
            string gekkoParquetVersion = "1.0";
            DateTime dt = DateTime.Now;
            int ndims = 0;
            string hdg = "Data from Gekko PLOT statement";
            
            List<string> ids1 = new List<string>();
            List<string> ids2 = new List<string>();
            // ============================================================================
            List<string> banks = new List<string>();
            List<string> names = new List<string>();
            List<string> freqs = new List<string>();
            List<string> labels = new List<string>();
            List<string> sources = new List<string>();
            List<string> units = new List<string>();
            List<int?> dims = new List<int?>();
            List<bool?> timelesss = new List<bool?>();
            List<DateTime?> date_starts = new List<DateTime?>();
            List<DateTime?> date_ends = new List<DateTime?>();
            List<string> period_starts = new List<string>();
            List<string> period_ends = new List<string>();
            List<DateTime?> stamps = new List<DateTime?>();
            // ---            
            List<DateTime?> dates = new List<DateTime?>();
            List<string> periods = new List<string>();
            List<double?> values = new List<double?>();

            // ================================================

            ParquetSchema schema = WriteParquetDatabankSchema(ndims);
            Dictionary<string, string> metadata = WriteParquetDatabankMetadata(ndims, hdg, gekkoParquetVersion);                        
                        
            for (int i = 0; i < plotTable.variables.Count; i++)
            {
                if (plotTable.variables[i].data.Count == 0) continue; //Can that ever happen?
                string xNameWithoutFreq = "x" + (i + 1);
                string freq = plotTable.variables[i].data[0].dateGekkoTime.freq.ToString().ToLower();
                string xName = G.Chop_AddFreq(xNameWithoutFreq, freq);
                string label = null;
                try { label = containerExplode[i].labelOLD[0]; } catch { }

                GekkoTime gt1 = GekkoTime.tNull;
                GekkoTime gt2 = GekkoTime.tNull;
                try
                {
                    gt1 = plotTable.variables[i].data[0].dateGekkoTime;
                    gt2 = plotTable.variables[i].data[plotTable.variables[i].data.Count - 1].dateGekkoTime;
                }
                catch { }

                ids1.Add(xName);
                banks.Add(null);
                names.Add(xNameWithoutFreq);
                freqs.Add(freq);                
                dims.Add(0);      
                labels.Add(label);
                sources.Add(null);
                units.Add(null);
                timelesss.Add(false); //Even with "plot 2;", it is non-interesting that 2 is timeless.

                DateTime? pq_timestamp = DateTime.UtcNow;  //Must be ok to give the stamp as the time where the transformation was done (could involve several timeseries: this is what a transformation like y = x1 + x2 does too, regardless of the stamps of x1 and x2).
                stamps.Add(pq_timestamp);

                DateTime? pq_date_starts; string pq_period_starts;
                ParquetHelper.WriteParquetPeriodStart(gt1, out pq_date_starts, out pq_period_starts);
                date_starts.Add(pq_date_starts); period_starts.Add(pq_period_starts);

                DateTime? pq_date_ends; string pq_period_ends;
                ParquetHelper.WriteParquetPeriodEnd(gt2, out pq_date_ends, out pq_period_ends);
                date_ends.Add(pq_date_ends); period_ends.Add(pq_period_ends);
                
                for (int j = 0; j < plotTable.variables[i].data.Count; j++)
                {
                    GekkoTime t = plotTable.variables[i].data[j].dateGekkoTime;
                    ids2.Add(xName);
                    dates.Add(GekkoTime.FromGekkoTimeToDateTime(t, O.GetDateChoices.FlexibleStart));
                    periods.Add(ParquetHelper.DateStringFormat(t));
                    values.Add(plotTable.variables[i].data[j].value);
                }
            }

            WriteParquetDatabankFile(ids1, ids2, banks, names, freqs, ndims, dims, null, labels, sources, units, timelesss, date_starts, date_ends, period_starts, period_ends, stamps, dates, periods, values, pathAndFilename, schema, metadata);
            new Writeln("PLOT created file " + pathAndFilename + " with " + ids1.Count + " expression" + G.S(ids1.Count) + " and " + (ids1.Count + ids2.Count) + " rows in two rowgroups in " + G.Seconds(dt));
        }        

        private static DateTime? WriteParquetDatabankGetTimestamp(string xtimestamp)
        {
            DateTime? pq_timestamp = null;
            bool b = false;
            if (xtimestamp != null)
            {
                int c1 = G.Count(xtimestamp, "-");
                int c2 = G.Count(xtimestamp, "/"); //older
                if (c1 == 2 || c2 == 2)
                {
                    string[] ss = null;
                    if (c1 == 2) ss = xtimestamp.Split('-');
                    else ss = xtimestamp.Split('/');
                    int i0; if (!int.TryParse(ss[0], out i0)) i0 = -12345;
                    int i1; if (!int.TryParse(ss[1], out i1)) i1 = -12345;
                    int i2; if (!int.TryParse(ss[2], out i2)) i2 = -12345;
                    if (i0 != -12345 && i1 != -12345 && i2 != -12345)
                    {
                        if (i2 >= 0 && i2 <= 99) i2 += 2000; //Gekko did not exist in year 19xx, so this should be safe regarding stamps.
                        if (i0 >= 1 && i0 <= 31 && i1 >= 1 && i1 <= 12 && G.IsYear(i2))
                        {
                            try { pq_timestamp = new DateTime(i2, i1, i0); /* stamps.Add(new DateTime(i2, i1, i0)); */ } //Non-UTC, but UtcDateTime(i2, i1, i0) does not change anything                             
                            catch { }
                        }
                    }
                }
            }
            return pq_timestamp;
        }

        /// <summary>
        /// Physically write the parquet file (group 0 and 1)
        /// </summary>        
        private static void WriteParquetDatabankFile(List<string> ids1, List<string> ids2, List<string> banks, List<string> names, List<string> freqs, int ndims, List<int?> dims, List<List<string>> dimss, List<string> labels, List<string> sources, List<string> units, List<bool?> timelesss, List<DateTime?> date_starts, List<DateTime?> date_ends, List<string> period_starts, List<string> period_ends, List<DateTime?> stamps, List<DateTime?> dates, List<string> periods, List<double?> values, string pathAndFilename, ParquetSchema schema, Dictionary<string, string> metadata)
        {
            //Note: All this ConfigureAwait(false).GetAwaiter().GetResult() stuff is because Excel-Dna will
            //      not work without it. Without it it either only writes 1 column, or scrambles the columns.
            //      Apparently the problem is because of some flushing etc. when writing, but reading does not
            //      seem to suffer from that problem.

            using (FileStream fileStream = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
            using (ParquetWriter writer = ParquetWriter.CreateAsync(schema, fileStream).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                writer.CustomMetadata = metadata;

                // ------------------------------
                // ROW GROUP 1 (series metadata)
                // ------------------------------
                using (ParquetRowGroupWriter group = writer.CreateRowGroup())
                {

                    int rowCount = ids1.Count;
                    int i = -1;

                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], ids1.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], banks.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], names.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], freqs.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], dims.ToArray()));

                    // ndims dynamic dimension labels
                    for (int ii = 0; ii < ndims; ii++)
                    {
                        ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], dimss[ii].ToArray()));
                    }

                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], labels.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], sources.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], units.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], timelesss.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], date_starts.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], date_ends.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], period_starts.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], period_ends.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], stamps.ToArray()));

                    // null columns
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<double?>(null, rowCount).ToArray()));
                }


                // ------------------------------
                // ROW GROUP 2 (values)
                // ------------------------------
                using (ParquetRowGroupWriter group = writer.CreateRowGroup())
                {
                    int rowCount = ids2.Count;
                    int i = -1;

                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], ids2.ToArray()));

                    // null-filled metadata columns for the values group
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<int?>(null, rowCount).ToArray()));

                    for (int ii = 0; ii < ndims; ii++)
                    {
                        ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    }

                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<bool?>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));

                    // actual values columns
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], dates.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], periods.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], values.ToArray()));
                }
            }
        }

        /// <summary>
        /// Create the parquet schema (column types)
        /// </summary>
        /// <param name="ndims"></param>
        /// <returns></returns>
        private static ParquetSchema WriteParquetDatabankSchema(int ndims)
        {
            List<DataField> m = new List<DataField>();
            m.Add(new DataField<string>("id"));
            m.Add(new DataField<string>("bank"));
            m.Add(new DataField<string>("name"));
            m.Add(new DataField<string>("freq"));
            m.Add(new DataField<int?>("dims"));
            for (int ii = 0; ii < ndims; ii++)
            {
                m.Add(new DataField<string>("dim" + (ii + 1)));
            }
            m.Add(new DataField<string>("label"));
            m.Add(new DataField<string>("source"));
            m.Add(new DataField<string>("unit"));
            m.Add(new DataField<bool?>("is_timeless"));
            m.Add(new DateTimeDataField("date_start", DateTimeFormat.DateAndTime, isNullable: true));
            m.Add(new DateTimeDataField("date_end", DateTimeFormat.DateAndTime, isNullable: true));
            m.Add(new DataField<string>("period_start"));
            m.Add(new DataField<string>("period_end"));
            m.Add(new DateTimeDataField("stamp", DateTimeFormat.DateAndTime, isNullable: true));
            // ----                
            m.Add(new DateTimeDataField("date", DateTimeFormat.DateAndTime, isNullable: true)); //Probably milliseconds, which with 64-bit can take a crazy big range of years.                
            m.Add(new DataField<string>("period"));
            m.Add(new DataField<double?>("value"));
            ParquetSchema schema = new ParquetSchema(m);
            return schema;
        }

        /// <summary>
        /// Create parquet metadata to write together with the normal data.
        /// </summary>
        /// <param name="ndims"></param>
        /// <param name="hdg"></param>
        /// <param name="gekkoParquetVersion"></param>
        /// <returns></returns>
        private static Dictionary<string, string> WriteParquetDatabankMetadata(int ndims, string hdg, string gekkoParquetVersion)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("software.name", "Gekko Timeseries and Modeling Software");
            metadata.Add("software.version", Globals.gekkoVersion);
            if (hdg != null) metadata.Add("table.label", hdg);
            metadata.Add("parquet.design.version", gekkoParquetVersion); //Gekko's version of the Parquet schema.              
            metadata.Add("parquet.design.url", "https://t-t.dk/gekko/docs/user-manual/index.html?appendix_parquet.htm");
            metadata.Add("export.timestamp", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
            metadata.Add("column.id.comment", "An id corresponding to the Gekko name, for merging rowgroup1 into rowgroup2. Only lower-case, no blanks. String.");
            metadata.Add("column.bank.comment", "Gekko databank name, often same as file name without extension (for future use, to store several databanks in 1 parquet file). String.");
            metadata.Add("column.name.comment", "The Gekko series name. Alphanumeric or underscore chars, lower or upper-case. String.");
            metadata.Add("column.freq.comment", "The Gekko frequency: a (annual), q (quarterly), m (monthly), w (weekly), d (daily), u (undated). Lower-case. String.");
            metadata.Add("column.dims.comment", "Number of dimensions of the given (array-) timeseries. Integer.");
            for (int ii = 0; ii < ndims; ii++)
            {
                metadata.Add("column.dim" + (ii + 1) + ".comment", "Dimension " + (ii + 1) + ". String.");
            }
            metadata.Add("column.label.comment", "The label of the given timeseries. String.");
            metadata.Add("column.source.comment", "The source of the given timeseries. String.");
            metadata.Add("column.unit.comment", "The unit of the given timeseries. String.");
            metadata.Add("column.is_timeless.comment", "True if the timeseries is constant for all periods. Boolean.");
            metadata.Add("column.date_start.comment", "The date corresponding to the first value of the timeseries in the Gekko databank. Date format, Unix time. Quarters etc. are identified as their *first* day. Not used by Gekko when importing.");
            metadata.Add("column.date_end.comment", "The date corresponding to the last value of the timeseries in the Gekko databank. Date format, Unix time. Quarters etc. are identified as their *first* day. Not used by Gekko when importing.");
            metadata.Add("column.period_start.comment", "The period corresponding to the first value of the timeseries in the Gekko databank. String format. Not used by Gekko when importing.");
            metadata.Add("column.period_end.comment", "The period corresponding to the last value of the timeseries in the Gekko databank. String format. Not used by Gekko when importing.");
            metadata.Add("column.stamp.comment", "The timestamp corresponding to the last time the timeseries was changed in the Gekko databank. Date format, Unix time.");
            metadata.Add("column.date.comment", "The date corresponding to the current data value. Date format, Unix time. Quarters etc. are identified as their *first* day. Not used by Gekko when importing.");
            metadata.Add("column.period.comment", "The period corresponding to the current data value. String.");
            metadata.Add("column.value.comment", "The data value. Numeric floating-point.");
            return metadata;
        }                

        public static async void WriteArrow(RecordBatch recordBatch, string fileName)
        {
            // Cannot get it to work without using a MemoryStream, which has a bit of overhead.
            // Without it, the file blocks.
            // Use a specific memory pool from which arrays will be allocated (optional)
            File.Delete(fileName);
            MemoryStream stream = new MemoryStream();
            ArrowFileWriter writer = new ArrowFileWriter(stream, recordBatch.Schema, leaveOpen: true);
            await writer.WriteRecordBatchAsync(recordBatch);
            await writer.WriteEndAsync();
            //using (FileStream fileStream = new FileStream(fileName, FileMode.Create, System.IO.FileAccess.Write))
            using (FileStream fileStream = Program.WaitForFileStream(fileName, null, Program.GekkoFileReadOrWrite.Write))
            {
                stream.WriteTo(fileStream);
            }
        }     
    }

    public class ParquetTypeAndName
    {
        public string name;
        public Type type;
        //public List<string> m_string = null;
        //public List<DateTime> m_datetime = null;
        //public List<int> m_int = null;
        //public List<long> m_long = null;
        System.Array data;
        
        public ParquetTypeAndName(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
    }

    public class TraceFrameParquetHelper
    {
        public static List<ParquetTypeAndName> names = new List<ParquetTypeAndName>()
        {
            //Must correspond to #qwldak7dad
            new ParquetTypeAndName("counter", typeof(long)),
            new ParquetTypeAndName("stamp", typeof(DateTime)),
            new ParquetTypeAndName("period_start", typeof(string)),
            new ParquetTypeAndName("period_end", typeof(string)),
            new ParquetTypeAndName("date_start", typeof(DateTime?)),
            new ParquetTypeAndName("date_end", typeof(DateTime?)),
            new ParquetTypeAndName("name", typeof(string)),
            new ParquetTypeAndName("text", typeof(string)),
            new ParquetTypeAndName("precedentsNames", typeof(string)),
            new ParquetTypeAndName("commandFile", typeof(string)),
            new ParquetTypeAndName("commandLine", typeof(int)),
            new ParquetTypeAndName("dataFile", typeof(string)),
            new ParquetTypeAndName("databankFile", typeof(string)),
            new ParquetTypeAndName("databankFileCounter", typeof(int)),
            new ParquetTypeAndName("depth", typeof(int))
        };
    }

    public class TraceFrameParquet
    {
        public static void GetSchemaHelper(List<DataField> m, ParquetTypeAndName nameAndType)
        {
            if (nameAndType.type == typeof(string)) m.Add(new DataField<string>(nameAndType.name));
            else if (nameAndType.type == typeof(DateTime)) m.Add(new DataField<DateTime>(nameAndType.name));
            else if (nameAndType.type == typeof(DateTime?)) m.Add(new DataField<DateTime?>(nameAndType.name));
            else if (nameAndType.type == typeof(int)) m.Add(new DataField<int>(nameAndType.name));
            else if (nameAndType.type == typeof(long)) m.Add(new DataField<long>(nameAndType.name));
            else new Error("Type is not supported");
        }

        private static ParquetSchema GetSchema(List<ParquetTypeAndName> names)
        {
            List<DataField> m = new List<DataField>();
            foreach (ParquetTypeAndName name in names) GetSchemaHelper(m, name);
            ParquetSchema schema = new ParquetSchema(m);
            return schema;
        }

        public static TraceFrame ReadParquetTraceFrame(string filePath)
        {
            TraceFrame traceFrame = new TraceFrame();
            using (Stream fileStream = File.OpenRead(filePath))
            using (ParquetReader reader = ParquetReader.CreateAsync(fileStream).GetAwaiter().GetResult())
            {
                using (ParquetRowGroupReader group = reader.OpenRowGroupReader(0))
                {
                    ////Must correspond to #qwldak7dad
                    int i = -1;
                    traceFrame.counter = ((long[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "counter")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.stamp = ((DateTime[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "stamp")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.period_start = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period_start")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.period_end = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period_end")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.date_start = ((DateTime?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date_start")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.date_end = ((DateTime?[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date_end")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.name = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "name")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.text = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "text")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.precedentsNames = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "precedentsNames")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.commandFile = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "commandFile")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.commandLine = ((int[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "commandLine")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.dataFile = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "dataFile")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.databankFile = ((string[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "databankFile")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.databankFileCounter = ((int[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "databankFileCounter")).GetAwaiter().GetResult()).Data).ToList();
                    traceFrame.depth = ((int[])(group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "depth")).GetAwaiter().GetResult()).Data).ToList();
                }
            }
            return traceFrame;
        }
    

        public static void WriteParquetTraceFrame(string pathAndFilename, TraceFrame traceFrame)
        {
            List<ParquetTypeAndName> parquetFields = TraceFrameParquetHelper.names;
            ParquetSchema schema = GetSchema(parquetFields);
            using (FileStream fileStream = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
            using (ParquetWriter writer = ParquetWriter.CreateAsync(schema, fileStream).ConfigureAwait(false).GetAwaiter().GetResult())
            {                
                using (ParquetRowGroupWriter group = writer.CreateRowGroup())
                {
                    //Must correspond to #qwldak7dad
                    int i = -1;            
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.counter.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.stamp.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.period_start.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.period_end.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.date_start.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.date_end.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.name.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.text.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.precedentsNames.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.commandFile.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.commandLine.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.dataFile.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.databankFile.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.databankFileCounter.ToArray()));
                    ParquetHelper.WriteCol(group, new DataColumn(schema.DataFields[++i], traceFrame.depth.ToArray()));
                }
            }
        }        
    }
}
