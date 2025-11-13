using Moq;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.EmailSender.Commands;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.Excluir;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAtividade;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Testes.Unidade.ModuloAtividade.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class ExcluirAtividadeMedicaRequestHandlerTests
{
    private Mock<IRepositorioAtividadeMedica> _repositorioAtividadeMedicaMock;
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<EnviarEmail> _enviarEmailMock;

    private ExcluirAtividadeMedicaRequestHandler _requestHandler;

    [TestInitialize]
    public void Inicializar()
    {
        this._repositorioAtividadeMedicaMock = new Mock<IRepositorioAtividadeMedica>();
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._enviarEmailMock = new Mock<EnviarEmail>();

        this._requestHandler = new ExcluirAtividadeMedicaRequestHandler(
            this._repositorioAtividadeMedicaMock.Object,
            this._contextoMock.Object,
            this._enviarEmailMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Excluir_Atividade_Com_Sucesso()
    {
        // Arrange
        Guid atividadeId = Guid.NewGuid();
        Consulta atividade = new(DateTime.Now, DateTime.Now.AddHours(1), new Medico("Dr. João", "12345-SP"));

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.SelecionarPorIdAsync(atividadeId))
            .ReturnsAsync(atividade);

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.ExcluirAsync(It.IsAny<AtividadeMedica>()))
            .ReturnsAsync(true);

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        ExcluirAtividadeMedicaRequest request = new(atividadeId);

        // Act
        FluentResults.Result<ExcluirAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioAtividadeMedicaMock.Verify(r => r.ExcluirAsync(atividade), Times.Once);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Nao_Deve_Excluir_Quando_Atividade_Nao_Encontrada()
    {
        // Arrange
        Guid atividadeId = Guid.NewGuid();

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.SelecionarPorIdAsync(atividadeId))
            .ReturnsAsync((AtividadeMedica)null);

        ExcluirAtividadeMedicaRequest request = new(atividadeId);

        // Act
        FluentResults.Result<ExcluirAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioAtividadeMedicaMock.Verify(r => r.ExcluirAsync(It.IsAny<AtividadeMedica>()), Times.Never);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Never);

        Assert.IsTrue(result.IsFailed);

        string mensagemErroEsperada = ErrorResults.NotFoundError(atividadeId).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }

    [TestMethod]
    public async Task Deve_Retornar_Erro_Interno_Em_Caso_De_Exception_Durante_Exclusao()
    {
        // Arrange
        Guid atividadeId = Guid.NewGuid();
        Consulta atividade = new(DateTime.Now, DateTime.Now.AddHours(1), new Medico("Dr. João", "12345-SP"));

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.SelecionarPorIdAsync(atividadeId))
            .ReturnsAsync(atividade);

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.ExcluirAsync(It.IsAny<AtividadeMedica>()))
            .Throws(new Exception("Erro inesperado"));

        ExcluirAtividadeMedicaRequest request = new(atividadeId);

        // Act
        FluentResults.Result<ExcluirAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._contextoMock.Verify(c => c.RollbackAsync(), Times.Once);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.InternalServerError(new Exception("Erro inesperado")).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }
}
