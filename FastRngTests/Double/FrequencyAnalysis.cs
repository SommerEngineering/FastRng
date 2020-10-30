using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FastRngTests.Double
{
    public sealed class FrequencyAnalysis
    {
        private readonly uint[] data;
        
        public FrequencyAnalysis(int samples = 100)
        {
            this.data = new uint[samples];
        }

        public void CountThis(double value)
        {
            var bucket = (int)Math.Floor(value * this.data.Length);
            this.data[bucket]++;
        }

        public double[] GetNormalizedEvents()
        {
            var max = (double) this.data.Max();
            var result = new double[this.data.Length];
            for (var n = 0; n < this.data.Length; n++)
            {
                result[n] = this.data[n] / max;
            }

            return result;
        }

        private double[] Normalize()
        {
            var max = (double)this.data.Max();
            var result = new double[this.data.Length];
            for (var n = 0; n < this.data.Length; n++)
                result[n] = this.data[n] / max;

            return result;
        }

        public double[] NormalizeAndPlotEvents(Action<string> writer)
        {
            var result = this.Normalize();
            FrequencyAnalysis.Plot(result, writer, "Event Distribution");
            
            return result;
        }
        
        public void PlotOccurence(Action<string> writer)
        {
            var data = this.data.Select(n => n > 0 ? 1.0 : 0.0).ToArray();
            FrequencyAnalysis.Plot(data, writer, "Occurrence Distribution");
        }

        private static void Plot(double[] data, Action<string> writer, string name)
        {
            const int HEIGHT = 16;
            
            var values = new double[data.Length];
            for (var n = 0; n < data.Length; n++)
            {
                values[n] = data[n] * HEIGHT;
            }

            var sb = new StringBuilder();
            for (var line = HEIGHT; line > 0; line--)
            {
                for (var column = 0; column < data.Length; column++)
                    sb.Append(values[column] >= line ? '█' : '░');
                
                writer.Invoke(sb.ToString());
                sb.Clear();
            }
            
            writer.Invoke(name);
            writer.Invoke(string.Empty);
        }
    }
}