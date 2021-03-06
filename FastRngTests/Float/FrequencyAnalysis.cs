using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace FastRngTests.Float
{
    [ExcludeFromCodeCoverage]
    public sealed class FrequencyAnalysis
    {
        private readonly uint[] data;
        
        public FrequencyAnalysis(int samples = 100)
        {
            this.data = new uint[samples];
        }

        public void CountThis(float value)
        {
            var bucket = (int)MathF.Floor(value * this.data.Length);
            this.data[bucket]++;
        }

        public float[] GetNormalizedEvents()
        {
            var max = (float) this.data.Max();
            var result = new float[this.data.Length];
            for (var n = 0; n < this.data.Length; n++)
            {
                result[n] = this.data[n] / max;
            }

            return result;
        }

        private float[] Normalize()
        {
            var max = (float)this.data.Max();
            var result = new float[this.data.Length];
            for (var n = 0; n < this.data.Length; n++)
                result[n] = this.data[n] / max;

            return result;
        }

        public float[] NormalizeAndPlotEvents(Action<string> writer)
        {
            var result = this.Normalize();
            FrequencyAnalysis.Plot(result, writer, "Event Distribution");
            
            return result;
        }
        
        public void PlotOccurence(Action<string> writer)
        {
            var data = this.data.Select(n => n > 0f ? 1.0f : 0.0f).ToArray();
            FrequencyAnalysis.Plot(data, writer, "Occurrence Distribution");
        }

        private static void Plot(float[] data, Action<string> writer, string name)
        {
            const int HEIGHT = 16;
            
            var values = new float[data.Length];
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