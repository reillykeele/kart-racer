using UnityEngine;

namespace Util.Helpers
{
    public static class MathHelper
    {
        public static Vector3 Abs(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

        public static float Distance2(Vector3 a, Vector3 b) =>
            Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2);

        public static float PercentOfRange(float percent, float min, float max) => percent * max + (1 - percent) * min;
    }
}