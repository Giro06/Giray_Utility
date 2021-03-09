using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EaseLib
{
    public static float EaseInSine(float value)
    {
        return 1 - Mathf.Cos((value * Mathf.PI) / 2);
    }

    public static float EaseInCubic(float value)
    {
        return value * value * value;
    }

    public static float EaseInQuint(float value)
    {
        return value * value * value * value;
    }

    public static float EaseInCirc(float value)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(value, 2));
    }

    public static float EaseInElastic(float value)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return value == 0 ? 0 : value == 1 ? 1 : -Mathf.Pow(2, 10 * value - 10) * Mathf.Sin((value * 10 - 10.75f) * c4);
    }

    public static float EaseOutSine(float value)
    {
        return Mathf.Sin((value * Mathf.PI) / 2);
    }

    public static float EaseOutCubic(float value)
    {
        return 1 - Mathf.Pow(1 - value, 3);
    }

    public static float EaseOutQuint(float value)
    {
        return 1 - Mathf.Pow(1 - value, 5);
    }

    public static float EaseOutCirc(float value)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(value - 1, 2));
    }

    public static float EaseOutElastic(float value)
    {
        float c4 = (2 * Mathf.PI) / 3;

        return value == 0 ? 0 : value == 1 ? 1 : Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 10 - 0.75f) * c4) + 1;
    }

    public static float EaseInOutSine(float value)
    {
        return -(Mathf.Cos(Mathf.PI * value) - 1) / 2;
    }

    public static float EaseInOutCubic(float value)
    {
        return value < 0.5 ? 4 * value * value * value : 1 - Mathf.Pow(-2 * value + 2, 3) / 2;
    }

    public static float EaseInOutQuint(float value)
    {
        return value < 0.5 ? 16 * value * value * value * value * value : 1 - Mathf.Pow(-2 * value + 2, 5) / 2;
    }

    public static float EaseInOutCirc(float value)
    {
        return value < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * value, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * value + 2, 2)) + 1) / 2;
    }

    public static float EaseInOutElastic(float value)
    {
        float c5 = (2 * Mathf.PI) / 4.5f;

        return value == 0 ? 0 : value == 1 ? 1 : value < 0.5f ? -(Mathf.Pow(2, 20 * value - 10) * Mathf.Sin((20 * value - 11.125f) * c5)) / 2 : (Mathf.Pow(2, -20 * value + 10) * Mathf.Sin((20 * value - 11.125f) * c5)) / 2 + 1;
    }

    public static float EaseInQuad(float value)
    {
        return value * value;
    }

    public static float EaseInQuart(float value)
    {
        return value * value * value * value;
    }

    public static float EaseInExpo(float value)
    {
        return value == 0 ? 0 : Mathf.Pow(2, 10 * value - 10);
    }

    public static float EaseInBack(float value)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return c3 * value * value * value - c1 * value * value;
    }

    public static float EaseInBounce(float value)
    {
        return 1 - EaseOutBounce(1 - value);
    }

    public static float EaseOutBounce(float value)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (value < 1 / d1)
        {
            return n1 * value * value;
        }
        else if (value < 2 / d1)
        {
            return n1 * (value -= 1.5f / d1) * value + 0.75f;
        }
        else if (value < 2.5f / d1)
        {
            return n1 * (value -= 2.25f / d1) * value + 0.9375f;
        }
        else
        {
            return n1 * (value -= 2.625f / d1) * value + 0.984375f;
        }
    }

    public static float EaseOutBack(float value)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;
        return 1 + c3 * Mathf.Pow(value - 1, 3) + c1 * Mathf.Pow(value - 1, 2);
    }

    public static float EaseOutExpo(float value)
    {
        return value == 1 ? 1 : 1 - Mathf.Pow(2, -10 * value);
    }

    public static float EaseOutQuart(float value)
    {
        return 1 - Mathf.Pow(1 - value, 4);
    }

    public static float EaseOutQuad(float value)
    {
        return 1 - (1 - value) * (1 - value);
    }

    public static float EaseInOutQuad(float value)
    {
        return value < 0.5f ? 2 * value * value : 1 - Mathf.Pow(-2 * value + 2, 2) / 2f;
    }

    public static float EaseInOutQuart(float value)
    {
        return value < 0.5f ? 8 * value * value * value * value : 1 - Mathf.Pow(-2 * value + 2, 4) / 2f;
    }

    public static float EaseInOutExpo(float value)
    {
        return value == 0
            ? 0
            : value == 1
                ? 1
                : value < 0.5f ? Mathf.Pow(2, 20 * value - 10) / 2f
                    : (2 - Mathf.Pow(2, -20 * value + 10)) / 2f;
    }

    public static float EaseInOutBack(float value)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;

        return value < 0.5
            ? (Mathf.Pow(2 * value, 2) * ((c2 + 1) * 2 * value - c2)) / 2f
            : (Mathf.Pow(2 * value - 2, 2) * ((c2 + 1) * (value * 2 - 2) + c2) + 2) / 2f;
    }
    public static float EaseInOutBounce(float value)
    {
        return value < 0.5
            ? (1 - EaseOutBounce(1 - 2 * value)) / 2f
            : (1 + EaseOutBounce(2 * value - 1)) / 2f;
    }
}