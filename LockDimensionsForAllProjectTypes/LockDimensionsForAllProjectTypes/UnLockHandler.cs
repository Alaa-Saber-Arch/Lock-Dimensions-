using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace LockDimensionsForAllProjectTypes
{
    public class UnLockHandler : IExternalEventHandler
    {
        public Autodesk.Revit.DB.Document Doc { get; set; }
        public bool Lock { get; set; }
        public bool ShowMessage { get; set; } = false;

        public void Execute(UIApplication app)
        {
            //collection Dimensions

            #region collection Dimensions
            var dimensions = new FilteredElementCollector(Doc)
                      .OfCategory(BuiltInCategory.OST_Dimensions)
                      .WhereElementIsNotElementType().Cast<Dimension>().ToList().Where(d =>
                       (d.DimensionType != null &&
                        d.DimensionType.StyleType == DimensionStyleType.Linear ||
                        d.DimensionType.StyleType == DimensionStyleType.Angular)); 
            #endregion

            // UnLock Case
            #region UnLock
            using (Transaction transaction = new Transaction(Doc, "UnLock Dimensions"))
            {
                transaction.Start();
                try
                {
                    foreach (Dimension Dimension in dimensions)
                    {
                        if (Dimension.NumberOfSegments > 0)
                        {
                            foreach (object segment in Dimension.Segments)
                            {
                                var DimSegment = segment as DimensionSegment;
                                DimSegment.IsLocked = false;
                            }
                        }
                        else
                        {
                            Dimension.IsLocked = false;
                        }
                    }
                    transaction.Commit();

                    TaskDialog.Show("UnLock Dimensions", "All Dimensions are Unlocked successfully.");


                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                    transaction.RollBack();
                }
            }
            #endregion
        }

        public string GetName()
        {
            return "UnLockHandler";
        }

    }
}


