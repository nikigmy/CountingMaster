using System;

[Serializable]
public class GateSetup
{
    public GateType leftGateType;
    public int leftAmount;
    public GateType rightGateType;
    public int rightAmount;
        
    public enum GateType
    {
        Add,
        Multply
    }
}
