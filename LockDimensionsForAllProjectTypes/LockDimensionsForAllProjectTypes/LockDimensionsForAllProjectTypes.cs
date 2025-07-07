using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace LockDimensionsForAllProjectTypes
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class LockDimensionsForAllProjectTypes : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Getting Project Session
            #region getting project session

            UIApplication App = commandData.Application;
            UIDocument uidoc = App.ActiveUIDocument;
            Document Doc = uidoc.Document;

            #endregion
            //handlers and events creation

            #region handlers and events creation

            var lockHandler = new LockHandler() { Doc = Doc };
            var unLockHandler = new UnLockHandler() { Doc = Doc };
            lockHandler.Doc = Doc;
            unLockHandler.Doc = Doc;
            var lockEvent = ExternalEvent.Create(lockHandler);
            var unLockEvent = ExternalEvent.Create(unLockHandler);
            var form = new UI(lockEvent, lockHandler, unLockEvent, unLockHandler); 

            #endregion

            form.ShowDialog();
            return Result.Succeeded;
        }
    }
}
