namespace GDD
{
    interface  IBuildngPlace
    {
        public void OnActivateBuildingPlace();
        public void OnDisableBuildingPlace();

        public void ShowPlacementAssistance(bool IsShow = true);
    }
}