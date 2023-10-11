using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FireWarriorController : EnemyController
{

    // Update is called once per frame
    public FireWarriorCharacter fireWarriorCharacter;
    public bool isAttacking = false;
    public bool attackEnable = false;

    void Update()
    {
        base.timeSinceLastShot += Time.deltaTime;
        base.timeSinceLastIdle += Time.deltaTime;

        if (!base.enemyCharactor.isDeath)
        {
            if (playerTarget != null)
            {
                if (isPlayerInRangeAttack)
                {
                    base.setFaceTarget();
                    // Kiểm tra nếu đủ thời gian giữa các lần bắn
                    if (!isAttacking && attackEnable)
                    {
                        attackRandomCombo();
                    }

                }
                else
                {
                    base.MoveToTarget();
                    isAttacking = false;
                }

            }
            else if (!isAttacking)
            {
                if (base.enemyCharactor.isGrounded)
                {
                    base.MoveRandom();

                }
            }

        }
    }

    private void attackRandomCombo()
    {
        List<IEnumerator> comboFunctions = new List<IEnumerator>();

        comboFunctions.Add(Combo1());
        comboFunctions.Add(Combo2());

        // Chọn ngẫu nhiên một hàm Combo từ danh sách và gọi nó
        int randomIndex = UnityEngine.Random.Range(0, comboFunctions.Count);
        StartCoroutine(comboFunctions[randomIndex]);
    }
    private IEnumerator Combo1()
    {
        isAttacking = true;
        attackEnable = false;

        if (isAttacking)
        {
            // Gọi hàm Attack1 và chờ 1 giây
            fireWarriorCharacter.Attack1();
            yield return new WaitForSeconds(0.5f);

            // Gọi hàm Attack2 và chờ 1 giây
            fireWarriorCharacter.Attack2();
            yield return new WaitForSeconds(0.5f);


            // Gọi hàm Attack3 và chờ 1 giây
            fireWarriorCharacter.Attack3();
            yield return new WaitForSeconds(1f);
        }


        // Gọi hàm Slide và chờ 1 giây
        base.enemyCharactor.Slide();
        yield return new WaitForSeconds(2f);


        if (isAttacking)
        {
            // Gọi hàm Spell1
            fireWarriorCharacter.Spell1();
            yield return new WaitForSeconds(0.5f);
            base.FireBullet();
            yield return new WaitForSeconds(3f);
        }

        isAttacking = false;
        attackEnable = true;

    }
    private IEnumerator Combo2()
    {
        isAttacking = true;
        attackEnable = false;

        if (isAttacking)
        {
            // Gọi hàm Spell1
            fireWarriorCharacter.Spell1();
            yield return new WaitForSeconds(0.5f);
            base.FireBullet();
            yield return new WaitForSeconds(1f);
        }

        if (isAttacking)
        {
            // Gọi hàm Spell1
            fireWarriorCharacter.Spell2();
            yield return new WaitForSeconds(3f);
        }


        isAttacking = false;
        attackEnable = true;

    }

}
