using System;

namespace Samples
{
	public class SimpleClass
	{
		public string Name;
		public string Occupation;
		public int Quantity;
		public bool IsSimpleEnough;


		public SimpleClass ()
		{
			Name = "";
			Occupation = "";
			Quantity = 0;
			IsSimpleEnough = false;
		}


		public override string ToString ()
		{
			return string.Format ("[SimpleClass]\n\tName : {0}\n\tOccupation : {1}\n\tQuantity : {2}\n\tIsSimpleEnough : {3}",
									this.Name,
									this.Occupation,
									this.Quantity,
									this.IsSimpleEnough
								);
		}
	}
}

