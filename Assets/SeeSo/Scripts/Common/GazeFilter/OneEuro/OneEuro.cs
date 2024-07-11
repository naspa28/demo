using System;

public class OneEuro
{
    private float freq, minCutoff, beta_, dCutoff;
    private LowPassFilter x, dx;
    private long lastTime;
    private static long UndefinedTime = -1;


    public OneEuro(float freq, float minCutoff, float beta_, float dCutoff)
    {
        init(freq, minCutoff, beta_, dCutoff);
    }

    private float alpha(float cutoff)
    {
        float te = 1.0F / freq;
        float tau = 1.0F / (2 * (float)Math.PI * cutoff);
        return 1.0F / (1.0F + tau / te);
    }

    private void setFrequency(float f) 
    {
        if (f > 0)
        {
            freq = f;
        }
        
    }

    private void setMinCutoff(float mc)
    {
        if (mc > 0)
        {
            minCutoff = mc;
        }
    }

    private void setBeta(float b)
    {
        beta_ = b;
    }

    private void setDerivateCutoff(float dc)
    {
        if (dc > 0)
        {
            dCutoff = dc;
        }
    }

    private void init(float _freq, float mincutoff, float _beta_, float dcutoff)
    {
        setFrequency(_freq);
        setMinCutoff(mincutoff);
        setBeta(_beta_);
        setDerivateCutoff(dcutoff);
        x = new LowPassFilter(alpha(mincutoff));
        dx = new LowPassFilter(alpha(dcutoff));
        lastTime = UndefinedTime;
    }

    public float filter(float value, long timestamp)
    {
        // update the sampling frequency based on timestamps
        if (lastTime != UndefinedTime && timestamp != UndefinedTime)
        {
            freq = 1000.0F / (timestamp - lastTime);
        }

        lastTime = timestamp;
        // estimate the current variation per second
        float dvalue = x.hasLastRawValue() ? (value - x.lastRawValue()) * freq : 0.0F;

        float edvalue = dx.filterWithAlpha(dvalue, alpha(dCutoff));
        // use it to update the cutoff frequency
        float cutoff = minCutoff + beta_ * Math.Abs(edvalue);
        // filter the given value
        return x.filterWithAlpha(value, alpha(cutoff));
    }

    private class LowPassFilter
    {

        float y, a, s;
        bool initialized;

        public LowPassFilter(float alpha)
        {
            init(alpha, 0);
        }

        public LowPassFilter(float alpha, float initval)
        {
            init(alpha, initval);
        }

        public float filter(float value)
        {
            float result;
            if (initialized)
            {
                result = a * value + (1.0F - a) * s;
            }
            else
            {
                result = value;
                initialized = true;
            }
            y = value;
            s = result;
            return result;
        }

        public void init(float alpha, float initval)
        {
            y = s = initval;
            setAlpha(alpha);
            initialized = false;
        }

        public float filterWithAlpha(float value, float alpha)
        {
            setAlpha(alpha);
            return filter(value);
        }

        public void setAlpha(float alpha)
        {
            if (alpha <= 0.0 || alpha > 1.0)
            {
                throw new Exception("alpha should be in (0.0., 1.0]");
            }
            a = alpha;
        }


        public bool hasLastRawValue()
        {
            return initialized;
        }

        public float lastRawValue()
        {
            return y;
        }
    }
}