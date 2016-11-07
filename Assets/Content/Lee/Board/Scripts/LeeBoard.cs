using UnityEngine;
using System.Collections;
using LeeMagic;
using VVOSS.D2d;

namespace LeeMagic
{

	public class LeeBoard : VVOSS.D2d.VOSMap2d<LeeBoardItem>,
		IVOSBuilder
	{

		[SerializeField]
		protected LeeBoardItemFactory _factory;
		[SerializeField]
		protected LeeBoardItemInfo _info;

		protected void Awake()
		{
			Initialize();
		}

		public override void Initialize()
		{
			base.Initialize();

			_InitializeItems();
		}

		protected void _InitializeItems()
		{
			for (int i = 0; i < rows; ++i)
			{
				for(int j = 0; j < columns; ++j)
				{
					var item = _factory.Allocate();
					item.Initialize(_info);
					SetItem(i, j, item);
				}
			}
		}


		//
		// < Events >
		//

		protected override void _OnSeleceted(IVOSGrid2dPointer pointer)
		{
			base._OnSeleceted(pointer);
		}

		//
		// </ Events >
		//


		[ContextMenu("Build")]
		public override void Build()
		{
			base.Build();

			_factory = gameObject.Build(_factory, "ItemFactory");
		}
	}
	
}
