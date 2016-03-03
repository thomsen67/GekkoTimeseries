#region Copyright ©2004 Joannes Vermorel

// MathNet Numerics, part of MathNet
//
// Copyright (c) 2004,	Joannes Vermorel, http://www.vermorel.com
// Based on JMP , Copyright (c) 2003 Bjørn-Ove Heimsund
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

#endregion

using System;
using MathNet.Numerics.LinearAlgebra.Sparse.Utilities;

namespace MathNet.Numerics.LinearAlgebra.Sparse
{
	/// <summary> Sequential BLAS implementation</summary>
	public class SequentialBLAS : AbstractBLAS, IBLAS
	{
		protected internal override IMatrix AddDiagonalI(double alpha, IMatrix A)
		{
			if (A is IElementalAccessMatrix)
				AddDiagonalI(alpha, (IElementalAccessMatrix) A, 0, A.RowCount);
			else
				throw new NotSupportedException();
			return A;
		}

		protected internal virtual void AddDiagonalI(double alpha, 
			IElementalAccessMatrix A, int start, int stop)
		{
			for (int i = start; i < stop && i < A.ColumnCount; ++i)
				A.AddValue(i, i, alpha);
		}

		protected internal override IVector AddI(double alpha, IVector x, double beta, IVector y, IVector z)
		{
			if (x is IDenseAccessVector && y is IDenseAccessVector && z is IDenseAccessVector)
				AddI(alpha, ((IDenseAccessVector) x).Vector, beta, 
					((IDenseAccessVector) y).Vector, ((IDenseAccessVector) z).Vector, 0, z.Length);
			else if (x is IBlockAccessVector && z is IBlockAccessVector && z is IBlockAccessVector)
				AddI(alpha, (IBlockAccessVector) x, beta, (IBlockAccessVector) y, 
					(IBlockAccessVector) z, 0, ((IBlockAccessVector) x).BlockCount);
			else
				throw new NotSupportedException();

			return z;
		}

		protected internal virtual void AddI(double alpha, IBlockAccessVector x, 
			double beta, IBlockAccessVector y, IBlockAccessVector z, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				Add(alpha, x.GetBlock(i), beta, y.GetBlock(i), z.GetBlock(i));
		}

		protected internal virtual void AddI(double alpha, double[] x, 
			double beta, double[] y, double[] z, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				z[i] = alpha*x[i] + beta*y[i];
		}

		protected internal override IMatrix CopyI(IMatrix A, IMatrix B, 
			int startRow, int stopRow, int startColumn, int stopColumn)
		{
			Zero(B);

			if (A is ISparseRowColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseRowColumnAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is ISparseRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseRowAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is ISparseColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseColumnAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is ISparseColumnRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseColumnRowAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is IDenseRowColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseRowColumnAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is IDenseRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseRowAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is IDenseColumnRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseColumnRowAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is IDenseColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseColumnAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is ICoordinateAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ICoordinateAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			else if (A is IElementalAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IElementalAccessMatrix) A, (IElementalAccessMatrix) B, 
					startRow, stopRow, startColumn, stopColumn);
			
			else throw new NotSupportedException();

			return B;
		}

		protected internal void CopyI(ICoordinateAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			int[] row = A.RowIndices, column = A.ColumnIndices;
			double[] data = A.Data;

			for (int i = 0; i < data.Length; ++i)
				if (row[i] >= startRow && row[i] < stopRow 
					&& column[i] >= startColumn && column[i] < stopColumn)
						B.SetValue(row[i] - startRow, column[i] - startColumn, data[i]);
		}

		protected internal virtual void CopyI(ISparseRowColumnAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = startRow; i < stopRow; ++i)
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					if (Amat.major[j] >= startColumn && Amat.major[j] < stopColumn)
						B.SetValue(i - startRow, Amat.major[j] - startColumn, Amat.data[j]);
		}

		protected internal void CopyI(ISparseRowAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			for (int i = startRow; i < stopRow; ++i)
			{
				IntDoubleVectorPair Arow = A.GetRow(i);
				for (int j = 0; j < Arow.indices.Length; ++j)
					if (Arow.indices[j] >= startColumn && Arow.indices[j] < stopColumn)
						B.SetValue(i - startRow, Arow.indices[j] - startColumn, Arow.data[j]);
			}
		}

