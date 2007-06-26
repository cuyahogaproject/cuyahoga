using System;

using Cuyahoga.Core.Domain;

namespace Cuyahoga.Modules.YetAnotherForum
{
	/// <summary>
	/// Summary description for CuyahogaYafModule.
	/// </summary>
	public class CuyahogaYafModule : ModuleBase
	{
		private int _boardId = -1;
		private int _categoryId = -1;


		public int BoardId
		{
			get { return _boardId; }
		}

		public int CategoryId
		{
			get { return _categoryId; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public CuyahogaYafModule()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override void ReadSectionSettings()
		{
			base.ReadSectionSettings();
			// Set dynamic module settings
			this._boardId = Convert.ToInt32(base.Section.Settings["BOARDID"]);
			if (base.Section.Settings["CATEGORYID"] != null)
			{
				this._categoryId = Convert.ToInt32(base.Section.Settings["CATEGORYID"]);
			}
		}
	}
}
