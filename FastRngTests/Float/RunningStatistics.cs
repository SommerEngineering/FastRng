using System;

namespace FastRngTests.Float
{
    internal sealed class RunningStatistics
    {
        private float previousM;
        private float previousS;
        private float nextM;
        private float nextS;
  
        public RunningStatistics()
        {
        }
 
        public int NumberRecords { get; private set; } = 0;
        
        public void Clear() => this.NumberRecords = 0;

        public void Push(float x)
        {
            this.NumberRecords++;
 
            // See Knuth TAOCP vol 2, 3rd edition, page 232
            if (this.NumberRecords == 1)
            {
                this.previousM = this.nextM = x;
                this.previousS = 0.0f;
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

        public float Mean => this.NumberRecords > 0 ? this.nextM : 0.0f;

        public float Variance => this.NumberRecords > 1 ? this.nextS / (this.NumberRecords - 1f) : 0.0f;

        public float StandardDeviation => MathF.Sqrt(this.Variance);
    }
}