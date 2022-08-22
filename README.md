<img src="https://drive.google.com/uc?id=1XPWLjUo2-j8iGw07ALcxu7oqJ3nkl2Ho" alt="Rocketseat+"/>

# Sample.CSharpNETCore.TelnetClientServer

Este repositório contem uma aplicação cliente/servidor baseado em telnet.

## Como Servidor

Você escolhe qual aplicação de linha de comando servir.

Por padrão o *Prompt do Windows* (`cmd.exe`) é servido na porta `23` usando:

```
TelnetClientServer.exe --server
```

Para ajustar os parâmetros, como ter o *PowerShell* (`pwsh.exe`) na porta `6500`, use:

```
TelnetClientServer.exe --server pwsh.exe:6500
```

## Como Cliente

Este é o modo padrão de execução, quando usado sem parâmetros.
Ele tenta conectar em `localhost` na porta `23`:

```
TelnetClientServer.exe
```

Para ajustar o servidor e porta de destino, por exemplo `telnet.service.sergiocabral.com` e `1025`, use:

```
TelnetClientServer.exe telnet.service.sergiocabral.com:1025
```

## Slides da aula

![Slide 1](./_assets/Slide01.png)

![Slide 2](./_assets/Slide02.png)

![Slide 3](./_assets/Slide03.png)

![Slide 4](./_assets/Slide04.png)

![Slide 5](./_assets/Slide05.png)

![Slide 6](./_assets/Slide06.png)

## Rocketseat+

| [<img src="https://avatars.githubusercontent.com/u/665373?v=4" width="75px;"/>](https://github.com/sergiocabral) |
| :-: |
|[sergiocabral.com](https://sergiocabral.com)|
