using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{
    Vector2 viewInput;

    // rotation
    float cameraRoationX = 0;

    // Other components
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    Camera localCamera;

    private void Awake()
    {
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // up & down cam rotation
        cameraRoationX += viewInput.y + Time.deltaTime * networkCharacterControllerPrototypeCustom.viewUpDownRotationSpeed;
        cameraRoationX = Mathf.Clamp(cameraRoationX, -90, 90);

        localCamera.transform.localRotation = Quaternion.Euler(cameraRoationX, 0, 0);
    }

    // 실제로 네트워크에서 발생하는 작업을 수행
    public override void FixedUpdateNetwork()
    {
        // Get the input the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            // rotation the view
            networkCharacterControllerPrototypeCustom.Rotate(networkInputData.rotationInput);

            // move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            // jump
            if (networkInputData.isJumpPressed)
            {
                networkCharacterControllerPrototypeCustom.Jump();
            }
        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}
