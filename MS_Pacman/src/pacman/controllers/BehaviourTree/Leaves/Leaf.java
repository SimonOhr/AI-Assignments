package pacman.controllers.BehaviourTree.Leaves;

import pacman.controllers.BehaviourTree.BTNode;
import pacman.game.Constants;

import java.util.Arrays;

public final class Leaf extends BTNode {

    public Constants.NODESTATUS UpdateNodeStatus() {
        return Constants.NODESTATUS.RUNNING;
    }

    public Constants.MOVE GetMove() {
        return null;
    }

    public  void Reset(){
        System.out.print(" Reset Leaf ");
    }
}
