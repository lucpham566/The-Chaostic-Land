using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWarriorCharacter : EnemyCharacter
{

    public void Attack1()
    {
        base.animator.SetTrigger("Attack1");
        base.enemyAudio.PlayClipOneShot(enemyAudio.swordSlashSound);
    }

    public void Attack2()
    {
        base.animator.SetTrigger("Attack2");
        base.enemyAudio.PlayClipOneShot(enemyAudio.swordSlashSound);
    }

    public void Attack3()
    {
        base.animator.SetTrigger("Attack3");
        base.enemyAudio.PlayClipOneShot(enemyAudio.swordSlashSound);

    }

    public void Spell1()
    {
        base.animator.SetTrigger("Spell1");
    }

    public void Spell2()
    {
        base.animator.SetTrigger("Spell2");
    }
}
