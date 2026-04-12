using System;
using System.Collections;
using UnityEngine;

public class WaveUpdater : SingletonBehaviour<WaveUpdater>
{
    private int _wave = 1;
    private float _remainingTime = 20f;
    private event Action _onChangeWave;

    public static event Action OnChangeWave
    {
        add { Instance._onChangeWave += value; }
        remove { Instance._onChangeWave -= value; }
    }
    public static int Wave
    {
        get { return Instance._wave; }
        set { Instance._wave = value; Instance._onChangeWave?.Invoke(); }
    }
    public static float RemainingTime
    {
        get { return Instance._remainingTime; }
        set { Instance._remainingTime = value; }
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
            PopupManager.ShowPopup<CardSelectPopup>();
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
