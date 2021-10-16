using System;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private ParticleSystem _deathEffect;

    public Action<Dummy> onDestroyed;
    public Action<Dummy> onCleanup;
    public Rigidbody Rigidbody => _rigidbody;

    public GameObject Model => _model;

    // Update is called once per frame
    void Update()
    {
        OrientModel();
    }

    private void OrientModel()
    {
        Model.transform.rotation = Quaternion.Euler(Vector3.forward);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Obstacle") || other.collider.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        _collider.enabled = false;
        _model.gameObject.SetActive(false);
        _deathEffect.gameObject.SetActive(true);
        _deathEffect.Play();
        onDestroyed?.Invoke(this);
        Invoke("Cleanup", 1);
    }

    private void Cleanup()
    {
        _collider.enabled = true;
        _deathEffect.gameObject.SetActive(false);
        _model.gameObject.SetActive(true);
        onCleanup?.Invoke(this);
    }
}
