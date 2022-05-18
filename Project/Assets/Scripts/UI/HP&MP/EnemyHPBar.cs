using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
	public float damPening = 5f;
	public float changeSpeed = .5f;
	float timeout = 0f;

	Material mat;
	float fillTarget = 1.0f;
	float delta = 0f;

	public GameObject enemy;
	public EnemyAct enemyData;
	float fill;
	void Awake()
	{
		enemy = GameObject.Find("Enemy");
		enemyData = enemy.GetComponent<EnemyAct>();

		Renderer rend = GetComponent<Renderer>();
		Image img = GetComponent<Image>();
		if (rend != null)
		{
			mat = new Material(rend.material);
			rend.material = mat;
		}
		else if (img != null)
		{
			mat = new Material(img.material);
			img.material = mat;
		}
		else
		{
			Debug.LogWarning("No Renderer or Image attached to " + name);
		}


	}

	void Update()
	{

		fill = enemyData.monster.mons[enemyData.GetEnemyNum()].Hp / enemyData.monster.mons[enemyData.GetEnemyNum()].MaxHp;

		delta -= fillTarget - fill;
		fillTarget = fill;

		delta = Mathf.Lerp(delta, 0, Time.deltaTime * damPening);

		mat.SetFloat("_Delta", delta);
		mat.SetFloat("_Fill", fillTarget);
	}
}
