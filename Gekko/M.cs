using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Gekko
{
    //Math stuff for solvers
    //There should be a mirror version of this in solver module

    public static class M
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //[MethodImpl(MethodImplOptions.AggressiveOptimization)] --> does not seem to exist for this .NET
        public static double Add(double x1, double x2)
        {
            return x1 + x2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Subtract(double x1, double x2)
        {
            return x1 - x2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Multiply(double x1, double x2)
        {
            return x1 * x2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Divide(double x1, double x2)
        {
            return x1 / x2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Power(double x1, double x2)
        {
            return Math.Pow(x1, x2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Negate(double x1)
        {
            return -x1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Log(double x1)
        {
            return Math.Log(x1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Exp(double x1)
        {
            return Math.Exp(x1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(double x1)
        {
            return Math.Abs(x1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Max(double x1, double x2)
        {
            return Math.Max(x1, x2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Min(double x1, double x2)
        {
            return Math.Min(x1, x2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sqrt(double x1)
        {
            return Math.Sqrt(x1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sqr(double x1)
        {
            return x1 * x1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Tanh(double x1)
        {
            return Math.Tanh(x1);
        }

        ///// <summary>
        ///// Note: -1.96 --> 0.0249978949718236, 0 --> 0.5, 1.96 --> 0.975002105028176. Just cumulative standard normal.
        ///// </summary>
        ///// <param name="x1"></param>
        ///// <returns></returns>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]        
        //public static double Errorf(double x1)
        //{            
        //    if(double.IsNaN(x1))return double.NaN;
        //    double d = Globals.cumulativeNormalDistribution.ValueOf(x1);
        //    return d;
        //}
    }
}
