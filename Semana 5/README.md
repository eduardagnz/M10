# A Importância de CI/CD no Desenvolvimento de Software

A adoção de pipelines de Integração Contínua (CI) e Entrega Contínua (CD) é fundamental para o desenvolvimento moderno de software. O CI/CD reduz significativamente o tempo entre a escrita do código e sua disponibilização em ambientes de produção, aumentando a produtividade e confiabilidade dos times de desenvolvimento. Com o CI, integrações frequentes garantem que módulos de código funcionem corretamente juntos, minimizando erros acumulados. Por outro lado, o CD permite que atualizações sejam disponibilizadas aos usuários finais de forma automática e segura. Juntos, esses processos otimizam a colaboração, reduzem o retrabalho e aceleram a entrega de valor ao negócio.
O GitHub Actions é uma ferramenta poderosa para automação de fluxos de trabalho de desenvolvimento. Um workflow é estruturado em um arquivo YAML e consiste nos seguintes componentes principais:
Triggers: Eventos que iniciam o workflow, como push, pull_request, ou a execução manual (workflow_dispatch).
Jobs: Conjuntos de tarefas que podem ser executados em paralelo ou sequencialmente.
Steps: Ações individuais dentro de um job, como executar comandos ou usar ações reutilizáveis do marketplace.
Runners: Ambientes onde os jobs são executados. Podem ser hospedados pelo GitHub ou auto-hospedados.

Exemplo de workflow simples:

```
name: CI Workflow
on:
  push:
    branches:
      - main

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Set up Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '16'

      - name: Install dependencies
        run: npm install

      - name: Run tests
        run: npm test
```

Nesse exemplo, o workflow é acionado sempre que há um push na branch main. Ele executa um job que verifica o código, instala dependências e executa os testes. A automação do ciclo de vida de desenvolvimento, desde a integração até a entrega, é um dos maiores benefícios do DevOps. Aqui é onde GitHub Actions e AWS CodeDeploy entram em cena para entregar valor ao negócio:

GitHub Actions pode ser configurado para construir, testar e empacotar o aplicativo Spring Boot.
Em seguida, ele dispara automaticamente a implantação com o CodeDeploy em instâncias EC2 gerenciadas por um Auto Scaling Group.
AWS CodeDeploy para Entrega Segura
CodeDeploy oferece suporte a estratégias de implantação, como blue/green e rolling, que minimizam o tempo de inatividade e reduzem riscos durante as atualizações.
Ele garante que apenas instâncias saudáveis recebam o novo código.
AWS Auto Scaling Group
O uso de Auto Scaling Groups aumenta a resiliência da aplicação. Qualquer nova instância criada automaticamente recebe a versão mais recente do aplicativo devido à integração com o CodeDeploy.
Fluxo Técnico de Implantação
O pipeline DevOps utilizando GitHub Actions e AWS CodeDeploy para implantar o aplicativo Java Spring Boot funciona da seguinte forma:
O GitHub Actions é usado para ompilar o aplicativo Spring Boot com Maven ou Gradle, testar e empacotar o código, fazer o upload do artefato empacotado (.jar ou .war) para o Amazon S3 e disparar uma implantação no AWS CodeDeploy. 
Já AWS CloudFormation é uma ferramenta de infraestrutura como código (IaC) que permite criar e gerenciar recursos na nuvem de forma automatizada e reprodutível. Ele usa templates YAML ou JSON para definir toda a infraestrutura, eliminando processos manuais suscetíveis a erros.

Um exemplo de template para criar uma instância EC2:

```
AWSTemplateFormatVersion: "2010-09-09"
Resources:
  MyEC2Instance:
    Type: "AWS::EC2::Instance"
    Properties:
      InstanceType: "t2.micro"
      KeyName: "my-key-pair"
      ImageId: "ami-12345678" # Substitua pelo AMI da sua região
      Tags:
        - Key: "Name"
          Value: "MyInstance"
```

Explicação do Template:

AWSTemplateFormatVersion: Define a versão do CloudFormation.
Resources: Contém a definição dos recursos que serão criados.
MyEC2Instance: Nome lógico da instância EC2.
InstanceType: Tipo da instância (neste caso, t2.micro).
KeyName: Nome do par de chaves para acessar a instância.
ImageId: ID da imagem (AMI) que será utilizada.
Tags: Adiciona uma tag com o nome MyInstance para a instância.
Esse template pode ser executado diretamente no console da AWS ou via CLI, garantindo consistência na criação de recursos.


