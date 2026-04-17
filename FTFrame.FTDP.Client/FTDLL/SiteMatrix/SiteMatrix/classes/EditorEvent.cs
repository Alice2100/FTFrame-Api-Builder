using System;
using SiteMatrix.forms;
using SiteMatrix.functions;
using SiteMatrix.consts;
using System.Windows.Forms;
using mshtml;
namespace SiteMatrix.classes
{
	/// <summary>
	/// EditorEvent 的摘要说明。
	/// </summary>
	public class EditorEvent
	{
		public EditorEvent()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		public static System.Windows.Forms.DragEventArgs DragE;
		public static void AddEvents()
		{
			try
			{
				Form[] fms=globalConst.MdiForm.MdiChildren;
				foreach(Form fm in fms)
				{
					if(fm.Name.Equals("Editor"))
					{
						Editor ed=(Editor)fm;
						if(!ed.EventAdded)
						{
                            ed.EventAdded = true;
							ed.iEvent.ondrop+=new HTMLTextContainerEvents2_ondropEventHandler(ed.iEvent_ondrop);
							ed.iEvent.ondragover+=new HTMLTextContainerEvents2_ondragoverEventHandler(ed.iEvent_ondragover);
							ed.iEvent.ondragenter+=new HTMLTextContainerEvents2_ondragenterEventHandler(ed.iEvent_ondragenter);
							ed.iEvent.ondragleave+=new HTMLTextContainerEvents2_ondragleaveEventHandler(ed.iEvent_ondragleave);
						}
					}
				}
				fms=null;
			}
			catch//(Exception e)
			{
				//new error(e);
			}
		}
		public static void MoveEvents()
		{
			try
			{
				Form[] fms=globalConst.MdiForm.MdiChildren;
				foreach(Form fm in fms)
				{
					if(fm.Name.Equals("Editor"))
					{
						Editor ed=(Editor)fm;
						if(ed.EventAdded)
						{
                            ed.EventAdded = false;
							ed.iEvent.ondrop-=new HTMLTextContainerEvents2_ondropEventHandler(ed.iEvent_ondrop);
							ed.iEvent.ondragover-=new HTMLTextContainerEvents2_ondragoverEventHandler(ed.iEvent_ondragover);
							ed.iEvent.ondragenter-=new HTMLTextContainerEvents2_ondragenterEventHandler(ed.iEvent_ondragenter);
							ed.iEvent.ondragleave-=new HTMLTextContainerEvents2_ondragleaveEventHandler(ed.iEvent_ondragleave);							
						}
					}
				}
				fms=null;
			}
			catch//(Exception e)
			{
				//new error(e);
			}
		}
	}
}
