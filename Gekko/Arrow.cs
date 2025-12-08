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


namespace Gekko
{
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

        public static async Task WriteParquetFile()
        {            
            string[] ids1 = { "mona:x1!a", "mona:x2!q[a,b]", "mona:x2!q[a,c]" };
            string[] ids2 = { "mona:x1!a", "mona:x1!a", "mona:x2!q[a,b]", "mona:x2!q[a,b]", "mona:x2!q[a,c]", "mona:x2!q[a,c]" };            
            // ============================================================================
            string[] banks = { "mona", "mona", "mona" };
            string[] names = { "x1", "x2", "x2" };
            string[] freqs = { "a", "q", "q" };
            string[] labels = { "Serie 1", "Serie 2", "Serie 2" };
            string[] sources = { "DST", "NB", "NB" };
            string[] units = { "Mio. kr.", "Mia. kr.", "Mia. kr." };
            int?[] dims = { 0, 2, 2 };            
            string[] dim1 = { null, "a", "a" };
            string[] dim2 = { null, "b", "c" };
            DateTime?[] date_starts = { UtcDateTime(1966, 1, 1), UtcDateTime(1980, 1, 1), UtcDateTime(1980, 1, 1) };
            DateTime?[] date_ends = { UtcDateTime(2024, 1, 1), UtcDateTime(2024, 10, 1), UtcDateTime(2024, 10, 1) };
            DateTime?[] stamps = { UtcDateTime(2025, 11, 17), UtcDateTime(2025, 11, 15), UtcDateTime(2025, 11, 16) };
            // ---            
            DateTime?[] dates = { UtcDateTime(2021, 1, 1), UtcDateTime(2022, 1, 1), UtcDateTime(2021, 1, 1), UtcDateTime(2021, 4, 1), UtcDateTime(2021, 1, 1), UtcDateTime(2021, 4, 1) };
            double?[] values = { 101, 102, 103, 104, 105, 106 };

            var idField = new Parquet.Schema.DataField<string>("id")
            {
                
            };

            // 1. Unified schema
            var schema = new Parquet.Schema.ParquetSchema(
                new Parquet.Schema.DataField<string>("id"),
                // ================================================
                new Parquet.Schema.DataField<string>("bank"),
                new Parquet.Schema.DataField<string>("name"),
                new Parquet.Schema.DataField<string>("freq"),                               
                new Parquet.Schema.DataField<int?>("dims"),                
                new Parquet.Schema.DataField<string>("dim1"),
                new Parquet.Schema.DataField<string>("dim2"),
                new Parquet.Schema.DataField<string>("label"),
                new Parquet.Schema.DataField<string>("source"),
                new Parquet.Schema.DataField<string>("unit"),
                new Parquet.Schema.DateTimeDataField("date_start", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true),
                new Parquet.Schema.DateTimeDataField("date_end", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true),
                new Parquet.Schema.DateTimeDataField("stamp", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true),
                // ----                
                new Parquet.Schema.DateTimeDataField("date", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true), //Probably milliseconds, which with 64-bit can take a crazy big range of years.                
                new Parquet.Schema.DataField<double?>("value")                
            );                        

            using (Stream fileStream = File.Create(@"c:\tools\multi.parquet"))
            using (ParquetWriter writer = await ParquetWriter.CreateAsync(schema, fileStream))
            {             
                                
                Dictionary<string, string> metadata = new Dictionary<string, string>();                
                metadata.Add("software.name", "Gekko Timeseries and Modeling Software");
                metadata.Add("software.version", Globals.gekkoVersion);
                metadata.Add("parquet.design.version", "1.0.0"); //Gekko's version of the Parquet schema.              
                metadata.Add("export.timestamp", DateTime.UtcNow.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
                metadata.Add("column.id.comment", "An id corresponding to the Gekko name, for merging rowgroup1 into rowgroup2. Only lower-case, no blanks.");
                metadata.Add("column.bank.comment", "Gekko databank name, same as file name without extension (for future use, to store several databanks in 1 parquet file).");
                metadata.Add("column.name.comment", "The Gekko series name. Alphanumeric or underscore chars, lower or upper-case.");
                metadata.Add("column.freq.comment", "The Gekko frequency: a (annual), q (quarterly), m (monthly), w (weekly), d (daily), u (undated). Lower-case.");
                metadata.Add("column.dims.comment", "Number of dimensions of the given (array-) timeseries. Integer.");
                metadata.Add("column.dim1.comment", "Dimension 1. String.");
                metadata.Add("column.dim2.comment", "Dimension 2. Possible dim3, dim3, etc., too. String.");
                metadata.Add("column.label.comment", "The label of the given timeseries. String.");
                metadata.Add("column.source.comment", "The source of the given timeseries. String.");
                metadata.Add("column.unit.comment", "The unit of the given timeseries. String.");
                metadata.Add("column.date_start.comment", "The date of the first value of the timeseries in the Gekko databank. Date format.");
                metadata.Add("column.date_end.comment", "The date of the last value of the timeseries in the Gekko databank. Date format.");
                metadata.Add("column.stamp.comment", "The timestamp corresponding to the last time the timeseries was changed in the Gekko databank. Date format.");
                metadata.Add("column.date.comment", "The date of the current data value (can represent a period like a full quarter). Date format.");
                metadata.Add("column.value.comment", "The data value. Numeric floating-point.");
                writer.CustomMetadata = metadata;

                using (ParquetRowGroupWriter group = writer.CreateRowGroup())
                {
                    int rowCount = names.Length;
                    int i = -1;                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], ids1));
                    //
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], banks));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], names));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], freqs));                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], dims));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], dim1));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], dim2));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], labels));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], sources));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], units));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], date_starts));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], date_ends));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], stamps));                    
                    //                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<double?>(null, rowCount).ToArray()));                                    
                }                

                using (ParquetRowGroupWriter group = writer.CreateRowGroup())
                {
                    int rowCount = ids2.Length;
                    int i = -1;
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], ids2));                    
                    //
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<int?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    //                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], dates));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], values));
                }
            }

            /* Works with this Python file
             * 
import pyarrow.parquet as pq
import pandas as pd
import matplotlib.pyplot as plt

# Tilpas filnavn...:
# parquet_file = pq.ParquetFile("c:\\tools\\makro.parquet")
parquet_file = pq.ParquetFile("c:\\tools\\mona.parquet")

# cols = id, bank, name, freq, dims, dim1, dim2, dim3, ... , label, source, unit, date_start, date_end, stamp, date, value
df0 = parquet_file.read_row_group(0).to_pandas()
df0 = df0.drop(columns=['date', 'value']) # Fjern null-kolonner
df1 = parquet_file.read_row_group(1).to_pandas()
df1 = df1[['id', 'date', 'value']] # Fjern null-kolonner
df = df1.merge(df0, on="id", how="left") # Sætter rowgroup0-dataframe (df0) ind i rowgroup1-dataframe (df1)

print(); print(df0)
print(); print(df1)
print(); print(df)

# Plot
i = 0; max = 2
for name, group in df.groupby("id"):
    plt.plot(group["date"], group["value"], label=name, marker = 'o', markersize=4)
    if (i >= max - 1): break
    i += 1
plt.xlabel("Date")
plt.ylabel("Value")
plt.title("Plot")
plt.legend()
plt.xticks(rotation=45)
plt.show()        

print('Færdig')

             * */
        }

        public static async Task ReadParquetDatabank(Databank databank, Program.ReadInfo readInfo, string filePath, List<string> errors)
        {
            //
            // NOTE: regarding dates/periods, only the string period for each observation is used, together with the DateTime stamp.
            //       The 4 start/end dates/periods are not used, and the DateTime date is not used.
            //

            DateTime dt1 = DateTime.Now;
            int yearMin = int.MaxValue;
            int yearMax = int.MinValue;
            GekkoDictionary<string, bool> nameCounter = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            int dateWarnings1 = 0;
            int dateWarnings2 = 0;

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
            using (ParquetReader reader = await ParquetReader.CreateAsync(fileStream))
            {
                string version = null;
                try
                {
                    Dictionary<string, string> metadata = reader.CustomMetadata;
                    metadata.TryGetValue("version", out version);
                }
                catch { Error("Metadata error", errors); }                

                using (ParquetRowGroupReader group = reader.OpenRowGroupReader(0))
                {
                    try { ids1 = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "id"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'id'", errors); }
                    try { banks = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "bank"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'bank'", errors); }
                    if (banks.Length > 1)
                    {
                        string bankTest = banks[0];
                        foreach (string s in banks)
                        {
                            if (!G.Equal(bankTest, s))
                            {
                                Error("Rowgroup 0: Some values in the column 'bank' are different: this is currently unsupported", errors);
                            }
                        }
                    }
                    try { names = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "name"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'name'", errors); }
                    try { freqs = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "freq"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'freq'", errors); }
                    try { dims = ((int?[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "dims"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'dims'", errors); }
                    int dimMax = 0;
                    foreach (int i in dims)
                    {
                        if (i < 0) Error("Rowgroup 0: Dims element with value " + i, errors);
                        dimMax = Math.Max(dimMax, i);
                    }
                    dimis = new string[dimMax][];
                    for (int i = 1; i <= dimMax; i++)
                    {
                        try { dimis[i - 1] = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "dim" + i))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column '" + "dim" + i + "'", errors); }
                    }
                    try { labels = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "label"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'label'", errors); }
                    try { sources = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "source"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'source'", errors); }
                    try { units = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "unit"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'unit'", errors); }
                    try { is_timelesss = ((bool?[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "is_timeless"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'is_timeless'", errors); }
                    try { date_starts = ((DateTime?[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date_start"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'date_start'", errors); }
                    try { date_ends = ((DateTime?[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date_end"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'date_end'", errors); }
                    try { period_starts = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period_start"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'period_start'", errors); }
                    try { period_ends = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period_end"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'period_end'", errors); }
                    try { stamps = ((DateTime?[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "stamp"))).Data).ToArray(); } catch { Error("Rowgroup 0: Could not find column 'stamp'", errors); }
                }

                using (ParquetRowGroupReader group = reader.OpenRowGroupReader(1))
                {
                    try { ids2 = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "id"))).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'id'", errors); }
                    try { dates = ((DateTime?[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "date"))).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'date'", errors); }
                    try { periods = ((string[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "period"))).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'period'", errors); }
                    try { values = ((double?[])(await group.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "value"))).Data).ToArray(); } catch { Error("Rowgroup 1: Could not find column 'value'", errors); }
                }
            }

            if (ids1 == null || ids1.Length == 0) Error("Rowgroup 1: Number of rows is = 0", errors);
            if (ids2 == null || ids2.Length == 0) Error("Rowgroup 2: Number of rows is = 0", errors);

            string[] namePrettys = new string[ids1.Length];

            Dictionary<string, int> vars = new Dictionary<string, int>();
            for (int i = 0; i < ids1.Length; i++)
            {
                try
                {
                    string id = ids1[i];
                    string id2 = names[i] + "!" + freqs[i];
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

                string namePretty = namePrettys[i1];
                // ---
                DateTime? date = dates[i2]; //Not used
                string period = periods[i2];
                double? value = values[i2];

                if (currentSeries == null || currentSeries != namePretty)
                {
                    currentSeries = namePretty;
                    ts = databank.GetIVariableMayCreateSeries(namePretty) as Series;
                    // ----
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

                        if (false)
                        {
                            //TODO
                            //TODO
                            //TODO Make an option to test for this
                            //TODO
                            //TODO
                            try
                            {
                                if (date != null && freq != null)
                                {
                                    GekkoTime gt2 = GekkoTime.FromDateTimeToGekkoTime(G.ConvertFreq(freq), (DateTime)date);
                                    if (!gt.Equals(gt2)) dateWarnings1++;
                                }
                            }
                            catch { } //Do not fail on this
                        }

                        ts.SetData(gt, d);

                        yearMin = Math.Min(yearMin, gt.super);
                        yearMax = Math.Max(yearMax, gt.super);
                    }
                }
            }
            if (dateWarnings1 > 0) G.Warning("w44.1", dateWarnings1 + " rows in rowgroup 1 with non-matching 'date' and 'period' values");
            if (dateWarnings2 > 0) G.Warning("w44.1", dateWarnings1 + " rows in rowgroup 1 with 'date' non-null and 'period' null");
            
            readInfo.startPerInFile = yearMin;
            readInfo.endPerInFile = yearMax;
            readInfo.nanCounter = 0;

            readInfo.variables = nameCounter.Count;
            readInfo.time = (DateTime.Now - dt1).TotalMilliseconds;

            readInfo.startPerResultingBank = readInfo.startPerInFile;
            readInfo.endPerResultingBank = readInfo.endPerInFile;
        }

        private static void Error(string s, List<string> errors)
        {
            errors.Add(s);
            throw new GekkoException();
        }

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
                RecordBatch rb = ReadArrowOld(_fileName);
                DataFrame df2 = DataFrame.FromArrowRecordBatch(rb);
                s3 = "Read arrow took: " + (DateTime.Now - dt1).TotalMilliseconds / 1000d;
                G.Writeln(s3);
                //IEnumerable<RecordBatch> rb2 = df2.ToArrowRecordBatches();
            }

            int ii = 1;
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

        public static async Task RunReaderExample(string pathAndFilename)
        {
            // Ensure the WriteArrow method is called first to create the file
            // Your existing code to write the file...

            List<ArrowDataRow> data = await ReadArrow(pathAndFilename);

            new Writeln($"Successfully read {data.Count} rows.");
            foreach (var row in data.Take(50)) // Print the first 5 rows
            {
                new Writeln("" + row);
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
                RecordBatch recordBatch2 = Arrow.ReadArrowOld(Globals.ttPath2 + @"\regres\Databanks\jul05.arrow");
                DataFrame df2 = DataFrame.FromArrowRecordBatch(recordBatch2);
                Databank db2 = new Databank(null);
            }

            string s = null; if (hasSubSeries) s = " (including array-subseries)";
            new Writeln("Wrote " + seriesCounter + " series" + s + " to arrow file with " + rowCounter + " rows in " + G.Seconds(dt));

        }

        public static async Task WriteParquetDatabank(List<Tuple<string, IVariable>> listSorted, GekkoTime t1, GekkoTime t2, string pathAndFilename)
        {            
            //Note: the input list is already sorted by name

            string gekkoParquetVersion = "1.0.0";

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
            List<Parquet.Schema.DataField> m = new List<Parquet.Schema.DataField>();
            m.Add(new Parquet.Schema.DataField<string>("id"));            
            m.Add(new Parquet.Schema.DataField<string>("bank"));
            m.Add(new Parquet.Schema.DataField<string>("name"));
            m.Add(new Parquet.Schema.DataField<string>("freq"));
            m.Add(new Parquet.Schema.DataField<int?>("dims"));
            for (int ii = 0; ii < ndims; ii++)
            {
                m.Add(new Parquet.Schema.DataField<string>("dim" + (ii + 1)));
            }            
            m.Add(new Parquet.Schema.DataField<string>("label"));
            m.Add(new Parquet.Schema.DataField<string>("source"));
            m.Add(new Parquet.Schema.DataField<string>("unit"));
            m.Add(new Parquet.Schema.DataField<bool?>("is_timeless"));
            m.Add(new Parquet.Schema.DateTimeDataField("date_start", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true));
            m.Add(new Parquet.Schema.DateTimeDataField("date_end", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true));            
            m.Add(new Parquet.Schema.DataField<string>("period_start"));
            m.Add(new Parquet.Schema.DataField<string>("period_end"));
            m.Add(new Parquet.Schema.DateTimeDataField("stamp", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true));
            // ----                
            m.Add(new Parquet.Schema.DateTimeDataField("date", Parquet.Schema.DateTimeFormat.DateAndTime, isNullable: true)); //Probably milliseconds, which with 64-bit can take a crazy big range of years.                
            m.Add(new Parquet.Schema.DataField<string>("period"));
            m.Add(new Parquet.Schema.DataField<double?>("value"));
            Parquet.Schema.ParquetSchema schema = new Parquet.Schema.ParquetSchema(m);           

            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("software.name", "Gekko Timeseries and Modeling Software");
            metadata.Add("software.version", Globals.gekkoVersion);
            metadata.Add("parquet.design.version", gekkoParquetVersion); //Gekko's version of the Parquet schema.              
            metadata.Add("export.timestamp", DateTime.UtcNow.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
            metadata.Add("column.id.comment", "An id corresponding to the Gekko name, for merging rowgroup1 into rowgroup2. Only lower-case, no blanks.");
            metadata.Add("column.bank.comment", "Gekko databank name, same as file name without extension (for future use, to store several databanks in 1 parquet file).");
            metadata.Add("column.name.comment", "The Gekko series name. Alphanumeric or underscore chars, lower or upper-case.");
            metadata.Add("column.freq.comment", "The Gekko frequency: a (annual), q (quarterly), m (monthly), w (weekly), d (daily), u (undated). Lower-case.");
            metadata.Add("column.dims.comment", "Number of dimensions of the given (array-) timeseries. Integer.");
            for (int ii = 0; ii < ndims; ii++)
            {
                metadata.Add("column.dim" + (ii + 1) + ".comment", "Dimension " + (ii + 1) + ". String.");
            }            
            metadata.Add("column.label.comment", "The label of the given timeseries. String.");
            metadata.Add("column.source.comment", "The source of the given timeseries. String.");
            metadata.Add("column.unit.comment", "The unit of the given timeseries. String.");
            metadata.Add("column.is_timeless.comment", "True if the timeseries is constant for all periods. Boolean.");
            metadata.Add("column.date_start.comment", "The date corresponding to the first value of the timeseries in the Gekko databank. Date format, Unix time. Quarters etc. are identified as their *first* day.");
            metadata.Add("column.date_end.comment", "The date corresponding to the last value of the timeseries in the Gekko databank. Date format, Unix time. Quarters etc. are identified as their *first* day");
            metadata.Add("column.period_start.comment", "The period corresponding to the first value of the timeseries in the Gekko databank. String format.");
            metadata.Add("column.period_end.comment", "The period corresponding to the last value of the timeseries in the Gekko databank. String format.");
            metadata.Add("column.stamp.comment", "The timestamp corresponding to the last time the timeseries was changed in the Gekko databank. Date format, Unix time.");
            metadata.Add("column.date.comment", "The date corresponding to the current data value. Date format, Unix time. Quarters etc. are identified as their *first* day.");
            metadata.Add("column.period.comment", "The period corresponding to the current data value. String format.");
            metadata.Add("column.value.comment", "The data value. Numeric floating-point.");

            int valuesCounter = 0;
            bool hasSubSeries = false;
            int seriesCounter = 0;
            
            string bank = Path.GetFileNameWithoutExtension(pathAndFilename);

            foreach (Tuple<string, IVariable> tup in listSorted)
            {
                if (tup.Item2.Type() != EVariableType.Series) continue;
                Series ts = tup.Item2 as Series;
                                
                bool isTimeless = ts.type == ESeriesType.Timeless;

                string fullName = G.Chop_AddBank(tup.Item1, bank).Replace(" ", "").ToLower();            
                string freq = G.Chop_GetFreq(tup.Item1);
                
                //MultidimItem mmi = ts.mmi;
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

                seriesCounter++;

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
                // It seems that Parquet.NET ignores any UTC or not flag anyway.
                // -------------------------------------------------------------------------------------------------------

                bool b = false;
                string xtimestamp = ts.MetaGetStamp();
                if (xtimestamp != null)
                {
                    int c1 = G.Count(xtimestamp, "-");
                    int c2 = G.Count(xtimestamp, "/"); //older
                    if (c1 == 2 || c2 == 2)
                    {
                        string[] ss = null;
                        if (c1 == 2) ss = ts.MetaGetStamp().Split('-');
                        else ss = ts.MetaGetStamp().Split('/');                        
                        int i0 = -12345; int.TryParse(ss[0], out i0);
                        int i1 = -12345; int.TryParse(ss[1], out i1);
                        int i2 = -12345; int.TryParse(ss[2], out i2);
                        if (i0 != -12345 && i1 != -12345 && i2 != -12345)
                        {
                            if (i2 >= 0 && i2 <= 99) i2 += 2000; //Gekko did not exist in year 19xx, so this should be safe regarding stamps.
                            if (i0 >= 1 && i0 <= 31 && i1 >= 1 && i1 <= 12 && G.IsYear(i2))
                            {
                                b = true;                                
                                try { stamps.Add(new DateTime(i2, i1, i0)); } //Non-UTC, but UtcDateTime(i2, i1, i0) does not change anything
                                catch { b = false; }
                            }
                        }
                    }
                }
                if (!b) stamps.Add(null);
                                
                if (!gt1.IsNull())
                {
                    try { date_starts.Add(GekkoTime.FromGekkoTimeToDateTime(gt1, O.GetDateChoices.FlexibleStart)); } //Non-utc, but if using .ToUniversalTime(), it messes up the hours
                    catch { date_starts.Add(null); }
                    period_starts.Add(DateStringFormat(gt1));
                }
                else
                {
                    date_starts.Add(null);
                    period_starts.Add(null);
                }
                
                if (!gt2.IsNull())
                {
                    try { date_ends.Add(GekkoTime.FromGekkoTimeToDateTime(gt2, O.GetDateChoices.FlexibleStart)); } //Non-utc, but if using .ToUniversalTime(), it messes up the hours
                    catch { date_ends.Add(null); }
                    period_ends.Add(DateStringFormat(gt2));
                }
                else
                {
                    date_ends.Add(null);
                    period_ends.Add(null);
                }

                if (gt1.IsNull() || gt2.IsNull())
                {
                    valuesCounter++;
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
                        valuesCounter++;
                        ids2.Add(fullName);
                        dates.Add(GekkoTime.FromGekkoTimeToDateTime(t, O.GetDateChoices.FlexibleStart));
                        periods.Add(DateStringFormat(t));
                        values.Add(ts.GetDataSimple(t));
                    }
                }
            }

            using (FileStream fileStream = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
            using (ParquetWriter writer = await ParquetWriter.CreateAsync(schema, fileStream))
            {
                writer.CustomMetadata = metadata;

                using (ParquetRowGroupWriter group = writer.CreateRowGroup())
                {
                    int rowCount = seriesCounter;
                    int i = -1;
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], ids1.ToArray()));
                    //
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], banks.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], names.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], freqs.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], dims.ToArray()));
                    for (int ii = 0; ii < ndims; ii++)
                    {
                        i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], dimss[ii].ToArray()));
                    }                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], labels.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], sources.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], units.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], timelesss.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], date_starts.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], date_ends.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], period_starts.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], period_ends.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], stamps.ToArray()));
                    //                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<double?>(null, rowCount).ToArray()));                    
                }

                using (ParquetRowGroupWriter group = writer.CreateRowGroup())
                {
                    int rowCount = valuesCounter;
                    int i = -1;
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], ids2.ToArray()));
                    //
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<int?>(null, rowCount).ToArray()));
                    for (int ii = 0; ii < ndims; ii++)
                    {
                        i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    }                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<bool?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<string>(null, rowCount).ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], Enumerable.Repeat<DateTime?>(null, rowCount).ToArray()));
                    //                    
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], dates.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], periods.ToArray()));
                    i++; await group.WriteColumnAsync(new DataColumn(schema.DataFields[i], values.ToArray()));                    
                }
            }

            string s = null; if (hasSubSeries) s = " (including array-subseries)";
            new Writeln("Wrote " + seriesCounter + " series" + s + " to parquet file with " + valuesCounter + " rows in two rowgroups in " + G.Seconds(dt));

        }

        private static string DateStringFormat(GekkoTime gt1)
        {
            //Output
            return gt1.ToString();
        }

        private static GekkoTime DateStringFormat(string s)
        {
            //Input
            return GekkoTime.FromStringToGekkoTime(s, true);
        }

        public static async void WriteArrow(RecordBatch recordBatch, string fileName)
        {
            if (true)
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

        //public static void WriteArrow(IEnumerable<RecordBatch> batches, string fileName)
        //{
        //    File.Delete(fileName);
        //    using (var stream = File.OpenWrite(fileName))
        //    using (var writer = new ArrowStreamWriter(stream, batches.First().Schema))
        //    {
        //        foreach (RecordBatch b in batches)
        //        {
        //            writer.WriteRecordBatchAsync(b);
        //        }
        //        writer.WriteEndAsync();
        //    }
        //}
    }
}