A integração entre GitHub Actions, AWS CloudFormation e Amazon EC2 permite automatizar tanto o desenvolvimento quanto a infraestrutura, criando um ciclo completo de entrega contínua. Um caso real seria um projeto onde, ao realizar um push em um repositório, o GitHub Actions testa o código usando CI, implanta a infraestrutura necessária no AWS usando um template CloudFormation e inicia uma instância EC2 com a nova versão da aplicação.

Exemplo de workflow para essa integração:

```
name: Deploy to AWS

on:
  push:
    branches:
      - main

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v2
        with:
          aws-access-key-id: ‘${{ secrets.AWS_ACCESS_KEY_ID }}’
          aws-secret-access-key: ‘${{ secrets.AWS_SECRET_ACCESS_KEY }}’
          aws-region: us-east-1

      - name: Deploy CloudFormation template
        run: |
          aws cloudformation deploy \
            --template-file ec2-template.yaml \
            --stack-name MyEC2Stack \
            --capabilities CAPABILITY_NAMED_IAM

      - name: Verify EC2 Instance
        run: aws ec2 describe-instances --filters "Name=tag:Name,Values=MyInstance"
```

Os Benefícios dessa abordagem contemplam a automatação de ponta a ponta, reduzindo intervenções manuais, garantia de consistência entre código e infraestrutura, e escalabilidade para lidar com requisitos dinâmicos de recursos.

Esse tipo de pipeline é particularmente útil em aplicações baseadas em microservices ou projetos que exigem alta frequência de atualizações, maximizando a agilidade e a confiabilidade do sistema.
Aplicação em Projetos Reais
Ao unir GitHub Actions, CodeDeploy e Auto Scaling Group, as empresas conseguem 
1. Garantir Continuidade: As implantações automáticas asseguram que novas instâncias no Auto Scaling Group sempre executem a versão mais recente do aplicativo.
2. Reduzir Riscos: A integração com o CodeDeploy permite testar antes de expor o tráfego às novas versões.
3. Aumentar a Eficiência: Os pipelines automatizados eliminam o trabalho manual, permitindo que as equipes foquem em inovações.
4. Escalabilidade Dinâmica: Aplicações hospedadas podem escalar conforme necessário, com novas instâncias configuradas e operacionais automaticamente.
5. Essa abordagem promove um ciclo completo de DevOps, unindo o melhor das práticas de integração contínua, entrega contínua e automação de infraestrutura.

Sendo algumas dessas, coisas que foram desafios no projeto. A primeira coisa foi a integra contínua onde foi adotado o modelo de Git Flow para organizar o fluxo de trabalho e as contribuições da equipe de desenvolvimento. Para garantir a qualidade cada commit enviado para as branches `main`, `dev`, `feature/*` e `feat/*` passa por uma compilação e execução de testes automatizados, conforme configurado no GitHub Actions.

**Workflow Configurado no GitHub Actions (.yml)**:
```yaml
name: Build and Test

on:
  push:
    branches:
      - main
      - dev
      - feature/*
      - feat/*

jobs:
  build:
    name: Build, Test, and Analyze
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Set up Node.js
        uses: actions/setup-node@v3
        with:
          node-version: '16'

      - name: Change to project directory
        run: cd ./extrato-api/api

      - name: Install dependencies
        run: npm install
        working-directory: ./extrato-api/api

      - name: Run unit tests
        run: npm run test
        env:
          CI: true
        working-directory: ./extrato-api/api

      - name: Run Snyk to check for vulnerabilities
        uses: snyk/actions/node@master
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}

      - name: SonarQube Scan
        uses: sonarsource/sonarqube-scan-action@v3
        env:
          SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
          SONAR_HOST_URL: 'https://sonar.jrcf.dev/'
```

**Exemplo de Logs no GitHub Actions**:
- **Branches Monitoradas**: `main`, `dev`, `feature/*`, `feat/*`
- **Build Status**: Sucesso
- **Saída de Testes**:
  - `Teste: Validação do endpoint de autenticação - SUCESSO`
  - `Cobertura total de testes: 85%`

Além da análise estática, onde utilizamos o SonarQUbe/SonarCloud onde os relatórios de análise são gerados automaticamente e disponibilizados na interface do SonarQube para correções de qualidade antes da integração.Com o pipeline de CI/CD estabelecido, a equipe conseguiu assegurar que todos os commits e merges respeitem os padrões de qualidade definidos. Este documento será atualizado periodicamente para refletir as melhorias implementadas no fluxo de trabalho. 
