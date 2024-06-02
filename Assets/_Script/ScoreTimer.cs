
using UnityEngine;

public class ScoreTimer : Singleton<ScoreTimer>
{

    public float timer { get; private set; }
    bool ticking = false;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    private void OnStateChanged(object sender, GameManager.EventGameStateArgs e)
    {
        if(e.state == GameManager.GameState.Playing)
        {
            ticking = true;
        } else
        {
            ticking = false;
        }
    }

    private void Update()
    {
        if(ticking)
        {
            timer += Time.deltaTime;
        }
    }
}
