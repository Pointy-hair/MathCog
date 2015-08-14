﻿using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections.Generic;
using AlgebraGeometry;
using CSharpLogic;
using NUnit.Framework;
using starPadSDK.MathExpr;
using Text = starPadSDK.MathExpr.Text;

namespace ExprGenerator
{
    public static class ExprG
    {
        public static Expr Generate(object obj)
        {
            var boolValue = obj as bool?;
            if (boolValue != null) return new WordSym(boolValue.ToString());

            var ss = obj as ShapeSymbol;
            if (ss != null) return Generate(ss);

            var eqGoal = obj as EqGoal;
            if (eqGoal != null) return Generate(eqGoal);

            var term = obj as Term;
            if (term != null) return Generate(term);

            var equation = obj as Equation;
            if (equation != null) return Generate(equation);

            var variable = obj as Var;
            if (variable != null)
            {
                return new WordSym(variable.Token.ToString());
            }

            int integer;
            if (LogicSharp.IsInt(obj, out integer))
            {
                return new IntegerNumber(integer.ToString());
            }

            double dNumber;
            if (LogicSharp.IsDouble(obj, out dNumber))
            {
                return new DoubleNumber(dNumber);
            }

            return null;
        }

        public static Expr Generate(ShapeSymbol ss)
        {
            var ps = ss as PointSymbol;
            if (ps != null) return ps.ToExpr();

            var ls = ss as LineSymbol;
            if (ls != null) return ls.ToExpr();

            var lineSeg = ss as LineSegmentSymbol;
            if (lineSeg != null) return lineSeg.ToExpr();

            return null;
        }

        public static Expr Generate(Equation eq)
        {
            var head = WellKnownSym.equals;
            var lhs = Generate(eq.Lhs);
            var rhs = Generate(eq.Rhs);
            //Debug.Assert(lhs != null && rhs != null);
            return new CompositeExpr(head, new Expr[] { lhs, rhs });
        }

        public static Expr Generate(EqGoal goal)
        {
            var head = WellKnownSym.equals;
            var lhs = Generate(goal.Lhs);
            var rhs = Generate(goal.Rhs);
            Debug.Assert(lhs!=null && rhs != null);
            return new CompositeExpr(head, new Expr[] { lhs, rhs });
        }

        private static Expr Generate(Term term)
        {
            if (term.Op.Method.Name.Equals("Add"))
            {
                var head = WellKnownSym.plus;
                var lst = term.Args as List<object>;
                Debug.Assert(lst != null);
                var exprLst = new List<Expr>();
                foreach(var obj in lst)
                {
                    exprLst.Add(Generate(obj));
                }
                return new CompositeExpr(head, exprLst.ToArray());
            }
            else if (term.Op.Method.Name.Equals("Subtract"))
            {
                var head = WellKnownSym.minus;
                var lst = term.Args as List<object>;
                Debug.Assert(lst != null);
                var exprLst = new List<Expr>();
                foreach (var obj in lst)
                {
                    exprLst.Add(Generate(obj));
                }
                return new CompositeExpr(head, exprLst.ToArray());
            }
            else if (term.Op.Method.Name.Equals("Multiply"))
            {
                var head = WellKnownSym.times;
                var lst = term.Args as List<object>;
                Debug.Assert(lst != null);
                var exprLst = new List<Expr>();
                foreach (var obj in lst)
                {
                    exprLst.Add(Generate(obj));
                }
                return new CompositeExpr(head, exprLst.ToArray());
            }
            else if (term.Op.Method.Name.Equals("Divide"))
            {
                var head = WellKnownSym.divide;
                var lst = term.Args as List<object>;
                Debug.Assert(lst != null);
                var exprLst = new List<Expr>();
                foreach (var obj in lst)
                {
                    exprLst.Add(Generate(obj));
                }
                return new CompositeExpr(head, exprLst.ToArray());
            }
            //TODO
            return null;
        }

        public static Expr Derive(Expr source, Expr target)
        {
            var cc  = new CompositeExpr(WellKnownSym.times, new Expr[] { source, new WordSym("→"), target });
            return new CompositeExpr(new WordSym("Step:"), new Expr[] {cc});
        }
    }

    public static class PointExpExtension
    {
        public static Expr ToExpr(this PointSymbol ps)
        {
            var xExpr = ToCoord(ps.SymXCoordinate);
            var yExpr = ToCoord(ps.SymYCoordinate);
            if (ps.Shape.Label == null)
            {
                //var comp = new CompositeExpr(new WordSym("comma"), new Expr[] { });
                var cc = new CompositeExpr(new WordSym(""), new Expr[] { xExpr, yExpr });
                return cc;                
            }
            else
            {
                var comp = new CompositeExpr(new WordSym(ps.Shape.Label), new Expr[] {xExpr, yExpr});
                return comp;
            }
            //return Text.Convert(form);
        }

        public static Expr ToCoord(string coord)
        {
            int number;
            bool result = LogicSharp.IsInt(coord, out number);
            if(result) return new IntegerNumber(coord);

            double dNumber;
            result = LogicSharp.IsDouble(coord, out dNumber);
            if(result) return new DoubleNumber(dNumber);

            return new WordSym(coord);            
        }
    }

    public static class LineExpExtension
    {
        public static Expr ToExpr(this LineSymbol ls)
        {
            return Text.Convert(ls.ToString());
        }
    }

    public static class LineSegExpExtension
    {
        public static Expr ToExpr(this LineSegmentSymbol lss)
        {
            Debug.Assert(lss.Shape.Label != null);
            return Text.Convert(lss.ToString());
        }
    }
}
