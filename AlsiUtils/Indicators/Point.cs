using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AlsiUtils
{
    public abstract class Point
    {
			public double _high;

				public double High
        {
            get { return _high; }
            set { _high = value; }
        }
				public double _open;

				public double Open
        {
            get { return _open; }
            set { _open = value; }
        }
				public double _low;

				public double Low
        {
            get { return _low; }
            set { _low = value; }
        }
				public double _close;

				public double Close
        {
            get { return _close; }
            set { _close = value; }
        }
        public int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public int _interval;

        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        public DateTime _timeStamp;

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }


        public virtual void create()
        {
        }

        public  virtual void calculate()
        {
        }

        public  virtual void Draw()
        {
        }

        public virtual string info()
        {
            return null;
        }
    }
}
