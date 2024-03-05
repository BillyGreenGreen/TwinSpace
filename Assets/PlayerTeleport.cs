using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTeleport : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    string side = "holy";
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            if (side == "holy"){
                //ANIMATION HERE LIKE LAST EPOCH WHEN YOU SWITCH AND CHANGE CHROMATIC ABERRATION QUICK UP AND DOWN
                //MAYBE WE DONT NEED TO HAVE THE CAMERA DAMPENING SET TO 0 IF WE HAVE AN ANIMATION TO HIDE IT MOVING
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
                //cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0;
                transform.position = new Vector2(transform.position.x + 80, transform.position.y);
                side = "void";
            }
            else{
                transform.position = new Vector2(transform.position.x - 80, transform.position.y);
                side = "holy";
            }
        }
    }
}