		protected internal virtual void CopyI(ISparseColumnRowAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = startColumn; i < stopColumn; ++i)
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					if (Amat.major[j] >= startRow && Amat.major[j] < stopRow)
						B.SetValue(Amat.major[j] - startRow, i - startColumn, Amat.data[j]);
		}

		protected internal virtual void CopyI(ISparseColumnAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			for (int i = startColumn; i < stopColumn; ++i)
			{
				IntDoubleVectorPair Acol = A.GetColumn(i);
				for (int j = 0; j < Acol.indices.Length; ++j)
					if (Acol.indices[j] >= startRow && Acol.indices[j] < stopRow)
						B.SetValue(Acol.indices[j] - startRow, i - startColumn, Acol.data[j]);
			}
		}

		protected internal void CopyI(IDenseRowColumnAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = startRow; i < stopRow; ++i)
				for (int j = Amat.indices[i] + startColumn; j < Amat.indices[i] + stopColumn; ++j)
					if (Amat.data[j] != 0.0)
						B.SetValue(i - startRow, j - Amat.indices[i] - startColumn, Amat.data[j]);
		}

		protected internal virtual void CopyI(IDenseRowAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			for (int i = startRow; i < stopRow; ++i)
			{
				double[] Arow = A.GetRow(i);
				for (int j = startColumn; j < stopColumn; ++j)
					if (Arow[j] != 0.0)
						B.SetValue(i - startRow, j - startColumn, Arow[j]);
			}
		}

		protected internal void CopyI(IDenseColumnRowAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = startColumn; i < stopColumn; ++i)
				for (int j = Amat.indices[i] + startRow; j < Amat.indices[i] + stopRow; ++j)
					if (Amat.data[j] != 0.0)
						B.SetValue(j - Amat.indices[j] - startRow, i - startColumn, Amat.data[j]);
		}

		protected internal void CopyI(IDenseColumnAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			for (int i = startColumn; i < stopColumn; ++i)
			{
				double[] Acol = A.GetColumn(i);
				for (int j = startRow; j < stopRow; ++j)
					if (Acol[j] != 0.0)
						B.SetValue(j - startRow, i - startColumn, Acol[j]);
			}
		}

		protected internal void CopyI(IElementalAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow, int startColumn, int stopColumn)
		{
			for (int i = startRow; i < stopRow; ++i)
				for (int j = startColumn; j < stopColumn; ++j)
				{
					double val = A.GetValue(i, j);
					if (val != 0.0)
						B.SetValue(i - startRow, j - startColumn, val);
				}
		}

		protected internal override IMatrix CopyI(IMatrix A, IMatrix B)
		{
			int stopRow = A.RowCount, stopColumn = A.ColumnCount;
			Zero(B);

			if (A is ISparseRowColumnAccessMatrix && B is ISparseRowColumnAccessMatrix)
				CopyI((ISparseRowColumnAccessMatrix) A, (ISparseRowColumnAccessMatrix) B);
			else if (A is ISparseRowColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseRowColumnAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopRow);
			else if (A is ISparseRowAccessMatrix && B is ISparseRowAccessMatrix)
				CopyI((ISparseRowAccessMatrix) A, (ISparseRowAccessMatrix) B, 0, stopRow);
			else if (A is ISparseRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseRowAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopRow);
			else if (A is ISparseColumnRowAccessMatrix && B is ISparseColumnRowAccessMatrix)
				CopyI((ISparseColumnRowAccessMatrix) A, (ISparseColumnRowAccessMatrix) B);
			else if (A is ISparseColumnRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseColumnRowAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopColumn);
			else if (A is ISparseColumnAccessMatrix && B is ISparseColumnAccessMatrix)
				CopyI((ISparseColumnAccessMatrix) A, (ISparseColumnAccessMatrix) B, 0, stopColumn);
			else if (A is ISparseColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((ISparseColumnAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopColumn);
			else if (A is IDenseRowColumnAccessMatrix && B is IDenseRowColumnAccessMatrix)
				CopyI((IDenseRowColumnAccessMatrix) A, (IDenseRowColumnAccessMatrix) B);
			else if (A is IDenseRowColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseRowColumnAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopRow);
			else if (A is IDenseRowAccessMatrix && B is IDenseRowAccessMatrix)
				CopyI((IDenseRowAccessMatrix) A, (IDenseRowAccessMatrix) B, 0, stopRow);
			else if (A is IDenseRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseRowAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopRow);
			else if (A is IDenseColumnRowAccessMatrix && B is IDenseColumnRowAccessMatrix)
				CopyI((IDenseColumnRowAccessMatrix) A, (IDenseColumnRowAccessMatrix) B);
			else if (A is IDenseColumnRowAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseColumnRowAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopColumn);
			else if (A is IDenseColumnAccessMatrix && B is IDenseColumnAccessMatrix)
				CopyI((IDenseColumnAccessMatrix) A, (IDenseColumnAccessMatrix) B, 0, stopColumn);
			else if (A is IDenseColumnAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IDenseColumnAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopColumn);
			else if (A is ICoordinateAccessMatrix && B is IElementalAccessMatrix)
			{
				CopyI((ICoordinateAccessMatrix) A, (IElementalAccessMatrix) B);
			}
			else if (A is IElementalAccessMatrix && B is IElementalAccessMatrix)
				CopyI((IElementalAccessMatrix) A, (IElementalAccessMatrix) B, 0, stopColumn);
			else
				throw new NotSupportedException();

			return B;
		}

		protected internal void CopyI(ICoordinateAccessMatrix A, IElementalAccessMatrix B)
		{
			int[] row = A.RowIndices, column = A.ColumnIndices;
			double[] data = A.Data;

			for (int i = 0; i < data.Length; ++i)
				B.SetValue(row[i], column[i], data[i]);
		}

		protected internal void CopyI(ISparseRowColumnAccessMatrix A, ISparseRowColumnAccessMatrix B)
		{
			B.Matrix = A.Matrix.Clone();
		}

		protected internal virtual void CopyI(ISparseRowColumnAccessMatrix A, IElementalAccessMatrix B, int startRow, int stopRow)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = startRow; i < stopRow; ++i)
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					B.SetValue(i, Amat.major[j], Amat.data[j]);
		}

		protected internal virtual void CopyI(ISparseRowAccessMatrix A, ISparseRowAccessMatrix B, int startRow, int stopRow)
		{
			for (int i = startRow; i < stopRow; ++i)
				B.SetRow(i, A.GetRow(i).Clone());
		}

		protected internal virtual void CopyI(ISparseRowAccessMatrix A, IElementalAccessMatrix B, int startRow, int stopRow)
		{
			for (int i = startRow; i < stopRow; ++i)
			{
				IntDoubleVectorPair Arow = A.GetRow(i);
				for (int j = 0; j < Arow.indices.Length; ++j)
					B.SetValue(i, Arow.indices[j], Arow.data[j]);
			}
		}

		protected internal virtual void CopyI(ISparseColumnRowAccessMatrix A, ISparseColumnRowAccessMatrix B)
		{
			B.Matrix = A.Matrix.Clone();
		}

		protected internal virtual void CopyI(ISparseColumnRowAccessMatrix A, IElementalAccessMatrix B, int startColumn, int stopColumn)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = startColumn; i < stopColumn; ++i)
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					B.SetValue(Amat.major[j], i, Amat.data[j]);
		}

		protected internal virtual void CopyI(ISparseColumnAccessMatrix A, ISparseColumnAccessMatrix B, int startColumn, int stopColumn)
		{
			for (int i = startColumn; i < stopColumn; ++i)
				B.SetColumn(i, A.GetColumn(i).Clone());
		}

		protected internal virtual void CopyI(ISparseColumnAccessMatrix A, 
			IElementalAccessMatrix B, int startColumn, int stopColumn)
		{
			for (int i = startColumn; i < stopColumn; ++i)
			{
				IntDoubleVectorPair Acol = A.GetColumn(i);
				for (int j = 0; j < Acol.indices.Length; ++j)
					B.SetValue(Acol.indices[j], i, Acol.data[j]);
			}
		}

		protected internal void CopyI(IDenseRowColumnAccessMatrix A, IDenseRowColumnAccessMatrix B)
		{
			B.Matrix = A.Matrix.Clone();
		}

		protected internal virtual void CopyI(IDenseRowColumnAccessMatrix A,
			IElementalAccessMatrix B, int startRow, int stopRow)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = startRow; i < stopRow; ++i)
				for (int j = Amat.indices[i], k = 0; j < Amat.indices[i + 1]; ++j, ++k)
					if (Amat.data[j] != 0.0)
						B.SetValue(i, k, Amat.data[j]);
		}

		protected internal virtual void CopyI(IDenseRowAccessMatrix A, 
			IDenseRowAccessMatrix B, int startRow, int stopRow)
		{
			for (int i = startRow; i < stopRow; ++i)
				Array.Copy(A.GetRow(i), 0, B.GetRow(i), 0, A.ColumnCount);
		}

		protected internal virtual void CopyI(IDenseRowAccessMatrix A, 
			IElementalAccessMatrix B, int startRow, int stopRow)
		{
			for (int i = startRow; i < stopRow; ++i)
			{
				double[] Arow = A.GetRow(i);
				for (int j = 0; j < Arow.Length; ++j)
					if (Arow[j] != 0.0)
						B.SetValue(i, j, Arow[j]);
			}
		}

		protected internal virtual void CopyI(IDenseColumnRowAccessMatrix A, IDenseColumnRowAccessMatrix B)
		{
			B.Matrix = A.Matrix.Clone();
		}

		protected internal virtual void CopyI(IDenseColumnRowAccessMatrix A, 
			IElementalAccessMatrix B, int startColumn, int stopColumn)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = startColumn; i < stopColumn; ++i)
				for (int j = Amat.indices[i]; j < Amat.indices[i + 1]; ++j)
					if (Amat.data[j] != 0.0)
						B.SetValue(j - Amat.indices[i], i, Amat.data[j]);
		}

		protected internal virtual void CopyI(IDenseColumnAccessMatrix A, IDenseColumnAccessMatrix B, int startColumn, int stopColumn)
		{
			for (int i = startColumn; i < stopColumn; ++i)
				Array.Copy(A.GetColumn(i), 0, B.GetColumn(i), 0, A.RowCount);
		}

		protected internal virtual void CopyI(IDenseColumnAccessMatrix A, IElementalAccessMatrix B, int startColumn, int stopColumn)
		{
			for (int i = startColumn; i < stopColumn; ++i)
			{
				double[] Acol = A.GetColumn(i);
				for (int j = 0; j < Acol.Length; ++j)
					if (Acol[j] != 0.0)
						B.SetValue(j, i, Acol[j]);
			}
		}

		protected internal virtual void CopyI(IElementalAccessMatrix A, IElementalAccessMatrix B, int startRow, int stopRow)
		{
			for (int i = startRow; i < stopRow; ++i)
				for (int j = 0; j < A.RowCount; ++j)
				{
					double val = A.GetValue(i, j);
					if (val != 0.0)
						B.SetValue(i, j, val);
				}
		}

		protected internal override IVector CopyI(IVector x, IVector y, int start, int stop)
		{
			Zero(y);

			if (x is IDenseAccessVector && y is IDenseAccessVector)
				CopyI((IDenseAccessVector) x, (IDenseAccessVector) y, start, stop);
			else if (x is IDenseAccessVector && y is IElementalAccessVector)
				CopyI((IDenseAccessVector) x, (IElementalAccessVector) y, start, stop);
			else if (x is ISparseAccessVector && y is IElementalAccessVector)
				CopyI((ISparseAccessVector) x, (IElementalAccessVector) y, start, stop);
			else if (x is IElementalAccessVector && y is IElementalAccessVector)
				CopyI((IElementalAccessVector) x, (IElementalAccessVector) y, start, stop);
			else
				throw new NotSupportedException();

			return y;
		}

		protected internal virtual void CopyI(IDenseAccessVector x, IDenseAccessVector y, int start, int stop)
		{
			double[] ydata = new double[stop - start];
			Array.Copy(x.Vector, start, ydata, 0, ydata.Length);
			y.Vector = ydata;
		}

		protected internal void CopyI(IDenseAccessVector x, IElementalAccessVector y, int start, int stop)
		{
			double[] xdata = x.Vector;
			for (int i = start; i < stop; ++i)
				if (xdata[i] != 0.0)
					y.SetValue(i - start, xdata[i]);
		}

		protected internal virtual void CopyI(ISparseAccessVector x, IElementalAccessVector y, int start, int stop)
		{
			IntDoubleVectorPair xdata = x.Vector;
			for (int i = 0; i < xdata.indices.Length; ++i)
				if (xdata.indices[i] >= start && xdata.indices[i] < stop)
					y.SetValue(xdata.indices[i] - start, xdata.data[i]);
		}

		protected internal virtual void CopyI(IElementalAccessVector x, IElementalAccessVector y, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				double val = x.GetValue(i);
				if (val != 0.0)
					y.SetValue(i - start, val);
			}
		}

		protected internal override IVector CopyI(IVector x, IVector y)
		{
			Zero(y);

			if (x is IDenseAccessVector && y is IDenseAccessVector)
				CopyI((IDenseAccessVector) x, (IDenseAccessVector) y);
			else if (x is IDenseAccessVector && y is IElementalAccessVector)
				CopyI((IDenseAccessVector) x, (IElementalAccessVector) y);
			else if (x is ISparseAccessVector && y is ISparseAccessVector)
				CopyI((ISparseAccessVector) x, (ISparseAccessVector) y);
			else if (x is ISparseAccessVector && y is IElementalAccessVector)
				CopyI((ISparseAccessVector) x, (IElementalAccessVector) y);
			else if (x is IBlockAccessVector && y is IBlockAccessVector)
				CopyI((IBlockAccessVector) x, (IBlockAccessVector) y, 0, ((IBlockAccessVector) x).BlockCount);
			else if (x is IElementalAccessVector && y is IElementalAccessVector)
				CopyI((IElementalAccessVector) x, (IElementalAccessVector) y);
			else
				throw new NotSupportedException();

			return y;
		}

		protected internal void CopyI(IBlockAccessVector x, IBlockAccessVector y, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				Copy(x.GetBlock(i), y.GetBlock(i));
		}

		protected internal virtual void CopyI(IDenseAccessVector x, IDenseAccessVector y)
		{
			double[] ydata = new double[x.Length];
			Array.Copy(x.Vector, 0, ydata, 0, x.Length);
			y.Vector = ydata;
		}

		protected internal virtual void CopyI(IDenseAccessVector x, IElementalAccessVector y)
		{
			double[] xdata = x.Vector;
			for (int i = 0; i < xdata.Length; ++i)
				if (xdata[i] != 0.0)
					y.SetValue(i, xdata[i]);
		}

		protected internal virtual void CopyI(ISparseAccessVector x, ISparseAccessVector y)
		{
			IntDoubleVectorPair xdata = x.Vector;
			int[] yindex = new int[xdata.indices.Length];
			double[] ydata = new double[xdata.data.Length];
			Array.Copy(xdata.indices, 0, yindex, 0, yindex.Length);
			Array.Copy(xdata.data, 0, ydata, 0, ydata.Length);
			y.Vector = new IntDoubleVectorPair(yindex, ydata);
		}

		protected internal virtual void CopyI(ISparseAccessVector x, IElementalAccessVector y)
		{
			IntDoubleVectorPair xdata = x.Vector;
			for (int i = 0; i < xdata.indices.Length; ++i)
				y.SetValue(xdata.indices[i], xdata.data[i]);
		}

		protected internal virtual void CopyI(IElementalAccessVector x, IElementalAccessVector y)
		{
			for (int i = 0; i < x.Length; ++i)
			{
				double val = x.GetValue(i);
				if (val != 0.0)
					y.SetValue(i, val);
			}
		}

		protected internal override double DotI(IVector x, IVector y)
		{
			if (x is IDenseAccessVector && y is IDenseAccessVector)
				return DotI((IDenseAccessVector) x, (IDenseAccessVector) y);
			else if (x is IDenseAccessVector && y is ISparseAccessVector)
				return DotI((IDenseAccessVector) x, (ISparseAccessVector) y);
			else if (x is ISparseAccessVector && y is IDenseAccessVector)
				return DotI((IDenseAccessVector) y, (ISparseAccessVector) x);
			else if (x is IBlockAccessVector && y is IBlockAccessVector)
				return DotI(block2array((IBlockAccessVector) x), block2array((IBlockAccessVector) y));
            else if (x is ISparseAccessVector && y is ISparseAccessVector)
                return DotI((ISparseAccessVector)x, (ISparseAccessVector)y);
			else
				throw new NotSupportedException();
		}

		protected internal virtual double DotI(double[] xdata, double[] ydata)
		{
			double dot = 0.0;
			for (int i = 0; i < xdata.Length; ++i)
				dot += xdata[i]*ydata[i];
			return dot;
		}

		protected internal virtual double DotI(IDenseAccessVector x, IDenseAccessVector y)
		{
			double[] xdata = x.Vector, ydata = y.Vector;
			return DotI(xdata, ydata);
		}

		protected internal virtual double DotI(IDenseAccessVector x, ISparseAccessVector y)
		{
			double dot = 0.0;
			double[] xdata = x.Vector;
			IntDoubleVectorPair ydata = y.Vector;
			for (int i = 0; i < ydata.indices.Length; ++i)
				dot += xdata[ydata.indices[i]]*ydata.data[i];
			return dot;
		}

        //TT!
        protected internal virtual double DotI(ISparseAccessVector x, ISparseAccessVector y)
        {
            double dot = 0.0;            
            IntDoubleVectorPair ydata = y.Vector;
            IntDoubleVectorPair xdata = x.Vector;
            if (xdata.indices.Length == 0 || ydata.indices.Length == 0) return 0d;
            int xcounter = 0;
            int ycounter = 0;
            while ((xcounter < xdata.indices.Length && ycounter < ydata.indices.Length))
            {
                int diff = xdata.indices[xcounter] - ydata.indices[ycounter];
                if (diff == 0)
                {
                    dot += xdata.data[xcounter] * ydata.data[ycounter];
                    xcounter++;
                    ycounter++;                    
                }
                else if (diff < 0)
                {
                    xcounter++;
                }
                else
                {
                    ycounter++;
                }                
            }
            return dot;
        }

		protected internal override IVector MultAddI(double alpha, 
			IMatrix A, IVector x, double beta, IVector y, IVector z)
		{
			if (A is IBlockAccessMatrix && x is IBlockAccessVector && y is IBlockAccessVector && z is IBlockAccessVector)
			{
				MultAddI(alpha, (IBlockAccessMatrix) A, (IBlockAccessVector) x, beta, (IBlockAccessVector) y, (IBlockAccessVector) z, 0, ((IBlockAccessMatrix) A).BlockCount);
				return z;
			}
			else if (A is IShellMatrix)
				return ((IShellMatrix) A).MultAdd(alpha, x, beta, y, z);

			// The following requires dense access to the vectors
			if (!(x is IDenseAccessVector) || !(y is IDenseAccessVector) || !(z is IDenseAccessVector))
				throw new NotSupportedException();
			double[] xdata = ((IDenseAccessVector) x).Vector, ydata = ((IDenseAccessVector) y).Vector, zdata = ((IDenseAccessVector) z).Vector;
			if (A is ISparseRowColumnAccessMatrix)
				MultAddI(alpha, (ISparseRowColumnAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.RowCount);
			else if (A is ISparseRowAccessMatrix)
				MultAddI(alpha, (ISparseRowAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.RowCount);
			else if (A is IDenseRowColumnAccessMatrix)
				MultAddI(alpha, (IDenseRowColumnAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.RowCount);
			else if (A is IDenseRowAccessMatrix)
				MultAddI(alpha, (IDenseRowAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.RowCount);
			else if (A is IDenseColumnAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);

				MultAddI((IDenseColumnAccessMatrix) A, xdata, zdata, 0, A.ColumnCount);
				
				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else if (A is IDenseColumnRowAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);
				
				MultAddI((IDenseColumnRowAccessMatrix) A, xdata, zdata, 0, A.ColumnCount);
				
				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else if (A is ISparseColumnAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);
				
				MultAddI((ISparseColumnAccessMatrix) A, xdata, zdata, 0, A.ColumnCount);  //double loop in A data, but only for the sparse elements
				
				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else if (A is ISparseColumnRowAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);

				MultAddI((ISparseColumnRowAccessMatrix) A, xdata, zdata, 0, A.ColumnCount);
				
				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else throw new NotSupportedException();

			return z;
		}

		protected internal virtual void MultAddI(double alpha, IBlockAccessMatrix A, IBlockAccessVector x, double beta, IBlockAccessVector y, IBlockAccessVector z, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				MultAdd(alpha, A.GetBlock(i), x.GetBlock(i), beta, y.GetBlock(i), z.GetBlock(i));
		}

		protected internal virtual void MultAddI(ISparseColumnAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				IntDoubleVectorPair Acol = A.GetColumn(i);
				for (int j = 0; j < Acol.indices.Length; ++j)
				{
					zdata[Acol.indices[j]] += Acol.data[j]*xdata[i];
				}
			}
		}

		protected internal virtual void MultAddI(ISparseColumnRowAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
				{
					zdata[Amat.major[j]] += Amat.data[j]*xdata[i];
				}
		}

		protected internal virtual void MultAddI(IDenseColumnAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				double[] Acol = A.GetColumn(i);
				for (int j = 0; j < Acol.Length; ++j)
				{
					zdata[j] += Acol[j]*xdata[i];
				}
			}
		}

		protected internal virtual void MultAddI(IDenseColumnRowAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
				for (int j = Amat.indices[i], k = 0; j < Amat.indices[i + 1]; ++j, ++k)
				{
						zdata[k] += Amat.data[j]*xdata[i];
				}
		}

		protected internal virtual void MultAddI(double alpha, IDenseRowColumnAccessMatrix A, 
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			int columns = A.ColumnCount;
			for (int i = start; i < stop; ++i)
			{
				double val = 0.0;

				int j = Amat.indices[i], l = 0;
				for (int k = columns%4; --k >= 0; )
					val += Amat.data[j++]*xdata[l++];
				for (int k = columns/4; --k >= 0; )
					val += Amat.data[j++]*xdata[l++] + Amat.data[j++]*xdata[l++] 
						+ Amat.data[j++]*xdata[l++] + Amat.data[j++]*xdata[l++];

				//for (int j = Amat.index[i], k = 0; j < Amat.index[i + 1];)
				//	val += Amat.data[j++] * xdata[k++];

				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal virtual void MultAddI(double alpha, IDenseRowAccessMatrix A, 
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			int columns = A.ColumnCount;
			for (int i = start; i < stop; ++i)
			{
				double[] Arow = A.GetRow(i);
				double val = 0.0;

				int j = 0, l = 0;
				for (int k = columns%4; --k >= 0; )
					val += Arow[j++]*xdata[l++];
				for (int k = columns/4; --k >= 0; )
					val += Arow[j++]*xdata[l++] + Arow[j++]*xdata[l++] 
						+ Arow[j++]*xdata[l++] + Arow[j++]*xdata[l++];

				//for (int j = 0; j < Arow.length; ++j)
				//	val += Arow[j] * xdata[j];

				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal virtual void MultAddI(double alpha, ISparseRowColumnAccessMatrix A, 
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
			{
				double val = 0.0;
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					val += Amat.data[j]*xdata[Amat.major[j]];
				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal virtual void MultAddI(double alpha, ISparseRowAccessMatrix A, 
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				IntDoubleVectorPair Amat = A.GetRow(i);
				double val = 0.0;
				for (int j = 0; j < Amat.indices.Length; ++j)
					val += Amat.data[j]*xdata[Amat.indices[j]];
				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal override double Norm1(IMatrix A)
		{
			if (A is IDenseRowColumnAccessMatrix)
				return Norm1((IDenseRowColumnAccessMatrix) A);
			else if (A is IDenseRowAccessMatrix)
				return Norm1((IDenseRowAccessMatrix) A);
			else if (A is ISparseRowAccessMatrix)
				return Norm1((ISparseRowAccessMatrix) A);
			else if (A is ISparseRowColumnAccessMatrix)
				return Norm1((ISparseRowColumnAccessMatrix) A);
			else
				throw new NotSupportedException();
		}

		protected internal double Norm1(IDenseRowColumnAccessMatrix A)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			double norm = 0;
			for (int i = 0; i < Amat.indices.Length; ++i)
				norm = Math.Max(norm, Norm1(Amat.data, Amat.indices[i], Amat.indices[i + 1]));
			return norm;
		}

		protected internal double Norm1(IDenseRowAccessMatrix A)
		{
			double norm = 0;
			for (int i = 0; i < A.RowCount; ++i)
				norm = Math.Max(norm, Norm1(A.GetRow(i)));
			return norm;
		}

		protected internal double Norm1(ISparseRowAccessMatrix A)
		{
			double norm = 0;
			for (int i = 0; i < A.RowCount; ++i)
				norm = Math.Max(norm, Norm1(A.GetRow(i).data));
			return norm;
		}

		protected internal double Norm1(ISparseRowColumnAccessMatrix A)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			double norm = 0;
			for (int i = 0; i < Amat.minor.Length; ++i)
				norm = Math.Max(norm, Norm1(Amat.data, Amat.minor[i], Amat.minor[i + 1]));
			return norm;
		}

		protected internal override double Norm1(IVector x)
		{
			if (x is IVectorAccess)
				return Norm1(((IVectorAccess) x).Data);
			else if (x is IBlockAccessVector)
				return Norm1(block2array((IBlockAccessVector) x));
			else
				throw new NotSupportedException();
		}

		protected internal virtual double Norm1(double[] data)
		{
			return Norm1(data, 0, data.Length);
		}

		protected internal virtual double Norm1(double[] data, int start, int stop)
		{
			double norm = 0.0;
			for (int i = start; i < stop; ++i)
				norm += Math.Abs(data[i]);
			return norm;
		}

		protected internal override double Norm2(IVector x)
		{
			if (x is IVectorAccess)
				return Norm2((IVectorAccess) x);
			else if (x is IBlockAccessVector)
				return Norm2(block2array((IBlockAccessVector) x));
			else
				throw new NotSupportedException();
		}

		protected internal virtual double Norm2(double[] xdata)
		{
			double scale = 0, ssq = 1;
			for (int i = 0; i < xdata.Length; ++i)
			{
				if (xdata[i] != 0)
				{
					double absxi = Math.Abs(xdata[i]);
					if (scale < absxi)
					{
						ssq = 1 + ssq*Math.Pow(scale/absxi, 2);
						scale = absxi;
					}
					else
						ssq = ssq + Math.Pow(absxi/scale, 2);
				}
			}
			return scale*Math.Sqrt(ssq);
		}

		protected internal virtual double Norm2(IVectorAccess x)
		{
			double[] xdata = x.Data;
			return Norm2(xdata);
		}

		protected internal override double NormF(IMatrix A)
		{
			if (A is IVectorAccess)
				return Norm2((IVectorAccess) A);
			else if (A is IMatrixAccess)
				return NormF((IMatrixAccess) A);
			else
				throw new NotSupportedException();
		}

		protected internal virtual double NormF(IMatrixAccess A)
		{
			double[][] Adata = A.Data;
			double scale = 0, ssq = 1;
			for (int i = 0; i < Adata.Length; ++i)
				for (int j = 0; j < Adata[i].Length; ++j)
				{
					if (Adata[i][j] != 0)
					{
						double absxi = Math.Abs(Adata[i][j]);
						if (scale < absxi)
						{
							ssq = 1 + ssq*Math.Pow(scale/absxi, 2);
							scale = absxi;
						}
						else
							ssq = ssq + Math.Pow(absxi/scale, 2);
					}
				}
			return scale*Math.Sqrt(ssq);
		}

		protected internal override double NormInf(IMatrix A)
		{
			if (A is IDenseColumnRowAccessMatrix)
				return NormInf((IDenseColumnRowAccessMatrix) A);
			else if (A is IDenseColumnAccessMatrix)
				return NormInf((IDenseColumnAccessMatrix) A);
			else if (A is ISparseColumnRowAccessMatrix)
				return NormInf((ISparseColumnRowAccessMatrix) A);
			else if (A is ISparseColumnAccessMatrix)
				return NormInf((ISparseColumnAccessMatrix) A);
			else
				throw new NotSupportedException();
		}

		protected internal double NormInf(IDenseColumnAccessMatrix A)
		{
			double norm = 0;
			for (int i = 0; i < A.ColumnCount; ++i)
				norm = Math.Max(norm, Norm1(A.GetColumn(i)));
			return norm;
		}

		protected internal virtual double NormInf(IDenseColumnRowAccessMatrix A)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			double norm = 0;
			for (int i = 0; i < Amat.indices.Length; ++i)
				norm = Math.Max(norm, Norm1(Amat.data, Amat.indices[i], Amat.indices[i + 1]));
			return norm;
		}

		protected internal double Norm1(ISparseColumnAccessMatrix A)
		{
			double norm = 0;
			for (int i = 0; i < A.RowCount; ++i)
				norm = Math.Max(norm, Norm1(A.GetColumn(i).data));
			return norm;
		}

		protected internal virtual double Norm1(ISparseColumnRowAccessMatrix A)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			double norm = 0;
			for (int i = 0; i < Amat.minor.Length; ++i)
				norm = Math.Max(norm, Norm1(Amat.data, Amat.minor[i], Amat.minor[i + 1]));
			return norm;
		}

		protected internal override double NormInf(IVector x)
		{
			if (x is IVectorAccess)
				return NormInf(((IVectorAccess) x).Data);
			else if (x is IBlockAccessVector)
				return NormInf(block2array((IBlockAccessVector) x));
			else
				throw new NotSupportedException();
		}

		protected internal virtual double NormInf(double[] data)
		{
			double norm = 0.0;
			for (int i = 0; i < data.Length; ++i)
				if (Math.Abs(data[i]) > norm)
					norm = Math.Abs(data[i]);
			return norm;
		}

		protected internal override IMatrix Rank1I(double alpha, IVector x, IVector y, IMatrix A)
		{
			try
			{
				Rank1I(alpha, (DenseVector) x, (DenseVector) y, (IElementalAccessMatrix) A, 0, x.Length);
			}
			catch (InvalidCastException)
			{
				throw new NotSupportedException();
			}

			return A;
		}

		protected internal virtual void Rank1I(double alpha, DenseVector x, DenseVector y, IElementalAccessMatrix A, int start, int stop)
		{
			double[] xdata = x.Data, ydata = y.Data;

			for (int i = start; i < stop; ++i)
			{
				double xval = xdata[i];
				if (xval != 0.0)
					for (int j = 0; j < ydata.Length; ++j)
					{
						double yval = ydata[j];
						if (yval != 0.0)
							A.AddValue(i, j, alpha*xval*yval);
					}
			}
		}

		protected internal override IMatrix ScaleI(double alpha, IMatrix A)
		{
			if (A is IVectorAccess)
				ScaleI(alpha, (IVectorAccess) A);
			else if (A is IMatrixAccess)
				ScaleI(alpha, (IMatrixAccess) A, 0, A.RowCount);
			else
				throw new NotSupportedException();
			return A;
		}

		protected internal override IVector ScaleI(double alpha, IVector x)
		{
			if (x is IVectorAccess)
				ScaleI(alpha, (IVectorAccess) x);
			else if (x is IBlockAccessVector)
				ScaleI(alpha, (IBlockAccessVector) x, 0, ((IBlockAccessVector) x).BlockCount);
			else
				throw new NotSupportedException();
			return x;
		}

		protected internal virtual void ScaleI(double alpha, IBlockAccessVector x, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				Scale(alpha, x.GetBlock(i));
		}

		protected internal virtual void ScaleI(double alpha, IVectorAccess x)
		{
			double[] xdata = x.Data;
			for (int i = 0; i < xdata.Length; ++i)
				xdata[i] *= alpha;
		}

		protected internal virtual void ScaleI(double alpha, IMatrixAccess A, int start, int stop)
		{
			double[][] Adata = A.Data;
			for (int i = start; i < stop; ++i)
				for (int j = 0; j < Adata[i].Length; ++j)
					Adata[i][j] *= alpha;
		}

		protected internal override IVector SetI(double alpha, IVector x)
		{
			if (x is IDenseAccessVector)
				return SetI(alpha, (IDenseAccessVector) x);
			else if (x is IElementalAccessMatrix)
				return SetI(alpha, (IElementalAccessVector) x);
			else
				throw new NotSupportedException();
		}

		protected internal IVector SetI(double alpha, IDenseAccessVector x)
		{
			double[] xdata = x.Vector;
			SupportClass.ArraySupport.Fill(xdata, alpha);
			return x;
		}

		protected internal IVector SetI(double alpha, IElementalAccessVector x)
		{
			for (int i = 0; i < x.Length; ++i)
				x.SetValue(i, alpha);
			return x;
		}

		protected internal override IVector TransMultAddI(double alpha, IMatrix A, IVector x, double beta, IVector y, IVector z)
		{
			if (A is IBlockAccessMatrix && x is IBlockAccessVector && y is IBlockAccessVector && z is IBlockAccessVector)
			{
				TransMultAddI(alpha, (IBlockAccessMatrix) A, (IBlockAccessVector) x, beta, (IBlockAccessVector) y, (IBlockAccessVector) z, 0, ((IBlockAccessMatrix) A).BlockCount);
				return z;
			}
			else if (A is IShellMatrix)
				return ((IShellMatrix) A).TransMultAdd(alpha, x, beta, y, z);

			// The following requires dense access to the vectors
			if (!(x is IDenseAccessVector) || !(y is IDenseAccessVector) || !(z is IDenseAccessVector))
				throw new NotSupportedException();
			double[] xdata = ((IDenseAccessVector) x).Vector, ydata = ((IDenseAccessVector) y).Vector, zdata = ((IDenseAccessVector) z).Vector;
			if (A is ISparseRowColumnAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);
				
				TransMultAddI((ISparseRowColumnAccessMatrix) A, xdata, zdata, 0, A.RowCount);
				
				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else if (A is ISparseRowAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);
				
				TransMultAddI((ISparseRowAccessMatrix) A, xdata, zdata, 0, A.RowCount);
				
				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else if (A is IDenseRowColumnAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);

				TransMultAddI((IDenseRowColumnAccessMatrix) A, xdata, zdata, 0, A.RowCount);
				
				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else if (A is IDenseRowAccessMatrix)
			{
				if (y != z) Zero(z);
				else Scale(beta/alpha, z);

				TransMultAddI((IDenseRowAccessMatrix) A, xdata, zdata, 0, A.RowCount);

				if (y != z) Add(beta, y, alpha, z);
				else Scale(alpha, z);
			}
			else if (A is ISparseColumnAccessMatrix)
				TransMultAddI(alpha, (ISparseColumnAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.ColumnCount);
			else if (A is ISparseColumnRowAccessMatrix)
				TransMultAddI(alpha, (ISparseColumnRowAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.ColumnCount);
			else if (A is IDenseColumnRowAccessMatrix)
				TransMultAddI(alpha, (IDenseColumnRowAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.ColumnCount);
			else if (A is IDenseColumnAccessMatrix)
				TransMultAddI(alpha, (IDenseColumnAccessMatrix) A, xdata, beta, ydata, zdata, 0, A.ColumnCount);
			else
				throw new NotSupportedException();

			return z;
		}

		protected internal virtual void TransMultAddI(double alpha, IBlockAccessMatrix A, 
			IBlockAccessVector x, double beta, IBlockAccessVector y, IBlockAccessVector z, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				TransMultAdd(alpha, A.GetBlock(i), x.GetBlock(i), beta, y.GetBlock(i), z.GetBlock(i));
		}

		protected internal virtual void TransMultAddI(double alpha, ISparseColumnRowAccessMatrix A, 
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
			{
				double val = 0.0;
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
					val += Amat.data[j]*xdata[Amat.major[j]];
				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal virtual void TransMultAddI(double alpha, ISparseColumnAccessMatrix A, 
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				IntDoubleVectorPair Acol = A.GetColumn(i);
				double val = 0.0;
				for (int j = 0; j < Acol.indices.Length; ++j)
					val += Acol.data[j]*xdata[Acol.indices[j]];
				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal virtual void TransMultAddI(double alpha, IDenseColumnRowAccessMatrix A, 
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			int rows = A.RowCount;
			for (int i = start; i < stop; ++i)
			{
				double val = 0.0;

				int j = Amat.indices[i], l = 0;
				for (int k = rows%4; --k >= 0; )
					val += Amat.data[j++]*xdata[l++];
				for (int k = rows/4; --k >= 0; )
					val += Amat.data[j++]*xdata[l++] + Amat.data[j++]*xdata[l++] 
						+ Amat.data[j++]*xdata[l++] + Amat.data[j++]*xdata[l++];

				//for (int j = Amat.index[i], k = 0; j < Amat.index[i + 1]; ++j, ++k)
				//	val += Amat.data[j] * xdata[k];

				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal virtual void TransMultAddI(double alpha, IDenseColumnAccessMatrix A,
			double[] xdata, double beta, double[] ydata, double[] zdata, int start, int stop)
		{
			int rows = A.RowCount;
			for (int i = start; i < stop; ++i)
			{
				double[] Acol = A.GetColumn(i);
				double val = 0.0;

				int j = 0, l = 0;
				for (int k = rows%4; --k >= 0; )
					val += Acol[j++]*xdata[l++];
				for (int k = rows/4; --k >= 0; )
					val += Acol[j++]*xdata[l++] + Acol[j++]*xdata[l++] 
						+ Acol[j++]*xdata[l++] + Acol[j++]*xdata[l++];

				//for (int j = 0; j < Arow.length; ++j)
				//	val += Arow[j] * xdata[j];

				zdata[i] = alpha*val + beta*ydata[i];
			}
		}

		protected internal virtual void TransMultAddI(ISparseRowColumnAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
			{
				for (int j = Amat.minor[i]; j < Amat.minor[i + 1]; ++j)
				{
					zdata[Amat.major[j]] += Amat.data[j]*xdata[i];
				}
			}
		}

		protected internal virtual void TransMultAddI(ISparseRowAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				IntDoubleVectorPair Arow = A.GetRow(i);
				for (int j = 0; j < Arow.indices.Length; ++j)
				{
					zdata[Arow.indices[j]] += Arow.data[j]*xdata[i];
				}
			}
		}

		protected internal virtual void TransMultAddI(IDenseRowColumnAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
			{
				for (int j = Amat.indices[i]; j < Amat.indices[i + 1]; ++j)
				{
					zdata[j - Amat.indices[i]] += Amat.data[j]*xdata[i];
				}
			}
		}

		protected internal virtual void TransMultAddI(IDenseRowAccessMatrix A, 
			double[] xdata, double[] zdata, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				double[] Arow = A.GetRow(i);
				for (int j = 0; j < Arow.Length; ++j)
				{
					zdata[j] += Arow[j]*xdata[i];
				}
			}
		}

		protected internal override IMatrix TransposeI(IMatrix A, IMatrix B)
		{
			if (A is ISparseRowAccessMatrix && B is ISparseColumnAccessMatrix)
				TransposeI((ISparseRowAccessMatrix) A, (ISparseColumnAccessMatrix) B, 0, A.RowCount);
			else if (A is ISparseRowColumnAccessMatrix && B is ISparseColumnAccessMatrix)
				TransposeI((ISparseRowColumnAccessMatrix) A, (ISparseColumnAccessMatrix) B, 0, A.RowCount);
			else if (A is ISparseColumnAccessMatrix && B is ISparseRowAccessMatrix)
				TransposeI((ISparseColumnAccessMatrix) A, (ISparseRowAccessMatrix) B, 0, A.ColumnCount);
			else if (A is ISparseColumnRowAccessMatrix && B is ISparseRowAccessMatrix)
				TransposeI((ISparseColumnRowAccessMatrix) A, (ISparseRowAccessMatrix) B, 0, A.ColumnCount);
			else if (A is IDenseRowAccessMatrix && B is IDenseColumnAccessMatrix)
				TransposeI((IDenseRowAccessMatrix) A, (IDenseColumnAccessMatrix) B, 0, A.RowCount);
			else if (A is IDenseRowColumnAccessMatrix && B is IDenseColumnAccessMatrix)
				TransposeI((IDenseRowColumnAccessMatrix) A, (IDenseColumnAccessMatrix) B, 0, A.RowCount);
			else if (A is IDenseColumnAccessMatrix && B is IDenseRowAccessMatrix)
				TransposeI((IDenseColumnAccessMatrix) A, (IDenseRowAccessMatrix) B, 0, A.ColumnCount);
			else if (A is IDenseColumnRowAccessMatrix && B is IDenseRowAccessMatrix)
				TransposeI((IDenseColumnRowAccessMatrix) A, (IDenseRowAccessMatrix) B, 0, A.ColumnCount);
			else if (A is IElementalAccessMatrix && B is IElementalAccessMatrix)
			{
				Zero(B);
				TransposeI((IElementalAccessMatrix) A, (IElementalAccessMatrix) B, 0, A.RowCount);
			}
			else
				throw new NotSupportedException();
			return B;
		}

		protected internal virtual void TransposeI(IElementalAccessMatrix A, 
			IElementalAccessMatrix B, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				for (int j = 0; j < A.ColumnCount; ++j)
				{
					double val = A.GetValue(i, j);
					if (val != 0.0)
						B.SetValue(j, i, val);
				}
		}

		protected internal virtual void TransposeI(ISparseRowAccessMatrix A, 
			ISparseColumnAccessMatrix B, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				B.SetColumn(i, A.GetRow(i).Clone());
		}

		protected internal virtual void TransposeI(ISparseRowColumnAccessMatrix A, 
			ISparseColumnAccessMatrix B, int start, int stop)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
			{
				B.SetColumn(i, Amat.copy(i));
			}
		}

		protected internal virtual void TransposeI(ISparseColumnAccessMatrix A, 
			ISparseRowAccessMatrix B, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				B.SetRow(i, A.GetColumn(i).Clone());
		}

		protected internal virtual void TransposeI(ISparseColumnRowAccessMatrix A, 
			ISparseRowAccessMatrix B, int start, int stop)
		{
			IntIntDoubleVectorTriple Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
			{
				B.SetRow(i, Amat.copy(i));
			}
		}

		protected internal virtual void TransposeI(IDenseRowAccessMatrix A, 
			IDenseColumnAccessMatrix B, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				double[] Bcol = B.GetColumn(i);
				Array.Copy(A.GetRow(i), 0, Bcol, 0, Bcol.Length);
			}
		}

		protected internal virtual void TransposeI(IDenseRowColumnAccessMatrix A, 
			IDenseColumnAccessMatrix B, int start, int stop)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
				B.SetColumn(i, Amat.Copy(i));
		}

		protected internal virtual void TransposeI(IDenseColumnAccessMatrix A, 
			IDenseRowAccessMatrix B, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
			{
				double[] Brow = B.GetRow(i);
				Array.Copy(A.GetColumn(i), 0, Brow, 0, Brow.Length);
			}
		}

		protected internal virtual void TransposeI(IDenseColumnRowAccessMatrix A, 
			IDenseRowAccessMatrix B, int start, int stop)
		{
			IntDoubleVectorPair Amat = A.Matrix;
			for (int i = start; i < stop; ++i)
				B.SetRow(i, Amat.Copy(i));
		}

		public override IMatrix Zero(IMatrix A)
		{
			if (A is IVectorAccess)
				ZeroI(((IVectorAccess) A).Data);
			else if (A is IMatrixAccess)
				ZeroI(((IMatrixAccess) A).Data);
			else if (A is IBlockAccessMatrix)
				ZeroI((IBlockAccessMatrix) A, 0, ((IBlockAccessMatrix) A).BlockCount);
			else
				throw new NotSupportedException();
			return A;
		}

		protected internal virtual void ZeroI(IBlockAccessMatrix A, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				Zero(A.GetBlock(i));
		}

		protected internal virtual void ZeroI(double[] data)
		{
			SupportClass.ArraySupport.Fill(data, 0.0);
		}

		protected internal virtual void ZeroI(double[][] data)
		{
			for (int i = 0; i < data.Length; ++i)
				SupportClass.ArraySupport.Fill(data[i], 0.0);
		}

		public override IVector Zero(IVector x)
		{
			if (x is IVectorAccess)
				ZeroI(((IVectorAccess) x).Data);
			else if (x is IBlockAccessVector)
				ZeroI((IBlockAccessVector) x, 0, ((IBlockAccessVector) x).BlockCount);
			else
				throw new NotSupportedException();

			return x;
		}

		protected internal virtual void ZeroI(IBlockAccessVector x, int start, int stop)
		{
			for (int i = start; i < stop; ++i)
				Zero(x.GetBlock(i));
		}

		public override int Cardinality(IMatrix A)
		{
			if (A is IVectorAccess)
				return CardinalityI(((IVectorAccess) A).Data);
			else if (A is IMatrixAccess)
				return CardinalityI(((IMatrixAccess) A).Data);
			else
				return A.RowCount*A.ColumnCount;
		}

		protected internal virtual int CardinalityI(double[] data)
		{
			return data.Length;
		}

		protected internal virtual int CardinalityI(double[][] data)
		{
			int nnz = 0;
			for (int i = 0; i < data.Length; ++i)
				nnz += data[i].Length;
			return nnz;
		}

		public override int Cardinality(IVector x)
		{
			if (x is IVectorAccess)
				return CardinalityI(((IVectorAccess) x).Data);
			else
				return x.Length;
		}

		public override IntDoubleVectorPair Gather(double[] x)
		{
			// Number of non-zeros
			int nnz = 0;
			for (int i = 0; i < x.Length; ++i)
				if (x[i] == 0.0)
					nnz++;

			// Locations of the non-zeros
			int[] nz = new int[nnz];
			int j = 0;
			for (int i = 0; i < x.Length; ++i)
				if (x[i] == 0.0)
					nz[j++] = i;

			return Gather(nz, x);
		}

		public override IntDoubleVectorPair Gather(int[] xIndex, double[] xData)
		{
			int[] index = new int[xIndex.Length];
			Array.Copy(xIndex, 0, index, 0, index.Length);

			double[] data = new double[xIndex.Length];
			for (int i = 0; i < xIndex.Length; ++i)
				data[i] = xData[xIndex[i]];

			return new IntDoubleVectorPair(index, data);
		}

		public override double[] Scatter(IntDoubleVectorPair x, double[] y)
		{
			SupportClass.ArraySupport.Fill(y, 0.0);
			for (int i = 0; i < x.indices.Length; ++i)
				y[x.indices[i]] = x.data[i];
			return y;
		}

		public override double[,] GetArrayCopy(IMatrix A)
		{
			if (A is IElementalAccessMatrix)
			{
				int[] row = new int[A.RowCount], 
					column = new int[A.ColumnCount];

				for (int i = 0; i < row.Length; row[i] = i++);
				for (int i = 0; i < column.Length; column[i] = i++);

				return ((IElementalAccessMatrix) A).GetValues(row, column);
			}
			else
				throw new NotSupportedException();
		}

		public override double[] GetArrayCopy(IVector x)
		{
			double[] ret = new double[x.Length];
			if (x is IDenseAccessVector)
				Array.Copy(((IDenseAccessVector) x).Vector, 0, ret, 0, ret.Length);
			else if (x is ISparseAccessVector)
				ret = Scatter(((ISparseAccessVector) x).Vector, ret);
			else if (x is IBlockAccessVector)
				Array.Copy(((IBlockAccessVector) x).Array, 0, ret, 0, ret.Length);
			else
				throw new NotSupportedException();
			return ret;
		}

		public override IVector SetVector(double[] x, IVector y)
		{
			if (y is IDenseAccessVector)
				((IDenseAccessVector) y).Vector = x;
			else if (y is ISparseAccessVector)
				Scatter(((ISparseAccessVector) y).Vector, x);
			else
				throw new NotSupportedException();
			return y;
		}

		/// <summary> Populates the dense array of x with its data, and returns it</summary>
		protected internal virtual double[] block2array(IBlockAccessVector x)
		{
			double[] y = x.Array;
			for (int i = 0; i < x.BlockCount; ++i)
			{
				int[] index = x.GetBlockIndices(i);
				double[] data = ((IDenseAccessVector) x.GetBlock(i)).Vector;
				for (int j = 0; j < data.Length; ++j)
					y[index[j]] += data[j];
			}
			return y;
		}
	}
}