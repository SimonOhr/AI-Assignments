package pacman.controllers.NeuralNetwork;

public class NeuronsConnection
{
    protected Neuron fromNeuron, toNeuron;
    protected double weight;

    public NeuronsConnection(Neuron fromNeuron, Neuron toNeuron, double weight)
    {
        this.fromNeuron = fromNeuron;
        this.toNeuron = toNeuron;
        this.weight = weight;
    }

    public double getWeight()
    {
        return weight;
    }

    public void setWeight(double weight)
    {
        this.weight = weight;
    }

    //public double getInput() {
    //    return 0f; //Add fromNeuron.calculateoutput
    //}

    public double getWeightedInput()
    {
        return weight * fromNeuron.getValue();
    }

    public Neuron getFromNeuron()
    {
        return fromNeuron;
    }

    public Neuron getToNeuron()
    {
        return toNeuron;
    }
}