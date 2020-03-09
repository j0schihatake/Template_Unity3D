using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Logo : MonoBehaviour
{
//Мой собственный логотип:
		private GameObject cub;
//Список кубов входящих в логотип:
		public List<GameObject> logoObject;
	
		void FixedUpdate ()
		{
				//Получаем случайный куб:
				int rand = (int)Random.Range (0, logoObject.ToArray ().Length);
				//Отключаем/включаем полученный куб:
				cub = logoObject [rand]; 
				if (cub.GetComponent<Renderer> ().enabled == true) {
						cub.GetComponent<Renderer> ().enabled = false;
				} else {
						cub.GetComponent<Renderer> ().enabled = true;
				}
				//randomColor;
				int index = (int)Random.Range (0, 7);
	
				switch (index) {
				case 0:
						cub.GetComponent<Renderer>().material.color = Color.white;
						break;
				case 1:
						cub.GetComponent<Renderer>().material.color = Color.red;
						break;
				case 2:
						cub.GetComponent<Renderer>().material.color = Color.green;
						break;
				case 3:
						cub.GetComponent<Renderer>().material.color = Color.blue;
						break;
				case 4:
						cub.GetComponent<Renderer>().material.color = Color.yellow;
						break;
				case 5:
						cub.GetComponent<Renderer>().material.color = Color.cyan;
						break;
				}
	
		}
}
