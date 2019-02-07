package pacman.controllers.NeuralNetwork;

import org.omg.PortableInterceptor.SYSTEM_EXCEPTION;
import pacman.controllers.Controller;
import pacman.game.Constants;
import pacman.game.Game;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

public final class NNPacman extends Controller<Constants.MOVE>
{
    static public Game game;
    static public int pacmanPos, closestGhostDistance, closestGhostPos, closestPillDistance, closestPillPos;

    boolean networkInitialized;

    static Random random = new Random();

    private GenerationManager generationManager;
    NeuralNetwork neuralNetwork;

    static public int getRandomInt(int max)
    {
        int min = 0;
        return ThreadLocalRandom.current().nextInt(min, max + 1);
    }

    static public double getRandomDouble()
    {
        double min = 0.0f;
        double max = 1.0f;
        return min + random.nextDouble() * (max - min);
    }

    public NNPacman()
    {
        try
        {
            generationManager = new GenerationManager();
        } catch (IOException e)
        {
            System.err.print("Something went wanky");
        }

        networkInitialized = false;
    }

    public void initialize()
    {
        double[] nnValues = generationManager.getNNValues();

        //Layer 1: Input nodes
        ArrayList<Neuron> inputNeurons = new ArrayList<>();
        inputNeurons.add(new Neuron("Distance to Ghost", closestGhostDistance));
        inputNeurons.add(new Neuron("Distance to Pill", closestPillDistance));

        //Layer 2: Hidden nodes
        ArrayList<Neuron> hiddenNeurons = new ArrayList<>();
        for (int i = 0; i < 2; i++)
        {
            //RandomValue will be replaced by GA generated number in future gens
            hiddenNeurons.add(new Neuron(Integer.toString(i)));
            hiddenNeurons.get(i).addInputConnection(inputNeurons.get(0), nnValues[i]);
            hiddenNeurons.get(i).addInputConnection(inputNeurons.get(1), nnValues[i + 2]);
        }

        //System.out.println("\nHidden neurons connection weights: ");
        //for (int i = 0; i < hiddenNeurons.size(); i++)
        //{
        //    for (int j = 0; j < hiddenNeurons.get(i).getInputConnections().size(); j++)
        //    {
        //        System.out.print("\t" + hiddenNeurons.get(i).getInputConnections().get(j).getWeight());
        //    }
        //}

        //Layer 3: Output nodes
        ArrayList<Neuron> outputNeurons = new ArrayList<>();
        outputNeurons.add(new Neuron("Evade Ghost"));
        outputNeurons.add(new Neuron("Eat Pill"));

        for (int i = 0; i < hiddenNeurons.size(); i++)
        {
            outputNeurons.get(0).addInputConnection(hiddenNeurons.get(i), nnValues[hiddenNeurons.size() * 2 + i]);
            outputNeurons.get(1).addInputConnection(hiddenNeurons.get(i), nnValues[hiddenNeurons.size() * 2 + i + 2]);
        }

        //System.out.println("\nOutput neurons connection weights: ");
        //for (int i = 0; i < outputNeurons.size(); i++)
        //{
        //    for (int j = 0; j < outputNeurons.get(i).getInputConnections().size(); j++)
        //    {
        //        System.out.print("\t" + outputNeurons.get(i).getInputConnections().get(j).getWeight());
        //    }
        //}
        //System.out.println("\n");

        NeuralNetLayer inputLayer = new NeuralNetLayer("Input Layer", inputNeurons);
        NeuralNetLayer outputLayer = new NeuralNetLayer("Output Layer", outputNeurons);
        NeuralNetLayer hiddenLayer = new NeuralNetLayer("Hidden Layer", hiddenNeurons);
        ArrayList<NeuralNetLayer> hiddenLayerList = new ArrayList<NeuralNetLayer>();
        hiddenLayerList.add(hiddenLayer);

        neuralNetwork = new NeuralNetwork("LonelyNetwork", inputLayer, hiddenLayerList, outputLayer);
    }

    public Constants.MOVE getMove(Game game, long timeDue)
    {
        this.game = game;
        updateInput();
        if (!networkInitialized)
        {
            initialize();
            networkInitialized = true;
        }
        return neuralNetwork.Update();
    }

    private void updateInput()
    {
        pacmanPos = game.getPacmanCurrentNodeIndex();

        //Input 1: Get distance to the closest ghost
        closestGhostDistance = Integer.MAX_VALUE;
        for (Constants.GHOST ghost : Constants.GHOST.values())
        {
            if (game.getGhostEdibleTime(ghost) == 0 && game.getGhostLairTime(ghost) == 0)
            {
                if (game.getShortestPathDistance(pacmanPos, game.getGhostCurrentNodeIndex(ghost))
                        < closestGhostDistance)
                {
                    closestGhostPos = game.getGhostCurrentNodeIndex(ghost);
                    closestGhostDistance = game.getShortestPathDistance(pacmanPos, closestGhostPos);
                }
            }
        }

        //Input 2: Get the distance to the closest pill
        int[] pills = game.getPillIndices();
        int[] powerPills = game.getPowerPillIndices();

        ArrayList<Integer> pillPositions = new ArrayList<Integer>();

        for (int i = 0; i < pills.length; i++)                    //check which pills are available
            if (game.isPillStillAvailable(i))
                pillPositions.add(pills[i]);

        for (int i = 0; i < powerPills.length; i++)            //check with power pills are available
            if (game.isPowerPillStillAvailable(i))
                pillPositions.add(powerPills[i]);

        closestPillDistance = Integer.MAX_VALUE;
        for (int i = 0; i < pillPositions.size(); i++)
        {
            if (game.getShortestPathDistance(pacmanPos, pillPositions.get(i))
                    < closestPillDistance)
            {
                closestPillPos = pillPositions.get(i);
                closestPillDistance = game.getShortestPathDistance(pacmanPos, closestPillPos);
            }
        }
    }

    public void setFitnessAfterGame(int fitness)
    {
        try
        {
            generationManager.setFitnessAfterGame(fitness);
        } catch (IOException e)
        {
            System.err.print("Something went wanky");
        }
    }

    public void resetGenerationTracker()
    {
        try
        {
            generationManager.resetGenerationTracker();
        } catch (IOException e)
        {
            System.err.print("Something went wanky");
        }
    }

    public void resetBreedLog()
    {
        try
        {
            generationManager.resetBreedLog();
        } catch (IOException e)
        {
            System.err.print("Something went wanky");
        }
    }
}
