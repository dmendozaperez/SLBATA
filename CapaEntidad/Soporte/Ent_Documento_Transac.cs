using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Soporte
{
    public class Ent_Documento_Transac
    {
        public string tienda_origen { get; set; }
        public string tran_id { get; set; }
        public string fecha_des { get; set; }
        public bool flg_novell { get; set; }
        public string fec_env { get; set; }
        public string cod_entid { get; set; }
        public string hidden { get; set; }
        public List<Ent_Documento_TransacDoc> LIST_ENT_DOC_TRA { get; set; }
    }

    public class Ent_Documento_TransacDoc
    {
        public string TIPO_DOC { get; set; }
        public string ESTADO { get; set; }
        public string NUM_FAC { get; set; }
        public string SERIE { get; set; }
        public string TOTAL { get; set; }
    }

    public class Ent_Extender_NC
    {
        public string Serial_Nbr { get; set; }
        public int? Organization_Id { get; set; }
        public int? Rtl_Loc_Id { get; set; }
        public int? Wkstn_Id { get; set; }
        public int? Trans_Seq { get; set; }
        public string String_Value { get; set; }
        public DateTime? Business_Date { get; set; }
        public DateTime? Expr_Date { get; set; }
        public DateTime? New_Expr_Date { get; set; }
        //Campos de busqueda
        public string Tienda { get; set; }
        public string Num_Doc { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
