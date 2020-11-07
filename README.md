# FastRng

FastRng is a multi-threaded pseudo-random number generator. Besides the generation of uniformly distributed random numbers, there are several other distributions to choose from. For performance reasons the parameters of the distributions are not user-definable. For some distributions, therefore, different parameter variations are available. If a different combination is desired, a separate class can be created.

Please note, that Math.NET's (https://www.mathdotnet.com/) random number generator is in some situations faster. Unlike Math.NET, MultiThreadedRng is multi-threaded and async. Consumers can await the next number without blocking resources. Additionally, consumers can use a token to cancel e.g. timeout an operation as well.

FastRng (class `MultiThreadedRng`) using a shape fitter (a rejection sampler) to enforce arbitrary shapes of probabilities for desired distributions. By using the shape fitter, it is even easy to define discontinuous, arbitrary functions as shapes. Any consumer can define and use own distributions.

The class `MultiThreadedRng` uses the George Marsaglia's MWC algorithm. The algorithm's implementation based loosely on John D. Cook's (johndcook.com) [implementation](https://www.codeproject.com/Articles/25172/Simple-Random-Number-Generation). Thanks John for the inspiration.

Please notice: When using the debug environment, MultiThreadedRng uses a smaller buffer size. Please ensure, that the production environment uses a release build, though.

## Usage
Example code:

```
using FastRng.Float;
using FastRng.Float.Distributions;

[...]

using var rng = new MultiThreadedRng();
var dist = new ChiSquareK1(rng);

var value1 = await dist.NextNumber();
var value2 = await dist.NextNumber(rangeStart: -1.0f, rangeEnd: 1.0f);
if(await dist.HasDecisionBeenMade(above: 0.8f, below: 0.9f))
{
    // Decision has been made
}
```

Notes:
- `MultiThreadedRng` and all distributions are available as `float` and `double` variations. Both are supporting `uint` and `ulong` as well.

- `MultiThreadedRng` is `IDisposable`. It is important to call `Dispose`, when the generator is not needed anymore. Otherwise, the supporting background threads are still running.

- `MultiThreadedRng` fills some buffers after creation. Thus, create and reuse it as long as needed. Avoid useless re-creation.

- Distributions need some time at creation to calculate probabilities. Thus, just create a distribution once and use reuse it. Avoid useless re-creation.

## Available Distributions

### Normal Distribution (std. dev.=0.2, mean=0.5)
![](images/normal.png)

Wikipedia: https://en.wikipedia.org/wiki/Normal_distribution


### Beta Distribution (alpha=2, beta=2)
![](images/beta-a2b2.png)

Wikipedia: https://en.wikipedia.org/wiki/Beta_distribution


### Beta Distribution (alpha=2, beta=5)
![](images/beta-a2b5.png)

Wikipedia: https://en.wikipedia.org/wiki/Beta_distribution


### Beta Distribution (alpha=5, beta=2)
![](images/beta-a5b2.png)

Wikipedia: https://en.wikipedia.org/wiki/Beta_distribution


### Cauchy / Lorentz Distribution (x0=0)
![](images/cauchy-lorentz-x0.png)

Wikipedia: https://en.wikipedia.org/wiki/Cauchy_distribution


### Cauchy / Lorentz Distribution (x0=1)
![](images/cauchy-lorentz-x1.png)

Wikipedia: https://en.wikipedia.org/wiki/Cauchy_distribution


### Chi-Square Distribution (k=1)
![](images/chi-square-k1.png)

Wikipedia: https://en.wikipedia.org/wiki/Chi-square_distribution


### Chi-Square Distribution (k=4)
![](images/chi-square-k4.png)

Wikipedia: https://en.wikipedia.org/wiki/Chi-square_distribution


### Chi-Square Distribution (k=10)
![](images/chi-square-k10.png)

Wikipedia: https://en.wikipedia.org/wiki/Chi-square_distribution


### Exponential Distribution (lambda=5)
![](images/exponential-la5.png)

Wikipedia: https://en.wikipedia.org/wiki/Exponential_distribution


### Exponential Distribution (lambda=10)
![](images/exponential-la10.png)

Wikipedia: https://en.wikipedia.org/wiki/Exponential_distribution


### Inverse Exponential Distribution (lambda=5)
![](images/inverse-exponential-la5.png)

Wikipedia: https://en.wikipedia.org/wiki/Inverse_distribution#Inverse_exponential_distribution


### Inverse Exponential Distribution (lambda=10)
![](images/inverse-exponential-la10.png)

Wikipedia: https://en.wikipedia.org/wiki/Inverse_distribution#Inverse_exponential_distribution


### Gamma Distribution (alpha=5, beta=15)
![](images/gamma-a5b15.png)

Wikipedia: https://en.wikipedia.org/wiki/Gamma_distribution


### Inverse Gamma Distribution (alpha=3, beta=0.5)
![](images/inverse-gamma-a3b05.png)

Wikipedia: https://en.wikipedia.org/wiki/Inverse-gamma_distribution


### Laplace Distribution (b=0.1, mu=0)
![](images/laplace-b01m0.png)

Wikipedia: https://en.wikipedia.org/wiki/Laplace_distribution


### Laplace Distribution (b=0.1, mu=0.5)
![](images/laplace-b01m05.png)

Wikipedia: https://en.wikipedia.org/wiki/Laplace_distribution


### Log-Normal Distribution (sigma=1, mu=0)
![](images/log-normal-s1m0.png)

Wikipedia: https://en.wikipedia.org/wiki/Log-normal_distribution


### StudentT Distribution (nu=1)
![](images/student-t-nu1.png)

Wikipedia: https://en.wikipedia.org/wiki/Student%27s_t-distribution


### Weibull Distribution (k=0.5, lambda=1)
![](images/weibull-k05la1.png)

Wikipedia: https://en.wikipedia.org/wiki/Weibull_distribution