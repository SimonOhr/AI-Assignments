package pacman.controllers.NeuralNetwork;

import java.lang.reflect.Array;
import java.util.ArrayList;

public class Individual
{
    final static int individualSize = 8;
    int id;
    double[] genotypes;
    int fitness;

    public Individual(int id, boolean randomized)
    {
        this.id = id;
        genotypes = new double[individualSize];
        if (randomized) randomizeGenotypes();
        fitness = 0;
    }

    public Individual(int id, double[] genotypes)
    {
        this.id = id;
        this.genotypes = genotypes;
        fitness = 0;
    }

    public String convertToString(boolean withFitness)
    {
        StringBuilder sb = new StringBuilder("");
        for (int i = 0; i < genotypes.length; i++)
        {
            sb.append(Double.toString(genotypes[i]));
            if (i <= genotypes.length - 1) sb.append(":");
        }
        if (withFitness)
        {
            sb.append(";" + fitness);
        }
        return sb.toString();
    }

    public int getId()
    {
        return id;
    }

    public void randomizeGenotypes()
    {
        for (int i = 0; i < genotypes.length; i++)
        {
            genotypes[i] = NNPacman.getRandomDouble();
        }
    }

    public double[] getGenotypes()
    {
        return genotypes;
    }

    public void setGenotypes(double[] newGenotypes)
    {
        genotypes = newGenotypes;
    }

    public void setFitness(int fitness)
    {
        this.fitness = fitness;
    }

    public int getFitness()
    {
        return fitness;
    }
}
