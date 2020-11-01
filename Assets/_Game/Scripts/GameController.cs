using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoundfoxStudios.SwingingTower
{
  public class GameController : MonoBehaviour
  {
    public float SpawnDelay = 3f;
    public GameObject[] Spawnables;
    public float PointsPerItem = 10;
    public float PointDegradationRate = 0.1f;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI ItemScoreText;

    private SwingerController _swingerController;
    private bool _hasItem = true;
    private float _currentPointsPerItem;
    private float _currentScore;

    private void Start()
    {
      _swingerController = FindObjectOfType<SwingerController>();
      _currentPointsPerItem = PointsPerItem;
      
      Spawn();
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        Release();
      }

      if (_hasItem)
      {
        _currentPointsPerItem = Mathf.Max(0, _currentPointsPerItem - PointDegradationRate * Time.deltaTime);
        ItemScoreText.text = _currentPointsPerItem.ToString("0.0");
      }
    }

    private void Release()
    {
      _hasItem = false;
      
      _swingerController.Release(_currentPointsPerItem);

      StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
      yield return new WaitForSecondsRealtime(SpawnDelay);

      Spawn();
    }

    private void Spawn()
    {
      var child = Instantiate(Spawnables[Random.Range(0, Spawnables.Length)]);
      _swingerController.Capture(child);
      _hasItem = true;
      _currentPointsPerItem = PointsPerItem;
    }

    public void UpdatePoints(float points)
    {
      _currentScore += points;
      ScoreText.text = _currentScore.ToString("0.0");
    }
  }
}
