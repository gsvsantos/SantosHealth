using FluentValidation.TestHelper;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Testes.Unidade.ModuloMedico.Dominio;

[TestClass]
[TestCategory("Testes de Unidade")]
public class MedicoTests
{
    private ValidadorMedico _validador;

    [TestInitialize]
    public void Inicializar() => this._validador = new ValidadorMedico();

    [TestMethod]
    public void Deve_Passar_Quando_Nome_E_Crm_Sao_Validos()
    {
        Medico medico = new("João da Silva", "12345-SP");

        this._validador.TestValidate(medico).ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Deve_Falhar_Quando_Nome_Estiver_Vazio()
    {
        Medico medico = new("", "12345-SP");

        TestValidationResult<Medico> result = this._validador.TestValidate(medico);

        result.ShouldHaveValidationErrorFor(m => m.Nome)
            .WithErrorMessage("O campo Nome é obrigatório");
    }

    [TestMethod]
    public void Deve_Falhar_Quando_Nome_For_Menor_Que_Tres_Caracteres()
    {
        Medico medico = new("Ab", "12345-SP");

        TestValidationResult<Medico> result = this._validador.TestValidate(medico);

        result.ShouldHaveValidationErrorFor(m => m.Nome)
            .WithErrorMessage("O campo Nome deve conter no mínimo 3 caracteres");
    }

    [TestMethod]
    public void Deve_Falhar_Quando_Crm_Estiver_Vazio()
    {
        Medico medico = new("João da Silva", "");

        TestValidationResult<Medico> result = this._validador.TestValidate(medico);

        result.ShouldHaveValidationErrorFor(m => m.Crm)
            .WithErrorMessage("O campo Crm é obrigatório");
    }

    [TestMethod]
    public void Deve_Falhar_Quando_Crm_Nao_Tiver_Formato_Correto()
    {
        Medico medico = new("João da Silva", "1234-SP");

        TestValidationResult<Medico> result = this._validador.TestValidate(medico);

        result.ShouldHaveValidationErrorFor(m => m.Crm)
            .WithErrorMessage("O campo Crm deve seguir o formato 00000-UF");
    }

    [TestMethod]
    public void Deve_Falhar_Quando_Crm_Tiver_Letras_Minusculas_No_Estado()
    {
        Medico medico = new("João da Silva", "12345-sp");

        TestValidationResult<Medico> result = this._validador.TestValidate(medico);

        result.ShouldHaveValidationErrorFor(m => m.Crm)
            .WithErrorMessage("O campo Crm deve seguir o formato 00000-UF");
    }
}
