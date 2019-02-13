using System;

[Serializable]
    public class HorarioRQ
    {
        public CondicionAgy eCondicionAgy { get; set; }
        public int intIdWeb { get; set; }
        public ModuloEasy eModuloEasy { get; set; }
        public int intOpcion { get; set; }
    }

public class HorarioRS
{
    public int intPermitirAutomatica { get; set; }
    public int intPermitirCounter { get; set; }
}

public enum ModuloEasy
{
    Emision = 1,
    Void = 2,
    Changer = 3
}

public enum CondicionAgy
{
    CON,
    CRED
}
