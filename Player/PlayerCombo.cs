using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    Animator animator;
    bool trigger;
    public int combo;
    public int comboNumber;
    public bool attacking;

    public float comboTiming;
    public float comboTempo;

    private PlayerCharacter playerCharacter;
    public GameObject attackPrefab; // vung tan cong duocc tao ra
    public List<GameObject> attackPrefabs; // vung tan cong duocc tao ra

    public float attackDuration = 0.5f; // thoi gian ton tai vung tan cong

    void Start()
    {
        animator = GetComponent<Animator>();
        playerCharacter = GetComponent<PlayerCharacter>();
        combo = 1;
        comboTiming = 0.5f;
        comboTempo = comboTiming;
        comboNumber = 3;
    }

    // Update is called once per frame
    void Update()
    {
        Combo();
    }

    public void Combo()
    {
        if (!playerCharacter.isAttacking)
        {
            comboTempo -= Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(AttackCombo());
            }
        }
    }


    private IEnumerator AttackCombo()
    {
        attacking = true;
        animator.SetBool("Attacking",true);

        if (comboTempo < 0)
        {
            playerCharacter.Attack();
            playerCharacter.isAttacking = true;
            combo = 1;
            animator.SetTrigger("Attack" + combo);
            comboTempo = comboTiming;
            yield return new WaitForSeconds(0.2f);
            GenAttackZone();
           
        }
        else if (comboTempo > 0 && comboTempo < 1)
        {
            playerCharacter.Attack();

            playerCharacter.isAttacking = true;
            combo++;
            if (combo > comboNumber)
            {
                combo = 1;
            }

            animator.SetTrigger("Attack" + combo);
            comboTempo = comboTiming;
            yield return new WaitForSeconds(0.2f);
            GenAttackZone();
        }

        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Attacking", false);
        playerCharacter.isAttacking = false;
    }


    private void GenAttackZone()
    {
        Collider2D myCollider = GetComponent<Collider2D>(); // Thay thế Collider2D bằng loại Collider bạn đang sử dụng
        Vector2 colliderExtents = myCollider.bounds.extents;

        // nếu quay trái thì trừ
        Vector3 spawnPosition = new Vector3(transform.position.x - colliderExtents.x, transform.position.y, transform.position.z);

        // nếu quay phải thì cộng
        if (playerCharacter.isFaceToRight())
        {
            spawnPosition = new Vector3(transform.position.x + colliderExtents.x, transform.position.y, transform.position.z);
        }
        attackPrefab = attackPrefabs[combo-1];
        if (combo == 1)
        {
            
        }
        if (combo == 2)
        {

        }
        if (combo == 3)
        {
    
        }
        else
        {

        }
        GameObject attackObject = Instantiate(attackPrefab, spawnPosition, transform.rotation);
        PlayerDamage damage = attackObject.GetComponent<PlayerDamage>();
        damage.damge = playerCharacter.Damage;
        Destroy(attackObject, attackDuration);
    }
}
