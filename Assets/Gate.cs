using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gate : MonoBehaviour
{
    [SerializeField] private List<GateSetup> _gateSetups;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _leftText;
    [SerializeField] private TextMeshProUGUI _rightText;
    [SerializeField] private GameObject _leftGate;
    [SerializeField] private GameObject _rightGate;
    private bool _passed;

    private int _currentSetupIndex;
    private void Awake()
    {
        SetupGate(Random.Range(0, _gateSetups.Count - 1));
        _canvas.worldCamera = Camera.main;
    }

    private void SetupGate(int setupIndex)
    {
        _currentSetupIndex = setupIndex;
        var leftPrefix = _gateSetups[_currentSetupIndex].leftGateType == GateSetup.GateType.Add ? "+" : "*";
        var rightPrefix = _gateSetups[_currentSetupIndex].rightGateType == GateSetup.GateType.Add ? "+" : "*";
        _leftText.text = leftPrefix + _gateSetups[_currentSetupIndex].leftAmount;
        _rightText.text = rightPrefix + _gateSetups[_currentSetupIndex].rightAmount;
    }

    public void LeftGateTriggerEnter(Collider collider)
    {
        if (!_passed && collider.CompareTag("Dummy"))
        {
            if (_gateSetups[_currentSetupIndex].leftGateType == GateSetup.GateType.Add)
            {
                PlayerController.Instance.AddCount(_gateSetups[_currentSetupIndex].leftAmount);
            }
            else
            {
                PlayerController.Instance.MultiplyCount(_gateSetups[_currentSetupIndex].leftAmount);
            }
            _leftGate.gameObject.SetActive(false);
            _leftText.gameObject.SetActive(false);
            _passed = true;
        }
    }
    public void RightGateTriggerEnter(Collider collider)
    {
        if (!_passed && collider.CompareTag("Dummy"))
        {
            if (_gateSetups[_currentSetupIndex].rightGateType == GateSetup.GateType.Add)
            {
                PlayerController.Instance.AddCount(_gateSetups[_currentSetupIndex].rightAmount);
            }
            else
            {
                PlayerController.Instance.MultiplyCount(_gateSetups[_currentSetupIndex].rightAmount);
            }

            _rightGate.gameObject.SetActive(false);
            _rightText.gameObject.SetActive(false);
            _passed = true;
        }
    }
    
}