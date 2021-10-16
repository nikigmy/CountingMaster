using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 CurrentInput => _currentInput;
    
    private int _currentFinger;
    private Vector2 _currentInput;
    private Vector2 _lastPos;

    void Awake()
    {
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if ((_currentFinger == -1 || touch.fingerId != _currentFinger) && touch.phase == TouchPhase.Began)
            {
                _currentFinger = touch.fingerId;
                _lastPos = touch.position;
                _currentInput = Vector2.zero;
            }
            
            if (touch.fingerId == _currentFinger )
            {
                if(touch.phase == TouchPhase.Moved){
                    var delta = touch.position - _lastPos;
                    _lastPos = touch.position;
                    _currentInput += delta;
                }
                else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _currentFinger = -1;
                    _currentInput = Vector2.zero;
                    _lastPos = Vector2.zero;
                }
            }
        }
        else
        {
            _currentInput = Vector2.zero;
        }

        _currentInput = new Vector2(Mathf.Clamp(_currentInput.x, -1, 1.0f), Mathf.Clamp(_currentInput.y, -1, 1.0f));
    }
}
