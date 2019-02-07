package pacman.controllers.BehaviourTree;

import pacman.controllers.BehaviourTree.Leaves.*;
import pacman.game.Constants;
import pacman.game.Constants.MOVE;
import java.util.ArrayList;
import java.util.List;

public class BehaviourTree {
    Selector topSelector, bottomSelector;
    Sequence topSequence, bottomSequence;
    BTNode ghostClose, ghostEdible, appproachGhosts, evadeGhosts, eatPills;

    static public int pacmanPos, closestGhostPos;
    static public Constants.GHOST closesGhost;

    public BehaviourTree() {

        pacmanPos=0;
        closestGhostPos=0;
        List<BTNode> childrenOfComposite = new ArrayList<BTNode>();

        ghostEdible = new LeafGhostEdible();
        appproachGhosts = new LeafApproachGhosts();
        childrenOfComposite.add(ghostEdible);
        childrenOfComposite.add(appproachGhosts);
        bottomSequence = new Sequence(childrenOfComposite);
        childrenOfComposite.clear();

        evadeGhosts = new LeafEvadeGhosts();
        childrenOfComposite.add(bottomSequence);
        childrenOfComposite.add(evadeGhosts);
        bottomSelector = new Selector(childrenOfComposite);
        childrenOfComposite.clear();

        ghostClose = new LeafGhostClose();
        childrenOfComposite.add(ghostClose);
        childrenOfComposite.add(bottomSelector);
        topSequence = new Sequence(childrenOfComposite);
        childrenOfComposite.clear();

        eatPills = new LeafEatPills();
        childrenOfComposite.add(topSequence);
        childrenOfComposite.add(eatPills);
        topSelector = new Selector(childrenOfComposite);
    }

    public MOVE UpdateNodes() {
        pacmanPos = BTPacman.game.getPacmanCurrentNodeIndex();
        topSelector.UpdateNodeStatus();
        return null;
    }
}
