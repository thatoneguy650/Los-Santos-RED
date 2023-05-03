using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable()]
public class GameState
{
    public string StateID { get; set; }
    public string StateName { get; set; }
    public string ColorPrefix { get; set; }
    public string ColorName => string.IsNullOrEmpty(ColorPrefix) ? StateName : ColorPrefix + StateName;
    public List<string> SisterStateIDs { get; set; } = new List<string>();
    public GameState()
    {
    }

    public GameState(string stateID, string stateName)
    {
        StateID = stateID;
        StateName = stateName;
    }

    public bool IsSisterState(GameState gameState) => gameState != null && SisterStateIDs != null && SisterStateIDs.Any(x => x == gameState.StateID);

}

