using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTest
{
    private GameObject Player = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/AlecPlayerStuff/Player.prefab");
    // A Test behaves as an ordinary method
    [Test]
    public void PlayerMovementTestSimplePasses()
    {
        
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerJumpTestWithEnumeratorPasses()
    {
        var playerObject = Object.Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
        //Vector3 StartingPoint = playerObject.transform.position;
        var MovementScript = playerObject.GetComponent<CharacterMove>();
        playerObject.GetComponent<CharacterMove>().CauseJump();
        Assert.IsTrue(playerObject.GetComponent<Rigidbody>().velocity.y>0);
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
    [UnityTest]
    public IEnumerator PlayerDashTestWithEnumeratorPasses()
    {
        var playerObject = Object.Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
        //Vector3 StartingPoint = playerObject.transform.position;
        var MovementScript = playerObject.GetComponent<CharacterMove>();
        playerObject.GetComponent<CharacterMove>().CauseDash();
        Assert.IsTrue(playerObject.GetComponent<Rigidbody>().velocity.x > 0);
        //Assert();
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerGrappleTestWithEnumeratorPasses()
    {
        var playerObject = Object.Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
        //playerObject.transform.localRotation.eulerAngles 


        yield return null;
    }

}
