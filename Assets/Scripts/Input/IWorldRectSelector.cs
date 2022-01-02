using System.Collections.Generic;
using Units;

namespace Input
{
    public interface IWorldRectSelector
    {
        public void StartDrawingRect();
        public bool HasDrawnSelectionRect();
        public List<ISelectableUnit> ReturnUnitsInRect(List<ISelectableUnit> allUnits);
    }
}