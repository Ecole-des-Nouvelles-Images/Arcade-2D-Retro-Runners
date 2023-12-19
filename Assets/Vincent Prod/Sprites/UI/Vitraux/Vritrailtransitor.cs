using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vincent_Prod.Sprites.UI.Vitraux
{
    public class Vritrailtransitor : MonoBehaviour
    {
        public List<Sprite> vitraux = new List<Sprite>();
        public Image vitrail;
        public Animator vitrailAnimator;
        public int choosedCharacter;

        private IEnumerator TransitionTimer() {
            vitrailAnimator.SetTrigger("Change");
            yield return new WaitForSeconds(0.20f);
            vitrail.sprite = vitraux[choosedCharacter];
        }

        public void ChangeInt(int value) {
            choosedCharacter = value;
            StartCoroutine(TransitionTimer());
        }
    }
}
