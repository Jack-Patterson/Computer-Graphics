using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outcode
{
    internal bool _up;
    internal bool _down;
    internal bool _left;
    internal bool _right;

    #region Constructors and References
    internal Outcode(Vector2 v)
    {
        _up = (v.y > 1);
        _down = (v.y < -1);
        _right = (v.x > 1);
        _left = (v.x < -1);
    }

    internal Outcode(bool up, bool down, bool left, bool right)
    {
        _up = up;
        _down = down;
        _left = left;
        _right = right;
    }

    internal Outcode()
    {
        _up = false;
        _down = false;
        _left = false;
        _right = false;
    }

    #endregion

    internal string Print()
    {
        return $"Up: {(_up ? "1" : "0")}, Down: {(_down ? "1" : "0")}, Right: {(_right ? "1" : "0")}, Left: {(_left ? "1" : "0")}";
    }

    public static bool operator ==(Outcode a, Outcode b)
    {
        return (a._up == b._up) && (a._down == b._down) && (a._left == b._left) && (a._right == b._right);
    }

    public static bool operator !=(Outcode a, Outcode b)
    {
        return !(a == b);
    }

    public static Outcode operator +(Outcode a, Outcode b)
    {
        return new Outcode(a._up || b._up, a._down || b._down, a._left || b._left, a._right || b._right);
    }

    public static Outcode operator *(Outcode a, Outcode b)
    {
        return new Outcode(a._up && b._up, a._down && b._down, a._left && b._left, a._right && b._right);
    }
}