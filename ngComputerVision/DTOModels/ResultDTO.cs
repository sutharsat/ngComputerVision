namespace ngComputerVision.DTOModels
{
    public class ResultDTO
    {
        //PII
        public string firstname { get; set; }
        public double firstnamecs { get; set; }
        public string lastname { get; set; }
        public double lastnamecs { get; set; }
        public string dateofbirth { get; set; }
        public double dateofbirthcs { get; set; }
        public string address { get; set; }
        public double addresscs { get; set; }
        // public string gender { get; set; }
        // public string ptan { get; set; }
        // public int medicareID { get; set; }
        // public int npi { get; set; }
        public string phonenumber { get; set; }
        public double phonenumbercs { get; set; }
        public string hospitalname { get; set; }
        public double hospitalnamecs { get; set; }
        //Health
        public string treatmentname { get; set; }
        public double treatmentnamecs { get; set; }
        public string gender { get; set; }
        public double gendercs { get; set; }
        public string careenvironment { get; set; }
        public double careenvironmentcs { get; set; }
        public string administrativeevent { get; set; }
        public double administrativeeventcs { get; set; }
        public string healthcareprofession { get; set; }
        public double healthcareprofessioncs { get; set; }
    }
}