﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehvaious
{
    class FlockingBehaviour
    {
        Vector2 alignment, cohersion, seperation;
        Boid[] boids;
        public FlockingBehaviour(Boid[] boidArray)
        {
            boids = boidArray;
        }
        public Vector2 Alignment(Boid myBoid)
        {
            Vector2 v = Vector2.Zero;
            int neighborCount = 0;

            foreach (Boid boid in boids)
            {
                if (boid != myBoid)
                {
                    if (Vector2.Distance(myBoid.GetPos(), boid.GetPos()) < 60)
                    {
                        v.X += boid.GetVelocity().X;
                        v.Y += boid.GetVelocity().Y;
                        neighborCount++;
                    }
                }
            }
            if (neighborCount == 0)
                return v;

            v.X /= neighborCount;
            v.Y /= neighborCount;
            return Vector2.Normalize(v);

        }

        public Vector2 Cohersion(Boid myBoid)
        {
            //Vector2 cohersion = Vector2.Zero;
            //System.Console.WriteLine(myBoid.GetPos());
            //int neighborCount = 0;

            //foreach (Boid boid in boids)
            //{
            //    if (boid != myBoid)
            //    {
            //        if (Vector2.Distance(boid.GetPos(), myBoid.GetPos()) < 50)
            //        {
            //            cohersion.X += boid.GetPos().X - myBoid.GetPos().X;
            //            cohersion.Y += boid.GetPos().Y - myBoid.GetPos().Y;
            //            System.Console.WriteLine(cohersion);
            //            neighborCount++;
            //        }
            //    }
            //}
            //if (neighborCount == 0)
            //    return cohersion;
            //System.Console.WriteLine(cohersion);
            //cohersion.X /= neighborCount;
            //cohersion.Y /= neighborCount;
            ////cohersion = new Vector2(cohersion.X - myBoid.GetPos().X, cohersion.Y - myBoid.GetPos().Y);
            //System.Console.WriteLine(Vector2.Normalize(cohersion));
            //return Vector2.Normalize(cohersion);

            Vector2 resultVector = Vector2.Zero;
            int neighborCount = 0;

            for (int i = 0; i < boids.GetLength(0); i++)
            {
                if (boids[i] != myBoid)
                    if (Vector2.Distance(boids[i].GetPos(), myBoid.GetPos()) < 40)
                    {
                        resultVector += boids[i].GetPos();
                        neighborCount++;
                    }
            }
            if (neighborCount == 0)
                return resultVector;

            resultVector /= neighborCount;
            Console.WriteLine(resultVector.ToString());


            return Vector2.Normalize(resultVector);
        }

        public Vector2 Seperation(Boid myBoid)
        {
            //Vector2 v = new Vector2(0, 0);
            //int neighborCount = 0;

            //foreach (Boid boid in boids)
            //{
            //    if (boid != myBoid)
            //    {
            //        if (Vector2.Distance(myBoid.GetPos(), boid.GetPos()) < 20)
            //        {
            //            v.X += myBoid.GetPos().X - boid.GetPos().X + 1f;
            //            v.Y += myBoid.GetPos().Y - boid.GetPos().Y + 1f;
            //            neighborCount++;
            //        }
            //    }
            //}
            //if (neighborCount == 0)
            //    return v;

            ////v.X /= neighborCount;
            //// v.Y /= neighborCount;
            ////v.X *= -1;
            ////v.Y *= -1;
            //// v = new Vector2(v.X - myBoid.GetPos().X, v.Y - myBoid.GetPos().Y);
            //var temp = Vector2.Normalize(v);
            //temp.X += 0.5f;
            //temp.Y += 0.5f;
            //return temp;

            Vector2 resultVec = Vector2.Zero;

            for (int i = 0; i < boids.GetLength(0); i++)
            {
                if (boids[i] != myBoid)
                {
                    Vector2 differenceVec = myBoid.GetPos() - boids[i].GetPos();
                    float magnitude = differenceVec.Length();
                    if (magnitude < 100.0f)
                    {
                        float weightedMagnitude = 15.0f / (magnitude * magnitude);
                        differenceVec = Vector2.Multiply(differenceVec, new Vector2(weightedMagnitude, weightedMagnitude));
                        resultVec += differenceVec;
                    }
                }
            }
            return resultVec;


        }

        public Vector2 Avoidance(Boid myBoid)
        {
            Vector2 v = new Vector2(0, 0);
            int neighborCount = 0;

            foreach (Boid boid in boids)
            {
                if (boid != myBoid)
                {
                    if (Vector2.Distance(myBoid.GetPos(), boid.GetPos()) < 20)
                    {
                        v.X += myBoid.GetPos().X - boid.GetPos().X;
                        v.Y += myBoid.GetPos().Y - boid.GetPos().Y;
                    }
                }
            }
            if (neighborCount == 0)
                return v;


            return Vector2.Normalize(v);
        }
    }
}
