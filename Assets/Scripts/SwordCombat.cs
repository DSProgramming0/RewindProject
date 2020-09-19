using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCombat : MonoBehaviour
{
    [SerializeField] private Transform parentHolder;

    [SerializeField] private Animator anim;
    AnimatorStateInfo animState;
    [SerializeField] bool canAttack = true;

    // Start is called before the first frame update
    [SerializeField] private float amount = 0.02f;
    [SerializeField] private float maxamount = 0.03f;
    [SerializeField] private float smooth = 3;
    private Quaternion def;
    [SerializeField] private bool pauseFreeMove = false;
   
    void Start()
    {
        def = transform.localRotation;
    }

    void Update()
    {
        float factorX = (Input.GetAxis("Mouse Y")) * amount;
        float factorY = -(Input.GetAxis("Mouse X")) * amount;
        //float factorZ = -Input.GetAxis("Vertical") * amount;
        float factorZ = 0 * amount;

        if (!pauseFreeMove)
        {
            if (factorX > maxamount)
                factorX = maxamount;

            if (factorX < -maxamount)
                factorX = -maxamount;

            if (factorY > maxamount)
                factorY = maxamount;

            if (factorY < -maxamount)
                factorY = -maxamount;

            if (factorZ > maxamount)
                factorZ = maxamount;

            if (factorZ < -maxamount)
                factorZ = -maxamount;

            Quaternion Final = Quaternion.Euler(def.x + factorX, def.y + factorY, def.z + factorZ);
            parentHolder.localRotation = Quaternion.Slerp(parentHolder.localRotation, Final, (Time.fixedDeltaTime * smooth));
        }

        if (Input.GetMouseButtonDown(1) && canAttack)
        {
            StartCoroutine(playSwingAnimation());
        }
    }

    private IEnumerator playSwingAnimation()
    {
        anim.enabled = true;

        yield return new WaitForSeconds(.1f);

        anim.Play("WeaponSwing_IdleToLeft", -1, 0);
        canAttack = false;

        yield return new WaitForSeconds(1.2f);

        anim.enabled = false;
        canAttack = true;
    }

}
