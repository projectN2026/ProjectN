using System;
using System.Collections;
using UnityEngine;

public class WaveUpdater : BaseBehaviour
{
    private int _wave = 1;
    private float _remainingTime = 20f;
    private event Action _onChangeWave;

    public int Wave
    {
        get { return _wave; }
        set { _wave = value; _onChangeWave?.Invoke(); }
    }
    public float RemainingTime
    {
        get { return _remainingTime; }
        set { _remainingTime = value; }
    }

    protected override void Start()
    {
        base.Start();
    
        StartCoroutine(UpdateEnemySpawn());
    }
    protected override void Update()
    {
        base.Update();

        UpdateRemainingTime();
    }

    private void UpdateRemainingTime()
    {
        RemainingTime -= Time.deltaTime;

        if (RemainingTime <= 0)
        {
            Wave++;
            RemainingTime = 20f;
            Managers.PopupManager.ShowPopup<CardSelectPopup>();
        }
    }
    private IEnumerator UpdateEnemySpawn()
    {
        var positions = new Vector2[] { new(-10, 0), new(10, 0) };

        while (true)
        {
            var spawnCount = UnityEngine.Random.Range(3, 5);
            for (int i = 0; i < spawnCount; i++)
            {
                var spawnPosition = positions[UnityEngine.Random.Range(0, positions.Length)];
                ObjectManager.SummonEnemy(spawnPosition);

                var shortInterval = UnityEngine.Random.Range(0.5f, 3f);
                yield return new WaitForSeconds(shortInterval);
            }


            yield break;
        }
    }
}
