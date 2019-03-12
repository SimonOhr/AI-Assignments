package pacman.controllers.NeuralNetwork;

import java.io.*;

public class FileManager
{
    String folderPath = "C:\\Users\\vikto\\OneDrive\\År3\\Spelinriktad AI\\AI-Assignments\\MS_Pacman\\Generations\\";
    //"C:\\MAU\\AI\\MS_Pacman\\Generations\\"

    public File getCurrentFilePath(int genID)
    {
        return new File(folderPath + "Generation" + genID + ".txt");
    }

    public int[] findGeneration() throws IOException
    {
        File file = new File(folderPath + "GenerationTracker.txt");
        BufferedReader bufferedReader = new BufferedReader(new FileReader(file));

        String st = bufferedReader.readLine();
        String[] parts = st.split(":");
        bufferedReader.close();

        System.out.println("\nGeneration tracker: " + Integer.parseInt(parts[0]) + ":" + Integer.parseInt(parts[1]));

        return new int[]{Integer.parseInt(parts[0]), Integer.parseInt(parts[1])};
    }

    public void updateGenerationTracker(int genID, int individualID) throws IOException
    {
        File file = new File(folderPath + "GenerationTracker.txt");

        FileWriter fw = new FileWriter(file);
        BufferedWriter bufferedWriter = new BufferedWriter(fw);

        bufferedWriter.write(genID + ":" + individualID);
        bufferedWriter.close();
    }


    public void writeGeneration(int genID, Individual[] generation) throws IOException
    {
        File file = getCurrentFilePath(genID);
        {
            if (!file.exists())
                file.createNewFile();
        }

        FileWriter fw = new FileWriter(file);
        BufferedWriter bufferedWriter = new BufferedWriter(fw);
        for (int i = 0; i < generation.length; i++)
        {
            bufferedWriter.write(Integer.toString(generation[i].getFitness()));
            bufferedWriter.write(";");
            double[] genotypes = generation[i].getGenotypes();
            for (int j = 0; j < genotypes.length; j++)
            {
                bufferedWriter.write(Double.toString(genotypes[j]));
                if (j < genotypes.length - 1) bufferedWriter.write("/");
            }
            bufferedWriter.newLine();
        }
        bufferedWriter.close();
    }

    public Individual[] readGeneration(int genID) throws IOException
    {
        Individual[] generation = new Individual[GenerationManager.generationSize];
        for (int i = 0; i < generation.length; i++)
        {
            generation[i] = readNewIndividual(genID, i);
        }
        return generation;
    }

    public Individual readNewIndividual(int genID, int indID) throws IOException
    {
        Individual newIndividual = new Individual(indID, false);

        File file = getCurrentFilePath(genID);
        BufferedReader bufferedReader = new BufferedReader(new FileReader(file));

        String line;
        int index = 0;
        while ((line = bufferedReader.readLine()) != null)
        {
            if (index >= newIndividual.getId()) break;
            index++;
        }

        String[] parts = line.split(";");
        bufferedReader.close();

        newIndividual.setFitness(Integer.parseInt(parts[0]));
        String[] stringGenotypes = parts[1].split("/");

        double[] newGenotypes = new double[stringGenotypes.length];
        for (int i = 0; i < newGenotypes.length; i++)
        {
            newGenotypes[i] = Double.parseDouble(stringGenotypes[i]);
        }
        newIndividual.setGenotypes(newGenotypes);

        return newIndividual;
    }

    public void writeBreedLog(Individual[] oldGen, Individual[] newGen, int genID) throws IOException
    {
        File file = new File(folderPath + "BreedLog.txt");

        FileWriter fw = new FileWriter(file, true);
        BufferedWriter bufferedWriter = new BufferedWriter(fw);

        bufferedWriter.write("Generation " + genID + ":\n");
        bufferedWriter.newLine();
        for (Individual tempInd : oldGen)
        {
            bufferedWriter.write("Fitness: " + tempInd.getFitness() + "\tID: " + tempInd.getId() + "\t Genotypes: ");
            for (double tempGenotype : tempInd.getGenotypes())
            {
                bufferedWriter.write(" / " + tempGenotype);
            }
            bufferedWriter.newLine();
        }
        bufferedWriter.newLine();

        bufferedWriter.write("Generation " + (genID + 1) + ":\n");
        bufferedWriter.newLine();
        for (Individual tempInd : newGen)
        {
            bufferedWriter.write("Fitness: " + tempInd.getFitness() + "\tID: " + tempInd.getId() + "\t Genotypes: ");
            for (double tempGenotype : tempInd.getGenotypes())
            {
                bufferedWriter.write(" / " + tempGenotype);
            }
            bufferedWriter.newLine();
        }
        bufferedWriter.newLine();
        bufferedWriter.newLine();
        bufferedWriter.newLine();
        bufferedWriter.write("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        bufferedWriter.newLine();
        bufferedWriter.newLine();
        bufferedWriter.newLine();

        bufferedWriter.close();
    }

    public void resetBreedLog() throws IOException
    {

        File file = new File(folderPath + "BreedLog.txt");
        FileWriter fw = new FileWriter(file);
        BufferedWriter bufferedWriter = new BufferedWriter(fw);
        bufferedWriter.write("");
        bufferedWriter.close();
    }

    public void writeFitness(int genID, Individual individual) throws IOException
    {
        Individual[] generation = readGeneration(genID);
        generation[individual.getId()] = individual;
        writeGeneration(genID, generation);
    }

    public void writeForGraphing(Individual[] generation) throws IOException
    {
        File file = new File(folderPath + "LogForGraph.txt");
        FileWriter fw = new FileWriter(file, true);
        BufferedWriter bufferedWriter = new BufferedWriter(fw);
        int averageFitness = 0;
        for (int i = 0; i < generation.length; i++)
        {
            averageFitness += generation[i].getFitness();
        }
        averageFitness = averageFitness / generation.length;

        bufferedWriter.write(generation[0].getFitness() + "\t");
        bufferedWriter.write(averageFitness + "\t");
        bufferedWriter.write(generation[generation.length - 1].getFitness() + "");
        bufferedWriter.newLine();
        bufferedWriter.close();
    }
}