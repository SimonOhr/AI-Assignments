package pacman.controllers.BehaviourTree.Leaves;

import pacman.controllers.BehaviourTree.BTNode;
import pacman.controllers.BehaviourTree.BTPacman;
import pacman.controllers.BehaviourTree.BehaviourTree;
import pacman.game.Constants;
import pacman.game.Constants.NODESTATUS;

public class LeafEvadeGhosts extends BTNode {
    @Override
    public NODESTATUS UpdateNodeStatus() {
        //BTPacman.myMove = BTPacman.game.getNextMoveAwayFromTarget(BehaviourTree.pacmanPos, BehaviourTree.closestGhostPos, Constants.DM.PATH);
        //BTPacman.myMove = BTPacman.game.getNextMoveTowardsTarget(BehaviourTree.pacmanPos, BehaviourTree.closestGhostPos, Constants.DM.PATH).opposite();

        BTPacman.myMove=BTPacman.game.getNextMoveAwayFromTarget(BehaviourTree.pacmanPos, BehaviourTree.closestGhostPos, Constants.DM.PATH);

        System.out.print("\n Pacmans position: " + BehaviourTree.pacmanPos + "\t Closest ghost position: " + BehaviourTree.closestGhostPos + "\n");
        System.out.print(" Pacman moves " + BTPacman.myMove.toString());
        currentStatus = (BTPacman.myMove != null) ? NODESTATUS.SUCCESS : NODESTATUS.FAILURE;
        System.out.print("\n EvadeGhosts returns " + currentStatus.toString() + "\n");
        return currentStatus;
    }
}