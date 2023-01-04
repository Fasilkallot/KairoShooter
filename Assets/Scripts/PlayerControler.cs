using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 7.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationFactor = 5f;
    [SerializeField]
    private Transform barrelTransform;
    [SerializeField]
    private float animationSmoothTime = 0.05f;
    [SerializeField]
    private float animationPlayTransition = 0.15f;
    [SerializeField]
    private Transform aimTarget;
    [SerializeField]
    private float aimDistance = 10f;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip shootingAudioClip;
    [SerializeField]
    private ParticleSystem gunFireFlash;
    [SerializeField]
    private ParticleSystem explotionFire;
    [SerializeField]
    private Animator enemyAnimator;
    [SerializeField]
    private PauseMenu uiScreen;

    private CharacterController controller;
    private PlayerInput input;  
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;

    private Animator anim;
    private int moveXAnimatorParameterId;
    private int moveZAnimatorParameterId;
    private int jumpAnimation;
    public int bulletCount = 10;
    public int score = 0;

    Vector2 CurrentAnimationBlendVector;
    Vector2 animaVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
        shootAction = input.actions["Shoot"];

        Cursor.lockState= CursorLockMode.Locked;
        
        anim = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Pistol Jump");
        moveXAnimatorParameterId = Animator.StringToHash("MoveX");
        moveZAnimatorParameterId = Animator.StringToHash("MoveZ");

    }
    private void OnEnable()
    {
        shootAction.performed += contex => ShootGun();
        shootAction.canceled += contex => ShootGun();
    }
    private void OnDisable()
    {
        shootAction.performed -= contex => ShootGun();
        shootAction.canceled -= contex => ShootGun();
    }
    void ShootGun()
    {
        if (bulletCount > 0)
        {
            audioSource.PlayOneShot(shootingAudioClip);
            gunFireFlash.Play();
            RaycastHit hit;
            //GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
            //BulletControler bulletControler = bullet.GetComponent<BulletControler>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                /*bulletControler.target = hit.point;
                bulletControler.hit = true;*/
                if (hit.collider.tag == "Enemy")
                {
                    enemyAnimator.SetBool("isDead", true);
                    Destroy(hit.collider.gameObject, 2f);
                    score++;
                }
                else if (hit.collider.tag == "Cylinder")
                {
                    explotionFire.transform.position = hit.collider.transform.position;
                    explotionFire.Play();
                    Destroy(hit.collider.gameObject);
                    score++;
                }
            }
            bulletCount--;
        }
        else
        {
            Debug.Log("GameOver");
            uiScreen.GameOver();
        }
        
    }
    void Update()
    {
        Movement();
        PlayerRotation();
        if(score > 5)
        {
            uiScreen.Winner();
        }
        
    }

    void Movement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 playerInput = moveAction.ReadValue<Vector2>();
        CurrentAnimationBlendVector = Vector2.SmoothDamp(CurrentAnimationBlendVector, playerInput, ref animaVelocity, animationSmoothTime);
        Vector3 move = new Vector3(CurrentAnimationBlendVector.x, 0, CurrentAnimationBlendVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            anim.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        anim.SetFloat(moveXAnimatorParameterId, CurrentAnimationBlendVector.x);
        anim.SetFloat(moveZAnimatorParameterId, CurrentAnimationBlendVector.y);

        aimTarget.position  = cameraTransform.position + cameraTransform.forward * aimDistance;
    }
    void PlayerRotation()
    {
        float targetRotation = cameraTransform.eulerAngles.y; 
        Quaternion rotate = Quaternion.Euler(0,targetRotation,0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, rotationFactor);
    }
}
