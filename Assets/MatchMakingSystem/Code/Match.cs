using Mirror;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

[Serializable]
public class Match
{

    [Flags]public enum StateFlags
    {
        Public = 1, Waiting = 2
    }
    //TODO: Add settings
    public string id;
    public List<Player> players;
    public Player host;
    public StateFlags states;

    public Match(string id, Player host)
    {
        this.id = id;
        players = new List<Player>();
        players.Add(host);
        this.host = host;
        states = StateFlags.Waiting;
    }

    public Match() { }
}

/*[Serializable]
public class SyncListPlayer : SyncList<Player> { };*/

public static class MatchExtensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] input = Encoding.Default.GetBytes(id);
        byte[] hash = provider.ComputeHash(input);
        return new Guid(hash);
    }
}