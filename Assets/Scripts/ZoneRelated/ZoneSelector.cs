// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class ZoneSelector : MonoBehaviour
// {
//     [SerializeField] Camera mainCam;
//     [SerializeField] float range = 100f;
//     void Update()
//     {
//         ProcessRayCasting();
//     }
//     void ProcessRayCasting()
//     {
//         Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
//         RaycastHit hit;
//         if (Physics.Raycast(ray, out hit, range))
//         {
//             Debug.Log(hit.transform.name);
//         }
//     }
// }
