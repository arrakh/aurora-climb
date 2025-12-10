namespace AuroraClimb.Items
{
    public struct ItemData
    {
        public readonly string id;
        public readonly int amount;

        public ItemData(string id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }
    }
}