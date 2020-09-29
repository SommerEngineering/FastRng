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
                this.previousM = this.nextM = x;
                this.previousS = 0.0;
            }
            else
            {
                this.nextM = this.previousM + (x - this.previousM) / this.NumberRecords;
                this.nextS = this.previousS + (x - this.previousM) * (x - this.nextM);
     
                // set up for next iteration
                this.previousM = this.nextM;
                this.previousS = this.nextS;
            }
        }

        public double Mean => this.NumberRecords > 0 ? this.nextM : 0.0;

        public double Variance => this.NumberRecords > 1 ? this.nextS / (this.NumberRecords - 1) : 0.0;

        public double StandardDeviation => Math.Sqrt(this.Variance);
    }
}