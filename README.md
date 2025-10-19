# 🛒 Sistema de Gestão - WPF

Sistema desktop completo para gerenciamento de pessoas, produtos e pedidos, desenvolvido em **C# com WPF** aplicando o padrão **MVVM**.

---

## 🎯 Sobre o Projeto

Este projeto nasceu de um desafio técnico que me instigou a sair da zona de conforto. Mesmo sem experiência prévia com WPF, encarei o desafio de criar uma aplicação desktop robusta, aplicando conceitos de arquitetura limpa e boas práticas de desenvolvimento.

A ideia central é simples: um sistema que gerencia o ciclo completo de vendas - desde o cadastro de clientes até o acompanhamento do status de entrega dos pedidos.

---

## 🚀 Funcionalidades

### 📋 Gestão de Pessoas
- Cadastro com validação real de CPF
- Busca por nome e CPF com filtros dinâmicos
- Histórico completo de pedidos por cliente
- Filtros inteligentes por status (Pagos, Entregues, Pendentes)

### 📦 Gestão de Produtos
- Cadastro com código único por produto
- Busca combinada: nome, código ou faixa de preço
- Validação de duplicidade de códigos

### 🛒 Gestão de Pedidos
- Pedidos vinculados a clientes específicos
- Carrinho com múltiplos produtos e quantidades
- Cálculo automático do total
- Workflow de status: Pendente → Pago → Enviado → Recebido
- Três formas de pagamento disponíveis

---

## 📸 Screenshots

> **Tela de Menu**
> 
> <img width="635" height="442" alt="menu png" src="https://github.com/user-attachments/assets/5004aabb-018e-4739-a8d8-35230d2b161d" />

> **Cadastro de Pessoa**
> 
> ![cadastroPF](https://github.com/user-attachments/assets/22a03a16-a9eb-423f-8ea2-0f225a393443)

> **Cadastrar Produto**
> 
> ![cadastroProduct](https://github.com/user-attachments/assets/9d20298c-2754-4937-8881-258b4dad18d5)

> **Gerar Pedido**
> 
> ![gerarPedido](https://github.com/user-attachments/assets/f9c594c0-1418-4072-add7-9c5265c9eb9a)

> **Confirmar Pedido**
> 
> ![confirmarPedido](https://github.com/user-attachments/assets/702f70c7-186d-4682-8a88-075f8b970567)

---

## 🏗️ Arquitetura

Organizei o projeto pensando em escalabilidade e manutenibilidade:
```
SistemaGestao/
├── Models/              # Entidades (Pessoa, Produto, Pedido)
├── Views/               # Telas XAML
├── ViewModels/          # Lógica de apresentação
├── Services/            # Regras de negócio + persistência
├── Helpers/             # Validações e conversores
└── Data/                # Arquivos JSON
```

### 🎯 Decisões de Arquitetura

**MVVM Pattern**  
Primeira vez trabalhando com esse padrão. No início confesso que achei complexo separar tudo em camadas, mas depois de entender o fluxo, vi como facilita a manutenção. A View não sabe nada da lógica, só exibe dados.

**JSON ao invés de XML**  
Escolhi JSON por ser mais leve e fácil de debugar.
**LINQ**  
Uma das exigências do desafio era usar LINQ intensivamente. Acabei usando em praticamente todas as consultas: filtros, ordenação, cálculos agregados.

---

## 🔥 Desafios e Aprendizados

### 1. **WPF**
Nunca tinha mexido com WPF antes. O conceito de binding bidirecional e a sintaxe do XAML foram os primeiros obstáculos. Assisti vários vídeos e li documentação até pegar o jeito.

### 2. **Helpers e Converters me quebraram**
Os **Converters** foram provavelmente a parte mais difícil. Entender como criar um `IValueConverter` pra converter um `bool` em `Visibility` não foi trivial. Tive que estudar bastante sobre como o WPF faz data binding por baixo dos panos.

O `BoolToVisibilityConverter` parece simples agora, mas demorei pra entender que ele roda toda vez que a propriedade muda, e que precisa implementar tanto `Convert` quanto `ConvertBack`.

### 3. **Validação de CPF do zero**
Implementei o algoritmo completo de validação de CPF. Foi desafiador entender a lógica dos dígitos verificadores, mas satisfatório quando funcionou perfeitamente.

### 4. **Persistência sem banco de dados**
Trabalhar com arquivos JSON ao invés de um banco foi diferente. Precisei pensar em como gerar IDs únicos, como fazer "joins" manuais entre entidades, e como garantir que os dados não seriam corrompidos.

### 5. **MVVM na prática**
Entender **quando** algo deve estar no ViewModel vs quando deve estar no Code-Behind foi um processo. No começo, queria colocar tudo no ViewModel, mas aprendi que alguns eventos de UI podem sim ficar no code-behind.

### 6. **ObservableCollection e INotifyPropertyChanged**
Fazer a UI atualizar automaticamente quando os dados mudavam foi outro desafio. Entender o `INotifyPropertyChanged` e porque usar `ObservableCollection` ao invés de `List` foi essencial.

---

## 🛠️ Stack Tecnológica

- **C# 10** - Linguagem principal
- **.NET Framework 4.6** - Runtime
- **WPF** - Framework de interface
- **MVVM** - Padrão arquitetural
- **Newtonsoft.Json** - Serialização de dados
- **LINQ** - Queries e manipulação de coleções

---

## 🚀 Como Rodar

### Pré-requisitos
- Visual Studio 2019+ 
- .NET Framework 4.6+

### Instalação

**1. Clone o repositório**
```bash
git clone https://github.com/seu-usuario/sistema-gestao.git
cd sistema-gestao
```

**2. Abra no Visual Studio**
```bash
SistemaGestao.sln
```

**3. Compile e rode**
Pressione `F5` ou clique no botão ▶️ Start

Na primeira execução, os arquivos JSON serão criados automaticamente em `Data/`.

---

## 📖 Como Usar

**Ordem recomendada pra testar:**

1. **Cadastre produtos** (Menu → Produtos → Incluir)
2. **Cadastre pessoas** (Menu → Pessoas → Incluir)  
   *Use um CPF válido, tipo: 123.456.789-09*
3. **Crie pedidos** (Pessoas → Selecionar pessoa → Incluir Pedido)
4. **Acompanhe status** (Mude de Pendente → Pago → Enviado → Recebido)

---

## 🎓 O Que Aprendi

Esse projeto me tirou completamente da zona de conforto. Principais aprendizados:

- **WPF é poderoso**, mas tem curva de aprendizado íngreme
- **MVVM faz sentido** depois que você entende o propósito
- **LINQ deixa o código muito mais limpo** que loops tradicionais
- **Arquitetura bem definida** facilita MUITO a manutenção

---

## 📝 Observações Finais

Esse foi meu primeiro projeto em WPF. Mesmo com os desafios, consegui entregar todas as funcionalidades solicitadas seguindo boas práticas.

Se você clonar e rodar, vai ver que o sistema funciona de ponta a ponta: cadastra, busca, edita, exclui, relaciona dados e persiste tudo corretamente.

---

## 👤 Autor

**Othávio Nogueira**
- GitHub: [@othaviolr](https://github.com/othaviolr)
- LinkedIn: [Othávio Nogueira](https://www.linkedin.com/in/othaviolr/)
