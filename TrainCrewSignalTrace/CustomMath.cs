using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainCrewSignalTrace
{
    internal static class CustomMath
    {
        /// <summary>
        /// 2点間の線形補間計算メソッド
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Lerp(float x0, float y0, float x1, float y1, float x)
        {
            return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
        }

        /// <summary>
        /// float の値が 0 かどうかを確認する拡張メソッド
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsZero(this float self)
        {
            return self.IsZero(float.Epsilon);
        }
        public static bool IsZero(this float self, float epsilon)
        {
            return Math.Abs(self) < epsilon;
        }
    }
}
