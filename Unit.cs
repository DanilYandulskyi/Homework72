using System;
using UnityEngine;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    private const float DistanceToStop = 0.2f;
    
    [SerializeField] private Vector3 _initialPosition;
   
    private Gold _gold;

    private UnitMover _mover;

    public event Action<Gold> CollectedResource;

    public bool IsResourceCollected { get; private set; } = false;
    public bool IsStanding { get; private set; } = true;

    private void Awake()
    {
        _mover = GetComponent<UnitMover>();
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (_gold != null)
        {
            _mover.Move(_gold.transform.position - transform.position);
            IsStanding = false;
        }

        if (IsResourceCollected & Vector2.SqrMagnitude(transform.position - _initialPosition) <= DistanceToStop)
        {
            OnCollectedResourse(ref _gold);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent(out Gold resource))
        {
            if (resource == _gold)
            {
                IsResourceCollected = true;
                _mover.SetDirection(_initialPosition - transform.position);
                resource.StartFollow(transform);
            }
        }
    }

    public void SetGold(Gold gold)
    {
        _gold = gold;
    }

    private void OnCollectedResourse(ref Gold resource)
    {
        CollectedResource?.Invoke(resource);
        _mover.Stop();
        IsResourceCollected = false;
        IsStanding = true;
        resource.StopFollow();
        resource.Disable();
        resource = null;
    }
}
