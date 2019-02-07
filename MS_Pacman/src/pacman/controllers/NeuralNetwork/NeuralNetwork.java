package pacman.controllers.NeuralNetwork;

import pacman.controllers.BehaviourTree.BTPacman;
import pacman.controllers.BehaviourTree.BehaviourTree;
import pacman.game.Constants;
import pacman.game.Constants.MOVE;
import pacman.game.internal.Node;

import javax.swing.plaf.basic.BasicInternalFrameTitlePane;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class NeuralNetwork
{

    static public int pacmanPos, closestGhostPos;
    static public Constants.GHOST closesGhost;

    private String id;

    private NeuralNetLayer inputLayer, outputLayer;
    private ArrayList<NeuralNetLayer> hiddenLayers;

    public NeuralNetwork(String id, NeuralNetLayer inputLayer, ArrayList<NeuralNetLayer> hiddenLayers, NeuralNetLayer outputLayer)
    {
        this.id = id;
        this.inputLayer = inputLayer;
        this.hiddenLayers = hiddenLayers;
        this.outputLayer = outputLayer;
    }

    public MOVE Update()
    {
        MOVE currentMove = MOVE.NEUTRAL;

        inputLayer.updateValues(true);

        for (int i = 0; i < hiddenLayers.size(); i++)
        {
            hiddenLayers.get(i).updateValues(false);
        }

        outputLayer.updateValues(false);

        switch (outputLayer.getHighestValueNeuron())
        {
            case "Evade Ghost":
                currentMove = EvadeGhostMove();
                break;
            case "Eat Pill":
                currentMove = EatPill();
                break;
        }

        //System.out.println("Current move: " + currentMove + "\n");
        return currentMove;
    }

    private void printWeights()
    {

    }

    private MOVE EvadeGhostMove()
    {
        //MOVE currentMove = NNPacman.game.getNextMoveTowardsTarget(NNPacman.pacmanPos, NNPacman.closestGhostPos, Constants.DM.PATH).opposite();
        MOVE currentMove = NNPacman.game.getNextMoveAwayFromTarget(NNPacman.pacmanPos, NNPacman.closestGhostPos, Constants.DM.PATH);
        //System.out.println("Pacmans position: " + NNPacman.pacmanPos + "\t Closest ghost position: " + NNPacman.closestGhostPos);
        //System.out.println("Closest ghost distance: " + NNPacman.closestGhostDistance);
        //System.out.println("Current output: EvadeGhost");
        return currentMove;
    }

    private MOVE EatPill()
    {
        MOVE currentMove = NNPacman.game.getNextMoveTowardsTarget(NNPacman.pacmanPos, NNPacman.closestPillPos, Constants.DM.PATH);
        //System.out.println("Pacmans position: " + NNPacman.pacmanPos + "\t Closest pill position: " + NNPacman.closestPillPos);
        //System.out.println("Closest pill distance: " + NNPacman.closestPillDistance);
        //System.out.println("Current output: EatPill");
        return currentMove;
    }
}