using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPipeManager : Singleton<OilPipeManager>
{

    private List<OilPipeGameplay> oilPipes = new List<OilPipeGameplay>();

    public void AddOilPipe(OilPipeGameplay oilPipe)
    {
        oilPipes.Add(oilPipe);
    }

    public void RemoveOilPipe(OilPipeGameplay oilPipe)
    {
        oilPipes.Remove(oilPipe);
    }
}
