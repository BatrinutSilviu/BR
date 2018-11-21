using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR
{
    public class ConvexHull
    {
        public IList<NewPoint> MakeHull(IList<NewPoint> points)
        {
            List<NewPoint> newPoints = new List<NewPoint>(points);
            newPoints.Sort();
            return MakeHullPresorted(newPoints);
        }

        public IList<NewPoint> MakeHullPresorted(IList<NewPoint> points)
        {
            if (points.Count <= 1)
            {
                return new List<NewPoint>(points);
            }

            List<NewPoint> upperHull = new List<NewPoint>();
            foreach (NewPoint p in points)
            {
                while (upperHull.Count >= 2)
                {
                    NewPoint q = upperHull[upperHull.Count - 1];
                    NewPoint r = upperHull[upperHull.Count - 2];
                    if ((q.x - r.x) * (p.y - r.y) >= (q.y - r.y) * (p.x - r.x))
                    {
                        upperHull.RemoveAt(upperHull.Count - 1);
                    }
                    else
                    {
                        break;
                    }
                }
                upperHull.Add(p);
            }
            upperHull.RemoveAt(upperHull.Count - 1);

            IList<NewPoint> lowerHull = new List<NewPoint>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                NewPoint p = points[i];
                while (lowerHull.Count >= 2)
                {
                    NewPoint q = lowerHull[lowerHull.Count - 1];
                    NewPoint r = lowerHull[lowerHull.Count - 2];
                    if ((q.x - r.x) * (p.y - r.y) >= (q.y - r.y) * (p.x - r.x))
                    {
                        lowerHull.RemoveAt(lowerHull.Count - 1);
                    }
                    else
                    {
                        break;
                    }
                }
                lowerHull.Add(p);
            }
            lowerHull.RemoveAt(lowerHull.Count - 1);

            if (!(upperHull.Count == 1 && Enumerable.SequenceEqual(upperHull, lowerHull)))
            {
                upperHull.AddRange(lowerHull);
            }
            return upperHull;
        }
    }
}
