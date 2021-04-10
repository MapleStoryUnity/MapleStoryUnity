using UnityEngine;
using System.Collections;

/** 
 * Example MonoBehaviour. 
 * 
 * Put it to a GameObject and give it some coordinates with `startTweenTo();`.
 */
namespace PeterVuorela.Tweener
{
    public class Example 
        : MonoBehaviour
    {
        private Tweener tweener = new Tweener();

        public void startTweenTo(Vector3 to)
        {
            if (tweener != null)
            {
                // Sets The Position From -> To
                //tweener.easeFromTo(transform.localPosition, to, 1f, Easing.BackEaseInOut, tweenComplete);
            }
        }

        private void tweenComplete()
        {
            Debug.Log("Tween Completed");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
                startTweenTo(Vector3.zero);
            if (Input.GetKeyDown(KeyCode.N))
                startTweenTo(new Vector3(10, 10, 0));

            if (tweener != null && tweener.animating)
            {
                // Updates the Tweener
                tweener.update();

                // Set the Position
                Vector3 newPos = transform.localPosition;
                newPos.x = tweener.progression;
                transform.localPosition = newPos;

                Debug.Log("Progression Percentage: " + tweener.progressPct);
                Debug.Log("Progression Position: " + tweener.progression);
            }
            else
            {
                // Tween not init or its completed
            }
        }
    }
}
