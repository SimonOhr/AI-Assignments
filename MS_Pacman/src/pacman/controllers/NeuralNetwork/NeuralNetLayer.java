package pacman.controllers.NeuralNetwork;
import java.util.ArrayList;

public class NeuralNetLayer
{
    private String id;
    protected ArrayList<Neuron> neurons;

    public NeuralNetLayer(String id, ArrayList<Neuron> neurons){
        this.id=id;
        this.neurons=neurons;
    }

    public String getHighestValueNeuron(){
        double highestValue = Double.MIN_VALUE;
        int index = Integer.MIN_VALUE;
        for (int i = 0; i < neurons.size(); i++) {
            //System.out.println(neurons.get(i).getId() + ": Activationvalue = " + neurons.get(i).getValue());
            if (neurons.get(i).getValue() > highestValue){
                highestValue=neurons.get(i).getValue();
                index=i;
            }
        }
        //System.out.println("\n");
        return neurons.get(index).getId();
    }

    public void updateValues(boolean inputLayer){
        if (inputLayer)
            updateValuesForInputlayer();
        else
            updateValues();
    }

    private void updateValues(){
        for (int i=0; i<neurons.size(); i++){
            neurons.get(i).calculateNewOutput();
        }
    }

    private void updateValuesForInputlayer(){
        for (int i=0; i<neurons.size(); i++){
            switch (neurons.get(i).getId()){
                case "Distance to Ghost":
                    neurons.get(i).setValue(NNPacman.closestGhostDistance);
                    break;
                case "Distance to Pill":
                    neurons.get(i).setValue(NNPacman.closestPillDistance);
                    break;
            }
        }
    }
}
