using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
   public class Parameter_SlowStoch:Parameter_General
    {
        private int _Fast_K;

        public int Fast_K
        {
            get { return _Fast_K; }
            set { _Fast_K = value; }
        }
        private int _Slow_K;

        public int Slow_K
        {
            get { return _Slow_K; }
            set { _Slow_K = value; }
        }
        private int _Slow_D;

        public int Slow_D
        {
            get { return _Slow_D; }
            set { _Slow_D = value; }
        }
        private int _Open_80;

        public int Open_80
        {
            get { return _Open_80; }
            set { _Open_80 = value; }
        }
        private int _Close_80;

        public int Close_80
        {
            get { return _Close_80; }
            set { _Close_80 = value; }
        }
        private int _Open_20;

        public int Open_20
        {
            get { return _Open_20; }
            set { _Open_20 = value; }
        }
        private int _Close_20;

        public int Close_20
        {
            get { return _Close_20; }
            set { _Close_20 = value; }
        }
      

        private int _Volume;

        public int Volume
        {
            get { return _Volume; }
            set { _Volume = value; }
        }

      
    }
}
