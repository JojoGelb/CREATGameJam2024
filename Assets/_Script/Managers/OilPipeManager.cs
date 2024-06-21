using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OilPipeManager : Singleton<OilPipeManager>
{

    private List<OilPipeGameplay> oilPipes = new List<OilPipeGameplay>();

    public UnityEvent<int> OnOilPipeCountChanged = new UnityEvent<int>();

    public void AddOilPipe(OilPipeGameplay oilPipe)
    {
        oilPipes.Add(oilPipe);
        OnOilPipeCountChanged.Invoke(oilPipes.Count);
    }

    public void RemoveOilPipe(OilPipeGameplay oilPipe)
    {
        oilPipes.Remove(oilPipe);
        OnOilPipeCountChanged.Invoke(oilPipes.Count);
    }

    public List<OilPipeGameplay> GetOliPipesInGame()
    {
        return oilPipes;
    }
}
