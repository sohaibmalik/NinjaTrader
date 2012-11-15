using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
	public class BollingerBand:Indicator
	{
		private double _P;

		public double P
		{
			get { return _P; }
			set { _P = value; }
		}
		private int _N;

		public int N
		{
			get { return _N; }
			set { _N = value; }
		}
		private double _Upper;

		public double Upper
		{
			get { return _Upper; }
			set { _Upper = value; }
		}
		private double _Lower;

		public double Lower
		{
			get { return _Lower; }
			set { _Lower = value; }
		}
		private double _Mid;

		public double Mid
		{
			get { return _Mid; }
			set { _Mid = value; }
		}
	}
}
