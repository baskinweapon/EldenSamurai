using System.Collections.Generic;

namespace Saver {
    
    public class GameHistory {
        public Stack<PlayerMemento> history {
            get;
            private set;
        }

        public GameHistory() {
            history = new Stack<PlayerMemento>();
        }
    }
}