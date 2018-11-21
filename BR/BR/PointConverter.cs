using System.Collections.Generic;
using System.Drawing;


namespace BR
{
    class PointConverter
    {
        public NewPoint PointToNewPoint(Point point)
        {
            NewPoint newPoint = new NewPoint
            {
                x = point.X,
                y = point.Y
            };
            return newPoint;
        }

        public Point[] IListNewPointToPointArray(IList<NewPoint> newPoint)
        {
            Point[] pointArray = new Point[newPoint.Count];
            for (short iterator = 0; iterator < newPoint.Count; iterator++)
            {
                pointArray[iterator].X = newPoint[iterator].x;
                pointArray[iterator].Y = newPoint[iterator].y;
            }
            return pointArray;
        }
    }
}
