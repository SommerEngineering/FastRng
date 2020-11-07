using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FastRng.Double.Distributions;

namespace FastRng.Double
{
    /// <summary>
    /// A fast multi-threaded pseudo random number generator.
    /// </summary>
    /// <remarks>
    /// Please note, that Math.NET's (https://www.mathdotnet.com/) random number generator is in some situations faster.
    /// Unlike Math.NET, MultiThreadedRng is multi-threaded and async. Consumers can await the next number without
    /// blocking resources. Additionally, consumers can use a token to cancel e.g. timeout an operation as well.<br/><br/>
    ///
    /// MultiThreadedRng using a shape fitter (a rejection sampler) to enforce arbitrary shapes of probabilities for
    /// desired distributions. By using the shape fitter, it is even easy to define discontinuous, arbitrary functions
    /// as shapes. Any consumer can define and use own distributions.<br/><br/>
    /// 
    /// This class uses the George Marsaglia's MWC algorithm. The algorithm's implementation based loosely on John D.
    /// Cook's (johndcook.com) implementation (https://www.codeproject.com/Articles/25172/Simple-Random-Number-Generation).
    /// Thanks John for the inspiration.<br/><br/>
    ///
    /// Please notice: When using the debug environment, MultiThreadedRng uses a smaller buffer size. Please ensure,
    /// that the production environment uses a release build, though.
    /// </remarks>
    public sealed class MultiThreadedRng : IRandom, IDisposable
    {
        #if DEBUG
            private const int BUFFER_SIZE = 10_000;
        #else
            private const int BUFFER_SIZE = 1_000_000;
        #endif

        // The queue size means, how many buffer we store in a queue at the same time:
        private const int QUEUE_SIZE = 2;
        
        // Gets used to stop the producer threads:
        private readonly CancellationTokenSource producerTokenSource = new CancellationTokenSource();
        
        // The time a thread waits e.g. to check if the queue needs a new buffer:
        private readonly TimeSpan waiter = TimeSpan.FromMilliseconds(10);
        
        // The first queue, where to store buffers of random uint numbers:
        private readonly ConcurrentQueue<uint[]> queueIntegers = new ConcurrentQueue<uint[]>();
        
        // The second queue, where to store buffers of uniform random double numbers:
        private readonly ConcurrentQueue<double[]> queueDoubles = new ConcurrentQueue<double[]>();

        // The uint producer thread:
        private Thread producerRandomUint;
        
        // The uniform double producer thread:
        private Thread producerRandomUniformDistributedDouble;
        
        // Variable w and z for the uint generator. Both get used
        // as seeding variable as well (cf. constructors)
        private uint mW;
        private uint mZ;
        
        // This is the current buffer for the consumer side i.e. the public interfaces:
        private double[] currentBuffer = Array.Empty<double>();
        
        // The current pointer to the next current buffer's address to read from:
        private int currentBufferPointer = BUFFER_SIZE;

        #region Constructors

        public MultiThreadedRng()
        {
            //
            // Initialize the mW and mZ by using
            // the system's time.
            //
            var now = DateTime.Now;
            var ticks = now.Ticks;
            this.mW = (uint) (ticks >> 16);
            this.mZ = (uint) (ticks % 4_294_967_296);
            this.StartProducerThreads();
        }

        public MultiThreadedRng(uint seedU)
        {
            this.mW = seedU;
            this.mZ = 362_436_069;
            this.StartProducerThreads();
        }
        
        public MultiThreadedRng(uint seedU, uint seedV)
        {
            this.mW = seedU;
            this.mZ = seedV;
            this.StartProducerThreads();
        }

        private void StartProducerThreads()
        {
            this.producerRandomUint = new Thread(() => this.RandomProducerUint(this.producerTokenSource.Token)) {IsBackground = true};
            this.producerRandomUint.Start();
            this.producerRandomUniformDistributedDouble = new Thread(() => this.RandomProducerUniformDistributedDouble(this.producerTokenSource.Token)) {IsBackground = true};
            this.producerRandomUniformDistributedDouble.Start();
        }

        #endregion

        #region Producers

