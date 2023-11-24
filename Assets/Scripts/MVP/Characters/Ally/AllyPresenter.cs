using Code.Strategy;
using UnityEngine;

namespace Code.Units
{
    public class AllyPresenter : UnitPresenter
    {
        public AllyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) :
            base(view, model, moveStrategy)
        {
        }

        public override void PlaceUnit(Vector3 pos)
        {
            base.PlaceUnit(pos);
            ((AllyUnit)_view).OrderedPosition = _view.transform.position;
            _strategy.Init(this);
        }
    }
}