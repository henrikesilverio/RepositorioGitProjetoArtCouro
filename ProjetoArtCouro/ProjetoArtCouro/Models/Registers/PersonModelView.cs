using System;

namespace ProjetoArtCouro.Web.Models.Registers
{
    public class PersonModelView
    {
        #region Dados de uso geral
        //public TypePerson TypePerson { get; set; }
        #endregion

        #region Dados de Pessoa Fisica
        public string PhysicalName { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Sex { get; set; }
        public int MaritalStatusId { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        #endregion

        #region Dados de Pessoa Juridica
        public string LegalName { get; set; }
        public string CNPJ { get; set; }
        public string Contact { get; set; }
        #endregion

        #region Dados de Endereço
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string CEP { get; set; }
        public int StateId { get; set; }
        #endregion

        #region Dados de Funcionario
        public string JobTitle { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime? DateOfResignation { get; set; }
        public double Salary { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        #endregion
    }
}