        [ExcludeFromCodeCoverage]
        private async void RandomProducerUint(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // A local next buffer, which gets filled next:
                    var nextBuffer = new uint[BUFFER_SIZE];
                    
                    // Produce the necessary number of random uints:
                    for (var n = 0; n < nextBuffer.Length && !cancellationToken.IsCancellationRequested; n++)
                    {
                        this.mZ = 36_969 * (this.mZ & 65_535) + (this.mZ >> 16);
                        this.mW = 18_000 * (this.mW & 65_535) + (this.mW >> 16);
                        nextBuffer[n] = (this.mZ << 16) + this.mW;
                    }

                    // Inside this loop, we try to enqueue the produced buffer:
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            // Ensure, that we do not produce more buffers, as configured: 
                            if (this.queueIntegers.Count < QUEUE_SIZE)
                            {
                                this.queueIntegers.Enqueue(nextBuffer);
                                break;
                            }

                            // The queue was full. Wait a moment and try it again:
                            await Task.Delay(this.waiter, cancellationToken);
                        }
                        catch (TaskCanceledException)
                        {
                            // The producers should be stopped:
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        [ExcludeFromCodeCoverage]
        private async void RandomProducerUniformDistributedDouble(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // A local source buffer of uints: 
                    uint[] bufferSource = null;
                    
                    // Try to get the next source buffer:
                    while (!this.queueIntegers.TryDequeue(out bufferSource) && !cancellationToken.IsCancellationRequested)
                        await Task.Delay(this.waiter, cancellationToken);

                    // Case: The producers should be stopped:
                    if(bufferSource == null)
                        return;
                    
                    // A local buffer to fill with uniform doubles:
                    var nextBuffer = new double[BUFFER_SIZE];
                    
                    // Generate the necessary number of doubles:
                    for (var n = 0; n < nextBuffer.Length && !cancellationToken.IsCancellationRequested; n++)
                        nextBuffer[n] = (bufferSource[n] + 1.0) * 2.328306435454494e-10;

                    // Inside this loop, we try to enqueue the generated buffer:
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            // Ensure, that the queue contains only the configured number of buffers:
                            if (this.queueDoubles.Count < QUEUE_SIZE)
                            {
                                this.queueDoubles.Enqueue(nextBuffer);
                                break;
                            }

                            // The queue was full. Wait a moment and try it again:
                            await Task.Delay(this.waiter, cancellationToken);
                        }
                        catch (TaskCanceledException)
                        {
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        #endregion

        #region Implementing interface

        public async ValueTask<double> GetUniform(CancellationToken cancel = default)
        {
            while (!cancel.IsCancellationRequested)
            {
                // Check, if we need a new buffer to read from:
                if (this.currentBufferPointer >= BUFFER_SIZE)
                {
                    // Create a local copy of the current buffer's pointer:
                    var currentBufferReference = this.currentBuffer;
                    
                    // Here, we store the next buffer until we implement it:
                    var nextBuffer = Array.Empty<double>();
                    
                    // Try to get the next buffer from the queue:
                    while (this.currentBufferPointer >= BUFFER_SIZE && currentBufferReference == this.currentBuffer && !this.queueDoubles.TryDequeue(out nextBuffer))
                    {
                        //
                        // Case: There is no next buffer available.
                        // Must wait for producer(s) to provide next.
                        //
                        try
                        {
                            await Task.Delay(this.waiter, cancel);
                        }
                        catch (TaskCanceledException)
                        {
                            //
                            // Case: The consumer cancelled the request.
                            //
                            return double.NaN;
                        }
                    }
                
                    //
                    // Note: In general, it does not matter if the following compare-exchange is successful.
                    // 1st case: It was successful -- everything is fine. But we are responsible to re-set the currentBufferPointer.
                    // 2nd case: It was not successful. This means, that another thread was successful, though.
                    //           That case is fine as well. But we would loose one buffer of work. Thus, we
                    //           check for this case and preserve the buffer full of work.
                    //
                    
                    // Try to implement the dequeued buffer without locking other threads:
                    if (Interlocked.CompareExchange(ref this.currentBuffer, nextBuffer, currentBufferReference) != currentBufferReference)
                    {
                        //
                        // Case: Another thread updated the buffer already.
                        // Thus, we enqueue our copy of the next buffer to preserve it.
                        //
                        this.queueDoubles.Enqueue(nextBuffer);
                        
                        // Next? We can go ahead and yield a random number...
                    }
                    else
                    {
                        //
                        // Case: We updated the buffer.
                        //
                        this.currentBufferPointer = 0;
                        
                        // Next? We can go ahead and yield a random number...
                    }
                }

                // Made a local copy of the current pointer:
                var myPointer = this.currentBufferPointer;
                
                // Increment the pointer for the next thread or call:
                var nextPointer = myPointer + 1;
                
                // Try to update the pointer without locking other threads:
                if (Interlocked.CompareExchange(ref this.currentBufferPointer, nextPointer, myPointer) == myPointer)
                {
                    //
                    // Case: Success. We updated the pointer and, thus, can use the pointer to read a number.
                    //
                    return this.currentBuffer[myPointer];
                }
                
                //
                // Case: Another thread updated the pointer already. Must restart the process
                // to get a random number.
                //
            }

            //
            // Case: The consumer cancelled the request.
            //
            return double.NaN;
        }
        
        private void StopProducer() => this.producerTokenSource.Cancel();

        public void Dispose() => this.StopProducer();

        #endregion
    }
}