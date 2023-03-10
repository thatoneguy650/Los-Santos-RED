using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangRepSave
{
    public GangRepSave()
    {
    }

    public GangRepSave(string gangID, int reputation, int membersHurt, int membersKilled, int membersCarJacked, int membersHurtInTerritory, int membersKilledInTerritory, int membersCarJackedInTerritory, int playerDebt, bool isMember, bool isEnemy, int tasksCompleted)
    {
        GangID = gangID;
        Reputation = reputation;
        MembersHurt = membersHurt;
        MembersKilled = membersKilled;
        MembersCarJacked = membersCarJacked;
        MembersHurtInTerritory = membersHurtInTerritory;
        MembersKilledInTerritory = membersKilledInTerritory;
        MembersCarJackedInTerritory = membersCarJackedInTerritory;
        PlayerDebt = playerDebt;
        IsMember = isMember;
        IsEnemy = isEnemy;
        TasksCompleted = tasksCompleted;
    }

    public string GangID { get; set; }
    public int Reputation { get; set; }
    public int MembersHurt { get; set; }
    public int MembersKilled { get; set; }
    public int MembersCarJacked { get; set; }

    public int MembersHurtInTerritory { get; set; }
    public int MembersKilledInTerritory { get; set; }
    public int MembersCarJackedInTerritory { get; set; }
    public int PlayerDebt { get; set; } = 0;
    public bool IsMember { get; set; } = false;
    public bool IsEnemy { get; set; } = false;
    public int TasksCompleted { get; set; } = 0;
}

