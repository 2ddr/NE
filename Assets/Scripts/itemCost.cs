

[System.Serializable]
public struct itemCost
{
    //Variable declaration
    //Note: I'm explicitly declaring them as public, but they are public by default. You can use private if you choose.
    public G.EqResources ResType;
    public int amount;

    //Constructor (not necessary, but helpful)
    /*
    public itemCost(bool isFree, string name)
    {
        this.IsFree = isFree;
        this.Name = name;
    }*/
}