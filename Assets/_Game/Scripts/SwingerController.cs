using System;
using UnityEngine;

namespace BoundfoxStudios.SwingingTower
{
  public class SwingerController : MonoBehaviour
  {
    public float MaximumAngle = 55;
    public float ChildOffset = 7.5f;
    public AnimationCurve Speed;
    public AudioSource AudioSource;

    private bool TryGetSwingingChild(out Rigidbody rigidbody)
    {
      rigidbody = null;
      var child = transform.GetChild(0);

      if (child)
      {
        rigidbody = child.GetComponent<Rigidbody>();
        return true;
      }

      return false;
    }

    private void Start()
    {
      OffsetChild();
    }

    private void Update()
    {
      var speed = Speed.Evaluate(Time.time);
      var zRotation = Mathf.Lerp(-MaximumAngle, MaximumAngle, Mathf.PingPong(Time.time, 1) / speed);

      transform.localRotation = Quaternion.Euler(0, 0, zRotation);
    }

    private void OffsetChild()
    {
      if (!TryGetSwingingChild(out var child))
      {
        return;
      }

      child.gameObject.transform.localPosition = new Vector3(0, -ChildOffset, 0);
    }

    public void Capture(GameObject child)
    {
      AudioSource.Play();
      child.transform.SetParent(transform);
      child.transform.localRotation = Quaternion.identity;
      OffsetChild();
    }

    public void Release(float currentPointsPerItem)
    {
      if (TryGetSwingingChild(out var child))
      {
        AudioSource.Stop();
        child.isKinematic = false;
        child.transform.SetParent(null);
        
        var points = child.GetComponent<Points>();
        points.PointsToGet = currentPointsPerItem;
        points.Release();
      }
    }
  }
}
