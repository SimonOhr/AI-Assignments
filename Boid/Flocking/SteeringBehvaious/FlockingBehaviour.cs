﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehaviour
{
    static class FlockingBehaviour
    {
        static readonly int alignmentRange = 60;
        static readonly int cohesionRange = 40;
        static readonly int separationRange = 30;
        static readonly int separationWeight = 15;

        static readonly int perceptionAngle = 270;

        //public static readonly int neighbourRange = 100;

        static Boid[] boids;

        public static void Initialize(ref Boid[] boids)
        {
            FlockingBehaviour.boids = boids;
        }

        private static float AngleToNeighbour(Boid currentBoid, Boid neighbourBoid)
        {
            Vector2 vecToNeighbour = neighbourBoid.Pos - currentBoid.Pos;
            vecToNeighbour.Normalize();

            float dotProduct = Vector2.Dot(vecToNeighbour, currentBoid.Direction);
            float angle = MathHelper.ToDegrees((float)Math.Acos(dotProduct));

            return angle;
        }

        public static void Update(Boid currentBoid)
        {
            Vector2 alignmentVector = Vector2.Zero;
            int alignmentNeighbors = 0;

            Vector2 cohesionVector = Vector2.Zero;
            int cohesionNeighbors = 0;

            Vector2 separationVector = Vector2.Zero;
            int separationNeighbors = 0;

            for (int i = 0; i < boids.Length; i++)
            {
                if (boids[i] != currentBoid)
                {
                    Vector2 differenceVec = currentBoid.Pos - boids[i].Pos;
                    float distance = differenceVec.Length();
                    float angle = float.MaxValue;

                    if (distance < alignmentRange)
                    {
                        if (angle == float.MaxValue) angle = AngleToNeighbour(currentBoid, boids[i]);
                        if (angle < perceptionAngle / 2)
                        {
                            alignmentVector += boids[i].Velocity;
                            alignmentNeighbors++;
                        }
                    }
                    if (distance < cohesionRange)
                    {
                        if (angle == float.MaxValue) angle = AngleToNeighbour(currentBoid, boids[i]);
                        if (angle < perceptionAngle / 2)
                        {
                            cohesionVector += boids[i].Pos;
                            cohesionNeighbors++;
                        }
                    }
                    if (distance < separationRange)
                    {
                        if (angle == float.MaxValue) angle = AngleToNeighbour(currentBoid, boids[i]);
                        if (angle < perceptionAngle / 2)
                        {
                            float weightedMagnitude = separationWeight / (distance * distance);
                            differenceVec = Vector2.Multiply(differenceVec, new Vector2(weightedMagnitude, weightedMagnitude));
                            separationVector += differenceVec;
                            separationNeighbors++;
                        }
                    }
                }
            }

            if (alignmentNeighbors == 0)
            {
                currentBoid.SetAlignment(alignmentVector);
            }
            else
            {
                alignmentVector /= alignmentNeighbors;
                currentBoid.SetAlignment(Vector2.Normalize(alignmentVector));
            }

            if (cohesionNeighbors == 0)
            {
                currentBoid.SetCohesion(cohesionVector);
            }
            else
            {
                cohesionVector /= cohesionNeighbors;
                currentBoid.SetCohesion(Vector2.Normalize(cohesionVector));
            }

            if (separationNeighbors == 0)
            {
                currentBoid.SetSeperation(separationVector);
            }
            else
            {
                separationVector /= separationNeighbors;
                currentBoid.SetSeperation(Vector2.Normalize(separationVector));
            }
        }

        public static Vector2 GetAlignment(Boid currentBoid)
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

        public static Vector2 GetCohesion(Boid currentBoid)
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
        }

        public static Vector2 GetSeperation(Boid currentBoid)
        {
            Vector2 resultVector = Vector2.Zero;
            int neighborCount = 0;

            for (int i = 0; i < boids.Length; i++)
            {
                if (boids[i] != currentBoid)
                {
                    Vector2 differenceVec = currentBoid.Pos - boids[i].Pos;
                    float distance = differenceVec.Length();
                    if (distance < separationRange)
                    {
                        if (AngleToNeighbour(currentBoid, boids[i]) < perceptionAngle / 2)
                        {
                            float weightedMagnitude = 15.0f / (distance * distance);
                            differenceVec = Vector2.Multiply(differenceVec, new Vector2(weightedMagnitude, weightedMagnitude));
                            resultVector += differenceVec;
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

        //public static Vector2 Rule1(Boid currentBoid)
        //{
        //    Vector2 pcj = Vector2.Zero;
        //    int neighbours = 0;

        //    for (int i = 0; i < boids.Length; i++)
        //    {
        //        if (currentBoid != boids[i])
        //        {
        //            float distance = Vector2.Distance(currentBoid.Pos, boids[i].Pos);
        //            if (distance < neighbourRange)
        //            {
        //                if (AngleToNeighbour(currentBoid, boids[i]) > perceptionAngle / 2)
        //                {
        //                    pcj += boids[i].Pos;
        //                    neighbours++;
        //                }

        //            }
        //        }
        //    }

        //    if (neighbours == 0) return pcj;

        //    pcj = pcj / neighbours;
        //    return (pcj - currentBoid.Pos) / 100;
        //}

        //public static Vector2 Rule2(Boid currentBoid)
        //{
        //    Vector2 c = Vector2.Zero;

        //    for (int i = 0; i < boids.Length; i++)
        //    {
        //        if (currentBoid != boids[i])
        //        {
        //            if ((boids[i].Pos - currentBoid.Pos).Length() < separationRange)
        //            {
        //                if (AngleToNeighbour(currentBoid, boids[i]) > perceptionAngle / 2)
        //                {
        //                    c -= (boids[i].Pos - currentBoid.Pos);
        //                }
        //            }
        //        }
        //    }

        //    return c;
        //}

        //public static Vector2 Rule3(Boid currentBoid)
        //{
        //    Vector2 pvj = Vector2.Zero;
        //    int neighbours = 0;

        //    for (int i = 0; i < boids.Length; i++)
        //    {
        //        if (currentBoid != boids[i])
        //        {
        //            if (AngleToNeighbour(currentBoid, boids[i]) > perceptionAngle / 2)
        //            {
        //                pvj += boids[i].Velocity;
        //                neighbours++;
        //            }
        //        }
        //    }

        //    if (neighbours == 0) return pvj;

        //    pvj = pvj / neighbours;
        //    return (pvj - currentBoid.Velocity) / 8;
        //}
    }
}