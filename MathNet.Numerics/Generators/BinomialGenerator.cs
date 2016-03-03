#region MathNet Numerics, Copyright ©2004 Thaddaeus Parker

// MathNet Numerics, part of MathNet
//
// Copyright (c) 2004,	Thaddaeus Parker
//
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
using MathNet.Numerics;

#region Testing includes
#if DEBUG
using NUnit.Framework;
#endif
#endregion

namespace MathNet.Numerics.Generators
{
	/// <summary>
	/// BinomialGenerator is a binomial deviate random number using the System.Math.Random as a seed generator.
	/// </summary>
	/// <remarks><i>Taken from Numerical Recipes in C</i>, William H. Press, et al; <b>Chap 7.3 pg 295 equation 7.3.7</b></remarks>
	public class BinomialGenerator : IRealGenerator
	{
		/* TODO: comments on all those variables */

		private static Random random = new Random();
		private static int oldNumber = -1;
		private static double inProbability; //used for the next function
		private static double oldProbability= -1.0, pc, plog, pclog, en, oldg;
		
		private double am, em, g,angle, prob, bnl, sq, t, y;
		
		private int numberTrials;
		private int j; //used for counters

		/// <summary>
		/// Default constructor that accepts a given probability and number of trials that is used for the entire 
		/// instance life.  For non-instance binomial deviates <see cref="Next(double,int)"/>
		/// </summary>
		/// <param name="probability"></param>
		/// <param name="numberTrials"></param>
		public BinomialGenerator(double probability, int numberTrials)
		{
			//initializes all of the internal static variables
			prob = (probability <= 0.5d) ? probability : 1.0-probability;
			am = numberTrials * prob;
			this.numberTrials = numberTrials;
			inProbability = probability;
		}
		
		/// <summary>
		/// Returns the next binomial value based on the instantiated BinomialGenerator
		/// <seealso cref="BinomialGenerator.Next(double,int)"/>
		/// </summary>
		/// <returns>A number from an integer value that is a random deviate drawn from a binomial
		/// distribution of x trials each of probability xx</returns>
		public double Next() 
		{
			// TODO: check that the call here below is correct
			return Next(this.prob, this.numberTrials);
		}

		/// <summary>
		/// Returns a number from an integer value that is a random deviate drawn from a binomial distribution of
		/// numTrials each of probability "probability", using the System.Random as a source of uniform random deviates.
		/// </summary>
		/// <param name="probability">The probability of each trial</param>
		/// <param name="numTrials">the number of trials to perform</param>
		/// <returns>A binomial deviated floating point number</returns>
		public double Next(double probability, int numTrials) 
		{
			//reset the probability and number of trials but keep the same seed.
			prob = (probability <= 0.5d)?probability:1.0-probability;
			am = numTrials * prob;
			numberTrials = numTrials;
			inProbability = probability;
			if(numberTrials < 25){
				bnl = 0.0d;
				for(j = 1; j <= numberTrials; j++){
					if(random.NextDouble() < prob)
						bnl++;
				}
			}else if(am < 1.0){
				g = System.Math.Exp(-am);
				t = 1.0;
				for(j = 0; j <= numberTrials; j++){
					t *= random.NextDouble();
					if( t < g)
						break;
				}
				bnl = ((j <= numberTrials) ? j : numberTrials);
			}else{
				//use rejection method
				if( numberTrials != oldNumber){
					en = numberTrials;
					oldg = Trig.GammaLn(en+1.0);
					oldNumber = numberTrials;
				}
				if(prob != oldProbability){
					pc = 1.0 - prob;
					plog = System.Math.Log(prob);
					pclog = System.Math.Log(pc);
					oldProbability = prob;
				}
				sq = System.Math.Sqrt(2.0*am*pc);
				do{
					do{
						angle = System.Math.PI*random.NextDouble();
						y = System.Math.Tan(angle);
						em = sq*y+am;
					}while(em < 0.0 || em >=(en+1.0)); //reject
					em = System.Math.Floor(em);
					t = 1.2* sq*(1.0+y*y)*System.Math.Exp(oldg-Trig.GammaLn(em+1.0) - Trig.GammaLn(en-em+1.0)+em*plog+(en-em)*pclog);
				}while(random.NextDouble()> t);
				bnl = em;
			}
			if(prob != inProbability)
				bnl = numberTrials-bnl;
			return bnl;
		}

	}//end of class BinomialGenerator

	#region Testing Area
#if DEBUG
	/// <summary>
	/// Testing suite for the <see cref="BinomialGenerator"/> class.
	/// </summary>
	[TestFixture]
	public class TestingSuite
	{
		private BinomialGenerator bg;

		/// <summary>
		/// Initializes each test 
		/// </summary>
		[SetUp]
		public void Init(){
			bg = new BinomialGenerator(0.65, 24);
		}

		/// <summary>
		/// Testing the <see cref="BinomialGenerator.Next"/>
		/// </summary>
		[Test] public void Next()
		{
			// TODO: implement this test
		}
	}
#endif
	#endregion
}
