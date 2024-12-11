using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMovementCharacter : MonoBehaviour
{
    public Animator CharacterAnimator;
    

    public float smoothSwitchAnimation = 0.2f;

    public float speed = 10;
    public float gravity = 20.0f;
    
    public float mouseSensitivity = 100.0f;

    private float rotY = 0.0f;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;    
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        MouseLook();
        MovementCharacter();
        Attack();
    }

    void MovementCharacter()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        CharacterAnimator.SetFloat("MoveX", moveHorizontal, smoothSwitchAnimation, Time.deltaTime);
        CharacterAnimator.SetFloat("MoveZ", moveVertical, smoothSwitchAnimation, Time.deltaTime);    
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int AttackNumber = Random.Range(0, 2);
            CharacterAnimator.SetTrigger("AttackState");
            CharacterAnimator.SetInteger("AttackNumber", AttackNumber);
        }
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;

        Quaternion localRotationY = Quaternion.Euler(0.0f, rotY, 0.0f);

        transform.rotation = localRotationY;
    }
}
