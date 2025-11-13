using FluentValidation;
using FluentValidation.Results;
using Moq;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.Inserir;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAutenticacao;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Testes.Unidade.ModuloMedico.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class InserirMedicoRequestHandlerTests
{
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IRepositorioMedico> _repositorioMedicoMock;
    private Mock<IValidator<Medico>> _validadorMock;
    private Mock<ITenantProvider> _tenantProviderMock;

    private InserirMedicoRequestHandler _handler;

    [TestInitialize]
    public void Inicializar()
    {
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._repositorioMedicoMock = new Mock<IRepositorioMedico>();
        this._validadorMock = new Mock<IValidator<Medico>>();
        this._tenantProviderMock = new Mock<ITenantProvider>();

        this._handler = new InserirMedicoRequestHandler(
            this._contextoMock.Object,
            this._repositorioMedicoMock.Object,
            this._tenantProviderMock.Object,
            this._validadorMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Inserir_Medico()
    {
        // Arrange
        InserirMedicoRequest request = new("João da Silva", "12345-SP");

        this._validadorMock.Setup(v => v.ValidateAsync(It.IsAny<Medico>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarTodosAsync())
            .ReturnsAsync([]);

        this._repositorioMedicoMock
            .Setup(r => r.InserirAsync(It.IsAny<Medico>()))
            .ReturnsAsync(Guid.NewGuid());

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<InserirMedicoResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioMedicoMock.Verify(x => x.InserirAsync(It.IsAny<Medico>()), Times.Once);
        this._contextoMock.Verify(x => x.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }
}
