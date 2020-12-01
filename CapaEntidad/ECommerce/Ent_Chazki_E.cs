using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ECommerce
{
    public class Ent_Chazki_E
    {
        public string storeId { get; set; }
        public string branchId { get; set; }
        public string deliveryTrackCode { get; set; }
        public string proofPayment { get; set; }
        public float deliveryCost { get; set; }
        public string mode { get; set; }
        public string time { get; set; }
        public string paymentMethod { get; set; }
        public string country { get; set; }
        public List<Ent_ItemSold_E> listItemSold { get; set; }
        public string notes { get; set; }
        public string documentNumber { get; set; }
        public string name_tmp { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string documentType { get; set; }
        public List<Ent_AddressClient_E> addressClient { get; set; }
    }
    public class Ent_ItemSold_E
    {
        public string name { get; set; }
        public string currency { get; set; }
        public double price { get; set; }
        public double weight { get; set; }
        public double volumen { get; set; }
        public int quantity { get; set; }
        public string unity { get; set; }
        public string size { get; set; }
    }
    public class Ent_AddressClient_E
    {
        public string nivel_2 { get; set; }
        public string nivel_3 { get; set; }
        public string nivel_4 { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public string alias { get; set; }
        public Ent_Position_E position { get; set; }
    }
    public class Ent_Position_E
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Response_Registro_E
    {
        public int response { get; set; }
        public string descriptionResponse { get; set; }
        public string codeDelivery { get; set; }
    }

}