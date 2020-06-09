using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.ChatShop
{
    public class Ent_Chazki
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
        public List<Ent_ItemSold_2> listItemSold { get; set; }
        public string notes { get; set; }
        public string documentNumber { get; set; }
        public string name_tmp { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string documentType { get; set; }
        public List<Ent_AddressClient_2> addressClient { get; set; }
    }
    public class Ent_ItemSold_2
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
    public class Ent_AddressClient_2
    {
        public string nivel_2 { get; set; }
        public string nivel_3 { get; set; }
        public string nivel_4 { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public string alias { get; set; }
        public Ent_Position_2 position { get; set; }
    }
    public class Ent_Position_2
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Response_Registro_2
    {
        public int response { get; set; }
        public string descriptionResponse { get; set; }
        public string codeDelivery { get; set; }
    }

}