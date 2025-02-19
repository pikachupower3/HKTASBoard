﻿using System;
using FFmpeg.AutoGen;

namespace TASBoardConsole.MovieReaders
{
    public readonly struct Fraction
    {
        public readonly long Num;
        public readonly long Den;

        private static long GCD(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        public Fraction(long num, long den)
        {
            if (den == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(den));
            }
            if (num == 0)
            {
                Num = 0;
                Den = 1;
                return;
            }
            if (den < 0)
            {
                num *= -1;
                den *= -1;
            }
            long hcf = GCD(Math.Abs(num), den);
            Num = num / hcf;
            Den = den / hcf;
        }

        public static Fraction operator +(Fraction a, Fraction b)
            => new(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);

        public static Fraction operator -(Fraction a, Fraction b)
            => new(a.Num * b.Den - b.Num * a.Den, a.Den * b.Den);

        public static Fraction operator -(Fraction a)
            => new(-a.Num, a.Den);

        public static Fraction operator ~(Fraction a)
            => new(a.Den, a.Num);

        public static bool operator >(Fraction a, Fraction b)
            => a.Num * b.Den > b.Num * a.Den;

        public static bool operator <(Fraction a, Fraction b)
            => a.Num * b.Den < b.Num * a.Den;

        public static bool operator >=(Fraction a, Fraction b)
            => a.Num * b.Den >= b.Num * a.Den;

        public static bool operator <=(Fraction a, Fraction b)
            => a.Num * b.Den <= b.Num * a.Den;

        public static bool operator ==(Fraction a, Fraction b)
            => a.Num * b.Den == b.Num * a.Den;

        public static bool operator !=(Fraction a, Fraction b)
            => a.Num * b.Den != b.Num * a.Den;

        public static implicit operator Fraction(int i) => new(i, 1);

        public static explicit operator double(Fraction f) => (double)f.Num / f.Den;

        public static explicit operator AVRational(Fraction f) => new() { num = (int)f.Num, den = (int)f.Den };

        public override bool Equals(object? obj)
        {
           return obj is Fraction fraction && fraction == this;
        }

        public override int GetHashCode()
        {
            return BitConverter.ToInt32(BitConverter.GetBytes((double)this));
        }
    }
}
