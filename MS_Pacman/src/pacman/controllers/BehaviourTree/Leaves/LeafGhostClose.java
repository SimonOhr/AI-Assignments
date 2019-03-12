package pacman.controllers.BehaviourTree.Leaves;

import pacman.controllers.BehaviourTree.BTNode;
import pacman.controllers.BehaviourTree.BTPacman;
import pacman.controllers.BehaviourTree.BehaviourTree;
import pacman.game.Constants;
import pacman.game.Constants.GHOST;
import pacman.game.Constants.NODESTATUS;

public class LeafGhostClose extends BTNode {
    int closeDistance = 20;
    int ghostPos, shortestDistanceToGhost;

    @Override
    public NODESTATUS UpdateNodeStatus() {
        //Input 1: Get distance to the closest ghost
        int closestGhostDistance = Integer.MAX_VALUE;
        for (Constants.GHOST ghost : Constants.GHOST.values())
        {
            if (BTPacman.game.getGhostEdibleTime(ghost) == 0 && BTPacman.game.getGhostLairTime(ghost) == 0)
            {
                if (BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, BTPacman.game.getGhostCurrentNodeIndex(ghost))
                        < closestGhostDistance)
                {
                    BehaviourTree.closestGhostPos = BTPacman.game.getGhostCurrentNodeIndex(ghost);
                    BehaviourTree.closesGhost=ghost;
                    closestGhostDistance = BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, BehaviourTree.closestGhostPos);
                }
            }
        }

        currentStatus = (closestGhostDistance <= closeDistance) ? NODESTATUS.SUCCESS : NODESTATUS.FAILURE;
        if (currentStatus == NODESTATUS.SUCCESS) {
            System.out.print("\n Shortest distance to ghost: " + closestGhostDistance + "\n");
        }
        System.out.print("\n GhostClose returns " + currentStatus.toString() + "\n");
        return currentStatus;


//        shortestDistanceToGhost=Integer.MAX_VALUE;
//
//        ghostPos = BTPacman.game.getGhostCurrentNodeIndex(GHOST.BLINKY);
//        BehaviourTree.closestGhostPos = ghostPos;
//        BehaviourTree.closesGhost=GHOST.BLINKY;
//        shortestDistanceToGhost = BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, ghostPos);
//
//        System.out.print("\n Blinkys position: " + ghostPos);
//        System.out.print("\n Pacmans position: " + BehaviourTree.pacmanPos);
//        System.out.print("\n Distance to Blinky: " + shortestDistanceToGhost);
//
//        ghostPos = BTPacman.game.getGhostCurrentNodeIndex(GHOST.PINKY);
//        if (shortestDistanceToGhost > BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, ghostPos)) {
//            shortestDistanceToGhost = BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, ghostPos);
//            BehaviourTree.closestGhostPos = ghostPos;
//            BehaviourTree.closesGhost=GHOST.PINKY;
//            System.out.print("\n Distance to Pinky is shorter: " + shortestDistanceToGhost);
//        }
//        ghostPos = BTPacman.game.getGhostCurrentNodeIndex(GHOST.INKY);
//        if (shortestDistanceToGhost > BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, ghostPos)) {
//            shortestDistanceToGhost = BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, ghostPos);
//            BehaviourTree.closestGhostPos = ghostPos;
//            BehaviourTree.closesGhost=GHOST.INKY;
//            System.out.print("\n Distance to Inky is shorter: " + shortestDistanceToGhost);
//        }
//        ghostPos = BTPacman.game.getGhostCurrentNodeIndex(GHOST.SUE);
//        if (shortestDistanceToGhost > BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, ghostPos)) {
//            shortestDistanceToGhost = BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, ghostPos);
//            BehaviourTree.closestGhostPos = ghostPos;
//            BehaviourTree.closesGhost=GHOST.SUE;
//            System.out.print("\n Distance to Sue is shorter: " + shortestDistanceToGhost);
//        }
//
//        currentStatus = (shortestDistanceToGhost <= closeDistance) ? NODESTATUS.SUCCESS : NODESTATUS.FAILURE;
//        if (currentStatus == NODESTATUS.SUCCESS) {
//            System.out.print("\n Shortest distance to ghost: " + shortestDistanceToGhost + "\n");
//        }
//        System.out.print("\n GhostClose returns " + currentStatus.toString() + "\n");
//        return currentStatus;
    }
}
