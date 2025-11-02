using UnityEngine;

public class TreeMover : MonoBehaviour
{
    public float speed = 2f;
    public float leftRightEdge = 7.5f;
    [Range(0f, 1f)] public float randomTurnChancePerSecond = 0.05f;

    void Update()
    {
        // simple horizontal movement
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // reverse at edges
        if (transform.position.x < -leftRightEdge && speed < 0) speed = Mathf.Abs(speed);
        else if (transform.position.x > leftRightEdge && speed > 0) speed = -Mathf.Abs(speed);

        // random direction change (chance per second)
        if (Random.value < randomTurnChancePerSecond * Time.deltaTime)
            speed *= -1f;
    }
}
