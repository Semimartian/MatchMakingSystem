using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    public static Player localPlayer;
    //[SyncVar] public string matchID;
    private NetworkMatch networkMatch;
    private Match currentMatch = null;

    private void Start()
    {
        if(isLocalPlayer)
        {
            localPlayer = this;
        }
        networkMatch = GetComponent<NetworkMatch>();

    }

    #region Host Match:

    public void HostMatch()
    {
        Cmd_HostMatch();
    }

    [Command]
    private void Cmd_HostMatch()
    {
        Match match = MatchMaker.instance.HostMatch(this,false);
        if (match != null)
        {
            GoToMatch(match);
           /* networkMatch.matchId = match.id.ToGuid();
            //matchID = match.id;
           // TargetRpc_HostMatch(true, match.id);
            TargetRpc_GoToGameScene();*/
        }
        else
        {
            Debug.LogError("Failed to create match!");
          //  TargetRpc_HostMatch(false, null);

        }

    }

   /* [TargetRpc]
    private void TargetRpc_HostMatch(bool success, string matchID)
    {
        Debug.Log($"matchID: {this.matchID} ==  + { matchID}");
    }*/
    #endregion
    #region Join Specific Match:

    public void JoinSpecificMatch(string matchID)
    {
        Cmd_JoinSpecificMatch(matchID);
    }

    [Command]
    private void Cmd_JoinSpecificMatch(string matchID)
    {
        Match match = MatchMaker.instance.JoinSpecificMatch(matchID, this);
        if (match != null)
        {
            // this.matchID = matchID;
            GoToMatch(match);
            //TargetRpc_JoinMatch(true, matchID);
           // TargetRpc_GoToGameScene();
            Debug.Log("joined match: " + matchID);

        }
        else
        {
            Debug.LogError("Failed to join match: " + matchID);
           // TargetRpc_JoinMatch(false, matchID);

        }

    }

    /*  [TargetRpc]
      private void TargetRpc_JoinMatch(bool success, string matchID)
      {
          Debug.Log($"matchID: {this.matchID} ==  + { matchID}");
      }*/
    #endregion

    #region Join Some Match:

    public void JoinSomeMatch()
    {
        Cmd_JoinSomeMatch();
    }

    [Command]
    private void Cmd_JoinSomeMatch()
    {
        Match match = MatchMaker.instance.JoinSomeMatch(this);
        if (match != null)
        {
            // this.matchID = matchID;
            GoToMatch(match);
            //TargetRpc_JoinMatch(true, matchID);
            // TargetRpc_GoToGameScene();
            Debug.Log("joined some match: " + match.id);

        }
        else
        {
            Debug.LogError("Failed to join some match");
            // TargetRpc_JoinMatch(false, matchID);
        }
    }

    /*  [TargetRpc]
      private void TargetRpc_JoinMatch(bool success, string matchID)
      {
          Debug.Log($"matchID: {this.matchID} ==  + { matchID}");
      }*/
    #endregion

    /*#region Start Match:

    public void StartMatch()
    {
        Cmd_StartMatch();
    }

    [Command]
    private void Cmd_StartMatch()
    {
        MatchMaker.StartMatch(matchID );
        Debug.Log("Cmd_StartMatch");

    }

    public void StartGame()
    {
        TargetRpc_StartMatch();

    }


    [TargetRpc]
    private void TargetRpc_StartMatch()
    {
        TargetRpc_GoToGameScene();
    }
    #endregion*/

    [Server]
    private void GoToMatch(Match match)
    {
        currentMatch = match;
        networkMatch.matchId = match.id.ToGuid();
        TargetRpc_GoToMatch();
    }

    [TargetRpc]
    private void TargetRpc_GoToMatch()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        MatchMakingUI.instance.ShowUI(false);
    }

    #region Host Privilige:

    public void SwitchMatchAccessibility()
    {
        Cmd_SwitchMatchAccessibility();
    }

    [Command]
    private void Cmd_SwitchMatchAccessibility()
    {
        if (currentMatch != null)
        {
            TargetRpc_OnMatchAccessibilitySet(MatchMaker.instance.SwitchMatchAccessibility(currentMatch, this));
        }
        else
        {
            Debug.LogError("Failed to create match!");
            //  TargetRpc_HostMatch(false, null);

        }
    }

     [TargetRpc]
     private void TargetRpc_OnMatchAccessibilitySet(Match.StateFlags matchStateFlags)
     {
         HostUI.instance.UpdateMatchAccessibilityText(matchStateFlags);
     }

    [TargetRpc]
    public void TargetRpc_OnBecomeHost(Match.StateFlags matchStateFlags)
    {
        HostUI.instance.ShowUI(true);
        HostUI.instance.UpdateMatchAccessibilityText(matchStateFlags);
    }
    #endregion

    #region Disconnect:
    public void DisconnectGame()
    {
        Cmd_DisconnectGame();
        SceneManager.UnloadSceneAsync(2);
        MatchMakingUI.instance.ShowUI(true);
        HostUI.instance.ShowUI(false);
    }

    [Command]
    private void Cmd_DisconnectGame()
    {
        ServerDisconnect();
    }

    [Server]
    private void ServerDisconnect()
    {
        if(currentMatch !=null)
        {
            MatchMaker.instance.PlayerDisconnected(this, currentMatch.id);
        }
        Rpc_DisconnectGame();
        currentMatch = null;
        networkMatch.matchId = string.Empty.ToGuid();
    }

    [ClientRpc]
    private void Rpc_DisconnectGame()
    {
        ClientDisconnect();
    }

    private void ClientDisconnect()
    {
        
    }
    #endregion
}
