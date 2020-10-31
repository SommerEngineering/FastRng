using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Double.Distributions
{
    public abstract class Distribution : IDistribution
    {
        private ShapeFitter fitter;
        private IRandom random;
        
        public IRandom Random
        {
            get => this.random;
            set
            {
                if(this.random != null)
                    return;
                
                this.random = value;
                this.fitter = new ShapeFitter(this.ShapeFunction, this.random, 100);
            }
        }

        protected abstract double ShapeFunction(double x);
        
        public async ValueTask<double> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return double.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}