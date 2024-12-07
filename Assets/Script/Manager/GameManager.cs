using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playStats;

    List<IEndGameObsever> endGameObsevers = new List<IEndGameObsever>();


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RigisterPlayer(CharacterStats player)
    {
        playStats = player;
    }

    public void AddObserver(IEndGameObsever obsever)
    {
        endGameObsevers.Add(obsever);
    }

    public void RemoveObserver(IEndGameObsever obsever)
    {
        endGameObsevers.Remove(obsever);
    }

    public void NotifyObserver()
    {
        foreach (var observer in endGameObsevers)
        {
            observer.EndNotify();
        }
    }
}
