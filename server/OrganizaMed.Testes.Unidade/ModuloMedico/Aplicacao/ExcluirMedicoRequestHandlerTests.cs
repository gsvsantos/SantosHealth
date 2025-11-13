using Moq;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.ModuloMedico.Commands.Excluir;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Testes.Unidade.ModuloMedico.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class ExcluirMedicoHandlerTests
{
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IRepositorioMedico> _repositorioMedicoMock;

    private ExcluirMedicoRequestHandler _handler;

    [TestInitialize]
    public void Inicializar()
    {
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._repositorioMedicoMock = new Mock<IRepositorioMedico>();

        this._handler = new ExcluirMedicoRequestHandler(
            this._repositorioMedicoMock.Object,
            this._contextoMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Excluir_Medico_Com_Sucesso()
    {
        // Arrange
        ExcluirMedicoRequest request = new(Guid.NewGuid());
        Medico medicoSelecionado = new("João da Silva", "12345-SP")
        {
            Id = request.Id
        };

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(medicoSelecionado);

        this._repositorioMedicoMock
            .Setup(r => r.ExcluirAsync(It.IsAny<Medico>()))
            .ReturnsAsync(true);

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<ExcluirMedicoResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioMedicoMock.Verify(r => r.ExcluirAsync(It.IsAny<Medico>()), Times.Once);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Nao_Deve_Excluir_Medico_Se_Nao_Encontrado()
    {
        // Arrange
        ExcluirMedicoRequest request = new(Guid.NewGuid());

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync((Medico)null);

        // Act
        FluentResults.Result<ExcluirMedicoResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioMedicoMock.Verify(r => r.ExcluirAsync(It.IsAny<Medico>()), Times.Never);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Never);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.NotFoundError(request.Id).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }

    [TestMethod]
    public async Task Deve_Retornar_Erro_Se_Ocorreu_Excecao_Ao_Excluir()
    {
        // Arrange
        ExcluirMedicoRequest request = new(Guid.NewGuid());

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(new Medico("João da Silva", "12345-SP") { Id = request.Id });

        this._repositorioMedicoMock
            .Setup(r => r.ExcluirAsync(It.IsAny<Medico>()))
            .ThrowsAsync(new Exception("Erro ao excluir médico"));

        this._contextoMock
            .Setup(c => c.RollbackAsync())
            .Returns(Task.CompletedTask);

        // Act
        FluentResults.Result<ExcluirMedicoResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioMedicoMock.Verify(r => r.ExcluirAsync(It.IsAny<Medico>()), Times.Once);
        this._contextoMock.Verify(c => c.RollbackAsync(), Times.Once);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.InternalServerError(new Exception()).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }
}
