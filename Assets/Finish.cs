using UnityEngine;

public class Finish : MonoBehaviour
{
    public void OnFinish(Collider collider)
    {
        if (collider.CompareTag("Dummy") && GameManager.Instance.currentGameState == GameState.PLAYING)
        {
            GameManager.Instance.Finish();
        }
    }
}
