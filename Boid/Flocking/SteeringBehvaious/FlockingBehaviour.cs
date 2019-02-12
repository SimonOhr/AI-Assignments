using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehaviour
{
    class FlockingBehaviour
    {
        static readonly int alignmentRange = 60;
        static readonly int cohesionRange = 40;
        static readonly int perceptionAngle = 270;

        Boid[] boids;

        public FlockingBehaviour(Boid[] boidArray)
        {
            boids = boidArray;
        }

        private float AngleToNeighbour(Boid currentBoid, Boid neighbourBoid)
        {
            Vector2 vecToNeighbour = neighbourBoid.Pos - currentBoid.Pos;
            vecToNeighbour.Normalize();

            float dotProduct = Vector2.Dot(vecToNeighbour, currentBoid.Direction);
            float angle = MathHelper.ToDegrees((float)Math.Acos(dotProduct));

            Console.WriteLine("Angle: " + angle);

            return angle;
        }

        public void Update(Boid currentBoid)
        {

        }

        public Vector2 GetAlignment(Boid currentBoid)
        {
            Vector2 resultVector = Vector2.Zero;
            int neighborCount = 0;

            for (int i = 0; i < boids.Length; i++)
            {
                if (boids[i] != currentBoid)
                {
                    if (Vector2.Distance(boids[i].Pos, currentBoid.Pos) < alignmentRange)
                    {
                        if (AngleToNeighbour(currentBoid, boids[i]) < perceptionAngle / 2)
                        {
                            resultVector += boids[i].Velocity;
                            neighborCount++;
                        }
                    }
                }
            }
            if (neighborCount == 0)
                return resultVector;

            resultVector /= neighborCount;

            return Vector2.Normalize(resultVector);
        }

        public Vector2 GetCohesion(Boid currentBoid)
        {
            Vector2 resultVector = Vector2.Zero;
            int neighborCount = 0;

            for (int i = 0; i < boids.Length; i++)
            {
                if (boids[i] != currentBoid)
                {
                    if (Vector2.Distance(boids[i].Pos, currentBoid.Pos) < cohesionRange)
                    {
                        if (AngleToNeighbour(currentBoid, boids[i]) < perceptionAngle / 2)
                        {
                            resultVector += boids[i].Pos;
                            neighborCount++;
                        }
                    }
                }
            }
            if (neighborCount == 0)
                return resultVector;

            resultVector /= neighborCount;

            return Vector2.Normalize(resultVector);

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
        }

        public Vector2 GetSeperation(Boid currentBoid)
        {
            Vector2 resultVector = Vector2.Zero;

            for (int i = 0; i < boids.Length; i++)
            {
                if (boids[i] != currentBoid)
                {
                    Vector2 differenceVec = currentBoid.Pos - boids[i].Pos;
                    float magnitude = differenceVec.Length();
                    if (magnitude < 100.0f)
                    {
                        if (AngleToNeighbour(currentBoid, boids[i]) < perceptionAngle / 2)
                        {
                            float weightedMagnitude = 15.0f / (magnitude * magnitude);
                            differenceVec = Vector2.Multiply(differenceVec, new Vector2(weightedMagnitude, weightedMagnitude));
                            resultVector += differenceVec;
                        }
                    }
                }
            }
            return resultVector;

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
        }

        //public Vector2 Avoidance(Boid currentBoid)
        //{
        //    Vector2 vector = new Vector2(0, 0);
        //    int neighborCount = 0;

        //    foreach (Boid boid in boids)
        //    {
        //        if (boid != currentBoid)
        //        {
        //            if (Vector2.Distance(currentBoid.Pos, boid.Pos) < 20)
        //            {
        //                vector.X += currentBoid.Pos.X - boid.Pos.X;
        //                vector.Y += currentBoid.Pos.Y - boid.Pos.Y;
        //            }
        //        }
        //    }
        //    if (neighborCount == 0)
        //        return vector;

        //    return Vector2.Normalize(vector);
        //}
    }
}