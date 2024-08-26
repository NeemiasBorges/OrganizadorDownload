# Worker Service - Organização de Arquivos

Este projeto é um **Worker Service** em C# que automatiza a organização de arquivos na pasta **Downloads** do usuário, mantendo-a limpa e organizada continuamente.

## Funcionalidades

- **Organização automática** de arquivos por extensão:
  - Executáveis (`.exe`) → `SOFTWARES/INSTALADORES`
  - Imagens (`.png`, `.jpg`, `.jpeg`, `.gif`) → `IMAGENS`
  - Documentos (`.pdf`, `.docx`, `.xlsx`) → `DOCUMENTOS`
- **Limpeza de pastas vazias**
- **Proteção contra movimentação de arquivos em uso**
- **Geração de relatórios** após cada execução

## Configurações Personalizadas

O serviço utiliza um arquivo `config.json` para configurações flexíveis:

```json
{
  "Paths": {
    "DownloadsPath": "C:\\Users\\Usuario\\Downloads",
    "SoftwaresPath": "C:\\Users\\Usuario\\Downloads\\SOFTWARES\\INSTALADORES",
    "ImagesPath": "C:\\Users\\Usuario\\Downloads\\IMAGENS",
    "DocumentsPath": "C:\\Users\\Usuario\\Downloads\\DOCUMENTOS"
  },
  "Extensions": {
    "Softwares": [".exe"],
    "Images": [".png", ".jpg", ".jpeg", ".gif"],
    "Documents": [".pdf", ".docx", ".xlsx"]
  }
}
```

## Como Usar

1. Clone o repositório
2. Certifique-se de ter o .NET instalado
3. Execute o comando:
   ```
   dotnet run
   ```

O serviço organizará a pasta de downloads a cada minuto.

### Exemplo de Log

```
[INFO] Movido: C:\Users\Usuario\Downloads\arquivo.pdf para C:\Users\Usuario\Downloads\DOCUMENTOS\arquivo.pdf
[WARN] Arquivo já existe: C:\Users\Usuario\Downloads\IMAGENS\imagem.png
[INFO] Resumo: 5 arquivos movidos, 2 arquivos falharam.
```

## Contribuição

Contribuições são bem-vindas! Por favor, siga estas diretrizes:

- Use o padrão [Conventional Commits](https://www.conventionalcommits.org/)
- Inclua ou atualize testes unitários para novas funcionalidades
- Mantenha o pipeline CI/CD verde
- Atualize a documentação conforme necessário

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
