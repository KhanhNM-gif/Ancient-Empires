namespace Assets.Asset.Model
{
    public interface IMatrixCoordi
    {
        int x { get; set; }
        int y { get; set; }

        public int Distance(IMatrixCoordi mc);
    }

}
