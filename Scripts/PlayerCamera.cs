using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform toFollow;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(toFollow.position.x, this.transform.position.y, -1);
    }
}
