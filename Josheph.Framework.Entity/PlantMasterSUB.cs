using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Josheph.Framework.Common;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Josheph.Framework.Entity
{
    public class PlantMasterSUB : TTCommonEntity
    {
        public PlantMasterSUB()
        {
            this.TableName = "PlantMaster";
            this.CreatedDateTime = DateTime.Now;
            this.SystemDateTime = DateTime.Now;
            this.UpdatedDateTime = DateTime.Now;
        }
        [Key]
        [TTAttributs("PlantMaster", FieldName = "PlantId", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "PlantId")]
        public System.Guid PlantId { get; set; }

        [TTAttributs("PlantMaster", FieldName = "PlantName", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "PlantName")]
        [MaxLength(100)]
        [Required]
        public string PlantName { get; set; }

        [TTAttributs("PlantMaster", FieldName = "ContactPerson", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "ContactPerson")]
        [MaxLength(100)]
        public string ContactPerson { get; set; }

        [TTAttributs("PlantMaster", FieldName = "Mobile", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Mobile")]
        [MaxLength(12)]
        public string Mobile { get; set; }

        [TTAttributs("PlantMaster", FieldName = "EmailId", ParamaterDataType = SqlDbType.NVarChar, isUpdateField = false)]
        [Display(Name = "EmailId")]
        [MaxLength(100)]
        public string EmailId { get; set; }

        [TTAttributs("PlantMaster", FieldName = "Password", ParamaterDataType = SqlDbType.NVarChar, isUpdateField = false)]
        [Display(Name = "Password")]
        [MaxLength(100)]
        public string Password { get; set; }

        //[TTAttributs("PlantMaster", FieldName = "CountryID", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false)]
        //[Display(Name = "CountryID")]
        //[MaxLength(100)]
        //public string CountryID { get; set; }

        //[TTAttributs("PlantMaster", FieldName = "CountryName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false)]
        //[Display(Name = "CountryName")]
        //[MaxLength(100)]
        //public string CountryName { get; set; }



        //[TTAttributs("PlantMaster", FieldName = "StateID", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false)]
        //[Display(Name = "StateID")]
        //[MaxLength(100)]
        //public string StateID { get; set; }

        //[TTAttributs("PlantMaster", FieldName = "StateName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isTableField = false)]
        //[Display(Name = "StateName")]
        //[MaxLength(100)]
        //public string StateName { get; set; }

        [TTAttributs("PlantMaster", FieldName = "CityID", ParamaterDataType = SqlDbType.UniqueIdentifier)]
        [Display(Name = "CityID")]
        public Guid CityID { get; set; }

        [TTAttributs("PlantMaster", FieldName = "CityName", ParamaterDataType = SqlDbType.NVarChar, isInsertField = false, isUpdateField = false, isSelectField = false, isTableField = false)]
        [Display(Name = "CityName")]
        [MaxLength(100)]
        public string CityName { get; set; }

        [TTAttributs("PlantMaster", FieldName = "Address", ParamaterDataType = SqlDbType.NVarChar, isMemoField = true)]
        [Display(Name = "Address")]
        [MaxLength(500)]
        public string Address { get; set; }

        [TTAttributs("PlantMaster", FieldName = "Logitude", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Logitude")]
        [MaxLength(50)]
        public string Logitude { get; set; }

        [TTAttributs("PlantMaster", FieldName = "Latitude", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "Latitude")]
        [MaxLength(50)]
        public string Latitude { get; set; }


        [TTAttributs("PlantMaster", FieldName = "InstallationSize", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "InstallationSize")]
        public int InstallationSize { get; set; }

        [TTAttributs("PlantMaster", FieldName = "InstallationType", ParamaterDataType = SqlDbType.Int)]
        [Display(Name = "InstallationType")]
        public int InstallationType { get; set; }

        [TTAttributs("PlantMaster", FieldName = "InstllationAngle", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "InstllationAngle")]
        [MaxLength(50)]
        public string InstllationAngle { get; set; }

        [TTAttributs("PlantMaster", FieldName = "InstllationAzimuth", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "InstllationAzimuth")]
        [MaxLength(50)]
        public string InstllationAzimuth { get; set; }

        [TTAttributs("PlantMaster", FieldName = "plantDate", ParamaterDataType = SqlDbType.NVarChar)]
        [Display(Name = "plantDate")]
        [MaxLength(50)]
        public string plantDate { get; set; }

        [TTAttributs("PlantMaster", FieldName = "Status", ParamaterDataType = SqlDbType.Int, isUpdateField = false)]
        [Display(Name = "Status")]
        public MyEnumration.MyStatus Status { get; set; }

        [TTAttributs("PlantMaster", FieldName = "AspNetUserID", ParamaterDataType = SqlDbType.UniqueIdentifier, isUpdateField = false)]
        [Display(Name = "AspNetUserID")]
        public Guid AspNetUserID { get; set; }

    }
}
