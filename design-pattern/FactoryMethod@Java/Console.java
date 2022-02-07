package FactorMethod;

import FactorMethod.CompFactory.Usage;

public class Console 
{
	private CompFactory compFactory = new CompFactory();
	
	Component comp1;
	Component comp2;
	Component comp3;
	
	void WithoutFactory()
	{
		comp1 = new Button();
		comp1 = new Switch();
		comp1 = new Dropdown();
	}
	
	void WithFactory()
	{
		comp1 = compFactory.GetComponent(Usage.PRESS);
		comp1 = compFactory.GetComponent(Usage.TOGGLE);
		comp1 = compFactory.GetComponent(Usage.EXPAND);
	}
}
