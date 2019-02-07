package pacman.controllers.BehaviourTree.Leaves;

import pacman.controllers.BehaviourTree.BTNode;
import pacman.controllers.BehaviourTree.BTPacman;
import pacman.controllers.BehaviourTree.BehaviourTree;
import pacman.game.Constants;

public class LeafEatPills extends BTNode {
    int[] activePillsIndices, normalPillsIndices, powerPillsIndices;
    int closestPillPos, shortestDistance;

    @Override
    public Constants.NODESTATUS UpdateNodeStatus() {
        System.out.print("\n Eat pills starts \n");
        normalPillsIndices = BTPacman.game.getActivePillsIndices();
        powerPillsIndices = BTPacman.game.getActivePowerPillsIndices();
        activePillsIndices = new int[normalPillsIndices.length + powerPillsIndices.length];
        int i;
        for (i = 0; i < normalPillsIndices.length; i++) {
            activePillsIndices[i] = normalPillsIndices[i];
        }
        for (int j = 0; j < powerPillsIndices.length; j++) {
            activePillsIndices[i] = powerPillsIndices[j];
        }

        closestPillPos = activePillsIndices[0];
        shortestDistance = BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, activePillsIndices[0]);
        int tempDistance;
        for (int j = 1; j < activePillsIndices.length; j++) {
            tempDistance = BTPacman.game.getShortestPathDistance(BehaviourTree.pacmanPos, activePillsIndices[j]);
            if (shortestDistance > tempDistance && BehaviourTree.pacmanPos != activePillsIndices[j]) {
                shortestDistance = tempDistance;
                closestPillPos = activePillsIndices[j];
            }
        }

        BTPacman.myMove = BTPacman.game.getNextMoveTowardsTarget(BehaviourTree.pacmanPos, closestPillPos, Constants.DM.PATH);
        currentStatus = (BTPacman.myMove != null) ? Constants.NODESTATUS.SUCCESS : Constants.NODESTATUS.FAILURE;
        System.out.print("Pacmans position: " + BehaviourTree.pacmanPos + "\tPill position: " + closestPillPos);
        System.out.print("\n EatPills returns " + currentStatus.toString() + "\n");
        return currentStatus;
    }
}
