/*
 * Created by C.J. Kimberlin
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2019
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * 
 * TERMS OF USE - EASING EQUATIONS
 * Open source under the BSD License.
 * Copyright (c)2001 Robert Penner
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE 
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 *
 * ============= Description =============
 *
 * Below is an example of how to use the easing functions in the file. There is a getting function that will return the function
 * from an enum. This is useful since the enum can be exposed in the editor and then the function queried during Start().
 * 
 * EasingFunction.Ease ease = EasingFunction.Ease.EaseInOutQuad;
 * EasingFunction.EasingFunc func = GetEasingFunction(ease;
 * 
 * float time = func(0, 10, 0.67f);
 * 
 * EasingFunction.EasingFunc derivativeFunc = GetEasingFunctionDerivative(ease);
 * 
 * float derivativeValue = derivativeFunc(0, 10, 0.67f);
 */

using System;
using UnityEngine;

namespace RichPackage.Easing
{
	public static class EasingFunction
	{		
		public enum Ease
		{
			EaseInQuad = 0,
			EaseOutQuad,
			EaseInOutQuad,
			EaseInCubic,
			EaseOutCubic,
			EaseInOutCubic,
			EaseInQuart,
			EaseOutQuart,
			EaseInOutQuart,
			EaseInQuint,
			EaseOutQuint,
			EaseInOutQuint,
			EaseInSine,
			EaseOutSine,
			EaseInOutSine,
			EaseInExpo,
			EaseOutExpo,
			EaseInOutExpo,
			EaseInCirc,
			EaseOutCirc,
			EaseInOutCirc,
			Linear,
			Spring,
			EaseInBounce,
			EaseOutBounce,
			EaseInOutBounce,
			EaseInBack,
			EaseOutBack,
			EaseInOutBack,
			EaseInElastic,
			EaseOutElastic,
			EaseInOutElastic,
		}

		private const float NaturalLogOf2 = 0.693147181f;
		const float s = 1.70158f;

		//
		// Easing functions
		//

		public static float Linear(float start, float end, float time)
		{
			return (1 - time) * start + time * end;
		}

