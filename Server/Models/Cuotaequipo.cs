using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models;

public partial class Cuotaequipo
{
    public int Idcuotaequipo { get; set; }

    public int Idcuotatorneo { get; set; }

    public int Idequipo { get; set; }

    public decimal Montoasignado { get; set; }

    public string Observaciones { get; set; }
}
