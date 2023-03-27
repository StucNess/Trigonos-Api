namespace TrigonosEnergyWebAPI.DTO
{
    public class NominasBciDto
    {
        public int ID { get; set; }
        public int? id_instruccions { get; set; }
        public string Glosa { get; set; }
        public string rutDeudor { get; set; }
        public string rutAcreedor { get; set; }
        public string nombreAcreedor { get; set; }
        public string nombreDeudor { get; set; }
        public string cuentaBancoAcreedor { get; set; }
        public string correoDteAcreedor { get; set; }
        public int valorNeto { get; set; }
        public string folio { get; set; }
        public string sBifAcreedor { get; set; }


    }
}
