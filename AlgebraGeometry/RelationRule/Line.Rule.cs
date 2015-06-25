﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CSharpLogic;

namespace AlgebraGeometry
{
    public static class LineGenerationRule
    {
        public static Line GenerateLine(Point pt1, Point pt2)
        {
            if (pt1.Equals(pt2)) return null;

            Debug.Assert(pt1.Concrete);
            Debug.Assert(pt2.Concrete);

            if (pt1.XCoordinate.Equals(pt2.XCoordinate))
            {
                double d;
                LogicSharp.IsDouble(pt1.XCoordinate, out d);
                return new Line(1, null, -1 * d);
            }

            if (pt1.YCoordinate.Equals(pt2.YCoordinate))
            {
                double d;
                LogicSharp.IsDouble(pt1.YCoordinate, out d);
                return new Line(null, 1, -1 * d);
            }

            //Strategy: y = mx+b, find slope m and intercept b
            //step 1: calc slope

            double p1x, p1y, p2x, p2y;
            LogicSharp.IsDouble(pt1.XCoordinate, out p1x);
            LogicSharp.IsDouble(pt1.YCoordinate, out p1y);
            LogicSharp.IsDouble(pt2.XCoordinate, out p2x);
            LogicSharp.IsDouble(pt2.YCoordinate, out p2y);
            double slope = (p2y - p1y) / (p2x - p1x);

            //step2: substitute slope into the slope-intercept form
            //y = slope*x+b

            //step3: calc intercept of the line
            double b = p2y - slope * p2x;

            //step4: get the line equation
            //y = slope*x+b

            //TODO trace
            Line line = null;
            if (Math.Abs(slope) < 0.0001)
            {
                line = new Line(null, -1, b);
            }
            else
            {
                line = new Line(slope, -1, b);
            }

            return line;
        }

        /// <summary>
        /// Goal must be slope or intercept
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public static Line GenerateLine(Point pt, EqGoal goal)
        {
            throw new Exception("TODO");
        }

        public static Line GenerateLine(EqGoal goal1, EqGoal goal2)
        {
            throw new Exception("TODO");
        }

        public static string IdentityPoints = "Cannot build the line as two identify points!";
    }
}