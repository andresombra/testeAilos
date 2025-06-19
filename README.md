## Observação para esse IdContaCorrente `B6BAFC09 -6967-ED11-A567-055DFA4A16C9`, ele foi cadastrado com um espaço. 
![IdContaCorrente com espaco](https://github.com/user-attachments/assets/db197f77-e313-4f98-9027-d9a583f2de17)

# Análise da Arquitetura do Sistema Bancário

Este documento detalha a análise da arquitetura de um sistema bancário, com foco na estrutura do código, dependências e interações dos componentes, conforme descrito no documento de requisitos. Também inclui um diagrama de arquitetura para visualização dos módulos principais e fluxos de dados.

## 1. Componentes Principais:

*   **API REST Existente do Banco:** A base do sistema, já funcional e conectada a um banco de dados SQLite. Ela expõe endpoints para funcionalidades bancárias existentes.

*   **Banco de Dados SQLite:** O armazenamento persistente para os dados do banco. Contém as seguintes tabelas:
    *   `contacorrente`: Armazena informações sobre as contas correntes (ID, número, nome do titular, status de ativação).
    *   `movimento`: Registra todas as transações (créditos e débitos) realizadas nas contas (ID do movimento, ID da conta, data, tipo de movimento, valor).
    *   `idempotencia`: Utilizada para garantir a idempotência das requisições, armazenando a chave de idempotência, dados da requisição e dados de retorno.

*   **Novas Funcionalidades (a serem desenvolvidas):**
    *   **Serviço de Movimentação de Conta Corrente:** Responsável por processar requisições de crédito e débito em contas. Inclui validações de negócio e persistência na tabela `movimento`.
    *   **Serviço de Consulta de Saldo da Conta Corrente:** Responsável por calcular e retornar o saldo atual de uma conta, somando créditos e subtraindo débitos da tabela `movimento`.

## 2. Dependências e Interações:

*   **API REST Existente <-> Banco de Dados SQLite:** A API existente interage diretamente com o banco de dados para operações de leitura e escrita nas tabelas `contacorrente`, `movimento` e `idempotencia`.

*   **Aplicativo (Cliente) <-> Novas Funcionalidades (API):** Um aplicativo externo (cliente) irá consumir os novos serviços da API para realizar movimentações e consultar saldos.

*   **Serviço de Movimentação de Conta Corrente <-> Banco de Dados SQLite:** Este serviço irá:
    *   Consultar a tabela `contacorrente` para validar a existência e o status da conta.
    *   Inserir registros na tabela `movimento` após validações bem-sucedidas.
    *   Utilizar a tabela `idempotencia` para gerenciar requisições repetidas e garantir a consistência.

*   **Serviço de Consulta de Saldo da Conta Corrente <-> Banco de Dados SQLite:** Este serviço irá:
    *   Consultar a tabela `contacorrente` para validar a existência e o status da conta.
    *   Consultar a tabela `movimento` para calcular o saldo da conta.

## 3. Padrões e Tecnologias Sugeridas (para pontos extras):

Embora não obrigatórios, o documento menciona padrões e tecnologias que a empresa costuma utilizar, indicando uma arquitetura mais robusta e modular:

*   **Dapper:** Como ORM (Object-Relational Mapper) para facilitar a interação com o banco de dados, mapeando objetos C# para tabelas SQL e vice-versa.

*   **CQRS (Command Query Responsibility Segregation):** Sugere a separação das operações de escrita (comandos, como movimentação) das operações de leitura (consultas, como saldo). Isso pode levar a modelos de dados otimizados para cada tipo de operação e maior escalabilidade.

*   **Mediator:** Um padrão comportamental que reduz as dependências diretas entre objetos, facilitando a comunicação indireta entre eles. No contexto da API, pode ser usado para orquestrar a lógica de negócio entre diferentes componentes (ex: validação, persistência, idempotência).

*   **Swagger:** Para documentação automática da API, incluindo endpoints, modelos de requisição/resposta e exemplos. Essencial para a integração com o aplicativo cliente.

*   **Testes Unitários (com NSubstitute):** Para garantir a qualidade do código, com mock de integrações e bancos de dados. Isso indica uma preocupação com a testabilidade e a manutenibilidade do sistema.

## 4. Fluxos de Dados:

*   **Movimentação de Conta:**
    1.  Aplicativo envia requisição (ID da requisição, ID da conta, valor, tipo de movimento) para o Serviço de Movimentação.
    2.  Serviço valida a requisição (conta existente/ativa, valor positivo, tipo válido).
    3.  Serviço consulta/atualiza a tabela `idempotencia`.
    4.  Serviço insere novo registro na tabela `movimento`.
    5.  Serviço retorna ID do movimento gerado (HTTP 200) ou mensagem de erro (HTTP 400).

*   **Consulta de Saldo:**
    1.  Aplicativo envia requisição (ID da conta) para o Serviço de Consulta de Saldo.
    2.  Serviço valida a requisição (conta existente/ativa).
    3.  Serviço consulta a tabela `movimento` para calcular o saldo.
    4.  Serviço retorna número da conta, nome do titular, data/hora da resposta e valor do saldo (HTTP 200) ou mensagem de erro (HTTP 400).
 
    ## Diagrama de Arquitetura

    ![diagrama_arquitetura](https://github.com/user-attachments/assets/78bf7cd6-60cd-466b-ba00-041871129f03)
