package pacman.controllers.BehaviourTree;

import java.util.ArrayList;
import java.util.List;

import pacman.game.Constants.MOVE;
import pacman.game.Constants.NODESTATUS;

public class BTNode {

    protected NODESTATUS currentStatus;
    protected List<BTNode> children;

    public BTNode() {
        children = new ArrayList<BTNode>();
        currentStatus = NODESTATUS.RUNNING;
    }

    public NODESTATUS UpdateNodeStatus() {
        return currentStatus;
    }
}