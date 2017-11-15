using UnityEngine;

/* 
 * Functions taken from Tween.js - Licensed under the MIT license
 * at https://github.com/sole/tween.js
 */
public enum EaseMode
{
    LINEAR,
    QUADRATIC, QUADRATIC_IN, QUADRATIC_OUT,
    CUBIC, CUBIC_IN, CUBIC_OUT,
    QUARTIC, QUARTIC_IN, QUARTIC_OUT,
    QUINTIC, QUINTIC_IN, QUINTIC_OUT,
    SIN, SIN_IN, SIN_OUT,
    EXP, EXP_IN, EXP_OUT,
    CIRCULAR, CIRCULAR_IN, CIRCULAR_OUT,
    ELASTIC, ELASTIC_IN, ELASTIC_OUT,
    BACK, BACK_IN, BACK_OUT,
    BOUNCE, BOUNCE_IN, BOUNCE_OUT
}
public class Easing
{
    

    public static float Linear(float k)
    {
       
        return k;
    }

    public class Quadratic
    {
        public static float In(float k)
        {
            return k * k;
        }

        public static float Out(float k)
        {
            return k * (2f - k);
        }

        public static float InOut(float k)
        {
            if ((k *= 2f) < 1f) return 0.5f * k * k;
            return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
        }
    };

    public class Cubic
    {
        public static float In(float k)
        {
            return k * k * k;
        }

        public static float Out(float k)
        {
            return 1f + ((k -= 1f) * k * k);
        }

        public static float InOut(float k)
        {
            if ((k *= 2f) < 1f) return 0.5f * k * k * k;
            return 0.5f * ((k -= 2f) * k * k + 2f);
        }
    };

    public class Quartic
    {
        public static float In(float k)
        {
            return k * k * k * k;
        }

        public static float Out(float k)
        {
            return 1f - ((k -= 1f) * k * k * k);
        }

        public static float InOut(float k)
        {
            if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
            return -0.5f * ((k -= 2f) * k * k * k - 2f);
        }
    };

    public class Quintic
    {
        public static float In(float k)
        {
            return k * k * k * k * k;
        }

        public static float Out(float k)
        {
            return 1f + ((k -= 1f) * k * k * k * k);
        }

        public static float InOut(float k)
        {
            if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
            return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
        }
    };

    public class Sinusoidal
    {
        public static float In(float k)
        {
            return 1f - Mathf.Cos(k * Mathf.PI / 2f);
        }

        public static float Out(float k)
        {
            return Mathf.Sin(k * Mathf.PI / 2f);
        }

        public static float InOut(float k)
        {
            return 0.5f * (1f - Mathf.Cos(Mathf.PI * k));
        }
    };

    public class Exponential
    {
        public static float In(float k)
        {
            return k == 0f ? 0f : Mathf.Pow(1024f, k - 1f);
        }

        public static float Out(float k)
        {
            return k == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * k);
        }

        public static float InOut(float k)
        {
            if (k == 0f) return 0f;
            if (k == 1f) return 1f;
            if ((k *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, k - 1f);
            return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
        }
    };

    public class Circular
    {
        public static float In(float k)
        {
            return 1f - Mathf.Sqrt(1f - k * k);
        }

        public static float Out(float k)
        {
            return Mathf.Sqrt(1f - ((k -= 1f) * k));
        }

        public static float InOut(float k)
        {
            if ((k *= 2f) < 1f) return -0.5f * (Mathf.Sqrt(1f - k * k) - 1);
            return 0.5f * (Mathf.Sqrt(1f - (k -= 2f) * k) + 1f);
        }
    };

    public class Elastic
    {
        public static float In(float k)
        {
            if (k == 0) return 0;
            if (k == 1) return 1;
            return -Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
        }

        public static float Out(float k)
        {
            if (k == 0) return 0;
            if (k == 1) return 1;
            return Mathf.Pow(2f, -10f * k) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) + 1f;
        }

        public static float InOut(float k)
        {
            if ((k *= 2f) < 1f) return -0.5f * Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
            return Mathf.Pow(2f, -10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) * 0.5f + 1f;
        }
    };

    public class Back
    {
        static float s = 1.70158f;
        static float s2 = 2.5949095f;

        public static float In(float k)
        {
            return k * k * ((s + 1f) * k - s);
        }

        public static float Out(float k)
        {
            return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
        }

        public static float InOut(float k)
        {
            if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
            return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
        }
    };

    public class Bounce
    {
        public static float In(float k)
        {
            return 1f - Out(1f - k);
        }

        public static float Out(float k)
        {
            if (k < (1f / 2.75f))
            {
                return 7.5625f * k * k;
            }
            else if (k < (2f / 2.75f))
            {
                return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
            }
            else if (k < (2.5f / 2.75f))
            {
                return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
            }
            else
            {
                return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
            }
        }

        public static float InOut(float k)
        {
            if (k < 0.5f) return In(k * 2f) * 0.5f;
            return Out(k * 2f - 1f) * 0.5f + 0.5f;
        }
    };
    public static float Ease(EaseMode mode, float k)
    {
        switch(mode)
        {
            case EaseMode.LINEAR: return Linear(k);
            case EaseMode.QUADRATIC: return Quadratic.InOut(k);
            case EaseMode.QUADRATIC_IN: return Quadratic.In(k);
            case EaseMode.QUADRATIC_OUT: return Quadratic.Out(k);
            case EaseMode.CUBIC: return Cubic.InOut(k);
            case EaseMode.CUBIC_IN: return Cubic.In(k);
            case EaseMode.CUBIC_OUT: return Cubic.Out(k);
            case EaseMode.QUARTIC: return Quartic.InOut(k);
            case EaseMode.QUARTIC_IN: return Quartic.In(k);
            case EaseMode.QUARTIC_OUT: return Quartic.Out(k);
            case EaseMode.QUINTIC: return Quintic.InOut(k);
            case EaseMode.QUINTIC_IN: return Quintic.In(k);
            case EaseMode.QUINTIC_OUT: return Quintic.Out(k);
            case EaseMode.SIN: return Sinusoidal.InOut(k);
            case EaseMode.SIN_IN: return Sinusoidal.In(k);
            case EaseMode.SIN_OUT: return Sinusoidal.Out(k);
            case EaseMode.EXP: return Exponential.InOut(k);
            case EaseMode.EXP_IN: return Exponential.In(k);
            case EaseMode.EXP_OUT: return Exponential.Out(k);
            case EaseMode.CIRCULAR: return Circular.InOut(k);
            case EaseMode.CIRCULAR_IN: return Circular.In(k);
            case EaseMode.CIRCULAR_OUT: return Circular.Out(k);
            case EaseMode.ELASTIC: return Elastic.InOut(k);
            case EaseMode.ELASTIC_IN: return Elastic.In(k);
            case EaseMode.ELASTIC_OUT: return Elastic.Out(k);
            case EaseMode.BACK: return Back.InOut(k);
            case EaseMode.BACK_IN: return Back.In(k);
            case EaseMode.BACK_OUT: return Back.Out(k);
            case EaseMode.BOUNCE: return Bounce.InOut   (k);
            case EaseMode.BOUNCE_IN: return Back.In(k);
            case EaseMode.BOUNCE_OUT: return Back.Out(k);
            default: return k;
        }
    }
}