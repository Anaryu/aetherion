public enum eEventType
{
	None = 0,
	CallFunction = 1000,
	PlaySound = 1010,
	Play3DSound = 1011,
	Wait = 1020,
	Message = 1030,
	Move = 1040,
	LookAt = 1041,
	SetSwitch = 1050,
	FlipSwitch = 1051,
	SetVariable = 1060,
	IncreaseVariable = 1061,
	DecreaseVariable = 1062,
}

public enum eEventTrigger
{
	None = 0,
	Action = 1,
	Touch = 2,
	Auto = 3,
}