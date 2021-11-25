using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region Unityfunc
    // Update is called once per frame
    void Update()
    {
        Vector3 moved = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y, transform.position.z);
        transform.position = moved;
    }
    #endregion
}
