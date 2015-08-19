namespace ProjetoArtCouroDomain.Registers
{
    public class PhysicalPerson : Person
    {
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Sex { get; set; }
        public int MaritalStatusId { get; set; }
        public string BirthDay { get; set; }
        public string MobilePhone { get; set; }
    }
}
