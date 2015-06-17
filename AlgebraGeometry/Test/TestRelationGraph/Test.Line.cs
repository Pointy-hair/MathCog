﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpLogic;
using NUnit.Framework;

namespace AlgebraGeometry
{
    [TestFixture]
    public class TestLineRelation
    {
        #region Concrete point-line relation

        [Test]
        public void Test_Line_Relation_0()
        {
            var graph = new RelationGraph();

            //point identity test
            var A = new Point(2.0, 3.0);
            var B = new Point(2.0, 3.0);

            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);

            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 2);
        }

        [Test]
        public void Test_Line_Relation_1()
        {
            var graph = new RelationGraph();
            var A = new Point(2.0, 3.0);
            var B = new Point(2.0, -1.0);
            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);
            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);
        }

        [Test]
        public void Test_Line_Relation_2()
        {
            var graph = new RelationGraph();
            var A = new Point(1.0, -1.0);
            var B = new Point(2.0, -1.0);
            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);
            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);
        }

        [Test]
        public void Test_Line_Relation_3()
        {
            var graph = new RelationGraph();
            var A = new Point(2.0, -2.0);
            var B = new Point(2.0, -1.0);
            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);
            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);
        }

        #endregion

        #region non-concrete point-line relation

        [Test]
        public void Test_Line_Relation_NonConcrete_Add0()
        {
            /*
             * (2.0,3.0) (2.0,a)
             */ 
            var graph = new RelationGraph();

            var variable = new Var('a');
            //point identity test
            var A = new Point(2.0, 3.0);
            var B = new Point(2.0, variable);

            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);

            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            var line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);

            var eqGoal = new EqGoal(variable, 1); // a=1
            graph.AddGoalNode(eqGoal);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count ==1);
            var gLine = line.CachedSymbols.ToList()[0] as Line;
            Assert.NotNull(gLine);
            Assert.True(gLine.A.Equals(1.0));
            Assert.Null(gLine.B);
            Assert.True(gLine.C.Equals(-2.0));
        }

        [Test]
        public void Test_Line_Relation_NonConcrete_Add1()
        {
            /*
            * (b,3.0) (5.0,3.0)
            */
            var graph = new RelationGraph();

            var variable = new Var('b');
            //point identity test
            var A = new Point(variable, 3.0);
            var B = new Point(5.0, 3.0);

            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);

            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            var line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);

            var eqGoal = new EqGoal(variable, 1); // b=1
            graph.AddGoalNode(eqGoal);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count == 1);
            var gLine = line.CachedSymbols.ToList()[0] as Line;
            Assert.NotNull(gLine);
            Assert.Null(gLine.A);
            Assert.True(gLine.B.Equals(1.0));
            Assert.True(gLine.C.Equals(-3.0));
        }

        [Test]
        public void Test_Line_Relation_NonConcrete_Add2()
        {
            /*
            * (b,3.0) (5.0,4.0)
            */
            var graph = new RelationGraph();

            var variable = new Var('b');
            //point identity test
            var A = new Point(variable, 3.0);
            var B = new Point(5.0, 4.0);

            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);

            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            var line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);

            var eqGoal = new EqGoal(variable, 4); // b=4.0
            graph.AddGoalNode(eqGoal);

            var points = graph.RetrieveSpecicShapes(ShapeType.Point);
            Assert.True(points.Count == 2);
            var gPoints = points.Where(pt => !pt.Concrete).ToList();
            Assert.True(gPoints.Count == 1);
            var gPoint = gPoints[0] as Point;
            Assert.NotNull(gPoint);
            Assert.True(gPoint.CachedGoals.Count == 1);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count == 1);
            var gLine = line.CachedSymbols.ToList()[0] as Line;
            Assert.NotNull(gLine);
            Assert.True(gLine.A.Equals(1.0));
            Assert.True(gLine.B.Equals(-1.0));
            Assert.True(gLine.C.Equals(-1.0));
            
            var eqGoal2 = new EqGoal(variable, 6); // b=6.0
            graph.AddGoalNode(eqGoal2);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count == 2);
        }

        [Test]
        public void Test_Line_Relation_NonConcrete_Delete0()
        {
            /*
             * (2.0,3.0) (2.0,a)
             */
            var graph = new RelationGraph();

            var variable = new Var('a');
            //point identity test
            var A = new Point(2.0, 3.0);
            var B = new Point(2.0, variable);

            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            var returnLine = graph.AddRelation(A, B, ShapeType.Line);

            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            var line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);

            var eqGoal = new EqGoal(variable, 1); // a=1
            graph.AddGoalNode(eqGoal);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count == 1);
            var gLine = line.CachedSymbols.ToList()[0] as Line;
            Assert.NotNull(gLine);
            Assert.True(gLine.A.Equals(1.0));
            Assert.Null(gLine.B);
            Assert.True(gLine.C.Equals(-2.0));

            graph.DeleteShapeNode(returnLine);
            var shapeNode = graph.RetrieveShapeNodes();
            Assert.True(shapeNode.Count == 2);
            foreach (ShapeNode sn in shapeNode)
            {
                Assert.True(sn.OutEdges.Count == 0);
            }

            graph.DeleteShapeNode(A);
            shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 1);
        }

        [Test]
        public void Test_Line_Relation_NonConcrete_Delete1()
        {
            /*
             * (2.0,3.0) (2.0,a)
             */
            var graph = new RelationGraph();

            var variable = new Var('a');
            //point identity test
            var A = new Point(2.0, 3.0);
            var B = new Point(2.0, variable);

            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            var returnLine = graph.AddRelation(A, B, ShapeType.Line);

            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            var line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);

            var eqGoal = new EqGoal(variable, 1); // a=1
            graph.AddGoalNode(eqGoal);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count == 1);
            var gLine = line.CachedSymbols.ToList()[0] as Line;
            Assert.NotNull(gLine);
            Assert.True(gLine.A.Equals(1.0));
            Assert.Null(gLine.B);
            Assert.True(gLine.C.Equals(-2.0));

            //when deleting A, it automatically delete all the dependent shapes
            graph.DeleteShapeNode(A);
            var shapeNodes = graph.RetrieveShapeNodes();
            Assert.True(shapeNodes.Count == 1); //B
            ShapeNode sn = shapeNodes[0];
            Assert.True(sn.OutEdges.Count == 0);
        }

        [Test]
        public void Test_Line_Relation_NonConcrete_Delete2()
        {
            /*
             * (2.0,3.0) (2.0,a)
             */
            var graph = new RelationGraph();

            var variable = new Var('a');
            //point identity test
            var A = new Point(2.0, 3.0);
            var B = new Point(2.0, variable);

            graph.AddShapeNode(A);
            graph.AddShapeNode(B);
            graph.AddRelation(A, B, ShapeType.Line);

            var shapes = graph.RetrieveShapes();
            Assert.True(shapes.Count == 3);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            var line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);

            var eqGoal = new EqGoal(variable, 1); // a=1
            graph.AddGoalNode(eqGoal);

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count == 1);
            var gLine = line.CachedSymbols.ToList()[0] as Line;
            Assert.NotNull(gLine);
            Assert.True(gLine.A.Equals(1.0));
            Assert.Null(gLine.B);
            Assert.True(gLine.C.Equals(-2.0));

            graph.DeleteGoalNode(eqGoal);
            List<ShapeNode> shapeNodes = graph.RetrieveShapeNodes(ShapeType.Point);
            Assert.True(shapeNodes.Count == 2);

            foreach (ShapeNode sn in shapeNodes)
            {
                Assert.IsInstanceOf(typeof(Point), sn.Shape);
                Assert.True(sn.InEdges.Count == 0);
            }

            shapes = graph.RetrieveSpecicShapes(ShapeType.Line);
            Assert.True(shapes.Count == 1);
            line = shapes[0] as Line;
            Assert.NotNull(line);
            Assert.True(line.RelationStatus);
            Assert.True(line.CachedSymbols.Count == 0);
        }

        #endregion

        [Test]
        public void TODO_Test()
        {
            //A(2,a), S=2 => Line

            /*
             * 
             * Input: A(2,a), B(a,3)
             * Input: a=3
             * 
             */ 
            var variable1 = new Var('a');
            var A = new Point(2, variable1);
            var B = new Point(variable1, 3);
/*
            object obj = RelationFactory.Instance.BuildRelation(A, B, ShapeType.Line);
            var line = obj as Line;
            Assert.NotNull(line);

            Assert.False(line.Concrete);
            var goal = new EqGoal(variable1, 3);
            A.Reify(goal);
            Assert.True(A.Concrete);
            Assert.True(B.Concrete);
            Assert.True(line.Concrete);
 */ 
        }
    }
}
