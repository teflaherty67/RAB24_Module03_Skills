namespace RAB24_Module03_Skills.Common
{
    internal static class Utils
    {
        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel curPanel;

            if (GetRibbonPanelByName(app, tabName, panelName) == null)
                curPanel = app.CreateRibbonPanel(tabName, panelName);

            else
                curPanel = GetRibbonPanelByName(app, tabName, panelName);

            return curPanel;
        }

        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

        #region Families

        internal static FamilySymbol GetFamilySymbolByName(Document curDoc, string famName, string fsName)
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


        #endregion

        #region Parameters        

        internal static string GetParameterValueAsString(Element element, string paramName)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter curParam = paramList.First();

            return curParam.AsString();
        }

        internal static void SetParameterValue(Element element, string paramName, string value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter curParam = paramList.First();
            
            curParam.Set(value);
        }

        internal static void SetParameterValue(Element element, string paramName, double value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter curParam = paramList.First();

            curParam.Set(value);
        }

        #endregion
    }
}
