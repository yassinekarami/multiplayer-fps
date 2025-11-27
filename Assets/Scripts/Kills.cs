using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kills : IComparable<Kills>
{
    public string playerName;
    public int playerKills;

    public Kills(string newPlayerName, int newPlayerScore)
    {
        this.playerName = newPlayerName;
        this.playerKills = newPlayerScore;
    }

    public int CompareTo(Kills other)
    {
        return other.playerKills - this.playerKills;
    }
}
