using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestRayCastHit : MonoBehaviour
{
    private List<RaycastHit> hit = new List<RaycastHit>();
    public RaycastHit[] hitAsArray;
     private void Update() {
         if(Input.GetKeyDown(KeyCode.M)){
         hitAsArray= Physics.RaycastAll(Camera.main.transform.position,Camera.main.transform.forward,300);
         //sort by distance
         if(hitAsArray.Length>0){
         hit= hitAsArray.OrderBy(
             x=> Vector3.Distance(transform.position,x.transform.position)
         ).ToList();
         print("the object near the player is "+ hit[0].collider.gameObject.name);
         }
         }
    }
}
