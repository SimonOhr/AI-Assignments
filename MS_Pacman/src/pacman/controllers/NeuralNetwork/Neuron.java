package pacman.controllers.NeuralNetwork;

import java.util.ArrayList;
import java.util.List;

public class Neuron
{
    private double value;
    private String id;
    protected ArrayList<NeuronsConnection> inputConnections, outputConnections;

    public Neuron(String id, float value)
    {
        this.id = id;
        this.value = value;
        this.inputConnections = new ArrayList<>();
        this.outputConnections = new ArrayList<>();
    }

    public Neuron(String id)
    {
        this.id = id;
        this.inputConnections = new ArrayList<>();
        this.outputConnections = new ArrayList<>();
    }

    public void addInputConnection(Neuron fromNeuron, double connectionWeight)
    {
        NeuronsConnection inputConnection = new NeuronsConnection(fromNeuron, this, connectionWeight);
        inputConnection.getFromNeuron().addOutputConnection(inputConnection);
        inputConnections.add(inputConnection);
    }

    public void addOutputConnection(NeuronsConnection outputConnection)
    {
        outputConnections.add(outputConnection);
    }

    public ArrayList<NeuronsConnection> getInputConnections()
    {
        return inputConnections;
    }

    public double getValue()
    {
        return value;
    }

    public void setValue(float value)
    {
        this.value = value;
    }

    public String getId()
    {
        return id;
    }

    public void calculateNewOutput()
    {
        value = 0;
        for (int i = 0; i < inputConnections.size(); i++)
        {
            value += inputConnections.get(i).getWeightedInput();
        }
        value = sigmoidification(value);
    }

    private double sigmoidification(double x)
    {
        return 1 / (1 + Math.pow(Math.E, -x));
    }
}