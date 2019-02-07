package pacman.controllers.BehaviourTree;
import pacman.game.Constants.NODESTATUS;
import java.util.Arrays;
import java.util.List;

public final class Sequence extends BTNode {
    NODESTATUS childStatus;

    public Sequence(List<BTNode> children) {
        for (int i = 0; i < children.size(); i++) {
            this.children.add(children.get(i));
        }
    }

    @Override
    public NODESTATUS UpdateNodeStatus() {
        System.out.print("\n Entered Sequence \n");
        for (int i = 0; i < children.size(); i++) {
            System.out.print("\n Sequence lap nr: " + i + "\n");
            childStatus=children.get(i).UpdateNodeStatus();
            if (childStatus == NODESTATUS.RUNNING) {
                currentStatus = NODESTATUS.RUNNING;
            } else if (childStatus == NODESTATUS.SUCCESS) {
                if (children.size() - 1 <= i) {
                    currentStatus = NODESTATUS.SUCCESS;
                    break;
                } else {
                    currentStatus = NODESTATUS.RUNNING;
                    continue;
                }
            } else if (childStatus == NODESTATUS.FAILURE) {
                currentStatus = NODESTATUS.FAILURE;
                break;
            }
        }
        System.out.print("\n Sequence returns " + currentStatus.toString() + "\n");
        return currentStatus;
    }
}
