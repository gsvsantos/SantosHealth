using FluentValidation;
using FluentValidation.Results;
using Moq;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.Editar;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Testes.Unidade.ModuloMedico.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class EditarMedicoHandlerTests
{
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IRepositorioMedico> _repositorioMedicoMock;
    private Mock<IValidator<Medico>> _validador;

    private EditarMedicoRequestHandler _handler;

    [TestInitialize]
    public void Inicializar()
    {
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._repositorioMedicoMock = new Mock<IRepositorioMedico>();
        this._validador = new Mock<IValidator<Medico>>();

        this._handler = new EditarMedicoRequestHandler(
            this._repositorioMedicoMock.Object,
            this._contextoMock.Object,
            this._validador.Object
        );
    }

    [TestMethod]
    public async Task Deve_Editar_Medico_Com_Sucesso()
    {
        // Arrange
        EditarMedicoRequest request = new(Guid.NewGuid(), "João da Silva", "12345-SP");

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(new Medico("Antônio Carlos", "67890-SP") { Id = request.Id });

        this._validador
            .Setup(v => v.ValidateAsync(It.IsAny<Medico>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarTodosAsync())
            .ReturnsAsync([]);

        this._repositorioMedicoMock
            .Setup(r => r.EditarAsync(It.IsAny<Medico>()))
            .ReturnsAsync(true);

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<EditarMedicoResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioMedicoMock.Verify(r => r.EditarAsync(It.IsAny<Medico>()), Times.Once);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Nao_Deve_Editar_Medico_Se_Nao_Encontrado()
    {
        // Arrange
        EditarMedicoRequest request = new(Guid.NewGuid(), "João da Silva", "12345-SP");

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync((Medico)null);

        // Act
        FluentResults.Result<EditarMedicoResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioMedicoMock.Verify(r => r.EditarAsync(It.IsAny<Medico>()), Times.Never);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Never);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.NotFoundError(request.Id).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }
}
