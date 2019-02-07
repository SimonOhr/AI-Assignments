package pacman.controllers.BehaviourTree.Leaves;

import pacman.controllers.BehaviourTree.BTNode;
import pacman.controllers.BehaviourTree.BTPacman;
import pacman.controllers.BehaviourTree.BehaviourTree;
import pacman.game.Constants;
import pacman.game.Constants.GHOST;
import pacman.game.Constants.NODESTATUS;
import pacman.game.internal.Ghost;

public class LeafGhostEdible extends BTNode {
    boolean ghostEdible;

    @Override
    public NODESTATUS UpdateNodeStatus() {
        ghostEdible = (BTPacman.game.isGhostEdible(BehaviourTree.closesGhost)) ? true : false;

        //if (BTPacman.game.isGhostEdible(GHOST.BLINKY) || BTPacman.game.isGhostEdible(GHOST.PINKY)
        //      || BTPacman.game.isGhostEdible(GHOST.INKY) || BTPacman.game.isGhostEdible(GHOST.SUE)) {
        //ghostEdible = true;
        //}
        currentStatus = ghostEdible ? NODESTATUS.SUCCESS : NODESTATUS.FAILURE;
        System.out.print("\n GhostEdible returns " + currentStatus.toString() + "\n");
        return currentStatus;
    }
}
