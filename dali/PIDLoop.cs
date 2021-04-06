using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dali
{
    public class PIDLoop
    {
        private static double _oldError;

        private static double _kP = 0.1;
        private static double _kI = (1 / (30 * 60));
        private static double _kD = 0;

        private static double _pContribution = 0;
        private static double _iContribution = 0;
        private static double _dContribution = 0;

        private static double _maxIContribution = double.MaxValue;
        private static double _maxDContribution = double.MaxValue;

        private static double _minIContribution = double.MinValue;
        private static double _minDContribution = double.MinValue;

        private static double _maxOutput = double.MaxValue;
        private static double _minOutput = double.MinValue;

        public static double Run(double target, double actual)
        {
            var error = target - actual;
            var output = Run(target, actual, error - _oldError);
            _oldError = error;
            return output;
        }

        private static double Run(double target, double actual, double deltaError)
        {
            var error = target - actual;
            double oldIContribution = 0;

            if (_kP == 0) return 0;

            _pContribution = error * _kP;

            if (_kI != 0)
            {
                oldIContribution = _iContribution;
                _iContribution += error * _kI;

                if (_iContribution > _maxIContribution)
                    _iContribution = _maxIContribution;

                if (_iContribution < _minIContribution)
                    _iContribution = _minIContribution;
            }

            if (_kD != 0)
            {
                _dContribution = deltaError * _kD;

                if (_dContribution > _maxDContribution)
                    _dContribution = _maxDContribution;

                if (_dContribution < _minDContribution)
                    _dContribution = _minDContribution;
            }

            double output = _pContribution + _iContribution + _dContribution;

            if (output > _maxOutput)
            {
                output = _maxOutput;
                if (_iContribution > oldIContribution)
                    _iContribution = oldIContribution;
            }
            else if (output < _minOutput)
            {
                output = _minOutput;
                if (_iContribution < oldIContribution)
                    _iContribution = oldIContribution;
            }

            return output;
        }
    }
}
