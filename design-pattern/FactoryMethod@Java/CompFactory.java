package FactorMethod;

public class CompFactory 
{
	public enum Usage
	{
		PRESS,
		TOGGLE,
		EXPAND,
	}
	
	public Component GetComponent (Usage usage) 
	{
		switch(usage)
		{
			case PRESS:		return new Button();
			case TOGGLE:	return new Switch();
			case EXPAND:	return new Dropdown();
		}
		
		return null;
	}
}
