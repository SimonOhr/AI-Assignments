package pacman.controllers.BehaviourTree;

import java.util.EnumMap;
import java.util.Random;

import pacman.controllers.Controller;
import pacman.game.Game;
import pacman.game.Constants.GHOST;
import pacman.game.Constants.MOVE;
import pacman.game.Constants.NODESTATUS;

public final class BTPacman extends Controller<MOVE> {
    static public Game game;
    static public MOVE myMove;
    BehaviourTree behaviourTree;

    public BTPacman() {
        behaviourTree = new BehaviourTree();
    }

    public MOVE getMove(Game game, long timeDue) {
        this.game = game;
        behaviourTree.UpdateNodes();
        return myMove;
    }

}
