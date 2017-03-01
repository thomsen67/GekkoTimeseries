using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gekko
{
    public class Functions
    {
        //NOTE:
        //NOTE: All function names should be with lower-case only!!
        //NOTE:

        public enum EElementByElementType {
            Times,
            Divide            
        }

        public enum ESumDim {
            Rows,
            Cols
        }
        
        public enum ESumType {
            Min,
            Max,
            Sum,
            Avg
        }

        //just to test against user defined function
        public static IVariable sum_test_method(GekkoTime t, IVariable x1, IVariable x2)
        {
            double y1 = x1.GetVal(t);
            double y2 = x2.GetVal(t);            
            double y = y1 + y2;
            return new ScalarVal(y);
        }

        public static IVariable concat(GekkoTime t, IVariable x1, IVariable x2)
        {
            //same as %s1 + %s2 anyway.
            string s1 = O.GetString(x1);
            string s2 = O.GetString(x2);
            return new ScalarString(s1 + s2);            
        }        

        //rename to substring()??
        public static IVariable piece(GekkoTime t, IVariable x1, IVariable x2, IVariable x3)
        {
            string s = null;
            string s1 = O.GetString(x1);
            int i2 = O.GetInt(x2);
            int i3 = O.GetInt(x3);
            if (i3 < 0)
            {
                //AREMOS supports this...
                G.Writeln2("*** ERROR: piece(): size < 0 not supported.");
                throw new GekkoException();
            }
            int a, b = 0;
            if (i2 < 0)
            {
                a = s1.Length + i2;
                b = i3;
            }
            else
            {
                a = i2 - 1;
                b = i3;
            }
            if (a < 0 || a > s1.Length - 1 || a + b > s1.Length)
            {
                G.Writeln2("*** ERROR: piece() function with start " + (a + 1) + " and size " + b + " is not possible");
                G.Writeln("           on a string of size " + s1.Length + ".", System.Drawing.Color.Red);
                throw new GekkoException();
            }
            s = s1.Substring(a, b);
            return new ScalarString(s);
        }

        public static void HandleLasp(GekkoTuple.Tuple2 tuple, IVariable p, IVariable q) {
            //This is pretty bad style, but the content of the tuple is put into p and q...

            TimeSeries tsp1 = O.GetTimeSeries(p);
            TimeSeries tsq1 = O.GetTimeSeries(q);

            TimeSeries tsp2 = O.GetTimeSeries(tuple.tuple0);
            TimeSeries tsq2 = O.GetTimeSeries(tuple.tuple1);

            tsp2.variableName = tsp1.variableName;
            tsq2.variableName = tsq1.variableName;

            tsp1.parentDatabank.RemoveVariable(tsp1.variableName);
            tsq1.parentDatabank.RemoveVariable(tsq1.variableName);

            tsp1.parentDatabank.AddVariable(tsp2);
            tsq1.parentDatabank.AddVariable(tsq2);
        }

        public static GekkoTuple.Tuple2 laspchain(GekkoTime t, IVariable plist, IVariable xlist, IVariable date)
        {            
            GekkoTuple.Tuple2 tuple = Program.GenrTuple("laspchain", plist, xlist, date.GetDate(O.GetDateChoices.Strict), Globals.globalPeriodStart, Globals.globalPeriodEnd);           
            return tuple;
        }

        public static GekkoTuple.Tuple2 laspfixed(GekkoTime t, IVariable plist, IVariable xlist, IVariable date)
        {
            GekkoTuple.Tuple2 tuple = Program.GenrTuple("laspfixed", plist, xlist, O.GetDate(date), Globals.globalPeriodStart, Globals.globalPeriodEnd);
            return tuple;
        }

        public static IVariable hpfilter(GekkoTime t, IVariable rightSide, IVariable ilambda)
        {
            return hpfilter(t, null, null, rightSide, ilambda, new ScalarVal(0d));
        }

        public static IVariable hpfilter(GekkoTime t, IVariable per1, IVariable per2, IVariable rightSide, IVariable ilambda)
        {
            return hpfilter(t, per1, per2, rightSide, ilambda, new ScalarVal(0d));
        }

        public static IVariable hpfilter(GekkoTime t, IVariable rightSide, IVariable ilambda, IVariable ilog)
        {
            return hpfilter(t, null, null, rightSide, ilambda, ilog);
        }

        public static IVariable hpfilter(GekkoTime t, IVariable per1, IVariable per2, IVariable rightSide, IVariable ilambda, IVariable ilog) 
        {
            GekkoTime tStart = Globals.tNull;
            GekkoTime tEnd = Globals.tNull;
            if (per1 == null && per2 == null)
            {
                tStart = Globals.globalPeriodStart;
                tEnd = Globals.globalPeriodEnd;
            }
            else
            {
                tStart = O.GetDate(per1);
                tEnd = O.GetDate(per2);
            }           
            
            int obs = GekkoTime.Observations(tStart, tEnd);

            double lambda = O.GetVal(ilambda, t);
            double log = O.GetVal(ilog, t);

            TimeSeries lhs = new TimeSeries(Program.options.freq, null);            

            TimeSeries rhs = O.GetTimeSeries(rightSide);                        

            bool isLog = false;
            if (log == 0d)
            {
                isLog = false;
            }
            else if (log == 1d)
            {
                isLog = true;
            }
            else
            {
                G.Writeln2("*** ERROR: hpfilter() logarithm argument should be 0 or 1");
                throw new GekkoException();
            }
            
            if (obs < 2)
            {
                G.Writeln2("*** ERROR: hpfilter() needs at least two observations to make sense");
                throw new GekkoException();
            }

            double[] input = new double[obs];

            int counter = -1;
            foreach (GekkoTime gt in new GekkoTimeIterator(tStart, tEnd))
            {
                counter++;
                if (isLog)
                {
                    input[counter] = Math.Log(rhs.GetData(gt));
                }
                else
                {
                    input[counter] = rhs.GetData(gt);
                }
            }

            HPfilter hpf = new HPfilter();
            double[] output = hpf.HPFilter(input, lambda);

            counter = -1;
            foreach (GekkoTime gt in new GekkoTimeIterator(tStart, tEnd))
            {
                counter++;
                if (isLog)
                {
                    lhs.SetData(gt, Math.Exp(output[counter]));
                }
                else
                {
                    lhs.SetData(gt, output[counter]);
                }
            }
                                  
            return new MetaTimeSeries(lhs);            
        }        

        // ====================== matrix stuff ===============================
        // ====================== matrix stuff ===============================
        // ====================== matrix stuff ===============================

        public static IVariable t(GekkoTime t, IVariable x1)
        {
            if (x1.Type() != EVariableType.Matrix)
            {
                G.Writeln("*** ERROR: t(): transpose can only be used on matrices");
                throw new GekkoException();
            }
            Matrix x = (Matrix)x1;
            int d1 = x.data.GetLength(0);
            int d2 = x.data.GetLength(1);
            Matrix y = new Matrix(d2, d1);
            for (int i = 0; i < d1; i++)
            {
                for (int j = 0; j < d2; j++)
                {
                    y.data[j, i] = x.data[i, j];
                }
            }
            return y;
        }        

        //Converts timeseries to matrix
        public static IVariable pack(GekkoTime t, params IVariable[] vars)
        {                        
            GekkoTime gt1 = Globals.globalPeriodStart;
            GekkoTime gt2 = Globals.globalPeriodEnd;
            int offset = 0;
            int obs = PackHelper(vars, ref gt1, ref gt2, ref offset);            
                        
            List<TimeSeries> tss = new List<TimeSeries>();
            for (int j = offset; j < vars.Length; j++)
            {
                if (vars[j].Type() == EVariableType.List)
                {
                    foreach (string s in ((MetaList)vars[j]).list)
                    {
                        TimeSeries tmp = Program.GetTimeSeriesFromString(s, O.ECreatePossibilities.None);
                        tss.Add(tmp);
                    }
                }
                else if (vars[j].Type() == EVariableType.TimeSeries)
                {
                    tss.Add(((MetaTimeSeries)vars[j]).ts);
                }       
                else
                {
                    G.Writeln2("*** ERROR: Expected timeseries or list as argument");
                    throw new GekkoException();
                }         
            }

            int n = tss.Count;
            if (n < 1)
            {
                G.Writeln2("*** ERROR: Number of items is " + n);
                throw new GekkoException();
            }

            Matrix m = new Matrix(obs, n);

            //    List<TimeSeries> tss = Program.GetTimeSeriesFromStringWildcard(s);

            int varcount = -1;
            foreach(TimeSeries ts in tss)
            {
                varcount++;
                int counter = -1;
                foreach (GekkoTime gt in new GekkoTimeIterator(gt1, gt2))
                {
                    counter++;
                    m.data[counter, varcount] = ts.GetData(gt);
                }
            }
            return m;
        }

        public static IVariable det(GekkoTime t, IVariable x)
        {
            Matrix m = O.GetMatrix(x);
            double d = alglib.rmatrixdet(m.data);
            return new ScalarVal(d);
        }

        public static IVariable rows(GekkoTime t, IVariable x1)
        {
            Matrix m = O.GetMatrix(x1);
            return new ScalarVal(m.data.GetLength(0));            
        }

        public static IVariable cols(GekkoTime t, IVariable x1)
        {
            Matrix m = O.GetMatrix(x1);
            return new ScalarVal(m.data.GetLength(1));
        }        

        public static IVariable multiply(GekkoTime t, IVariable x1, IVariable x2)
        {
            return ElementByElementHelper(EElementByElementType.Times, t, x1, x2);
        }

        public static IVariable divide(GekkoTime t, IVariable x1, IVariable x2)
        {
            return ElementByElementHelper(EElementByElementType.Divide, t, x1, x2);
        }

        public static IVariable zeroes(GekkoTime t, IVariable x1, IVariable x2)
        {
            return zeros(t, x1, x2);
        }

        public static IVariable i(GekkoTime t, IVariable x)
        {
            int n = O.GetInt(x);
            Matrix m = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                m.data[i, i] = 1d;
            }            
            return m;
        }

        public static IVariable diag(GekkoTime t, IVariable x)
        {
            Matrix m = null;
            Matrix xx = O.GetMatrix(x);
            //for a 1x1 matrix, either of the two cases below yields the same!
            if (xx.data.GetLength(1) == 1)
            {
                int n = xx.data.GetLength(0);  //rows
                m = new Matrix(n, n);
                for (int i = 0; i < n; i++)
                {
                    m.data[i, i] = xx.data[i, 0];
                }
            }
            else
            {
                int n = CheckSquare(xx);
                m = new Matrix(n, 1);
                for (int i = 0; i < n; i++)
                {
                    m.data[i, 0] = xx.data[i, i];
                }
            }
            return m;
        }

        public static IVariable trace(GekkoTime t, IVariable x)
        {
            Matrix m = O.GetMatrix(x);
            int n = CheckSquare(m);
            double d = 0d;
            for (int i = 0; i < n; i++)
            {
                d += m.data[i, i];
            }
            return new ScalarVal(d);
        }

        private static int CheckSquare(Matrix m)
        {
            if (m.data.GetLength(0) != m.data.GetLength(1))
            {
                G.Writeln("*** ERROR: The matrix is not square (rows = " + m.data.GetLength(0) + ", cols = " + m.data.GetLength(1));
                throw new GekkoException();
            }
            return m.data.GetLength(0);
        }

        public static IVariable inv(GekkoTime t, IVariable x)
        {
            Matrix m = O.GetMatrix(x);
            int n = CheckSquare(m);
            Matrix clone = m.Clone();
            clone.data = Program.InvertMatrix(clone.data);
            return clone;
        }
        
        public static IVariable zeros(GekkoTime t, IVariable x1, IVariable x2)
        {
            int n1 = O.GetInt(x1);
            int n2 = O.GetInt(x2);
            Matrix m = new Matrix(n1, n2);
            return m;
        }

        public static IVariable sumr(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Rows, ESumType.Sum);
        }

        public static IVariable sumc(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Cols, ESumType.Sum);
        }

        public static IVariable avgr(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Rows, ESumType.Avg);
        }

        public static IVariable avgc(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Cols, ESumType.Avg);
        }

        public static IVariable minr(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Rows, ESumType.Min);
        }

        public static IVariable minc(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Cols, ESumType.Min);
        }
        
        public static IVariable maxr(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Rows, ESumType.Max);
        }

        //missing value
        public static IVariable miss(GekkoTime t)
        {
            return new ScalarVal(double.NaN);
        }       

        public static IVariable miss(GekkoTime t, IVariable x1, IVariable x2)
        {
            int n1 = O.GetInt(x1);
            int n2 = O.GetInt(x2);
            Matrix m = new Matrix(n1, n2, double.NaN);
            return m;
        }

        public static IVariable maxc(GekkoTime t, IVariable x)
        {
            return SumHelper(t, x, ESumDim.Cols, ESumType.Max);
        }
        
        public static IVariable SumHelper(GekkoTime t, IVariable x, ESumDim dim, ESumType type)
        {
            Matrix m = O.GetMatrix(x);
            int rows = m.data.GetLength(0);
            int cols = m.data.GetLength(1);

            int max = -12345;
            if (dim == ESumDim.Rows) max = rows;
            else if (dim == ESumDim.Rows) max = cols;

            Matrix m2 = null;
            if (dim == ESumDim.Cols)
            {
                if (type == ESumType.Min)
                {
                    m2 = new Matrix(1, cols, double.MaxValue);
                }
                else if (type == ESumType.Max)
                {
                    m2 = new Matrix(1, cols, double.MinValue);
                }
                else if (type == ESumType.Sum || type == ESumType.Avg)
                {
                    m2 = new Matrix(1, cols);
                }
                else throw new GekkoException();
            }
            else if (dim == ESumDim.Rows)
            {
                if (type == ESumType.Min)
                {
                    m2 = new Matrix(rows, 1, double.MaxValue);
                }
                else if (type == ESumType.Max)
                {
                    m2 = new Matrix(rows, 1, double.MinValue);
                }
                else if (type == ESumType.Sum || type == ESumType.Avg)
                {
                    m2 = new Matrix(rows, 1);
                }
                else throw new GekkoException();
            }
            else throw new GekkoException();

            if (dim == ESumDim.Cols)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (type == ESumType.Min)
                        {
                            if (m2.data[0, j] > m.data[i, j]) m2.data[0, j] = m.data[i, j];
                        }
                        else if (type == ESumType.Max)
                        {
                            if (m2.data[0, j] < m.data[i, j]) m2.data[0, j] = m.data[i, j];
                        }
                        else if (type == ESumType.Sum || type == ESumType.Avg)
                        {
                            m2.data[0, j] += m.data[i, j];
                        }
                        else throw new GekkoException();
                    }                    
                }
                if (type == ESumType.Avg)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        m2.data[0, j] = m2.data[0, j] / (double)rows;
                    }
                }
            }
            else if (dim == ESumDim.Rows)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (type == ESumType.Min)
                        {
                            if (m2.data[i, 0] > m.data[i, j]) m2.data[i, 0] = m.data[i, j];
                        }
                        else if (type == ESumType.Max)
                        {
                            if (m2.data[i, 0] < m.data[i, j]) m2.data[i, 0] = m.data[i, j];
                        }
                        else if (type == ESumType.Sum || type == ESumType.Avg)
                        {
                            m2.data[i, 0] += m.data[i, j];
                        }
                        else throw new GekkoException();
                    }
                }
                if (type == ESumType.Avg)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        m2.data[i, 0] = m2.data[i, 0] / (double)cols;
                    }
                }
            }
            else throw new GekkoException();
            return m2;
        }

        public static IVariable ones(GekkoTime t, IVariable x1, IVariable x2)
        {
            int n1 = O.GetInt(x1);
            int n2 = O.GetInt(x2);
            Matrix m = new Matrix(n1, n2, 1d);
            return m;
        }

        //Multiplication element by element
        private static IVariable ElementByElementHelper(EElementByElementType type, GekkoTime t, IVariable x1, IVariable x2)
        {
            Matrix m1 = O.GetMatrix(x1);
            Matrix m2 = O.GetMatrix(x2);
            bool fixedRow1 = false;
            bool fixedCol1 = false;
            if (m1.data.GetLength(0) != m2.data.GetLength(0))
            {
                if (m2.data.GetLength(0) != 1)
                {
                    G.Writeln2("*** ERROR: " + type.ToString() + "(): There are " + m1.data.GetLength(0) + " and " + m2.data.GetLength(0) + " rows in the matrices");
                    throw new GekkoException();
                }
                else
                {
                    //for instance div ( 2x3 , 1x3 ) --> use fixed row 1 instead of i
                    fixedRow1 = true;
                }
            }
            if (m1.data.GetLength(1) != m2.data.GetLength(1))
            {
                if (m2.data.GetLength(1) != 1)
                {
                    G.Writeln2("*** ERROR: " + type.ToString() + "(): There are " + m1.data.GetLength(1) + " and " + m2.data.GetLength(1) + " cols in the matrices");
                    throw new GekkoException();
                }
                else
                {                    
                    //for instance div ( 2x3 , 2x1 ) --> use fixed column 1 instead of j
                    fixedCol1 = true;
                }
            }
            Matrix m = new Matrix(m1.data.GetLength(0), m1.data.GetLength(1));
            for (int i = 0; i < m1.data.GetLength(0); i++)
            {
                for (int j = 0; j < m1.data.GetLength(1); j++)
                {
                    int ii = i;
                    int jj = j;
                    if (fixedRow1) ii = 0; //internal arrays are 0-based
                    if (fixedCol1) jj = 0; //internal arrays are 0-based
                    if (type == EElementByElementType.Times)
                    {                        
                        m.data[i, j] = m1.data[i, j] * m2.data[ii, jj];
                    }
                    else if (type == EElementByElementType.Divide)
                    {
                        m.data[i, j] = m1.data[i, j] / m2.data[ii, jj];
                    }
                }
            }
            return m;
        }

        //Converts matrix to timeseries
        public static IVariable unpack(GekkoTime t, params IVariable[] vars)
        {
            
            GekkoTime gt1 = Globals.globalPeriodStart;
            GekkoTime gt2 = Globals.globalPeriodEnd;
            int offset = 0;
            int obs = PackHelper(vars, ref gt1, ref gt2, ref offset);

            int n = vars.Length - offset;
            if (n < 1)
            {
                G.Writeln2("*** ERROR: No matrix given");
                throw new GekkoException();
            }
            else if (n > 1)
            {
                G.Writeln2("*** ERROR: Only 1 matrix should be given");
                throw new GekkoException();
            }
            Matrix m = O.GetMatrix(vars[offset]);

            if (m.data.GetLength(1) > 1)
            {
                G.Writeln2("*** ERROR: The matrix provided should have 1 column only");
                throw new GekkoException();
            }

            if (m.data.GetLength(0) != obs)
            {
                G.Writeln2("*** ERROR: You provided " + obs + " periods for a matrix with " + m.data.GetLength(0) + " rows");
                throw new GekkoException();
            }

            TimeSeries ts = new TimeSeries(Program.options.freq, null);
            int counter = -1;
            foreach (GekkoTime gt in new GekkoTimeIterator(gt1, gt2))
            {
                counter++;
                ts.SetData(gt, m.data[counter, 0]);
            }
            return new MetaTimeSeries(ts);            
        }

        private static int PackHelper(IVariable[] vars, ref GekkoTime gt1, ref GekkoTime gt2, ref int offset)
        {
            if (vars.Length > 2)  //must be at least 1 variable
            {
                if ((vars[0].Type() == EVariableType.Date || vars[0].Type() == EVariableType.Val) || (vars[1].Type() == EVariableType.Date || vars[1].Type() == EVariableType.Val))
                {
                    //seems like two dates first
                    offset = 2;
                    gt1 = O.GetDate(vars[0]);
                    gt2 = O.GetDate(vars[1]);
                }
            }
            int obs = GekkoTime.Observations(gt1, gt2);
            if (obs < 1)
            {
                G.Writeln2("*** ERROR: Number of observations is " + obs);
                throw new GekkoException();
            }
            return obs;
        }

        public static IVariable length(GekkoTime t, IVariable x1)
        {            
            string s1 = O.GetString(x1);
            return new ScalarVal(s1.Length);
        }

        public static IVariable chol(GekkoTime t, IVariable x)
        {
            return chol(t, x, new ScalarString("upper"));
        }

        public static IVariable chol(GekkoTime t, IVariable x, IVariable type)
        {
            if (x.Type() != EVariableType.Matrix)
            {
                G.Writeln2("*** ERROR: Chol() only accepts a matrix");
                throw new GekkoException();
            }

            if (type.Type() != EVariableType.String)
            {
                G.Writeln2("*** ERROR: Chol() only accepts a string as type");
                throw new GekkoException();
            }

            double[,] y = ((Matrix)x).data;

            string s = ((ScalarString)type)._string2;

            bool upper = true;
            if(G.equal(s,"upper"))
            {

            }
            else if (G.equal(s, "lower"))
            {
                upper = false;
            }
            else
            {
                G.Writeln2("*** ERROR: Type must be 'upper' or 'lower'");
                throw new GekkoException();
            }

            double[,] z = Cholesky(y, upper);
            Matrix rv = new Matrix();
            rv.data = z;
            return rv;
        }

        public static IVariable rseed(GekkoTime t, IVariable seed)
        {
            double seed2 = O.GetVal(seed, t);
            int i = (int)seed2;
            Globals.random = new Random(i);            
            return new ScalarVal(i);
        }

        public static IVariable rnorm(GekkoTime t, IVariable means, IVariable vcov)
        {
            //Maybe it is stupid that we are using stddev versus matrix of covariance

            if (means.Type() == EVariableType.Matrix && vcov.Type() == EVariableType.Matrix)
            {
                //This will be SLOW, because of cholesky decomp for each drawing
                //To circumvent this, we would have to define an object to store to fixed cover matrix
                
                //Otherwise, we could output a n x sims matrix instead of n x 1.

                Matrix m = (Matrix)vcov;
                int n = m.data.GetLength(0);
                if (m.data.GetLength(1) != n)
                {
                    G.Writeln2("*** ERROR: Covar matrix is not square");
                    throw new GekkoException();
                }

                Matrix mean = (Matrix)means;
                if (mean.data.GetLength(0) != n || mean.data.GetLength(1) != 1)
                {
                    G.Writeln2("*** ERROR: Mean matrix does not correspond to covar matrix");
                    throw new GekkoException();
                }
                                
                double[,] tmp = Cholesky(m.data, false);
                //after this, #m = t(#tmp)*#tmp
                
                double[,] randoms = new double[n, 1];
                //https://en.wikipedia.org/wiki/Multivariate_normal_distribution#Drawing_values_from_the_distribution
                for (int i = 0; i < n; i++)
                {
                    double random = O.GetVal(rnorm(t, new ScalarVal(0d), new ScalarVal(1d)), t); //could be sped up by interfacing to the interior of the method
                    randoms[i, 0] = random;
                }                
                
                Matrix rv = new Matrix();
                rv.data = O.AddMatrixMatrix(mean.data, Program.MultiplyMatrices(tmp, randoms), n, 1);
                
                return rv;


            }
            else
            {
                double mean = O.GetVal(means, t);
                double stdDev = Math.Sqrt(O.GetVal(vcov, t));
                Random rand = new Random(); //reuse this if you are generating many
                double u1 = Globals.random.NextDouble(); //these are uniform(0,1) random doubles
                double u2 = Globals.random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
                double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
                return new ScalarVal(randNormal);
            }
        }

        private static double[,] Cholesky(double[,] m, bool upper)
        {
            if (m.GetLength(0) != m.GetLength(1))
            {
                G.Writeln2("*** ERROR: Matrix must be square");
                throw new GekkoException();
            }
            int n = m.GetLength(0);
            double[,] tmp = new double[n, n];

            if (upper)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = i; j < n; j++)
                    {
                        tmp[i, j] = m[i, j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < n; j++)
                {
                    for (int i = j; i < n; i++)
                    {
                        tmp[i, j] = m[i, j];
                    }
                }
            }

            bool result = alglib.spdmatrixcholesky(ref tmp, n, upper);

            if (!result)
            {
                G.Writeln2("*** ERROR: Could not perform Cholesky decomposition");
                throw new GekkoException();
            }

            return tmp;
        }

        public static IVariable runif(GekkoTime t)
        {
            double u2 = Globals.random.NextDouble();            
            return new ScalarVal(u2);
        }

        public static IVariable sum(GekkoTime t, params IVariable[] items)
        {
            if (items.Length == 0)
            {
                G.Writeln2("*** ERROR: sum() function must have > 0 arguments.");
                throw new GekkoException();
            }
            double v = 0d;
            foreach (IVariable a in items)
            {
                if (a.Type() == EVariableType.List)
                {
                    foreach (string s in ((MetaList)a).GetList()) v += O.IndirectionHelper(s).GetVal(t);
                }
                else
                {
                    v += a.GetVal(t);
                }
            }            
            return new ScalarVal(v);
        }

        public static IVariable avg(GekkoTime t, params IVariable[] items)
        {
            if (items.Length == 0)
            {
                G.Writeln2("*** ERROR: avg() function must have > 0 arguments.");
                throw new GekkoException();
            }
            double v = 0d;
            double n = 0d;
            foreach (IVariable a in items)
            {
                if (a.Type() == EVariableType.List)
                {
                    foreach (string s in ((MetaList)a).GetList())
                    {
                        v += O.IndirectionHelper(s).GetVal(t);
                        n++;
                    }
                }
                else
                {
                    v += a.GetVal(t);
                    n++;
                }
            }
            return new ScalarVal(v / n);
        }

        public static IVariable percentile(GekkoTime t, IVariable inputVar, IVariable percent)
        {
            //Mimics Excel's percentile function, see unit tests
            GekkoTime t1 = Globals.globalPeriodStart;
            GekkoTime t2 = Globals.globalPeriodEnd;

            TimeSeries ts = O.GetTimeSeries(inputVar);
            double percent2 = O.GetVal(percent, Globals.tNull);

            int index1 = -12345;
            int index2 = -12345;
            double[] data = ts.GetDataSequence(out index1, out index2, t1, t2);

            double[] data2 = new double[index2 - index1 + 1];  //we copy the array, to avoid mishaps if it is altered in the median method (= a little bit slack)
            for (int i = index1; i <= index2; i++)
            {
                data2[i - index1] = data[i];
            }

            double median = Program.Percentile(data2, percent2);

            ScalarVal z2 = new ScalarVal(median);
            return z2;
        }

        public static IVariable abs(GekkoTime t, IVariable x)
        {
            IVariable rv = null;
            if (IsValOrTimeseries(x))
            {
                double d = O.GetVal(x, t);
                rv = new ScalarVal(Math.Abs(d));
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                Matrix m = O.GetMatrix(x);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Abs(m.data[i, j]);
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: abs(): type " + x.Type().ToString() + " not supported");
            }
            return rv;
        }

        public static IVariable iif(GekkoTime t, IVariable i1, IVariable op, IVariable i2, IVariable o1, IVariable o2)
        {            
            double result=double.NaN;
            if (!IsValOrTimeseries(i1))
            {
                G.Writeln2("*** ERROR: iif(): arg 1, type " + i1.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            if (!IsValOrTimeseries(i2))
            {
                G.Writeln2("*** ERROR: iif(): arg 3, type " + i2.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            if (!IsValOrTimeseries(o1))
            {
                G.Writeln2("*** ERROR: iif(): arg 4, type " + o1.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            if (!IsValOrTimeseries(o2))
            {
                G.Writeln2("*** ERROR: iif(): arg 5, type " + o2.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            double di1 = O.GetVal(i1, t);
            double di2 = O.GetVal(i2, t);
            double do1 = O.GetVal(o1, t);
            double do2 = O.GetVal(o2, t);
            string x = O.GetString(op).Trim();

            if (x == "==")
            {
                if (di1 == di2)
                {
                    result = do1;
                }
                else
                {
                    result = do2;
                }
            }
            else if (x == "<>")
            {
                if (di1 != di2)
                {
                    result = do1;
                }
                else
                {
                    result = do2;
                }
            }
            else if (x == ">")
            {
                if (di1 > di2)
                {
                    result = do1;
                }
                else
                {
                    result = do2;
                }
            }
            else if (x == ">=")
            {
                if (di1 >= di2)
                {
                    result = do1;
                }
                else
                {
                    result = do2;
                }
            }
            else if (x == "<")
            {
                if (di1 < di2)
                {
                    result = do1;
                }
                else
                {
                    result = do2;
                }
            }
            else if (x == "<=")
            {
                if (di1 <= di2)
                {
                    result = do1;
                }
                else
                {
                    result = do2;
                }
            }
            else
            {
                G.Writeln2("*** ERROR: iif(): Expected operator '==', '<>', '<', '<=', '>' or '>='");
                throw new GekkoException();
            }
            return new ScalarVal(result);            
        }

        public static IVariable log(GekkoTime t, IVariable x)
        {
            IVariable rv = null;
            if (IsValOrTimeseries(x))
            {
                double d = O.GetVal(x, t);
                rv = new ScalarVal(Math.Log(d));
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                Matrix m = O.GetMatrix(x);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Log(m.data[i, j]);
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: log(): type " + x.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            return rv;            
        }

        public static IVariable exp(GekkoTime t, IVariable x)
        {
            IVariable rv = null;
            if (IsValOrTimeseries(x))
            {
                double d = O.GetVal(x, t);
                rv = new ScalarVal(Math.Exp(d));
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                Matrix m = O.GetMatrix(x);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Exp(m.data[i, j]);
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: exp(): type " + x.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            return rv;
        }

        public static IVariable sqrt(GekkoTime t, IVariable x)
        {
            IVariable rv = null;
            if (IsValOrTimeseries(x))
            {
                double d = O.GetVal(x, t);
                rv = new ScalarVal(Math.Sqrt(d));
            }
            else if (x.Type() == EVariableType.Matrix)
            {
                Matrix m = O.GetMatrix(x);
                rv = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        ((Matrix)rv).data[i, j] = Math.Sqrt(m.data[i, j]);
                    }
                }
            }
            else
            {
                G.Writeln2("*** ERROR: abs(): type " + x.Type().ToString() + " not supported");
                throw new GekkoException();
            }
            return rv;
        }

        private static bool IsValOrTimeseries(IVariable x)
        {
            return x.Type() == EVariableType.Val || x.Type() == EVariableType.TimeSeries;
        }

        public static IVariable pow(GekkoTime t, IVariable x1, IVariable x2)
        {
            double d1 = O.GetVal(x1, t);
            double d2 = O.GetVal(x2, t);
            return new ScalarVal(Math.Pow(d1, d2));
        }

        public static IVariable pch(GekkoTime t, IVariable x1)
        {
            if (x1.Type() != EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: pch() function only valid for time series arguments");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x1;
            MetaTimeSeries mtsLag = new MetaTimeSeries(mts.ts);
            mtsLag.offset = mts.offset - 1;
            double d1 = O.GetVal(mts, t);
            double d1Lag = O.GetVal(mtsLag, t);
            return new ScalarVal((d1 / d1Lag - 1) * 100d);
        }
        
        public static IVariable dlog(GekkoTime t, IVariable x1)
        {
            if (x1.Type() != EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: dlog() function only valid for time series arguments");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x1;
            MetaTimeSeries mtsLag = new MetaTimeSeries(mts.ts);
            mtsLag.offset = mts.offset - 1;
            double d1 = O.GetVal(mts, t);
            double d1Lag = O.GetVal(mtsLag, t);
            return new ScalarVal(Math.Log(d1 / d1Lag));
        }

        public static IVariable dif(GekkoTime t, IVariable x1)
        {
            if (x1.Type() != EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: dif() function only valid for time series arguments");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x1;
            MetaTimeSeries mtsLag = new MetaTimeSeries(mts.ts);
            mtsLag.offset = mts.offset - 1;
            double d1 = O.GetVal(mts, t);
            double d1Lag = O.GetVal(mtsLag, t);
            return new ScalarVal(d1 - d1Lag);
        }

        public static IVariable lag(GekkoTime t, IVariable x, IVariable ilag)
        {            
            if (x.Type() != EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: lag() function only valid for time series arguments");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x;
            MetaTimeSeries mtsLag = new MetaTimeSeries(mts.ts);
            mtsLag.offset = mts.offset - O.GetInt(ilag);
            double d1Lag = O.GetVal(mtsLag, t);
            return new ScalarVal(d1Lag);
        }

        public static IVariable movsum(GekkoTime t, IVariable x, IVariable ilags)
        {
            double sum, n;
            MovAvgSum(t, x, ilags, out sum, out n);
            return new ScalarVal(sum);
        }

        public static IVariable movavg(GekkoTime t, IVariable x, IVariable ilags)
        {
            double sum, n;
            MovAvgSum(t, x, ilags, out sum, out n);
            return new ScalarVal(sum / n);
        }

        private static void MovAvgSum(GekkoTime t, IVariable x, IVariable ilags, out double sum, out double n)
        {
            if (x.Type() != EVariableType.TimeSeries)
            {
                if (x.Type() == EVariableType.Val)
                {
                    //See the MEGA HACK and fix it there
                    G.Writeln2("*** ERROR: At the moment, movavg() and movsum() only work on pure timeseries, not expressions.");
                    G.Writeln("           So SERIES y = movavg(x1/x2, 2); will not work, whereas");
                    G.Writeln("           SERIES x = x1/x2; SERIES y = movavg(x, 2); is ok.");
                    G.Writeln("           This limitation will be addressed.");
                    throw new GekkoException();
                }
                else
                {
                    G.Writeln2("*** ERROR: Function movavg() or movsum() expects a timeseries as first argument.");
                    throw new GekkoException();
                }
            }
            int lags = O.GetInt(ilags);
            if (lags < 1)
            {
                G.Writeln("*** ERROR: Expected second argument of movavg/movsum() to be > 0");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x;

            sum = 0d;
            n = 0d;
            for (int i = 0; i < lags; i++)
            {
                sum += mts.ts.GetData(t.Add(-i + mts.offset));
                n++;
            }
        }

        public static IVariable pchy(GekkoTime t, IVariable x1)
        {
            if (x1.Type() != EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: pchy() function only valid for time series arguments");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x1;
            MetaTimeSeries mtsLag = new MetaTimeSeries(mts.ts);
            mtsLag.offset = mts.offset - O.CurrentSubperiods();
            double d1 = O.GetVal(mts, t);
            double d1Lag = O.GetVal(mtsLag, t);
            return new ScalarVal((d1 / d1Lag - 1) * 100d);
        }

        public static IVariable dlogy(GekkoTime t, IVariable x1)
        {
            if (x1.Type() != EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: dlogy() function only valid for time series arguments");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x1;
            MetaTimeSeries mtsLag = new MetaTimeSeries(mts.ts);
            mtsLag.offset = mts.offset - O.CurrentSubperiods();
            double d1 = O.GetVal(mts, t);
            double d1Lag = O.GetVal(mtsLag, t);
            return new ScalarVal(Math.Log(d1 / d1Lag));
        }

        public static IVariable dify(GekkoTime t, IVariable x1)
        {
            if (x1.Type() != EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: dify() function only valid for time series arguments");
                throw new GekkoException();
            }
            MetaTimeSeries mts = (MetaTimeSeries)x1;
            MetaTimeSeries mtsLag = new MetaTimeSeries(mts.ts);
            mtsLag.offset = mts.offset - O.CurrentSubperiods();
            double d1 = O.GetVal(mts, t);
            double d1Lag = O.GetVal(mtsLag, t);
            return new ScalarVal(d1 - d1Lag);
        }
        
        public static IVariable format(GekkoTime t, IVariable x1, IVariable x2)
        {
            double d = O.GetVal(x1, t);
            string format2 = O.GetString(x2);
            string x = Program.NumberFormat(d, format2);
            ScalarString ss = new ScalarString(x);
            return ss;
        }

        
        

        public static IVariable round(GekkoTime t, IVariable x1, IVariable x2)
        {            
            double d2 = O.GetVal(x2, t);            
            int aaa1 = 0;
            if (!G.Round(out aaa1, d2))
            {
                G.Writeln("*** ERROR: Could not convert decimals variable to integer");
                throw new GekkoException();
            }
            int decimals = aaa1;
            if (decimals < 0)
            {
                G.Writeln2("*** ERROR: number of decimals in round() must be positive");
                throw new GekkoException();
            }

            if (IsValOrTimeseries(x1))
            {
                double d1 = O.GetVal(x1, t);
                double value2 = Math.Round(d1, decimals);
                return new ScalarVal(value2);
            }
            else if (x1.Type() == EVariableType.Matrix)
            {
                Matrix m = O.GetMatrix(x1);
                Matrix m2 = new Matrix(m.data.GetLength(0), m.data.GetLength(1));
                for (int i = 0; i < m.data.GetLength(0); i++)
                {
                    for (int j = 0; j < m.data.GetLength(1); j++)
                    {
                        m2.data[i, j] = Math.Round(m.data[i, j], decimals);
                    }
                }
                return m2;
            }
            else
            {
                G.Writeln2("*** ERROR: round() does not support type " + x1.Type().ToString());
                throw new GekkoException();
            }
        }

        public static IVariable search(GekkoTime t, IVariable x1, IVariable x2)
        {
            string s1 = O.GetString(x1);
            string s2 = O.GetString(x2);
            int i = s1.IndexOf(s2);            
            return new ScalarVal(i + 1);
        }

        public static IVariable startswith(GekkoTime t, IVariable x1, IVariable x2)
        {
            string s1 = O.GetString(x1);
            string s2 = O.GetString(x2);
            int i = 0;
            if (s1.StartsWith(s2)) i = 1;
            return new ScalarVal(i);
        }

        public static IVariable endswith(GekkoTime t, IVariable x1, IVariable x2)
        {
            string s1 = O.GetString(x1);
            string s2 = O.GetString(x2);
            int i = 0;
            if (s1.EndsWith(s2)) i = 1;
            return new ScalarVal(i);
        }

        public static IVariable trim(GekkoTime t, IVariable x1)
        {
            string s1 = O.GetString(x1);
            return new ScalarString(s1.Trim());
        }

        public static IVariable upper(GekkoTime t, IVariable x1)
        {
            string s1 = O.GetString(x1);
            return new ScalarString(s1.ToUpper());
        }

        public static IVariable lower(GekkoTime t, IVariable x1)
        {
            string s1 = O.GetString(x1);
            return new ScalarString(s1.ToLower());
        }

        public static IVariable strip(GekkoTime t, IVariable x1, IVariable x2)
        {
            return replace(t, x1, x2, new ScalarString(""));
        }

        public static IVariable tostring(GekkoTime t, IVariable x)  //'string' not allowed as method name
        {
            string s = null;
            if (x.Type() == EVariableType.Val)
            {
                double v = ((ScalarVal)x).val;
                if (G.isNumericalError(v)) s = "M";
                else s = v.ToString();
            }
            else if (x.Type() == EVariableType.Date)
            {
                GekkoTime gt = ((ScalarDate)x).date;
                s = G.FromDateToString(gt);
            }
            else if (x.Type() == EVariableType.String)
            {
                s = ((ScalarString)x)._string2;  //maybe could just return x here, but maybe that is not safe
            }
            else if (x.Type() == EVariableType.List)
            {
                List<string> l = ((MetaList)x).list;
                foreach (string ss in l)
                {
                    s += ss + ", ";
                }                
                if (s.EndsWith(", ")) s = s.Substring(0, s.Length - 2);
            }
            else if (x.Type() == EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: Cannot convert a SERIES to a STRING");
                throw new GekkoException();
            }
            return new ScalarString(s);
        }

        public static IVariable date(GekkoTime t, IVariable x)  //'string' not allowed as method name
        {
            GekkoTime d = Globals.tNull;
            if (x.Type() == EVariableType.Val)
            {
                d = O.GetDate(x);  //already has auto-conversion from VAL to DATE
            }
            else if (x.Type() == EVariableType.Date)
            {
                d = ((ScalarDate)x).date;                
            }
            else if (x.Type() == EVariableType.String)
            {
                string s = ((ScalarString)x)._string2;
                d = G.FromStringToDate(s);
            }
            else if (x.Type() == EVariableType.List)
            {
                G.Writeln2("*** ERROR: Cannot convert a LIST to a DATE");
                throw new GekkoException();
            }
            else if (x.Type() == EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: Cannot convert a SERIES to a DATE");
                throw new GekkoException();
            }
            return new ScalarDate(d);
        }

        public static IVariable val(GekkoTime t, IVariable x)  //'string' not allowed as method name
        {
            double v = double.NaN;
            if (x.Type() == EVariableType.Val)
            {
                v = ((ScalarVal)x).val;
            }
            else if (x.Type() == EVariableType.Date)
            {
                ScalarDate sd = (ScalarDate)x;
                if (sd.date.freq == EFreq.Annual || sd.date.freq == EFreq.Undated)
                {
                    v = sd.date.super;
                }
                else
                {
                    G.Writeln2("*** ERROR: Cannot only convert annual or undated DATE to VAL");
                    throw new GekkoException();
                }
            }
            else if (x.Type() == EVariableType.String)
            {
                string s = ((ScalarString)x)._string2;
                if (G.equal(s, "m"))
                {
                    v = double.NaN;
                }
                else
                {
                    if (double.TryParse(s, out v))
                    {
                        //ok
                    }
                    else
                    {
                        G.Writeln2("*** ERROR: Could not convert STRING '" + s + "' to VAL");
                        throw new GekkoException();
                    }
                }
            }
            else if (x.Type() == EVariableType.List)
            {
                G.Writeln2("*** ERROR: Cannot convert a LIST to a VAL");
                throw new GekkoException();
            }
            else if (x.Type() == EVariableType.TimeSeries)
            {
                G.Writeln2("*** ERROR: Cannot convert a SERIES to a DATE");
                throw new GekkoException();
            }
            return new ScalarVal(v);
        }

        public static IVariable replace(GekkoTime t, IVariable x1, IVariable x2, IVariable x3)
        {
            //does not exist in AREMOS
            string s1 = O.GetString(x1);
            string s2 = O.GetString(x2);
            string s3 = O.GetString(x3);
            string s4 = s1.Replace(s2, s3);
            return new ScalarString(s4);
        }

        public static IVariable gekkoversion(GekkoTime t)
        {            
            return new ScalarString(Globals.gekkoVersion);
        }

        public static IVariable currentfreq(GekkoTime t)
        {
            return new ScalarString(G.GetFreq(Program.options.freq));
        }

        public static IVariable currentperstart(GekkoTime t)
        {
            return new ScalarDate(Globals.globalPeriodStart);
        }

        public static IVariable currentperend(GekkoTime t)
        {
            return new ScalarDate(Globals.globalPeriodEnd);
        }

        public static IVariable currentdatetime(GekkoTime t)
        {
            return new ScalarString(Program.GetDateTimeStamp());
        }

        public static IVariable currenttime(GekkoTime t)
        {
            return new ScalarString(Program.GetTimeStamp());
        }

        public static IVariable currentdate(GekkoTime t)
        {
            //See also #80927435209843
            return new ScalarString(Program.GetDateStamp());
        }

        public static IVariable filteredperiods(GekkoTime t, IVariable x1, IVariable x2)
        {
            GekkoTime t1 = Globals.tNull;
            GekkoTime t2 = Globals.tNull;
            try
            {
                t1 = O.GetDate(x1);
                t2 = O.GetDate(x2);
            }
            catch
            {
                G.Writeln("*** ERROR: Function 'filteredperiods' takes two dates as arguments.");
                throw new GekkoException();
            }

            ScalarVal z2 = null;

            if (Globals.globalPeriodTimeFilters2.Count == 0)
            {
                //for a quick return in most cases
                z2 = new ScalarVal(0d);
            }
            else
            {
                int counter = 0;
                foreach (GekkoTime gt in new GekkoTimeIterator(t1, t2))
                {
                    if (Program.ShouldFilterPeriod(gt)) counter++;
                }
                z2 = new ScalarVal((double)counter);
            }
            return z2;
        }

        public static IVariable exist(GekkoTime t, IVariable x1)
        {
            double d = 0d;
            string s1 = O.GetString(x1);
            ExtractBankAndRestHelper h = Program.ExtractBankAndRest(s1, EExtrackBankAndRest.OnlyStrings);
            if (Program.databanks.GetDatabank(h.bank) == null)
            {
                G.Writeln2("*** ERROR: The databank '" + h.bank + "' is not open");
                throw new GekkoException();
            }
            if (Program.databanks.GetDatabank(h.bank).ContainsVariable(true, h.name)) d = 1d;
            ScalarVal v = new ScalarVal(d);
            return v;
        }

        public static IVariable fromseries(GekkoTime t, IVariable x1, IVariable x2)
        {
            string s1 = O.GetString(x1);
            string s2 = O.GetString(x2);

            //TODO TODO
            //TODO TODO
            //TODO TODO use ExtractBankAndRest() here
            //TODO TODO
            //TODO TODO

            string bank = Program.databanks.GetFirst().aliasName;
            string name = null;
            string[] split = s1.Split(':');
            if (split.Length > 2)
            {
                G.Writeln2("*** ERROR: fromSeries() first arg has > 1 colons");
                throw new GekkoException();
            }
            else if (split.Length == 2)
            {
                bank = split[0].Trim();
                name = split[1].Trim();
            }
            else
            {
                name = split[0].Trim();
            }

            Databank db = Program.databanks.GetDatabank(bank);
            if (db == null)
            {
                G.Writeln2("*** ERROR: fromSeries(): Databank '" + db + "' could not be found.");
                throw new GekkoException();
            }
            TimeSeries ts = db.GetVariable(name);
            if (ts == null)
            {
                G.Writeln2("*** ERROR: fromSeries(): TimeSeries '" + name + "' could not be found in '" + bank + "'.");
                throw new GekkoException();
            }
            
            if (G.equal(s2, "label"))
            {
                return new ScalarString(ts.label);
            }
            else if (G.equal(s2, "source"))
            {
                return new ScalarString(ts.source);
            }
            else if (G.equal(s2, "stamp"))
            {
                return new ScalarString(ts.stamp);
            }
            else if (G.equal(s2, "perStart"))
            {
                return new ScalarDate(ts.GetPeriodFirst());
            }
            else if (G.equal(s2, "dataStart"))
            {                
                return new ScalarDate(ts.GetRealDataPeriodFirst());
            }
            else if (G.equal(s2, "perEnd"))
            {
                return new ScalarDate(ts.GetPeriodLast());
            }
            else if (G.equal(s2, "dataEnd"))
            {
                return new ScalarDate(ts.GetRealDataPeriodLast());
            }
            else if (G.equal(s2, "freq"))
            {
                return new ScalarString(G.GetFreq(ts.freqEnum));
            }
            else
            {
                G.Writeln2("*** ERROR: fromSeries(): Argument '" + s2 + "' not recognized.");
                throw new GekkoException();
            }            
        }

        // -----------------------------------
        // LIST functions start
        // -----------------------------------

        public static IVariable union(GekkoTime t, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og tilføjer dem fra b (uden dubletter). Hvis dubletter i b skal med, skal der bruges komma...
            List<string> lx1 = O.GetList(x1);
            List<string> lx2 = O.GetList(x2);
            List<string> union = new List<string>();
            union.AddRange(lx1);
            GekkoDictionary<string, bool> result = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in lx1)
            {
                if (!result.ContainsKey(s)) result.Add(s, true);
            }
            foreach (string s in lx2)
            {
                if (!result.ContainsKey(s))
                {
                    result.Add(s, true);
                    union.Add(s);
                }
            }
            //union.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new MetaList(union);
        }               

        public static IVariable difference(GekkoTime t, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og retainer dem hvis ikke i b.
            List<string> lx1 = O.GetList(x1);
            List<string> lx2 = O.GetList(x2);
            List<string> difference = new List<string>();
            GekkoDictionary<string, bool> temp = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in lx2)
            {
                if (!temp.ContainsKey(s)) temp.Add(s, true);
            }
            foreach (string s in lx1)
            {
                if (!temp.ContainsKey(s))
                {
                    difference.Add(s);
                }
            }
            //difference.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new MetaList(difference);
        }

        public static IVariable intersect(GekkoTime t, IVariable x1, IVariable x2)
        {
            //tager dem der nu er i a (inkl. dubletter) og retainer dem hvis også i b.
            List<string> lx1 = O.GetList(x1);
            List<string> lx2 = O.GetList(x2);
            List<string> intersection = new List<string>();
            if (lx1.Count > lx2.Count)  //for speedup, we do the heaviest looping on the smaller list.
            {
                ListMultiplyHelper(lx1, lx2, intersection);
            }
            else
            {
                ListMultiplyHelper(lx2, lx1, intersection);
            }
            //intersection.Sort(StringComparer.InvariantCulture);  //or maybe only sort when printing/reporting/iterating?
            return new MetaList(intersection);
        }

        private static void ListMultiplyHelper(List<string> x1, List<string> x2, List<string> intersection)
        {
            GekkoDictionary<string, bool> temp = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in x1)
            {
                if (!temp.ContainsKey(s))  //there can be dublets
                {
                    temp.Add(s, true);
                }
            }
            GekkoDictionary<string, bool> result = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string s in x2)
            {
                if (temp.ContainsKey(s))
                {
                    if (!result.ContainsKey(s))
                    {
                        result.Add(s, true);
                    }
                }
            }
            intersection.AddRange(result.Keys);
        }

        // -----------------------------------
        // LIST functions end
        // -----------------------------------

        //SOME HARDCODED FUNCTIONS FOR MODELS:
        //See #09875209837532

        public static double CES_UC(double p1rel, double p2rel, double theta, double sigma)
        {
            double c = Math.Pow(theta * Math.Pow(p1rel, 1 - sigma) + (1 - theta) * Math.Pow(p2rel, 1 - sigma), 1 / (1 - sigma));
            return c;
        }

        public static double CES_XL(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p1rel, sigma);
        }

        public static double CES_XR(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p2rel, sigma);
        }

        //See also #9823750983752

        public static double ces_costs(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return y / kappa * Math.Pow(Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p1, 1 - sigma) + Math.Pow(1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p2, 1 - sigma), 1 / (1 - sigma));
        }

        public static double ces_ac(double p1, double p2, double kappa, double phi, double sigma)
        {
            return 1d / kappa * Math.Pow(Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p1, 1 - sigma) + Math.Pow(1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), sigma) * Math.Pow(p2, 1 - sigma), 1 / (1 - sigma));
        }

        public static double ces_factor1(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return Math.Pow((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))), (sigma / (1 - sigma))) * y / kappa * Math.Pow((Math.Pow((p2 / p1), (1 - sigma)) * Math.Pow(((1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))) / (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))), sigma) + 1), (sigma / (1 - sigma)));
        }

        public static double ces_factor2(double y, double p1, double p2, double kappa, double phi, double sigma)
        {
            return Math.Pow((1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))), (sigma / (1 - sigma))) * y / kappa * Math.Pow((Math.Pow((p1 / p2), (1 - sigma)) * Math.Pow((((Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma)))) / (1 - (Math.Exp(phi / sigma) / (1 + Math.Exp(phi / sigma))))), sigma) + 1), (sigma / (1 - sigma)));
        }
        
    }
}


