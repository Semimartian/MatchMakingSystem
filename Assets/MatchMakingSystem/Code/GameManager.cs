using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    List<Player> players = new List<Player>();
    public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}
