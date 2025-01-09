using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private List<Gold> _collectedGold = new List<Gold>();

    [SerializeField] private Scanner _scanner;

    public event UnityAction ResourseCollected;
    
    public int CollectedResounrseAmount => _collectedGold.Count;
    

    private void Start()
    {
        foreach (Unit unit in _units)
        {
            unit.CollectedResource += CollectResource;
        }
    }
    
    private void Update()
    {
        if (_scanner.Gold.Count > 0)
        {
            for (int i = 0; i < _units.Count; i++)
            {
                if (_units[i].IsStanding)
                {
                    _units[i].SetGold(_scanner.Gold[0]);
                    _scanner.RemoveGold(_scanner.Gold[0]);
                    break;
                }
            }
        }
    }

    private void CollectResource(Gold resource)
    {
        _collectedGold.Add(resource);
        ResourseCollected?.Invoke();
    }

    private void OnDestroy()
    {
        foreach (Unit unit in _units)
        {
            unit.CollectedResource -= CollectResource;
        }
    }
}
