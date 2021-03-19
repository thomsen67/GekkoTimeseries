using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class Matrix : IVariable
    {
        //Because protobuf only handles 1d arrays
        [ProtoBeforeSerialization]
        public void BeforeProtobufWrite()
        {            
            this.dataProtobufHelper0___NOTOUCH = this.data.GetLength(0);
            this.dataProtobufHelper1___NOTOUCH = this.data.GetLength(1);
            this.dataProtobufHelper___NOTOUCH = new double[this.data.Length];
            int counter = 0;
            foreach(double d in this.data)
            {
                //this iterator will will take it col-wise, first running the j's in [i, j]
                this.dataProtobufHelper___NOTOUCH[counter] = d;
                counter++;
            }            
        }

        //Because protobuf only handles 1d arrays
        [ProtoAfterDeserialization]
        public void AfterProtobufRead()
        {
            this.data = new double[this.dataProtobufHelper0___NOTOUCH, this.dataProtobufHelper1___NOTOUCH];
            int counter = 0;
            for (int i = 0; i < this.dataProtobufHelper0___NOTOUCH; i++)
            {
                for (int j = 0; j < this.dataProtobufHelper1___NOTOUCH; j++)
                {
                    this.data[i, j] = this.dataProtobufHelper___NOTOUCH[counter];
                    counter++;
                }                
            }
            this.dataProtobufHelper___NOTOUCH = null;
            this.dataProtobufHelper0___NOTOUCH = 0;
            this.dataProtobufHelper1___NOTOUCH = 0;
        }

        //Abstract class containing a Matrix                
        
        public double[,] data = null;
        [ProtoMember(4)]
        public List<string> colnames = null;
        [ProtoMember(5)]
        public List<string> rownames = null;

        // ====================== DO NOT TOUCH THESE! =======================================================
        [ProtoMember(1)]
        private double[] dataProtobufHelper___NOTOUCH = null; //only because protobuf does not handle 2d arrays
        [ProtoMember(2)]
        private int dataProtobufHelper0___NOTOUCH = 0; //only because protobuf does not handle 2d arrays
        [ProtoMember(3)]
        private int dataProtobufHelper1___NOTOUCH = 0; //only because protobuf does not handle 2d arrays
        // ============================================ =====================================================

        public Matrix()
        {
            this.data = null;
        }

        public Matrix(int n1, int n2)
        {
            this.data = new double[n1, n2];
        }

        public Matrix(int n1, int n2, double d)
        {
            this.data = new double[n1, n2];
            for (int i = 0; i < n1; i++)
            {
                for (int j = 0; j < n2; j++)
                {
                    this.data[i, j] = d;
                }
            }
        }        

        public IVariable Indexer(GekkoSmpl t, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                IVariable index = indexes[0];
                int d1 = this.data.GetLength(0);
                int d2 = this.data.GetLength(1);
                if (d2 == 1)
                {
                    //1 col: column vector
                    IVariable one = Globals.scalarVal1;
                    IVariable[] newIndex = new IVariable[2];
                    newIndex[0] = index;
                    newIndex[1] = one;
                    return Handle2dIndexer(newIndex);  //we implicitly understand #a[3] as #a[3,1] here. But we cannot do the inverse on a row vector.
                }
                string s = null;
                if (index.Type() == EVariableType.Val) s += "" + O.ConvertToInt(index);
                else if (index.Type() == EVariableType.Range) s += "" + O.ConvertToInt(((Range)index).first) + ".." + O.ConvertToInt(((Range)index).first);
                new Error("You are trying to use [" + s + "] on a " + d1 + " x " + d2 + " matrix. This notation can only be used regarding nx1 matrices (column vectors)"); return null;
            }
            else if (indexes.Length == 2)
            {
                return Handle2dIndexer(indexes);
            }
            else
            {
                new Error("Cannot use " + indexes.Length + "-dimensional indexer on MATRIX"); return null;
            }
        }

        private IVariable Handle2dIndexer(IVariable[] indexes)
        {

            IVariable index1 = indexes[0];
            IVariable index2 = indexes[1];

            if (index1.Type() == EVariableType.Val && index2.Type() == EVariableType.Val)
            {
                int i1 = O.ConvertToInt(index1);
                int i2 = O.ConvertToInt(index2);
                try
                {
                    double d = this.data[i1 - 1, i2 - 1];
                    return new ScalarVal(d);
                }
                catch (System.IndexOutOfRangeException e)  // CS0168
                {
                    HandleIndexOutOfRange(index1, index2);
                    throw new GekkoException();
                }
            }
            else
            {
                IVariable xx1; IVariable xx2;
                RangeHelper(index1, index2, out xx1, out xx2);
                return GetDataHelper((Range)xx1, (Range)xx2);
            }
        }        

        public IVariable Negate(GekkoSmpl t)
        {
            int ni = this.data.GetLength(0);
            int nj = this.data.GetLength(1);
            Matrix m = new Matrix(ni, nj);
            for (int i = 0; i < ni; i++)
            {
                for (int j = 0; j < nj; j++)
                {
                    m.data[i, j] = -this.data[i, j];
                }
            }
            return m;
        }
        
        public double GetValOLD(GekkoSmpl t)
        {
            if (this.data.GetLength(0) == 1 && this.data.GetLength(1) == 1)
            {
                return this.data[0, 0];
            }
            else
            {
                new Error("Type mismatch: you are trying to extract a VAL from a matrix. Maybe you need an [x, y]-indexer on the matrix, for instance #a[2, 3]?"); return double.NaN;
            }         
        }

        public double GetVal(GekkoTime t)
        {
            return ConvertToVal();
        }

        public double ConvertToVal()
        {
            if (this.data.GetLength(0) == 1 && this.data.GetLength(1) == 1)
            {
                return this.data[0, 0];
            }
            else
            {
                new Error("Type mismatch: you are trying to extract a VAL from a " + this.data.GetLength(0) + " x " + this.data.GetLength(1) + " matrix."); return double.NaN;
            }
        }

        public string ConvertToString()
        {
            new Error("Type mismatch: you are trying to extract a STRING from a matrix."); return null;
        }

        public string DimensionsAsString()
        {
            return this.data.GetLength(0) + " x " + this.data.GetLength(1);
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            new Error("Type mismatch: you are trying to extract a DATE from a matrix."); return GekkoTime.tNull;
        }

        public List<IVariable> ConvertToList()
        {
            new Error("Type mismatch: you are trying to extract a LIST from a matrix."); return null;
        }

        public EVariableType Type()
        {
            return EVariableType.Matrix;
        }

        public Matrix Clone()
        {
            //TODO: could use double[,] b = (double[,])a.Clone();

            int n1 = this.data.GetLength(0);
            int n2 = this.data.GetLength(1);
            Matrix m = new Matrix(n1, n2);
            for (int i = 0; i < n1; i++)
            {
                for (int j = 0; j < n2; j++)
                {
                    m.data[i, j] = this.data[i, j];
                }
            }
            return m;
        }

        public IVariable Add(GekkoSmpl t, IVariable x)
        {
            //clone this Add() method and do with O.SubtractMatrixScalar, also for Matrix and matrix            
            switch (x.Type())
            {
                case EVariableType.Matrix:
                    {
                        double[,] a = this.data;
                        double[,] b = ((Matrix)x).data;
                        int m = a.GetLength(0);
                        int k = a.GetLength(1);
                        if (b.GetLength(0) != m || b.GetLength(1) != k)
                        {
                            new Error("The two matrices are not compatible for addition, " + m + " x " + k + " and " + b.GetLength(0) + " x " + b.GetLength(1) + " do not match");
                        }
                        double[,] c = Program.AddMatrixMatrix(a, b, m, k);
                        Matrix z = new Matrix();
                        z.data = c;
                        return z;
                    }
                case EVariableType.Val:
                    {
                        //addition of a scalar is not legal, this is like AREMOS
                        new Error("You cannot add a MATRIX and a VAL"); return null;
                    }
                default:
                    {
                        new Error("You are trying to add a MATRIX and a " + x.Type().ToString().ToUpper()); return null;
                    }
            }
        }

        public IVariable Subtract(GekkoSmpl t, IVariable x)
        {
            //clone this Add() method and do with O.SubtractMatrixScalar, also for Matrix and matrix            
            EVariableType type = x.Type();
            if (type == EVariableType.Matrix)
            {
                double[,] a = this.data;
                double[,] b = ((Matrix)x).data;
                int m = a.GetLength(0);
                int k = a.GetLength(1);
                if (b.GetLength(0) != m || b.GetLength(1) != k)
                {
                    new Error("The two matrices are not compatible for subtraction, " + m + " x " + k + " and " + b.GetLength(0) + " x " + b.GetLength(1) + " do not match");
                }
                double[,] c = Program.SubtractMatrixMatrix(a, b, m, k);
                Matrix z = new Matrix();
                z.data = c;
                return z;
            }
            else
            {
                //subtraction of a scalar is not legal, this is like AREMOS
                new Error("You are trying to add a MATRIX and a " + type.ToString().ToUpper()); return null;
            }
        }

        public IVariable Multiply(GekkoSmpl t, IVariable x)
        {
            EVariableType type = x.Type();
            if (type == EVariableType.Matrix)
            {
                double[,] a = this.data;
                double[,] b = ((Matrix)x).data;
                // m x k  *  p x n
                int m = a.GetLength(0);
                int k = a.GetLength(1);
                int p = b.GetLength(0);
                int n = b.GetLength(1);

                if (p == 1 && n == 1)
                {
                    //Special case, #a * #onebyone
                    double[,] c = Program.MultiplyMatrixScalar(a, b[0, 0], m, k);
                    Matrix z = new Matrix();
                    z.data = c;
                    return z;
                }
                else if (m == 1 && k == 1)
                {
                    //Special case,  #onebyone * #a
                    double[,] c = Program.MultiplyMatrixScalar(b, a[0, 0], p, n);
                    Matrix z = new Matrix();
                    z.data = c;
                    return z;
                }
                else
                {

                    if (k != p)
                    {
                        new Error("The two matrices are not compatible for multiplication, " + m + " x " + k + " and " + b.GetLength(0) + " x " + b.GetLength(1) + " do not match");
                    }
                    double[,] c = null;

                    if (false)
                    {
                        c = new double[m, n];
                        alglib.rmatrixgemm(m, n, k, 1, a, 0, 0, 0, b, 0, 0, 0, 0, ref c, 0, 0);
                    }
                    else
                    {
                        c = Program.MultiplyMatrices(a, b);
                    }

                    Matrix z = new Matrix();
                    z.data = c;
                    return z;
                    //c = {{74, 80, 86, 92}, {173, 188, 203, 218}}
                }
            }
            else if (type == EVariableType.Val)
            {
                //This is allowed in AREMOS, too
                double[,] a = this.data;
                double b = O.ConvertToVal(x);  //#875324397
                int m = a.GetLength(0);
                int k = a.GetLength(1);
                double[,] c = Program.MultiplyMatrixScalar(a, b, m, k);
                Matrix z = new Matrix();
                z.data = c;
                return z;
            }
            else
            {
                new Error("You are trying to multiply a MATRIX and a " + type.ToString().ToUpper()); return null;
            }
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            new Error("Type error regarding concat and MATRIX"); return null;
        }

        public IVariable Divide(GekkoSmpl t, IVariable x)
        {
            switch (x.Type())
            {

                case EVariableType.Matrix:
                    {
                        double[,] a = this.data;
                        double[,] b = ((Matrix)x).data;
                        // m x k  *  p x n
                        int m = a.GetLength(0);
                        int k = a.GetLength(1);
                        int p = b.GetLength(0);
                        int n = b.GetLength(1);

                        if (p == 1 && n == 1)
                        {
                            //Special case
                            double[,] c = Program.MultiplyMatrixScalar(a, 1d / b[0, 0], m, k);
                            Matrix z = new Matrix();
                            z.data = c;
                            return z;
                        }
                        else
                        {
                            new Error("You can only divide a matrix with a scalar or 1x1 matrix."); return null;
                        }
                    }
                case EVariableType.Val:
                    {
                        double[,] a = this.data;
                        double b = O.ConvertToVal(x);  //#875324397
                        int m = a.GetLength(0);
                        int k = a.GetLength(1);
                        double[,] c = Program.MultiplyMatrixScalar(a, 1d / b, m, k);
                        Matrix z = new Matrix();
                        z.data = c;
                        return z;
                    }
                default:
                    {
                        new Error("You can only divide a matrix with a scalar or 1x1 matrix."); return null;
                    }
            }
        }

        public IVariable Power(GekkoSmpl t, IVariable x)
        {
            new Error("You cannot use power function with matrices"); return null;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] dims)
        {
            if (dims.Length == 1 || dims.Length == 2)
            {
                IVariable x1 = dims[0];

                IVariable x2 = null;
                if (dims.Length == 1)
                {
                    x2 = Globals.scalarVal1;  //set to 1
                }
                else
                {
                    x2 = dims[1];
                }

                if (x1.Type() == EVariableType.Val && x2.Type() == EVariableType.Val)
                {
                    try
                    {
                        this.data[O.ConvertToInt(x1) - 1, O.ConvertToInt(x2) - 1] = O.ConvertToVal(rhsExpression);  //#875324397
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        HandleIndexOutOfRange(x1, x2);
                        throw new GekkoException();
                    }
                }
                else
                {
                    //at least one of the indices is a range
                    IVariable xx1, xx2;
                    RangeHelper(x1, x2, out xx1, out xx2);
                    SetDataHelper((Range)xx1, (Range)xx2, rhsExpression);
                }
            }

        }

        private static void RangeHelper(IVariable x1, IVariable x2, out IVariable xx1, out IVariable xx2)
        {
            xx1 = x1;
            xx2 = x2;
            if (x1.Type() == EVariableType.Val) xx1 = new Range(x1, x1);
            if (x2.Type() == EVariableType.Val) xx2 = new Range(x2, x2);
            if (xx1.Type() != EVariableType.Range || xx1.Type() != EVariableType.Range)
            {
                new Error("Matrix []-indexer on left-hand side has wrong type");
            }
        }

        public IVariable GetDataHelper(Range indexRange1, Range indexRange2)
        {
            int i1 = 1;
            int i2 = this.data.GetLength(0);
            int j1 = 1;
            int j2 = this.data.GetLength(1);
            if (indexRange1.first != null) i1 = O.ConvertToInt(indexRange1.first);
            if (indexRange1.last != null) i2 = O.ConvertToInt(indexRange1.last);
            if (indexRange2.first != null) j1 = O.ConvertToInt(indexRange2.first);
            if (indexRange2.last != null) j2 = O.ConvertToInt(indexRange2.last);
            if (i1 > i2)
            {
                new Error("Range " + i1 + ".." + i2 + " is descending");
            }
            if (j1 > j2)
            {
                new Error("Range " + j1 + ".." + j2 + " is descending");
            }

            int dimI = i2 - i1 + 1;
            int dimJ = j2 - j1 + 1;
            Matrix m = new Matrix(dimI, dimJ);
            double[,] data = m.data; 

            try
            {
                int ii1 = i1 - 1;
                int jj1 = j1 - 1;
                for (int i = i1 - 1; i <= i2 - 1; i++)
                {
                    for (int j = j1 - 1; j <= j2 - 1; j++)
                    {
                        data[i - ii1, j - jj1] = this.data[i, j];
                    }
                }
                return m;
            }
            catch (System.IndexOutOfRangeException e)  // CS0168
            {
                using (var error = new Error())
                {
                    error.MainAdd("Left-side index out of range: [" + i1 + " .. " + i2 + ", " + j1 + " .. " + j2 + " ]. ");
                    if (i1 == 0 || i2 == 0 || j1 == 0 || j2 == 0) error.MainAdd("Please note that indicies are 1-based");
                }
                return null;
            }
        }

        public IVariable SetDataHelper(Range indexRange1, Range indexRange2, IVariable x3)
        {
            int i1 = 1;
            int i2 = this.data.GetLength(0);
            int j1 = 1;
            int j2 = this.data.GetLength(1);
            if (indexRange1.first != null) i1 = O.ConvertToInt(indexRange1.first);
            if (indexRange1.last != null) i2 = O.ConvertToInt(indexRange1.last);
            if (indexRange2.first != null) j1 = O.ConvertToInt(indexRange2.first);
            if (indexRange2.last != null) j2 = O.ConvertToInt(indexRange2.last);
            if (i1 > i2)
            {
                G.Writeln2("*** ERROR: Range " + i1 + ".." + i2 + " is descending");
                throw new GekkoException();
            }
            if (j1 > j2)
            {
                G.Writeln2("*** ERROR: Range " + j1 + ".." + j2 + " is descending");
                throw new GekkoException();
            }

            if (x3.Type() == EVariableType.Matrix)
            {
                Matrix m = (Matrix)x3;
                int dimI = i2 - i1 + 1;
                int dimJ = j2 - j1 + 1;
                if (dimI != m.data.GetLength(0) || dimJ != m.data.GetLength(1))
                {
                    G.Writeln2("*** ERROR: Left-hand side selection is " + dimI + " x " + dimJ + ", but right-hand matrix is " + m.data.GetLength(0) + " x " + m.data.GetLength(1));
                    throw new GekkoException();
                }

                try
                {
                    int ii1 = i1 - 1;
                    int jj1 = j1 - 1;
                    for (int i = i1 - 1; i <= i2 - 1; i++)
                    {
                        for (int j = j1 - 1; j <= j2 - 1; j++)
                        {
                            this.data[i, j] = m.data[i - ii1, j - jj1];
                        }
                    }
                    return this;
                }
                catch (System.IndexOutOfRangeException e)  // CS0168
                {
                    G.Writeln("*** ERROR: Left-side index out of range: [" + i1 + " .. " + i2 + ", " + j1 + " .. " + j2 + " ]");
                    if (i1 == 0 || i2 == 0 || j1 == 0 || j2 == 0) G.Writeln("           Please note that indicies are 1-based");
                    throw new GekkoException();
                }
            }
            else if (x3.Type() == EVariableType.Val)
            {
                ScalarVal v = (ScalarVal)x3;
                try
                {
                    int ii1 = i1 - 1;
                    int jj1 = j1 - 1;
                    for (int i = i1 - 1; i <= i2 - 1; i++)
                    {
                        for (int j = j1 - 1; j <= j2 - 1; j++)
                        {
                            this.data[i, j] = v.val;
                        }
                    }
                    return this;
                }
                catch (System.IndexOutOfRangeException e)  // CS0168
                {
                    G.Writeln("*** ERROR: Left-side index out of range: [" + i1 + " .. " + i2 + ", " + j1 + " .. " + j2 + " ]");
                    if (i1 == 0 || i2 == 0 || j1 == 0 || j2 == 0) G.Writeln("           Please note that indicies are 1-based");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Expected right-hand side to be a matrix or a scalar");
                throw new GekkoException();
            }
        }

        private void HandleIndexOutOfRange(IVariable x1, IVariable x2)
        {
            int dim1 = O.ConvertToInt(x1);
            int dim2 = O.ConvertToInt(x2);
            int maxDim1 = this.data.GetLength(0);
            int maxDim2 = this.data.GetLength(1);
            if (dim1 > maxDim1 || dim2 > maxDim2)
            {
                G.Writeln2("*** ERROR: Indexer [" + dim1 + ", " + dim2 + "] on a matrix with dimensions " + maxDim1 + " x " + maxDim2);
            }
            if (dim1 < 1 || dim2 < 1)
            {
                G.Writeln2("*** ERROR: Indexer [" + dim1 + ", " + dim2 + "]: indexers must be >= 1");
            }
        }

        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            Matrix m = new Gekko.Matrix();
            if (this.colnames != null)
            {
                m.colnames = new List<string>();
                m.colnames.AddRange(this.colnames);
            }
            if (this.rownames != null)
            {
                m.rownames = new List<string>();
                m.rownames.AddRange(this.rownames);
            }
            m.data = new double[this.data.GetLength(0), this.data.GetLength(1)];
            Array.Copy(this.data, m.data, this.data.Length);
            return m;
        }

        public void DeepTrim()
        {
            //do nothing, nothing to trim
        }

        public void DeepCleanup(TwoInts yearMinMax)
        {
            //do nothing
        }
    }
}
