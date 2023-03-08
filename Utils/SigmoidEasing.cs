using System;

//This function based on the EaseInOut algorithm described at:
//https://medium.com/hackernoon/ease-in-out-the-sigmoid-factory-c5116d8abce9

namespace TASBoardConsole.Utils
{
    public sealed class SigmoidEasing
    {
        private double _k;
        private double _scalar;

        public SigmoidEasing(double k)
        {
            _k = k;
            _scalar = 0.5/Sigmoid(1);
        }

        public double EaseInOut(double t)
        {
            return _scalar*Sigmoid(2*t - 1) + 0.5;
        }

        private double Sigmoid(double t)
        {
            return 1/(1 + Math.Exp(-_k*t)) - 0.5;
        }
    }
}
