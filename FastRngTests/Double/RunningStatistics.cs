using System;

namespace FastRngTests.Double
{
    internal sealed class RunningStatistics
    {
        private double previousM;
        private double previousS;
        private double nextM;
        private double nextS;
  
        public RunningStatistics()
        {
        }
 
        public int NumberRecords { get; private set; } = 0;
        
        public void Clear() => this.NumberRecords = 0;

        public void Push(double x)
        {
            this.NumberRecords++;
 
            // See Knuth TAOCP vol 2, 3rd edition, page 232
            if (this.NumberRecords == 1)
            {
                previousM = nextM = x;
                previousS = 0.0;
            }
            else
            {
                nextM = previousM + (x - previousM) / this.NumberRecords;
                nextS = previousS + (x - previousM) * (x - nextM);
     
                // set up for next iteration
                previousM = nextM;
                previousS = nextS;
            }
        }

        public double Mean => this.NumberRecords > 0 ? nextM : 0.0;

        public double Variance => this.NumberRecords > 1 ? nextS / (NumberRecords - 1) : 0.0;

        public double StandardDeviation => Math.Sqrt(this.Variance);
    }
}