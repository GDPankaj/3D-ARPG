using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    float _horizontalInput;
    float _verticalInput;
    bool _mouseButtonDown;
    bool _spaceKeyDown;
    void Update()
    {
        if (!_mouseButtonDown && Time.timeScale!=0)
        {
            _mouseButtonDown = Input.GetMouseButtonDown(0);
        }

        if(!_spaceKeyDown && Time.timeScale != 0)
        {
            _spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable()
    {
        ClearInputCache();
    }

    public float GetHorizontalInput() { return _horizontalInput; }
    public float GetVerticalInput() { return _verticalInput; }

    public bool GetMouseButtonDown() { return _mouseButtonDown;}

    public void SetMouseButtonDown(bool value) {  _mouseButtonDown = value; }

    public bool GetSpaceKeyDown() { return _spaceKeyDown; }
    public void SetSpaceKeyDown(bool value) { _spaceKeyDown = value; }

    public void ClearInputCache()
    {
        _mouseButtonDown = false;
        _horizontalInput = _verticalInput = 0;
        _spaceKeyDown = false;
    }
}
