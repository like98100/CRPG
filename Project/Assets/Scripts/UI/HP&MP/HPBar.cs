using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
	public float damPening = 5f;
	public float changeSpeed = .5f;
	float timeout = 0f;

	Material mat;
	float fillTarget = 1.0f;
	float delta = 0f;

	public GameObject player;
	public PlayerAct playerData;
	float fill;

	void Awake()
	{
		player = GameObject.Find("Player");
		playerData = player.GetComponent<PlayerAct>();

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

        fill = playerData.player.Hp / playerData.player.MaxHp;
		//Debug.Log("fill : " + fill + ", HP : " + playerData.player.Hp + ", MaxHP : " + playerData.player.MaxHp);

        delta -= fillTarget - fill;
        fillTarget = fill;

        delta = Mathf.Lerp(delta, 0, Time.deltaTime * damPening);

		mat.SetFloat("_Delta", delta);
		mat.SetFloat("_Fill", fillTarget);
	}
}
