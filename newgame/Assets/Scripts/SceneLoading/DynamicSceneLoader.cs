using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicSceneLoader : MonoBehaviour
{
    [SerializeField] private List<Transform> sceneTransforms;
    [SerializeField] private float loadDistance;

    private void Update()
    {
        for(int i = 0; i < sceneTransforms.Count; i++)
        {
            Transform currentSceneTransform = sceneTransforms[i];
            float distance = Vector3.Distance(transform.position, currentSceneTransform.position);


            if(distance <= loadDistance)
            {
                SceneTransform st = currentSceneTransform.gameObject.GetComponent<SceneTransform>();
                if (!st.isLoaded)
                {
                    SceneManager.LoadSceneAsync(st.buildIndex, LoadSceneMode.Additive);
                    st.isLoaded = true;
                }
            }
            else
            {
                SceneTransform st = currentSceneTransform.gameObject.GetComponent<SceneTransform>();
                if (st.isLoaded)
                {
                    Scene sc = SceneManager.GetSceneByName(st.sceneName);
                    SceneManager.UnloadSceneAsync(sc.buildIndex);
                    st.isLoaded = false;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, loadDistance);
    }
}