		public static float Spring(float start, float end, float time)
		{
			time = Mathf.Clamp01(time);
			time = (Mathf.Sin(time * Mathf.PI * (0.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f) + time) * (1f + (1.2f * (1f - time)));
			return start + (end - start) * time;
		}

		public static float EaseInQuad(float start, float end, float time)
		{
			end -= start;
			return end * time * time + start;
		}

		public static float EaseOutQuad(float start, float end, float time)
		{
			end -= start;
			return -end * time * (time - 2) + start;
		}

		public static float EaseInOutQuad(float start, float end, float time)
		{
			//time /= .5f;
			time *= 2f; //prefer mul over div
			end -= start;
			if (time < 1) return end * 0.5f * time * time + start;
			time--;
			return -end * 0.5f * (time * (time - 2) - 1) + start;
		}

		public static float EaseInCubic(float start, float end, float time)
		{
			end -= start;
			return end * time * time * time + start;
		}

		public static float EaseOutCubic(float start, float end, float time)
		{
			time--;
			end -= start;
			return end * (time * time * time + 1) + start;
		}

		public static float EaseInOutCubic(float start, float end, float time)
		{
			//time /= .5f;
			time *= 2f; //prefer mul over div
			end -= start;
			if (time < 1) return end * 0.5f * time * time * time + start;
			time -= 2;
			return end * 0.5f * (time * time * time + 2) + start;
		}

		public static float EaseInQuart(float start, float end, float time)
		{
			end -= start;
			return end * time * time * time * time + start;
		}

		public static float EaseOutQuart(float start, float end, float time)
		{
			time--;
			end -= start;
			return -end * (time * time * time * time - 1) + start;
		}

		public static float EaseInOutQuart(float start, float end, float time)
		{
			//time /= .5f;
			time *= 2f; //prefer mul over div
			end -= start;
			if (time < 1) return end * 0.5f * time * time * time * time + start;
			time -= 2;
			return -end * 0.5f * (time * time * time * time - 2) + start;
		}

		public static float EaseInQuint(float start, float end, float time)
		{
			end -= start;
			return end * time * time * time * time * time + start;
		}

		public static float EaseOutQuint(float start, float end, float time)
		{
			time--;
			end -= start;
			return end * (time * time * time * time * time + 1) + start;
		}

		public static float EaseInOutQuint(float start, float end, float time)
		{
			//time /= .5f;
			time *= 2f; //prefer mul over div
			end -= start;
			if (time < 1) return end * 0.5f * time * time * time * time * time + start;
			time -= 2;
			return end * 0.5f * (time * time * time * time * time + 2) + start;
		}

		public static float EaseInSine(float start, float end, float time)
		{
			end -= start;
			return -end * Mathf.Cos(time * (Mathf.PI * 0.5f)) + end + start;
		}

		public static float EaseOutSine(float start, float end, float time)
		{
			end -= start;
			return end * Mathf.Sin(time * (Mathf.PI * 0.5f)) + start;
		}

		public static float EaseInOutSine(float start, float end, float time)
		{
			end -= start;
			return -end * 0.5f * (Mathf.Cos(Mathf.PI * time) - 1) + start;
		}

		public static float EaseInExpo(float start, float end, float time)
		{
			end -= start;
			return end * Mathf.Pow(2, 10 * (time - 1)) + start;
		}

		public static float EaseOutExpo(float start, float end, float time)
		{
			end -= start;
			return end * (-Mathf.Pow(2, -10 * time) + 1) + start;
		}

		public static float EaseInOutExpo(float start, float end, float time)
		{
			//time /= .5f;
			time *= 2f; //prefer mul over div
			end -= start;
			if (time < 1) return end * 0.5f * Mathf.Pow(2, 10 * (time - 1)) + start;
			time--;
			return end * 0.5f * (-Mathf.Pow(2, -10 * time) + 2) + start;
		}

		public static float EaseInCirc(float start, float end, float time)
		{
			end -= start;
			return -end * (Mathf.Sqrt(1 - time * time) - 1) + start;
		}

		public static float EaseOutCirc(float start, float end, float time)
		{
			time--;
			end -= start;
			return end * Mathf.Sqrt(1 - time * time) + start;
		}

		public static float EaseInOutCirc(float start, float end, float time)
		{
			//time /= .5f;
			time *= 2f; //prefer mul over div
			end -= start;
			if (time < 1) return -end * 0.5f * (Mathf.Sqrt(1 - time * time) - 1) + start;
			time -= 2;
			return end * 0.5f * (Mathf.Sqrt(1 - time * time) + 1) + start;
		}

		public static float EaseInBounce(float start, float end, float time)
		{
			end -= start;
			const float d = 1f;
			return end - EaseOutBounce(0, end, d - time) + start;
		}

		public static float EaseOutBounce(float start, float end, float time)
		{
			//time /= .5f;
			time *= 2f; //prefer mul over div
			end -= start;
			if (time < (1 / 2.75f))
			{
				return end * (7.5625f * time * time) + start;
			}

			if (time < (2 / 2.75f))
			{
				time -= (1.5f / 2.75f);
				return end * (7.5625f * (time) * time + .75f) + start;
			}
			if (time < (2.5 / 2.75))
			{
				time -= (2.25f / 2.75f);
				return end * (7.5625f * (time) * time + .9375f) + start;
			}
			time -= (2.625f / 2.75f);
			return end * (7.5625f * (time) * time + .984375f) + start;
		}

		public static float EaseInOutBounce(float start, float end, float time)
		{
			end -= start;
			const float d = 1f;
			if (time < d * 0.5f) return EaseInBounce(0, end, time * 2) * 0.5f + start;
			return EaseOutBounce(0, end, time * 2 - d) * 0.5f + end * 0.5f + start;
		}

		public static float EaseInBack(float start, float end, float time)
		{
			end -= start;
			time /= 1; //??
			return end * (time) * time * ((s + 1) * time - s) + start;
		}

		public static float EaseOutBack(float start, float end, float time)
		{
			end -= start;
			time -= 1;
			return end * ((time) * time * ((s + 1) * time + s) + 1) + start;
		}

		public static float EaseInOutBack(float start, float end, float time)
		{
			float s = 1.70158f;
			end -= start;
			time /= .5f;
			if ((time) < 1)
			{
				s *= (1.525f);
				return end * 0.5f * (time * time * (((s) + 1) * time - s)) + start;
			}
			time -= 2;
			s *= (1.525f);
			return end * 0.5f * ((time) * time * (((s) + 1) * time + s) + 2) + start;
		}

		public static float EaseInElastic(float start, float end, float time)
		{
			end -= start;

			const float d = 1f;
			const float p = d * .3f;
			float s;
			float a = 0;

			if (Math.Abs(time) < float.Epsilon) return start;

			if (Math.Abs((time /= d) - 1) < float.Epsilon) return start + end;

			if (Math.Abs(a) < float.Epsilon || a < Mathf.Abs(end))
			{
				a = end;
				s = p / 4;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			return -(a * Mathf.Pow(2, 10 * (time -= 1)) * Mathf.Sin((time * d - s) * (2 * Mathf.PI) / p)) + start;
		}

		public static float EaseOutElastic(float start, float end, float time)
		{
			end -= start;

			const float d = 1f;
			const float p = d * .3f;
			float s;
			float a = 0;

			if (Math.Abs(time) < float.Epsilon) return start;

			if (Math.Abs((time /= d) - 1) < float.Epsilon) return start + end;

			if (Math.Abs(a) < float.Epsilon || a < Mathf.Abs(end))
			{
				a = end;
				s = p * 0.25f;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			return (a * Mathf.Pow(2, -10 * time) * Mathf.Sin((time * d - s) * (2 * Mathf.PI) / p) + end + start);
		}

		public static float EaseInOutElastic(float start, float end, float time)
		{
			end -= start;

			const float d = 1f;
			const float p = d * .3f;
			float s;
			float a = 0;

			if (Math.Abs(time) < float.Epsilon) return start;

			if (Math.Abs((time /= d * 0.5f) - 2) < float.Epsilon) return start + end;

			if (Math.Abs(a) < float.Epsilon || a < Mathf.Abs(end))
			{
				a = end;
				s = p / 4;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			if (time < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (time -= 1)) * Mathf.Sin((time * d - s) * (2 * Mathf.PI) / p)) + start;
			return a * Mathf.Pow(2, -10 * (time -= 1)) * Mathf.Sin((time * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
		}

		//
		// These are derived functions that the motor can use to get the speed at a specific time.
		//
		// The easing functions all work with a normalized time (0 to 1) and the returned time here
		// reflects that. Values returned here should be divided by the actual time.
		//
		// TODO: These functions have not had the testing they deserve. If there is odd behavior around
		//       dash speeds then this would be the first place I'd look.

		public static float LinearD(float start, float end, float time)
		{
			return end - start;
		}

		public static float EaseInQuadD(float start, float end, float time)
		{
			return 2f * (end - start) * time;
		}

		public static float EaseOutQuadD(float start, float end, float time)
		{
			end -= start;
			return -end * time - end * (time - 2);
		}

		public static float EaseInOutQuadD(float start, float end, float time)
		{
			time /= .5f;
			end -= start;

			if (time < 1)
			{
				return end * time;
			}

			time--;

			return end * (1 - time);
		}

		public static float EaseInCubicD(float start, float end, float time)
		{
			return 3f * (end - start) * time * time;
		}

		public static float EaseOutCubicD(float start, float end, float time)
		{
			time--;
			end -= start;
			return 3f * end * time * time;
		}

		public static float EaseInOutCubicD(float start, float end, float time)
		{
			time /= .5f;
			end -= start;

			if (time < 1)
			{
				return (3f / 2f) * end * time * time;
			}

			time -= 2;

			return (3f / 2f) * end * time * time;
		}

		public static float EaseInQuartD(float start, float end, float time)
		{
			return 4f * (end - start) * time * time * time;
		}

		public static float EaseOutQuartD(float start, float end, float time)
		{
			time--;
			end -= start;
			return -4f * end * time * time * time;
		}

		public static float EaseInOutQuartD(float start, float end, float time)
		{
			time /= .5f;
			end -= start;

			if (time < 1)
			{
				return 2f * end * time * time * time;
			}

			time -= 2;

			return -2f * end * time * time * time;
		}

		public static float EaseInQuintD(float start, float end, float time)
		{
			return 5f * (end - start) * time * time * time * time;
		}

		public static float EaseOutQuintD(float start, float end, float time)
		{
			time--;
			end -= start;
			return 5f * end * time * time * time * time;
		}

		public static float EaseInOutQuintD(float start, float end, float time)
		{
			time /= .5f;
			end -= start;

			if (time < 1)
			{
				return (5f / 2f) * end * time * time * time * time;
			}

			time -= 2;

			return (5f / 2f) * end * time * time * time * time;
		}

		public static float EaseInSineD(float start, float end, float time)
		{
			return (end - start) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * time);
		}

		public static float EaseOutSineD(float start, float end, float time)
		{
			end -= start;
			return (Mathf.PI * 0.5f) * end * Mathf.Cos(time * (Mathf.PI * 0.5f));
		}

		public static float EaseInOutSineD(float start, float end, float time)
		{
			end -= start;
			return end * 0.5f * Mathf.PI * Mathf.Sin(Mathf.PI * time);
		}
		
		public static float EaseInExpoD(float start, float end, float time)
		{
			return 10f * NaturalLogOf2 * (end - start) * Mathf.Pow(2f, 10f * (time - 1));
		}

		public static float EaseOutExpoD(float start, float end, float time)
		{
			end -= start;
			return 5f * NaturalLogOf2 * end * Mathf.Pow(2f, 1f - 10f * time);
		}

		public static float EaseInOutExpoD(float start, float end, float time)
		{
			time /= .5f;
			end -= start;

			if (time < 1)
			{
				return 5f * NaturalLogOf2 * end * Mathf.Pow(2f, 10f * (time - 1));
			}

			time--;

			return (5f * NaturalLogOf2 * end) / (Mathf.Pow(2f, 10f * time));
		}

		public static float EaseInCircD(float start, float end, float time)
		{
			return (end - start) * time / Mathf.Sqrt(1f - time * time);
		}

		public static float EaseOutCircD(float start, float end, float time)
		{
			time--;
			end -= start;
			return (-end * time) / Mathf.Sqrt(1f - time * time);
		}

		public static float EaseInOutCircD(float start, float end, float time)
		{
			time /= .5f;
			end -= start;

			if (time < 1)
			{
				return (end * time) / (2f * Mathf.Sqrt(1f - time * time));
			}

			time -= 2;

			return (-end * time) / (2f * Mathf.Sqrt(1f - time * time));
		}

		public static float EaseInBounceD(float start, float end, float time)
		{
			end -= start;
			const float d = 1f;

			return EaseOutBounceD(0, end, d - time);
		}

		public static float EaseOutBounceD(float start, float end, float time)
		{
			time /= 1f;
			end -= start;

			if (time < (1 / 2.75f))
			{
				return 2f * end * 7.5625f * time;
			}

			if (time < (2 / 2.75f))
			{
				time -= (1.5f / 2.75f);
				return 2f * end * 7.5625f * time;
			}
			if (time < (2.5 / 2.75))
			{
				time -= (2.25f / 2.75f);
				return 2f * end * 7.5625f * time;
			}
			time -= (2.625f / 2.75f);
			return 2f * end * 7.5625f * time;
		}

		public static float EaseInOutBounceD(float start, float end, float time)
		{
			end -= start;
			const float d = 1f;

			if (time < d * 0.5f)
			{
				return EaseInBounceD(0, end, time * 2) * 0.5f;
			}

			return EaseOutBounceD(0, end, time * 2 - d) * 0.5f;
		}

		public static float EaseInBackD(float start, float end, float time)
		{
			const float s = 1.70158f;

			return 3f * (s + 1f) * (end - start) * time * time - 2f * s * (end - start) * time;
		}

		public static float EaseOutBackD(float start, float end, float time)
		{
			const float s = 1.70158f;
			end -= start;
			time -= 1;

			return end * ((s + 1f) * time * time + 2f * time * ((s + 1f) * time + s));
		}

		public static float EaseInOutBackD(float start, float end, float time)
		{
			float s = 1.70158f;
			end -= start;
			time /= .5f;

			if ((time) < 1)
			{
				s *= (1.525f);
				return 0.5f * end * (s + 1) * time * time + end * time * ((s + 1f) * time - s);
			}

			time -= 2;
			s *= (1.525f);
			return 0.5f * end * ((s + 1) * time * time + 2f * time * ((s + 1f) * time + s));
		}

		public static float EaseInElasticD(float start, float end, float time)
		{
			return EaseOutElasticD(start, end, 1f - time);
		}

		public static float EaseOutElasticD(float start, float end, float time)
		{
			end -= start;

			const float d = 1f;
			const float p = d * .3f;
			float s;
			float a = 0;

			if (Math.Abs(a) < float.Epsilon || a < Mathf.Abs(end))
			{
				a = end;
				s = p * 0.25f;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			return (a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * time) *
				Mathf.Cos((2f * Mathf.PI * (d * time - s)) / p)) / p - 5f * NaturalLogOf2 * a *
				Mathf.Pow(2f, 1f - 10f * time) * Mathf.Sin((2f * Mathf.PI * (d * time - s)) / p);
		}

		public static float EaseInOutElasticD(float start, float end, float time)
		{
			end -= start;

			const float d = 1f;
			const float p = d * .3f;
			float s;
			float a = 0;

			if (Math.Abs(a) < float.Epsilon || a < Mathf.Abs(end))
			{
				a = end;
				s = p / 4;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
			}

			if (time < 1)
			{
				time -= 1;

				return -5f * NaturalLogOf2 * a * Mathf.Pow(2f, 10f * time) * Mathf.Sin(2 * Mathf.PI * (d * time - 2f) / p) -
					a * Mathf.PI * d * Mathf.Pow(2f, 10f * time) * Mathf.Cos(2 * Mathf.PI * (d * time - s) / p) / p;
			}

			time -= 1;

			return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * time - s) / p) / (p * Mathf.Pow(2f, 10f * time)) -
				5f * NaturalLogOf2 * a * Mathf.Sin(2f * Mathf.PI * (d * time - s) / p) / (Mathf.Pow(2f, 10f * time));
		}

		public static float SpringD(float start, float end, float time)
		{
			time = Mathf.Clamp01(time);
			end -= start;

			// Damn... Thanks http://www.derivative-calculator.net/
			// TODO: And it's a little bit wrong
			return end * (6f * (1f - time) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - time, 1.2f) *
				Mathf.Sin(Mathf.PI * time * (2.5f * time * time * time + 0.2f)) + Mathf.Pow(1f - time, 2.2f) *
				(Mathf.PI * (2.5f * time * time * time + 0.2f) + 7.5f * Mathf.PI * time * time * time) *
				Mathf.Cos(Mathf.PI * time * (2.5f * time * time * time + 0.2f)) + 1f) -
				6f * end * (Mathf.Pow(1 - time, 2.2f) * Mathf.Sin(Mathf.PI * time * (2.5f * time * time * time + 0.2f)) + time
				/ 5f);

		}

		public delegate float Function(float s, float e, float v);

		/// <summary>
		/// Returns the function associated to the easingFunction enum. This time returned should be cached as it allocates memory
		/// to return.
		/// </summary>
		/// <param name="easingFunction">The enum associated with the easing function.</param>
		/// <returns>The easing function</returns>
		public static Function GetEasingFunction(Ease easingFunction)
		{
			switch (easingFunction)
			{
				case Ease.EaseInQuad:
					return EaseInQuad;
				case Ease.EaseOutQuad:
					return EaseOutQuad;
				case Ease.EaseInOutQuad:
					return EaseInOutQuad;
				case Ease.EaseInCubic:
					return EaseInCubic;
				case Ease.EaseOutCubic:
					return EaseOutCubic;
				case Ease.EaseInOutCubic:
					return EaseInOutCubic;
				case Ease.EaseInQuart:
					return EaseInQuart;
				case Ease.EaseOutQuart:
					return EaseOutQuart;
				case Ease.EaseInOutQuart:
					return EaseInOutQuart;
				case Ease.EaseInQuint:
					return EaseInQuint;
				case Ease.EaseOutQuint:
					return EaseOutQuint;
				case Ease.EaseInOutQuint:
					return EaseInOutQuint;
				case Ease.EaseInSine:
					return EaseInSine;
				case Ease.EaseOutSine:
					return EaseOutSine;
				case Ease.EaseInOutSine:
					return EaseInOutSine;
				case Ease.EaseInExpo:
					return EaseInExpo;
				case Ease.EaseOutExpo:
					return EaseOutExpo;
				case Ease.EaseInOutExpo:
					return EaseInOutExpo;
				case Ease.EaseInCirc:
					return EaseInCirc;
				case Ease.EaseOutCirc:
					return EaseOutCirc;
				case Ease.EaseInOutCirc:
					return EaseInOutCirc;
				case Ease.Linear:
					return Linear;
				case Ease.Spring:
					return Spring;
				case Ease.EaseInBounce:
					return EaseInBounce;
				case Ease.EaseOutBounce:
					return EaseOutBounce;
				case Ease.EaseInOutBounce:
					return EaseInOutBounce;
				case Ease.EaseInBack:
					return EaseInBack;
				case Ease.EaseOutBack:
					return EaseOutBack;
				case Ease.EaseInOutBack:
					return EaseInOutBack;
				case Ease.EaseInElastic:
					return EaseInElastic;
				case Ease.EaseOutElastic:
					return EaseOutElastic;
				case Ease.EaseInOutElastic:
					return EaseInOutElastic;
				default:
					return null;
			}
		}

		/// <summary>
		/// Gets the derivative function of the appropriate easing function. If you use an easing function for position then this
		/// function can get you the speed at a given time (normalized).
		/// </summary>
		/// <param name="easingFunction"></param>
		/// <returns>The derivative function</returns>
		public static Function GetEasingFunctionDerivative(Ease easingFunction)
		{
			switch (easingFunction)
			{
				case Ease.EaseInQuad:
					return EaseInQuadD;
				case Ease.EaseOutQuad:
					return EaseOutQuadD;
				case Ease.EaseInOutQuad:
					return EaseInOutQuadD;
				case Ease.EaseInCubic:
					return EaseInCubicD;
				case Ease.EaseOutCubic:
					return EaseOutCubicD;
				case Ease.EaseInOutCubic:
					return EaseInOutCubicD;
				case Ease.EaseInQuart:
					return EaseInQuartD;
				case Ease.EaseOutQuart:
					return EaseOutQuartD;
				case Ease.EaseInOutQuart:
					return EaseInOutQuartD;
				case Ease.EaseInQuint:
					return EaseInQuintD;
				case Ease.EaseOutQuint:
					return EaseOutQuintD;
				case Ease.EaseInOutQuint:
					return EaseInOutQuintD;
				case Ease.EaseInSine:
					return EaseInSineD;
				case Ease.EaseOutSine:
					return EaseOutSineD;
				case Ease.EaseInOutSine:
					return EaseInOutSineD;
				case Ease.EaseInExpo:
					return EaseInExpoD;
				case Ease.EaseOutExpo:
					return EaseOutExpoD;
				case Ease.EaseInOutExpo:
					return EaseInOutExpoD;
				case Ease.EaseInCirc:
					return EaseInCircD;
				case Ease.EaseOutCirc:
					return EaseOutCircD;
				case Ease.EaseInOutCirc:
					return EaseInOutCircD;
				case Ease.Linear:
					return LinearD;
				case Ease.Spring:
					return SpringD;
				case Ease.EaseInBounce:
					return EaseInBounceD;
				case Ease.EaseOutBounce:
					return EaseOutBounceD;
				case Ease.EaseInOutBounce:
					return EaseInOutBounceD;
				case Ease.EaseInBack:
					return EaseInBackD;
				case Ease.EaseOutBack:
					return EaseOutBackD;
				case Ease.EaseInOutBack:
					return EaseInOutBackD;
				case Ease.EaseInElastic:
					return EaseInElasticD;
				case Ease.EaseOutElastic:
					return EaseOutElasticD;
				case Ease.EaseInOutElastic:
					return EaseInOutElasticD;
				default:
					return null;
			}
		}
	}
}
