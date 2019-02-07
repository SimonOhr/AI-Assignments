package pacman.controllers.BehaviourTree.Leaves;

import pacman.controllers.BehaviourTree.BTNode;
import pacman.controllers.BehaviourTree.BTPacman;
import pacman.controllers.BehaviourTree.BehaviourTree;
import pacman.game.Constants;
import pacman.game.Constants.NODESTATUS;

public class LeafApproachGhosts extends BTNode {
    @Override
    public NODESTATUS UpdateNodeStatus() {
        BTPacman.myMove = BTPacman.game.getNextMoveTowardsTarget(BehaviourTree.pacmanPos, BehaviourTree.closestGhostPos, Constants.DM.PATH);
        currentStatus = (BTPacman.myMove != null) ? NODESTATUS.SUCCESS : NODESTATUS.FAILURE;
        System.out.print("\n ApproachGhosts returns " + currentStatus.toString() + "\n");
        return currentStatus;
    }
}
