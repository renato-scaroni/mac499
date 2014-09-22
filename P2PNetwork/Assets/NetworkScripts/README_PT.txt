Este � um curto guia para aqueles que resolverem testar este plugin. Valhe lembrar que este programa ainda est� em desenvolvimento, 
portanto alguns recursos podem estar quebrados, al�m da implementa��o ainda n�o ter sido terminada ainda. Uma pequena lista do que j� foi
implementado pode ser vista abaixo:

Recursos implementados:

- Todas as classes de comunica��o UDP implementadas, incluindo APIs simples para broadcast de mensagens.
- Primeiro estado do protocolo implementado e completamente funcional. (hosts conseguem se enxergar na rede, mais detalhes na monografia
associada ao projeto)

Como usar:

- Basta criar um GameObject vazio e associar a ele um component chamado P2PNetworking. N�o esque�a de definir a porta para o canal
de comunica��o de broadcast atrav�s do inspector da unity.
- Por padr�o, o plugin iniciar� no Modo StanBy. Isto significa que ele j� estar� escutando por outros peers conectados � rede.
- Para obter uma lista com todos os peers encontrados acessar o atributo p�blico knownHosts na instancia ativa classe P2PNetworking. 
Para facilitar acesso a esta inst�ncia, P2PNetworking possui um atributo pulic chamado instance, que guardar� uma ref�rencia � mesma.
