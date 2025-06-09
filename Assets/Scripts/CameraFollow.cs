using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float endScreen;
    [SerializeField] private Transform player;

    private void Update()
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y + endScreen, -10f);
        transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
    }
}
