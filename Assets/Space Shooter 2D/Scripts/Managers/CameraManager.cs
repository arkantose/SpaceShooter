using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Camera Shake Help Variables
    private WaitForSeconds _delay;
    private Vector3 _newPos;
    private Vector3 _invertXYPos;
    private Vector3 _newRot;
    //Camera Original Position Values
    private float _originalFOV;
    private Vector3 _originalPos;
    //Simulate Camera Shake Variables new
    [SerializeField]
    private float _newFOV = 58.5f;
    [SerializeField]
    private float _newXPos = 0.25f;
    [SerializeField]
    private float _newYPos = 0.25f;
    [SerializeField]
    private float _newRotation = 2f;
    [SerializeField]
    private float _duration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize original Values
        _originalFOV = Camera.main.fieldOfView;
        _originalPos = Camera.main.transform.position;
        //Initialize Help Variables with the new values
        _delay = new WaitForSeconds(_duration / 3f);
        _newPos = new Vector3(_newXPos, -_newYPos, Camera.main.transform.position.z);
        _invertXYPos = new Vector3(-1, -1, 1);
        _newRot = new Vector3(0, 0, _newRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShakeCamera()
    {
        //Change values between delays to simulate camera shake
        Camera.main.fieldOfView = _newFOV;
        yield return _delay;

        Camera.main.transform.position = _newPos;
        Camera.main.transform.eulerAngles = _newRot;
        yield return _delay;

        Camera.main.transform.position = Vector3.Scale(_invertXYPos, _newPos);
        Camera.main.transform.eulerAngles = -_newRot;
        yield return _delay;

        //return to normal state
        Camera.main.fieldOfView = _originalFOV;
        Camera.main.transform.position = _originalPos;
        Camera.main.transform.eulerAngles = Vector3.zero;
    }
}
