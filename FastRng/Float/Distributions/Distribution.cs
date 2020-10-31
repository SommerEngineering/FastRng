using System.Threading;
using System.Threading.Tasks;

namespace FastRng.Float.Distributions
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

        protected abstract float ShapeFunction(float x);
        
        public async ValueTask<float> GetDistributedValue(CancellationToken token = default)
        {
            if (this.Random == null)
                return float.NaN;
            
            return await this.fitter.NextNumber(token);
        }
    }
}