package pacman.controllers.BehaviourTree;

import pacman.game.Constants.NODESTATUS;

import java.util.Arrays;
import java.util.List;

public final class Selector extends BTNode {
    NODESTATUS childStatus;

    public Selector(List<BTNode> children) {
        for (int i = 0; i < children.size(); i++) {
            this.children.add(children.get(i));
        }
    }

    @Override
    public NODESTATUS UpdateNodeStatus() {
        System.out.print("\n Entered Selector \n");

        for (int i = 0; i < children.size(); i++) {
            System.out.print("\n Selector lap nr: " + i + "\n");
            childStatus = children.get(i).UpdateNodeStatus();
            if (childStatus == NODESTATUS.RUNNING) {
                currentStatus = NODESTATUS.RUNNING;
            } else if (childStatus == NODESTATUS.FAILURE) {
                if (children.size() - 1 <= i) {
                    currentStatus = NODESTATUS.FAILURE;
                    break;
                } else {
                    currentStatus = NODESTATUS.RUNNING;
                    continue;
                }
            } else if (childStatus == NODESTATUS.SUCCESS) {
                currentStatus = NODESTATUS.SUCCESS;
                break;
            }
        }
        System.out.print("\n Selector returns " + currentStatus.toString() + "\n");
        return currentStatus;
    }
}
