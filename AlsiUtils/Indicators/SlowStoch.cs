using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
	public class SlowStoch:Indicator
	 {
	     public int Slow_D { get; set; }

	     private int _Slow_K;

		 public int Slow_K
		 {
			 get { return _Slow_K; }
			 set { _Slow_K = value; }
		 }
		 private int _Fast_K;

		 public int Fast_K
		 {
			 get { return _Fast_K; }
			 set { _Fast_K = value; }
		 }

		 private double _K;

		 public double K
		 {
			 get { return _K; }
			 set { _K = value; }
		 }
		 private double _D;

		 public double D
		 {
			 get { return _D; }
			 set { _D = value; }
		 }
	 }
}
