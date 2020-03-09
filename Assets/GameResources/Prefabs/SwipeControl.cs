using UnityEngine;
using UnityEngine.Events;
using System.Collections;

//переписываю под себя
public class SwipeControl : MonoBehaviour {
	
	float swipeStartTime;
	public Vector2 swipeStartPos;									//позиция старта свайпа
	public Vector2 swipeVectorDirection;							//вектор получившийся из свайпа
	public Vector2 swipeFinishPosition;

	public float swipeDistance = 0.0f;								//длинна свайпа

//----------------------------------------------Задаем зону свайпа------------------------------------------------------
	public RectTransform transformRect = null;						//подходит любой uiобьект у которого есть RectTransform
	public bool inRect = false;
//----------------------------------------------Получаем скорость и силу------------------------------------------------
	public float touchSpeed = 0.0f;									//скорость с которой пальцем проводили по сенсору
//-----------------------------задаем интервал касчание в котором будет обозначать свайп--------------------------------
	public float minSwipeDist = 20f;								
	public float maxSwipeTime = 1000f;
	public bool wasASwipe = false;									//произошел ли свайп
	public bool wasAOneTach = false;								//одиночное нажатие на точку(например для кнопки)
	public bool wasAUpdateTach = false;								//удержание точки пальцем(например для кнопки)

//-----------------------------------------------Получение времени свайпа-----------------------------------------------
	public float time = 0.0f;										//общее время от начала касания до момента когда палец убран
	public float timeBegan = 0.0f;									//время когда палец коснулся
	public float timeMoved = 0.0f;									//все время за которое палец перемещался
	public float timeStationary = 0.0f;								//время когда палец косался но был неподвижен
	public float timeEnded = 0.0f;									//время когда палец был убран

	//определяем направление свайпа:
	public SwipeDirection swipeDirection;
	public enum SwipeDirection
	{
		none,														//нет направления
		Left,														//влево											
		Up,															//вверх
		Right,														//вправо
		Down,														//вниз
		UpLeft,														//влево и вверх
		UpRight,													//вправо и вверх
		DownLeft,													//вниз и влево
		DownRight													//вверх и вправо
	}
		
	public UnityEvent onTach;										//событие на одиносный тач
	public UnityEvent onTachUpdate;									//событие на тач и удержание кнопки
	public UnityEvent onSwipe;										//событие при свайпе

	void Update () 
	{
		if (Input.touchCount > 0)
		{
			wasASwipe = false;
			wasAUpdateTach = false;

			Touch touch = Input.touches[0];
			if (positionIsRect (touch.position)) {
				switch (touch.phase) {
				case TouchPhase.Began:													//начало прикосновения
					inRect = true;
					clear ();
					swipeStartPos = touch.position;
					timeBegan = Time.time;												//запоминаем время
					wasAOneTach = true;
					onTach.Invoke ();
					//Debug.Log ("Я работаю");
					break;
				case TouchPhase.Moved:
					//Определяем направление движения
					setDirection (touch);
					//Нужно отрезать именно время которое палец движется по сенсору
					time += touch.deltaTime;
					timeMoved += touch.deltaTime;

					break;
				case TouchPhase.Stationary:												//палец с последнего момента не двигался
//					timeStationary += touch.deltaTime;
//					time += touch.deltaTime;
//					wasAUpdateTach = true;
//					onTachUpdate.Invoke ();
					break;
				case TouchPhase.Ended:													//палец убран
					//swipeFinishPosition = touch.position;
					swipeVectorDirection = touch.position - swipeStartPos;
					swipeDistance = swipeVectorDirection.magnitude;
					time = touch.deltaTime;
					inRect = false;
					touchSpeed = speedCalcualtor(time, Vector2.Distance(swipeStartPos, swipeFinishPosition));
					cancels(touch);
					break;
				default:
					break;
				}
			} ///*
			else {
				if(inRect){
					inRect = false;
					cancels (touch);
				}
			}
		//*/
		}
	}

	//метод определяет свайпы только на нашем ректе
	bool positionIsRect(Vector2 touchPosition){
		bool inRect = false;
		//большое условие на расположение в квадрате
		if((touchPosition.x > transformRect.anchoredPosition.x & touchPosition.x < transformRect.anchoredPosition.x + transformRect.rect.size.x) & (touchPosition.y > transformRect.anchoredPosition.y & touchPosition.y < transformRect.anchoredPosition.y + transformRect.rect.size.y)){
			inRect = true;
		}
		return inRect;
	}

	//обнуление таймеров
	void clear(){
		time = 0.0f;										//общее время от начала касания до момента когда палец убран
		timeBegan = 0.0f;									//время когда палец коснулся
		timeMoved = 0.0f;									//все время за которое палец перемещался
		timeStationary = 0.0f;								//время когда палец косался но был неподвижен
		timeEnded = 0.0f;									//время когда палец был убран
	}

	//получаем скорость свайпа:
	float speedCalcualtor(float timeSwipe, float distance){
		float speed = distance / timeSwipe;
		return speed;
	}

	//если свайп продолжается вне зоны ректа требуется это проигнорироват и закончить текущий
	void cancels(Touch touch){
		swipeVectorDirection = touch.position - swipeStartPos;
		swipeDistance = swipeVectorDirection.magnitude;
		touchSpeed = speedCalcualtor(time, Vector2.Distance(swipeStartPos, swipeFinishPosition));
		swipeDirection = SwipeDirection.none;
		wasAOneTach = false;
		wasAUpdateTach = false;
	}

	//определяем направление свайпа
	void setDirection(Touch touch){
		swipeVectorDirection = touch.position - swipeStartPos;
		swipeDistance = swipeVectorDirection.magnitude;
		//также уточнение 
		swipeFinishPosition = touch.position;
		touchSpeed = speedCalcualtor(time, Vector2.Distance(swipeStartPos, swipeFinishPosition));

		if (time < maxSwipeTime && swipeDistance > minSwipeDist) {
			wasASwipe = true;
			//старое с модулями(нахера!)
			if (Mathf.Abs (swipeVectorDirection.x) > Mathf.Abs (swipeVectorDirection.y)) {
				if (Mathf.Sign (swipeVectorDirection.x) > 0)
					swipeDirection = SwipeDirection.Right;
				else
					swipeDirection = SwipeDirection.Left;
			} else {
				if (Mathf.Sign (swipeVectorDirection.y) > 0)
					swipeDirection = SwipeDirection.Up;
				else
					swipeDirection = SwipeDirection.Down;
			}
		}
		onSwipe.Invoke ();
	}
//-----------------------------анчер у трансформректа должен быть привязан к нулю!--------------------------------------
}