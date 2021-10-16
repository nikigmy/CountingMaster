using UnityEngine;
using UnityEngine.Events;

public class CollisionListener : MonoBehaviour
{
    [System.Serializable]
    public class StateChangedEvent : UnityEvent<Collider> { }

    public StateChangedEvent onTriggerEnter;
    public StateChangedEvent onTriggerStay;
    public StateChangedEvent onTriggerExit;
    public StateChangedEvent onCollisionEnter;
    public StateChangedEvent onCollisionStay;
    public StateChangedEvent onCollisionExit;

    [SerializeField] private LayerMask _layerMask;
    public void Init(LayerMask layerMask)
    {
        _layerMask = layerMask;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_layerMask == (_layerMask | (1 << other.gameObject.layer))) //fits in layer mask, should be setup properly in editor
        {
            onTriggerEnter?.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_layerMask == (_layerMask | (1 << other.gameObject.layer)))
        {
            onTriggerStay?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_layerMask == (_layerMask | (1 << other.gameObject.layer)))
        {
            onTriggerExit?.Invoke(other);
        }
    }

    private void OnCollisionEnter(Collision other)
    
    {if (_layerMask == (_layerMask | (1 << other.collider.gameObject.layer))) //fits in layer mask, should be setup properly in editor
        {
            onCollisionEnter?.Invoke(other.collider);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (_layerMask == (_layerMask | (1 << other.collider.gameObject.layer))) //fits in layer mask, should be setup properly in editor
        {
            onCollisionStay?.Invoke(other.collider);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (_layerMask == (_layerMask | (1 << other.collider.gameObject.layer))) //fits in layer mask, should be setup properly in editor
        {
            onCollisionExit?.Invoke(other.collider);
        }
    }
}
