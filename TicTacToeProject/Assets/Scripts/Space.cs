using System;

[Serializable]
public class Space
{
    public (byte, byte) coordinates;
    public SignType currentSignType;

    public void Select(SignType sign)
    {
        currentSignType = sign;
    }

    public Space GetClone() => new Space() { coordinates = this.coordinates, currentSignType = this.currentSignType };
}