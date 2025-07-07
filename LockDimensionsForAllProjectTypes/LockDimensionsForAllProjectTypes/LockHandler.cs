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
    public class LockHandler : IExternalEventHandler
    {
        public Autodesk.Revit.DB.Document Doc { get; set; }
        public bool Lock { get; set; }
        public bool ShowMessage { get; set; } = false;

        public void Execute(UIApplication app)
        {
            // getting all dimensions in the project 

            #region getting all dimensions in the project 
            var allowedTypesOfRefernce = new List<ElementReferenceType>
            {
              (ElementReferenceType)1, // CutEdge
              (ElementReferenceType)2, // Surface
              (ElementReferenceType)3,  // Linear
            };

            var dimensions = new FilteredElementCollector(Doc)
                    .OfCategory(BuiltInCategory.OST_Dimensions)
                    .WhereElementIsNotElementType().Cast<Dimension>().ToList().Where(d =>
                      d.DimensionType != null &&
                     (d.DimensionType.StyleType == DimensionStyleType.Linear ||
                      d.DimensionType.StyleType == DimensionStyleType.Angular) &&
                      d.References != null &&
                      d.References.Cast<Reference>().All(r => allowedTypesOfRefernce.Contains(r.ElementReferenceType)))
                    .ToList(); 
            #endregion

            // Lock dimensions
            #region Lock
            using (Transaction transaction = new Transaction(Doc, "Lock Dimensions"))
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
                                DimSegment.IsLocked = true;
                            }
                        }
                        else
                        {
                            Dimension.IsLocked = true;
                        }
                    }
                    transaction.Commit();

                    TaskDialog.Show("Lock Dimensions", "All Dimensions are locked successfully.");


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
            return "LockHandler";
        }

    }
}


