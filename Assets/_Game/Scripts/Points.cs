using System;
using UnityEngine;

namespace BoundfoxStudios.SwingingTower
{
  public class Points : MonoBehaviour
  {
    public AudioSource FallSound;
    public AudioSource KaboomSound;
    public AudioSource BoingSound;
    
    [HideInInspector]
    public float PointsToGet;
    private GameController _gameController;
    private Rigidbody _rigidbody;

    private bool _hasAddedPoints;
    private bool _hasBeenReleased;

    private void Start()
    {
      _gameController = FindObjectOfType<GameController>();
      _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
      if (_hasBeenReleased && !_hasAddedPoints && _rigidbody.IsSleeping())
      {
        _hasAddedPoints = true;
        _gameController.UpdatePoints(PointsToGet);
      }
    }

    private void FixedUpdate()
    {
      _rigidbody.inertiaTensorRotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
      _gameController.UpdatePoints(-PointsToGet);
    }

    public void Release()
    {
      _hasBeenReleased = true;
      FallSound.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
      if (FallSound.isPlaying)
      {
        FallSound.Stop();
      }
      
      BoingSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Death"))
      {
        Destroy(gameObject, 5f);
        KaboomSound.Play();
      }
    }
  }
}
