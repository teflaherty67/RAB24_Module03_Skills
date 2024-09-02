using RAB24_Module03_Skills.Common;

namespace RAB24_Module03_Skills
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document curDoc = uidoc.Document;

            // create instances of the class
            Building theater = new Building("Grand Opera House", "5 Main Street", 4, 35000);
            Building hotel = new Building("Fancy Hotel", "10 Main Street", 10, 10000);
            Building office = new Building("Big Office Building", "15 Main Street", 4, 150000);

            // create a list of buildings
            List<Building> buildingList = new List<Building>();
            buildingList.Add(theater);
            buildingList.Add(hotel);
            buildingList.Add(office);
            buildingList.Add(new Building("Hospital", "20 Main Street", 20, 350000));

            // create a new neighborhood
            Neighbohood downtown = new Neighbohood("Downtown", "Middletown", "CT", buildingList);

            TaskDialog.Show("Test", $"There are {downtown.GetBuildingCount()} " +
                $"buildings in the {downtown.Name} neighborhood.");

            // working with rooms

            // get all the rooms
            FilteredElementCollector colRooms = new FilteredElementCollector(curDoc)
                .OfCategory(BuiltInCategory.OST_Rooms);

            // get the family symbol
            FamilySymbol curFS = GetFamilySymbolByName(curDoc, "Desk", "60\" x 30\"");

            using (Transaction t = new Transaction(curDoc, "Insert family symbols"))
            {
                t.Start();

                // activate the family symbol
                curFS.Activate();

                // loop through the rooms and get the location point
                foreach (SpatialElement curRoom in colRooms)
                {
                    LocationPoint locPoint = curRoom.Location as LocationPoint;
                    XYZ roomPoint = locPoint.Point as XYZ;

                    // add family instance to room
                    FamilyInstance newFI = curDoc.Create.NewFamilyInstance(roomPoint, curFS,
                        StructuralType.NonStructural);

                    // get the room department value
                    string nameDept = Utils.GetParameterValueAsString(curRoom, "Department");

                    // set parameter values
                    Utils.SetParameterValue(curRoom, "Floor Finish", "CT");
                }
            }

            return Result.Succeeded;
        }

        internal FamilySymbol GetFamilySymbolByName(Document curDoc, string famName, string fsName)
        {
            FilteredElementCollector m_colFamSym = new FilteredElementCollector(curDoc)
                .OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol curFS in m_colFamSym)
            {
                if (curFS.Name == fsName && curFS.FamilyName == famName)
                    return curFS;
            }

            return null;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData.Data;
        }
    }

    
    // create a class
    public class Building
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumFloors { get; set; }
        public double Area { get; set; }

        // create a constructor
        public Building(string _name,  string _address, int _numFloors, double _area)
        {
            Name = _name;
            Address = _address;
            NumFloors = _numFloors;
            Area = _area;
        }
    }

    public class Neighbohood
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<Building> BuildingList { get; set; }

        public Neighbohood(string _name, string _city, string _state, List<Building> _buildings)
        {
            Name = _name;
            City = _city;
            State = _state;
            BuildingList = _buildings;
        }

        public int GetBuildingCount()
        {
            return BuildingList.Count;
        }
    }
}
