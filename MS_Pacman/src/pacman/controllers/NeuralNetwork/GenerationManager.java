package pacman.controllers.NeuralNetwork;

import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class GenerationManager
{
    static public int generationSize = 12;
    static public int totalGenerations = 500;

    FileManager fileManager;

    int currentGeneration;
    Individual currentIndividual;

    public GenerationManager() throws IOException
    {
        fileManager = new FileManager();

        //Loads which generation and individual we are currently processing
        int temp[] = fileManager.findGeneration();
        currentGeneration = temp[0];
        currentIndividual = new Individual(temp[1], false);

        // First time the project is run it randomizes and saves a generation
        if (currentGeneration == 0 && currentIndividual.getId() == 0)
        {
            fileManager.writeGeneration(currentGeneration, getRandomGeneration(currentGeneration));

        } else if (currentIndividual.getId() >= generationSize)
        {
            Individual[] oldGen = fileManager.readGeneration(currentGeneration);
            Individual[] newGen = breedNewGeneration();

            if (isSame(oldGen, newGen))
                System.out.println("\n\n\n\n\nTwo generations are identical\n\n\n\n\n");

            if (currentGeneration == 0)
                System.out.println("\n\n\n\n\nGeneration is 0\n\n\n\n\n");

            currentGeneration++;
            fileManager.writeGeneration(currentGeneration, newGen);
            currentIndividual = new Individual(0, false);
            fileManager.updateGenerationTracker(currentGeneration, 0);
        }

        currentIndividual = fileManager.readNewIndividual(currentGeneration, currentIndividual.getId());
        System.out.println("Current individual: " + currentIndividual.getId());
    }

    private boolean isSame(Individual[] oldGen, Individual[] newGen)
    {
        for (int i = 0; i < generationSize; i++)
        {
            for (int j = 0; j < Individual.individualSize; j++)
            {
                if (oldGen[i].getGenotypes()[j] != newGen[i].getGenotypes()[j])
                    return false;
            }
        }
        return true;
    }

    private Individual[] getRandomGeneration(int genID) throws IOException
    {
        Individual[] generation = new Individual[GenerationManager.generationSize];
        for (int i = 0; i < generation.length; i++)
        {
            generation[i] = new Individual(i, true);
        }
        System.out.println("FIRST GENERATION RANDOMIZED");
        return generation;
    }

    public void setFitnessAfterGame(int fitness) throws IOException
    {
        currentIndividual.setFitness(fitness);
        fileManager.writeFitness(currentGeneration, currentIndividual);
        fileManager.updateGenerationTracker(currentGeneration, currentIndividual.getId() + 1);
    }

    private Individual[] breedNewGeneration() throws IOException
    {
        Individual[] oldGeneration = fileManager.readGeneration(currentGeneration);

        System.out.print("\n");
        System.out.print("Before sort:");
        for (int i = 0; i < generationSize; i++)
        {
            System.out.print("\n\tFitness score: " + oldGeneration[i].getFitness() + "\t");
            for (int j = 0; j < Individual.individualSize; j++)
            {
                System.out.print(" / " + oldGeneration[i].getGenotypes()[j]);
            }
        }

        sortByFitness(oldGeneration);

        System.out.print("\nAfter sort:");
        for (int i = 0; i < generationSize; i++)
        {
            System.out.print("\n\tFitness score: " + oldGeneration[i].getFitness() + "\t");
            for (int j = 0; j < Individual.individualSize; j++)
            {
                System.out.print(" / " + oldGeneration[i].getGenotypes()[j]);
            }
        }
        System.out.print("\n\n");

        fileManager.writeForGraphing(oldGeneration);
        fileManager.writeGeneration(currentGeneration, oldGeneration);

        ArrayList<Individual> newGeneration = new ArrayList<>();

        for (int i = 0; i < oldGeneration.length / 2; i++)
        {
            newGeneration.add(new Individual(i, oldGeneration[i].getGenotypes()));
        }

        int numberOfParents = generationSize / 2;
        for (int indexParentA = 0; indexParentA < numberOfParents; indexParentA += 2)
        {
            int indexParentB = indexParentA + 1;

            List<Double> newChildA = new ArrayList<>();
            List<Double> newChildB = new ArrayList<>();

            for (int indexParentGenotype = 0; indexParentGenotype < Individual.individualSize; indexParentGenotype++)
            {
                if (indexParentGenotype < Individual.individualSize / 2)
                {
                    newChildA.add(newGeneration.get(indexParentA).genotypes[indexParentGenotype]);
                    newChildB.add(newGeneration.get(indexParentB).genotypes[indexParentGenotype]);
                } else
                {
                    newChildA.add(newGeneration.get(indexParentB).genotypes[indexParentGenotype]);
                    newChildB.add(newGeneration.get(indexParentA).genotypes[indexParentGenotype]);
                }
            }

            if (NNPacman.getRandomInt(100) <= 15)
            {
                System.out.println("Child mutated: ");
                System.out.print("\tBefore: ");
                for (int i = 0; i < newChildA.size(); i++)
                    System.out.print(newChildA.get(i) + " / ");
                System.out.print("\n");

                newChildA = mutate(newChildA);

                System.out.print("\tAfter: ");
                for (int i = 0; i < newChildA.size(); i++)
                    System.out.print(newChildA.get(i) + " / ");
                System.out.print("\n");
            }
            if (NNPacman.getRandomInt(100) <= 15)
            {
                System.out.println("Child mutated (ID " + (numberOfParents + indexParentA) + "): ");
                System.out.print("\tBefore: ");
                for (int i = 0; i < newChildB.size(); i++)
                    System.out.print(newChildB.get(i) + " / ");
                System.out.print("\n");

                newChildB = mutate(newChildB);

                System.out.print("\tAfter: ");
                for (int i = 0; i < newChildB.size(); i++)
                    System.out.print(newChildB.get(i) + " / ");
                System.out.print("\n");
            }
            newGeneration.add(new Individual(numberOfParents + indexParentA, listToArrayDouble(newChildA)));
            newGeneration.add(new Individual(numberOfParents + indexParentB, listToArrayDouble(newChildB)));
        }

        System.out.println("New generation bred");
        //if (isSame(oldGeneration, listToArrayIndividual(newGeneration)))
        //{
        //    System.out.println("\n\n\n\n\nTHINGS GONE FUCKED UP!\n\n\n\n\n");
        //}

        fileManager.writeBreedLog(oldGeneration, listToArrayIndividual(newGeneration), currentGeneration);
        return listToArrayIndividual(newGeneration);
    }

    private List<Double> mutate(List<Double> child)
    {
        int id = NNPacman.getRandomInt(child.size() - 1);
        child.set(id, NNPacman.getRandomDouble());
        //System.out.println("\tChild ID: " + id);
        return child;
    }

    //private boolean isFucked(Individual[] oldGen, List<Individual> newGen)
    //{
    //    for (int i = 0; i < generationSize; i++)
    //    {
    //        for (int j = 0; j < Individual.individualSize; j++)
    //        {
    //            if (oldGen[i].getGenotypes()[j] != newGen.get(i).getGenotypes()[j])
    //            {
    //                return false;
    //            }
    //        }
    //    }
    //    return true;
    //}

    private double[] listToArrayDouble(List<Double> list)
    {
        double[] array = new double[list.size()];
        for (int i = 0; i < array.length; i++)
            array[i] = list.get(i);
        return array;
    }

    private Individual[] listToArrayIndividual(List<Individual> list)
    {
        Individual[] array = new Individual[list.size()];
        for (int i = 0; i < array.length; i++)
            array[i] = list.get(i);
        return array;
    }

    public void sortByFitness(Individual[] generation)
    {
        Individual tempIndividual;
        for (int i = 1; i < generation.length; i++)
        {
            for (int j = i; j > 0; j--)
            {
                if (generation[j].getFitness() > generation[j - 1].getFitness())
                {
                    tempIndividual = generation[j];
                    generation[j] = generation[j - 1];
                    generation[j - 1] = tempIndividual;
                }
            }
        }
    }

    public void resetGenerationTracker() throws IOException
    {
        fileManager.updateGenerationTracker(0, 0);

    }

    public void resetBreedLog() throws IOException
    {
        fileManager.resetBreedLog();
    }

    public double[] getNNValues()
    {
        double[] nnValues = new double[Individual.individualSize];
        for (int i = 0; i < Individual.individualSize; i++)
        {
            nnValues[i] = currentIndividual.getGenotypes()[i];
        }
        return nnValues;
    }
}