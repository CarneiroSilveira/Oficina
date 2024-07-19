CREATE DATABASE oficina IF NOT EXISTS;
use oficina;

CREATE TABLE `ServicoAtendimento`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `idServico` INT UNSIGNED NOT NULL,
    `idAtendimento` INT UNSIGNED NOT NULL
);
CREATE TABLE `Cliente`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Nome` VARCHAR(255) NOT NULL,
    `CPF` VARCHAR(14) NULL DEFAULT 'True',
    `Numero` VARCHAR(255) NOT NULL,
    `Email` VARCHAR(255) NULL
);
CREATE TABLE `AtendimentoProdutos`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `idProdutos` INT UNSIGNED NOT NULL,
    `idAtendimento` INT UNSIGNED NOT NULL,
    `Quantidade` INT NOT NULL
);
CREATE TABLE `Produtos`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Nome` VARCHAR(255) NOT NULL,
    `Preco` DOUBLE NOT NULL
);
CREATE TABLE `Servico`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `Nome` VARCHAR(255) NOT NULL,
    `Preco` DOUBLE NOT NULL
);
CREATE TABLE `Atendimento`(
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
    `DataInicio` DATETIME NOT NULL,
    `DataFim` DATETIME NOT NULL,
    `IdCliente` INT UNSIGNED NOT NULL,
    `CustoTotal` DOUBLE NOT NULL,
    `Descricao` VARCHAR(2048) NULL,
    `CustoExtra` DOUBLE NULL,
    `Desconto` DOUBLE NULL
);
ALTER TABLE
    `ServicoAtendimento` ADD CONSTRAINT `servicoatendimento_idservico_foreign` FOREIGN KEY(`idServico`) REFERENCES `Servico`(`id`);
ALTER TABLE
    `ServicoAtendimento` ADD CONSTRAINT `servicoatendimento_idatendimento_foreign` FOREIGN KEY(`idAtendimento`) REFERENCES `Atendimento`(`id`);
ALTER TABLE
    `AtendimentoProdutos` ADD CONSTRAINT `atendimentoprodutos_idprodutos_foreign` FOREIGN KEY(`idProdutos`) REFERENCES `Produtos`(`id`);
ALTER TABLE
    `AtendimentoProdutos` ADD CONSTRAINT `atendimentoprodutos_idatendimento_foreign` FOREIGN KEY(`idAtendimento`) REFERENCES `Atendimento`(`id`);
ALTER TABLE
    `Atendimento` ADD CONSTRAINT `atendimento_idcliente_foreign` FOREIGN KEY(`IdCliente`) REFERENCES `Cliente`(`id`);

-- Inserção de dados de teste

-- Inserindo Clientes
INSERT INTO `Cliente` (`Nome`, `CPF`, `Numero`, `Email`) VALUES
('João Silva', '123.456.789-00', '11987654321', 'joao.silva@example.com'),
('Maria Souza', '987.654.321-00', '21987654321', 'maria.souza@example.com');

-- Inserindo Produtos
INSERT INTO `Produtos` (`Nome`, `Preco`) VALUES
('Produto A', 10.50),
('Produto B', 20.75);

-- Inserindo Serviços
INSERT INTO `Servico` (`Nome`, `Preco`) VALUES
('Serviço A', 150.00),
('Serviço B', 200.00);

-- Inserindo Atendimentos
INSERT INTO `Atendimento` (`DataInicio`, `DataFim`, `IdCliente`, `CustoTotal`, `Descricao`, `CustoExtra`, `Desconto`) VALUES
('2023-07-01 10:00:00', '2023-07-01 12:00:00', 1, 170.00, 'Atendimento de teste 1', 20.00, 0.00),
('2023-07-02 14:00:00', '2023-07-02 16:00:00', 2, 220.00, 'Atendimento de teste 2', 20.00, 0.00);

-- Inserindo AtendimentoProdutos
INSERT INTO `AtendimentoProdutos` (`idProdutos`, `idAtendimento`, `Quantidade`) VALUES
(1, 1, 2),
(2, 2, 1);

-- Inserindo ServicoAtendimento
INSERT INTO `ServicoAtendimento` (`idServico`, `idAtendimento`) VALUES
(1, 1),
(2, 2);